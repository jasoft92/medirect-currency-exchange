# medirect-currency-exchange

## Overview
The MeDirect currency exchange service provides an API whereby client requests can be made to convert an amount from a one currency to another.

The request expects the following parameters:

- CustomerID
- SourceCurrency
- TargetCurrency
- ExchangeAmount

When a request is received, checks are made to validate that the bank customer has enough funds in his source currency wallet and after the exchange rate is retrieved, the converted amount is calculated and deducted from his source currency wallet, and added to his target currency account.

The below is an example request made, followed by screenshots from SwaggerUI to which shows the response received. 
In the below example, customer ID 1 is requested to exchange 5 EUR to GBP, and from the response it is shown that the converted amount for that exchange is 4.31, the new balance in the source account (EUR) was deducted by 5, while the balance in the target account (GBP) increased by 4.31

```
{
  "customerId": 1,
  "sourceCurrency": "EUR",
  "targetCurrency": "GBP",
  "exchangeAmount": 5
}
```
![image](https://user-images.githubusercontent.com/29455350/203649229-86a3e993-78fe-4c43-b097-6d3aedaecc4b.png)

## Database setup

### ConnectionStrings

In the ```appsettings.json``` file, the connection string *(CurrencyExchangeDbConnectionString)* requires the placeholders for **#server#** **#user#** and **#password#** to be replaced
```
  "ConnectionStrings": {
    "CurrencyExchangeDbConnectionString": "Server=#server#;Database=currencyexchangedb;User ID=#user#;Password=#password#;Encrypt=true;Connection Timeout=300;"
  },
```

### EF Core Migrations
When running EF Core Migrations, the Default project should be set to ```medirect-currency-exchange.Database``` in Visual Studio

![image](https://user-images.githubusercontent.com/29455350/203651407-d48e0ade-56ae-4fe9-86e0-82f542e27aa6.png)
