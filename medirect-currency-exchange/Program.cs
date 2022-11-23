using AutoMapper;
using FluentValidation;
using medirect_currency_exchange.Application.Clients;
using medirect_currency_exchange.Application.Services;
using medirect_currency_exchange.Contracts;
using medirect_currency_exchange.Database.Context;
using medirect_currency_exchange.Database.Repositories;
using medirect_currency_exchange.Domain.Profiles;
using medirect_currency_exchange.Logger;
using medirect_currency_exchange.Validators;
using Microsoft.EntityFrameworkCore;
using NLog;


var builder = WebApplication.CreateBuilder(args);

LogManager.LoadConfiguration(string.Concat(Directory.GetCurrentDirectory(), "/nlog.config"));

// Add services to the container.
builder.Services.AddControllers();

builder.Services.AddScoped<ILoggerManager, LoggerManager>();
builder.Services.AddSingleton<IValidator<CurrencyExchangeRequest>,ExchangeRequestValidator>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<CurrencyExchangeDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("CurrencyExchangeDbConnectionString")));

builder.Services.AddScoped<ICurrencyExchangeRepository, CurrencyExchangeRepository>();
builder.Services.AddScoped<ICurrencyExchangeService, CurrencyExchangeService>();
builder.Services.AddScoped<IExchangeRateApiClient, ExchangeRateApiClient>();

builder.Services.AddHttpClient<IExchangeRateApiClient, ExchangeRateApiClient>(client =>
{
	client.BaseAddress = new Uri("https://api.apilayer.com/exchangerates_data/");
	client.DefaultRequestHeaders.Add("apikey", "WdopSdwXLg67GbYzfS2JQ8bfmIx40FfL");
});

builder.Services.AddMemoryCache();

var config = new MapperConfiguration(cfg =>
{
	cfg.AddProfile(new CurrencyExchangeProfile());
});
var mapper = config.CreateMapper();
builder.Services.AddSingleton(mapper);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
