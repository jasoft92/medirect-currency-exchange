using AutoMapper;
using medirect_currency_exchange.Application.Services;
using medirect_currency_exchange.Contracts;
using medirect_currency_exchange.Domain.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace medirect_currency_exchange.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class CurrencyExchangeController : Controller
	{
		private readonly IMapper _mapper;
		private readonly ICurrencyExchangeService _currencyExchangeService;

		public CurrencyExchangeController(IMapper mapper, ICurrencyExchangeService currencyExchangeService)
		{
			_mapper = mapper;
			_currencyExchangeService = currencyExchangeService;
		}

		[HttpPost]
		public async Task<IActionResult> Trade(CurrencyExchangeRequest request)
		{
			await _currencyExchangeService.ProcessExchange(_mapper.Map<CurrencyExchangeRequest, CurrencyExchangeDto>(request));
			return Ok();
		}

	}
}