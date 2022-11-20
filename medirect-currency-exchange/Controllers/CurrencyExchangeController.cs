using AutoMapper;
using medirect_currency_exchange.Contracts;
using medirect_currency_exchange.Database.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace medirect_currency_exchange.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class CurrencyExchangeController : Controller
	{
		private readonly ICurrencyExchangeRepository _currencyExchangeRepository;
		private readonly IMapper _mapper;

		public CurrencyExchangeController(ICurrencyExchangeRepository currencyExchangeRepository, IMapper mapper)
		{
			_currencyExchangeRepository = currencyExchangeRepository;
			_mapper = mapper;
		}

		[HttpPost]
		public IActionResult Trade(CurrencyExchangeRequest request)
		{
			return null;
		}

	}
}
