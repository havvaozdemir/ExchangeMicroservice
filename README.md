## Currency Exchange Servise

A web-api which takes the client and currency information, obtains the current exchange rate information from the exchange rate provider and makes the exchange rate conversion. It stores the operations performed with date and customer trade rates information.
For the current rate information is updated from the provider every 30 minutes.
Limiting each client to 10 currency exchange trades per hour using ClientRateLimiting system except ClientWhiteList.
Supported Exchange Symbols: "TRY","EUR","USD","GBP"
It uses Real-Time Exchange Rate Api Service, for individual usage please get key from https://exchangeratesapi.io/ and than set ExchangeRatesApiKey in appsetting.json file.
If you work on Mac you can get docker image for SqlServer.

## Usage

Fist of all, clone the repo with the command below. 
[https://github.com/havvaozdemir/ExchangeMicroservice]
(git clone https://github.com/havvaozdemir/ExchangeMicroservice.git)


* You must have Microsoft.NET.Sdk net6.0, SqlServer and Visual Studio Code installed on your computer to run the project. Docker Desktop for dockerize options.
* Change ConnectionStrings parameter from appsettings.json and environment variables from docker-compose.yml based on your SqlServer.
* Run command below in terminal in project directory. You can use also use Makefile if you enter ExcahegeApi folder.

    ##### dotnet run or make run
* Docker files also added project directory. Open terminal and inside ExcahegeApi folder you can run commands
    ##### docker-compose build  or make dbuild
    ##### docker-compose up (-d optional) or make drun

## Endpoint Table

| Endpoint | Method | Description |
| ---------|--------|-------------|
|   /exchange/customers/{id}/currency/convert   |  POST   | Fetch exchangeratesapi.io when cache doesn't exists and save exchange rate trades data for customer, response rate.
