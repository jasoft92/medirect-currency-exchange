using AutoMapper;
using medirect_currency_exchange.Application.Services;
using medirect_currency_exchange.Contracts;
using medirect_currency_exchange.Domain.DTOs;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using FluentValidation;
using medirect_currency_exchange.Application.Exception;
using medirect_currency_exchange.Logger;
using Swashbuckle.AspNetCore.Annotations;

namespace medirect_currency_exchange.Controllers
{
	[ApiController]
	[Route("[controller]")]
	[SwaggerResponse(200, type: typeof(CurrencyExchangeResponse))]
	[SwaggerResponse(400, type: typeof(ErrorResponse))]
	[SwaggerResponse(422, type: typeof(ErrorResponse))]
	[SwaggerResponse(500, type: typeof(ErrorResponse))]
	public class CurrencyExchangeController : ControllerBase
	{
		private readonly IMapper _mapper;
		private readonly ICurrencyExchangeService _currencyExchangeService;
		private readonly ILoggerManager _loggerManager;
		private readonly IValidator<CurrencyExchangeRequest> _requestValidator;

		public CurrencyExchangeController(IMapper mapper, ICurrencyExchangeService currencyExchangeService, ILoggerManager loggerManager, IValidator<CurrencyExchangeRequest> requestValidator)
		{
			_mapper = mapper;
			_currencyExchangeService = currencyExchangeService;
			_loggerManager = loggerManager;
			_requestValidator = requestValidator;
		}

		[HttpPost]
		public async Task<IActionResult> Exchange(CurrencyExchangeRequest request)
		{
			try
			{
				_loggerManager.LogInfo($"Currency Exchange Request received. CustomerID: {request.CustomerId} | From {request.SourceCurrency} To {request.TargetCurrency} | Requested Amount: {request.ExchangeAmount}");

				var requestValidationResult = await _requestValidator.ValidateAsync(request);

				if (!requestValidationResult.IsValid)
				{
					var errorList = requestValidationResult.Errors.Select(err => err.ErrorMessage);
					var requestErrorMessage = $"Invalid Request: {string.Join(" | ", errorList)}";

					_loggerManager.LogError(requestErrorMessage);
					return CreateResponse(HttpStatusCode.BadRequest, new ErrorResponse(HttpStatusCode.BadRequest, requestErrorMessage));
				}

				var processResult = await _currencyExchangeService.ProcessExchange(_mapper.Map<CurrencyExchangeRequest, ExchangeRequestDto>(request));

				if (processResult.ExchangeResponseDto != null && processResult.ErrorResponse == null)
				{
					var response = _mapper.Map<ExchangeResponseDto, CurrencyExchangeResponse>(processResult.ExchangeResponseDto);
					_loggerManager.LogInfo($"Currency Exchange Trade completed. CustomerID: {response.CustomerId} | From {response.SourceCurrencyCode} To {response.TargetCurrencyCode} | Converted Amount: {response.ExchangeAmount}");
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
				_loggerManager.LogError($"Error when requesting exchange rates API updates for customer {request.CustomerId}: {apiException.Message}");
				return CreateResponse(apiException.HttpStatusCode, new ErrorResponse(apiException.HttpStatusCode, apiException.Message));
			}
			catch (Exception ex)
			{
				_loggerManager.LogError($"Error occured: {ex.Message}");
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