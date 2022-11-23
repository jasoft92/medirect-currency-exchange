using AutoMapper;
using medirect_currency_exchange.Contracts;
using medirect_currency_exchange.Domain.DTOs;

namespace medirect_currency_exchange.Domain.Profiles
{
	public class CurrencyExchangeProfile : Profile
	{
		public CurrencyExchangeProfile()
		{
			CreateMap<CurrencyExchangeRequest, ExchangeRequestDto>();

			CreateMap<ExchangeResponseDto, CurrencyExchangeResponse>();
		}
	}
}