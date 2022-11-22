using AutoMapper;
using medirect_currency_exchange.Application.Services;
using medirect_currency_exchange.Contracts;
using medirect_currency_exchange.Domain.DTOs;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using medirect_currency_exchange.Application.Exception;

namespace medirect_currency_exchange.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class CurrencyExchangeController : ControllerBase
	{
		private readonly IMapper _mapper;
		private readonly ICurrencyExchangeService _currencyExchangeService;

		public CurrencyExchangeController(IMapper mapper, ICurrencyExchangeService currencyExchangeService)
		{
			_mapper = mapper;
			_currencyExchangeService = currencyExchangeService;
		}

		[HttpPost]
		public async Task<IActionResult> Exchange(CurrencyExchangeRequest request)
		{
			try
			{
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