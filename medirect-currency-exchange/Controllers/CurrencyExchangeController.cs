using AutoMapper;
using medirect_currency_exchange.Application.Services;
using medirect_currency_exchange.Contracts;
using medirect_currency_exchange.Domain.DTOs;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using medirect_currency_exchange.Application.Exception;
using medirect_currency_exchange.Logger;

namespace medirect_currency_exchange.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class CurrencyExchangeController : ControllerBase
	{
		private readonly IMapper _mapper;
		private readonly ICurrencyExchangeService _currencyExchangeService;
		private readonly ILoggerManager _loggerManager;

		public CurrencyExchangeController(IMapper mapper, ICurrencyExchangeService currencyExchangeService, ILoggerManager loggerManager)
		{
			_mapper = mapper;
			_currencyExchangeService = currencyExchangeService;
			_loggerManager = loggerManager;
		}

		[HttpPost]
		public async Task<IActionResult> Exchange(CurrencyExchangeRequest request)
		{
			try
			{
				_loggerManager.LogInfo($"Currency Exchange Request received. CustomerID: {request.CustomerId} | From {request.SourceCurrency} To {request.TargetCurrency} | Amount: {request.ExchangeAmount}");
				
				var processResult = await _currencyExchangeService.ProcessExchange(_mapper.Map<CurrencyExchangeRequest, ExchangeRequestDto>(request));

				if (processResult.ExchangeResponseDto != null && processResult.ErrorResponse == null)
				{
					var response = _mapper.Map<ExchangeResponseDto, CurrencyExchangeResponse>(processResult.ExchangeResponseDto);
					return CreateResponse(HttpStatusCode.OK, response);
				}

				if (processResult.ErrorResponse != null)
				{
					var errorCode = processResult.ErrorResponse.ErrorCode;
					return CreateResponse(errorCode, new ErrorResponse(errorCode, processResult.ErrorResponse.Message));
				}

				return CreateResponse(HttpStatusCode.InternalServerError, new ErrorResponse(HttpStatusCode.InternalServerError, "Error Performing Currency Exchange"));

			}
			catch (ApiException apiException)
			{
				return CreateResponse(apiException.HttpStatusCode, new ErrorResponse(apiException.HttpStatusCode, apiException.Message));
			}
			catch (Exception ex)
			{
				return CreateResponse(HttpStatusCode.InternalServerError, new ErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
			}
		}

		private IActionResult CreateResponse<T>(HttpStatusCode httpStatusCode, T value)
		{
			return new ObjectResult(value)
			{
				StatusCode = (int)httpStatusCode
			};
		}
	}
}