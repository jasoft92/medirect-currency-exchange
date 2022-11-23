using medirect_currency_exchange.Application.Clients;
using medirect_currency_exchange.Application.Services;
using medirect_currency_exchange.Database.Repositories;
using medirect_currency_exchange.Domain.DTOs;
using medirect_currency_exchange.Domain.Models;
using medirect_currency_exchange.Logger;
using Microsoft.Extensions.Caching.Memory;
using Moq;

namespace medirect_currency_exchange.Tests
{
	internal class CurrencyExchangeServiceTests
	{
		private CurrencyExchangeService _currencyExchangeService;
		private ExchangeRequestDto _defaultExchangeRequestDto;
		private List<CustomerWallet?> _defaultCustomerWallets;
		private Customer _defaultCustomer;

		private decimal _defaultEurAccountBalance;
		private decimal _defaultGbpAccountBalance;

		private Mock<IMemoryCache> _memoryCacheMock;
		private Mock<IExchangeRateApiClient> _exchangeRateApiClientMock;
		private Mock<ICurrencyExchangeRepository> _currencyExchangeRepositoryMock;
		private Mock<ILoggerManager> _loggerManagerMock;

		[SetUp]
		public void Setup()
		{
			_memoryCacheMock = new Mock<IMemoryCache>();
			_exchangeRateApiClientMock = new Mock<IExchangeRateApiClient>();
			_currencyExchangeRepositoryMock = new Mock<ICurrencyExchangeRepository>();
			_loggerManagerMock = new Mock<ILoggerManager>();

			_currencyExchangeService = new CurrencyExchangeService(
				_memoryCacheMock.Object,
				_exchangeRateApiClientMock.Object,
				_currencyExchangeRepositoryMock.Object,
				_loggerManagerMock.Object);

			_defaultExchangeRequestDto = new ExchangeRequestDto(customerId: 1,
				sourceCurrency: "EUR",
				targetCurrency: "GBP",
				exchangeAmount: 5);

			_defaultEurAccountBalance = 100;
			_defaultGbpAccountBalance = 50;

			_defaultCustomerWallets = new List<CustomerWallet?>
			{
				new()
				{
					CustomerId = 1,
					CurrencyCode = "EUR",
					Amount = _defaultEurAccountBalance
				},
				new()
				{
					CustomerId = 1,
					CurrencyCode = "GBP",
					Amount = _defaultGbpAccountBalance
				}
			};

			_defaultCustomer = new Customer
			{
				Name = "Joseph",
				Surname = "Attard",
				Email = "jos.attard92@gmail.com",
				Id = 1,
				IdCard = "13392G"
			};

			_memoryCacheMock
				.Setup(x => x.CreateEntry(It.IsAny<object>()))
				.Returns(Mock.Of<ICacheEntry>);
		}

		[Test]
		public async Task GivenValidRequest_ThenExecuteSuccessfully()
		{
			// Arrange
			_currencyExchangeRepositoryMock.Setup(m => m.GetCustomerWallets(1))
				.ReturnsAsync(_defaultCustomerWallets);

			_currencyExchangeRepositoryMock.Setup(m => m.GetCustomer(1))
				.ReturnsAsync(_defaultCustomer);

			_currencyExchangeRepositoryMock.Setup(m => m.GetRecentCurrencyExchangeTransactions(1))
				.ReturnsAsync(new List<CurrencyExchangeTransaction>());

			_exchangeRateApiClientMock.Setup(m => m.GetExchangeRate("EUR", "GBP"))
				.ReturnsAsync(0.87m);

			// Act
			var result = await _currencyExchangeService.ProcessExchange(_defaultExchangeRequestDto);

			// Assert
			Assert.IsNull(result.ErrorResponse);
			Assert.IsNotNull(result.ExchangeResponseDto);
			Assert.AreEqual(_defaultExchangeRequestDto.ExchangeAmount * 0.87m, result.ExchangeResponseDto.ExchangeAmount);
			Assert.AreEqual(_defaultEurAccountBalance - _defaultExchangeRequestDto.ExchangeAmount, result.ExchangeResponseDto.SourceAccountBalance);
			Assert.AreEqual(_defaultGbpAccountBalance + result.ExchangeResponseDto.ExchangeAmount, result.ExchangeResponseDto.TargetAccountBalance);

			_exchangeRateApiClientMock.Verify(m => m.GetExchangeRate(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
			_currencyExchangeRepositoryMock.Verify(m => m.AddCustomerWallet(It.IsAny<CustomerWallet>()), Times.Never);
		}

		[Test]
		public async Task GivenCustomerDoesNotExist_ThenReturnError()
		{
			// Arrange
			_currencyExchangeRepositoryMock.Setup(m => m.GetCustomerWallets(1))
				.ReturnsAsync(_defaultCustomerWallets);

			_currencyExchangeRepositoryMock.Setup(m => m.GetCustomer(It.IsAny<long>()))
				.ReturnsAsync((Customer?)null);

			_currencyExchangeRepositoryMock.Setup(m => m.GetRecentCurrencyExchangeTransactions(1))
				.ReturnsAsync(new List<CurrencyExchangeTransaction>());


			// Act
			var result = await _currencyExchangeService.ProcessExchange(_defaultExchangeRequestDto);

			// Assert
			Assert.IsNotNull(result.ErrorResponse);
			Assert.Null(result.ExchangeResponseDto);
			Assert.AreEqual("Customer does not exist", result.ErrorResponse.Message);
		}

		[Test]
		public async Task GivenCustomerDoesNotHaveSourceCurrency_ThenReturnError()
		{
			// Arrange
			_currencyExchangeRepositoryMock.Setup(m => m.GetCustomerWallets(1))
				.ReturnsAsync(_defaultCustomerWallets);

			_currencyExchangeRepositoryMock.Setup(m => m.GetCustomer(1))
				.ReturnsAsync(_defaultCustomer);

			_currencyExchangeRepositoryMock.Setup(m => m.GetRecentCurrencyExchangeTransactions(1))
				.ReturnsAsync(new List<CurrencyExchangeTransaction>());

			var requestDto = new ExchangeRequestDto(1, "USD", "GBP", 5);

			// Act
			var result = await _currencyExchangeService.ProcessExchange(requestDto);

			// Assert
			Assert.IsNotNull(result.ErrorResponse);
			Assert.Null(result.ExchangeResponseDto);
			Assert.AreEqual($"Invalid Request. Client does not have an account with {requestDto.SourceCurrency} currency", result.ErrorResponse.Message);
		}

		[Test]
		public async Task GivenCustomerDoesNotHaveTargetCurrency_ThenExecuteSuccessfully()
		{
			// Arrange
			_currencyExchangeRepositoryMock.Setup(m => m.GetCustomerWallets(1))
				.ReturnsAsync(_defaultCustomerWallets);

			_currencyExchangeRepositoryMock.Setup(m => m.GetCustomer(1))
				.ReturnsAsync(_defaultCustomer);

			_currencyExchangeRepositoryMock.Setup(m => m.GetRecentCurrencyExchangeTransactions(1))
				.ReturnsAsync(new List<CurrencyExchangeTransaction>());

			_currencyExchangeRepositoryMock.Setup(m => m.AddCustomerWallet(It.IsAny<CustomerWallet>()))
				.ReturnsAsync(new CustomerWallet { CustomerId = 1, CurrencyCode = "USD", Amount = 0, LastModified = DateTime.Now });

			_exchangeRateApiClientMock.Setup(m => m.GetExchangeRate("EUR", "USD"))
				.ReturnsAsync(0.87m);

			var requestDto = new ExchangeRequestDto(1, "EUR", "USD", 5);

			// Act
			var result = await _currencyExchangeService.ProcessExchange(requestDto);

			// Assert
			Assert.IsNull(result.ErrorResponse);
			Assert.IsNotNull(result.ExchangeResponseDto);
			Assert.AreEqual(_defaultExchangeRequestDto.ExchangeAmount * 0.87m, result.ExchangeResponseDto.ExchangeAmount);
			Assert.AreEqual(_defaultEurAccountBalance - _defaultExchangeRequestDto.ExchangeAmount, result.ExchangeResponseDto.SourceAccountBalance);
			Assert.AreEqual(result.ExchangeResponseDto.ExchangeAmount, result.ExchangeResponseDto.TargetAccountBalance);

			_exchangeRateApiClientMock.Verify(m => m.GetExchangeRate(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
			_currencyExchangeRepositoryMock.Verify(m => m.AddCustomerWallet(It.IsAny<CustomerWallet>()), Times.Once);

		}

		[Test]
		public async Task GivenCustomerHasInsufficientFunds_ThenReturnError()
		{
			// Arrange
			_currencyExchangeRepositoryMock.Setup(m => m.GetCustomerWallets(1))
				.ReturnsAsync(_defaultCustomerWallets);

			_currencyExchangeRepositoryMock.Setup(m => m.GetCustomer(1))
				.ReturnsAsync(_defaultCustomer);

			_currencyExchangeRepositoryMock.Setup(m => m.GetRecentCurrencyExchangeTransactions(1))
				.ReturnsAsync(new List<CurrencyExchangeTransaction>());


			var requestDto = new ExchangeRequestDto(1, "EUR", "GBP", 500);

			// Act
			var result = await _currencyExchangeService.ProcessExchange(requestDto);

			// Assert
			Assert.IsNotNull(result.ErrorResponse);
			Assert.Null(result.ExchangeResponseDto);
			Assert.AreEqual($"Client has insufficient funds in his {requestDto.SourceCurrency} account to perform the requested exchange", result.ErrorResponse.Message);
		}

		[Test]
		public async Task GivenCustomerExceededHourlyExchangeTradeLimit_ThenReturnError()
		{
			// Arrange
			_currencyExchangeRepositoryMock.Setup(m => m.GetCustomerWallets(1))
				.ReturnsAsync(_defaultCustomerWallets);

			_currencyExchangeRepositoryMock.Setup(m => m.GetCustomer(1))
				.ReturnsAsync(_defaultCustomer);


			var pastTransactions = new List<CurrencyExchangeTransaction>();
			for (var i = 1; i <= 10; i++)
			{
				pastTransactions.Add(new CurrencyExchangeTransaction
				{
					FromCurrencyCode = "EUR",
					ToCurrencyCode = "GBP",
					ConvertedAmount = 5,
					TimeStamp = DateTime.Now.AddMinutes(i * -1)
				});
			}

			_currencyExchangeRepositoryMock.Setup(m => m.GetRecentCurrencyExchangeTransactions(1))
				.ReturnsAsync(pastTransactions);

			var requestDto = new ExchangeRequestDto(1, "EUR", "GBP", 5);

			// Act
			var result = await _currencyExchangeService.ProcessExchange(requestDto);

			// Assert
			Assert.IsNotNull(result.ErrorResponse);
			Assert.Null(result.ExchangeResponseDto);
			Assert.AreEqual("Client exceeded maximum allowed exchange trades per hour", result.ErrorResponse.Message);

		}

		[Test]
		public async Task GivenCachedCurrencyExchange_ThenExchangeRateApiOnlyIsNotCalled()
		{

			// Arrange
			_currencyExchangeRepositoryMock.Setup(m => m.GetCustomerWallets(1))
				.ReturnsAsync(_defaultCustomerWallets);

			_currencyExchangeRepositoryMock.Setup(m => m.GetCustomer(1))
				.ReturnsAsync(_defaultCustomer);

			_currencyExchangeRepositoryMock.Setup(m => m.GetRecentCurrencyExchangeTransactions(1))
				.ReturnsAsync(new List<CurrencyExchangeTransaction>());

			_exchangeRateApiClientMock.Setup(m => m.GetExchangeRate("EUR", "GBP"))
				.ReturnsAsync(0.87m);

			object cachedExchangeRate = 0.85m;
			var memoryCache = GetMemoryCache(cachedExchangeRate);

			var currencyExchangeService = new CurrencyExchangeService(
				memoryCache,
				_exchangeRateApiClientMock.Object,
				_currencyExchangeRepositoryMock.Object,
				_loggerManagerMock.Object);

			// Act
			var result = await currencyExchangeService.ProcessExchange(_defaultExchangeRequestDto);

			// Assert
			Assert.IsNull(result.ErrorResponse);
			Assert.IsNotNull(result.ExchangeResponseDto);
			Assert.AreEqual(_defaultExchangeRequestDto.ExchangeAmount * (decimal)cachedExchangeRate, result.ExchangeResponseDto.ExchangeAmount);
			Assert.AreEqual(_defaultEurAccountBalance - _defaultExchangeRequestDto.ExchangeAmount, result.ExchangeResponseDto.SourceAccountBalance);
			Assert.AreEqual(_defaultGbpAccountBalance + result.ExchangeResponseDto.ExchangeAmount, result.ExchangeResponseDto.TargetAccountBalance);

			_exchangeRateApiClientMock.Verify(m => m.GetExchangeRate(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
			_currencyExchangeRepositoryMock.Verify(m => m.AddCustomerWallet(It.IsAny<CustomerWallet>()), Times.Never);
		}

		private static IMemoryCache GetMemoryCache(object expectedValue)
		{
			var mockMemoryCache = new Mock<IMemoryCache>();
			mockMemoryCache
				.Setup(x => x.TryGetValue(It.IsAny<object>(), out expectedValue))
				.Returns(true);
			return mockMemoryCache.Object;
		}
	}
}