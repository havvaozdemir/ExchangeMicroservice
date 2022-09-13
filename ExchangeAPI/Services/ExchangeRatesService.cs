using System;
using System.Threading.Tasks;
using ExchangeAPI.Services.Handlers;
using ExchangeAPI.Services.Contracts;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using System.Linq;
using System.Collections.Generic;

namespace ExchangeAPI.Services
{

    public interface IExchangeRatesService
    {
        Task<ExchangeRatesResultModel> GetRatesAsync(string baseCurrency, string targetCurrency);
    }

    public class ExchangeRatesService : IExchangeRatesService
    {
        private readonly int m_CacheExpirationMinutes;
        private readonly string m_ApiKey;
        private readonly IMemoryCache m_Cache;
        private readonly IHttpClient m_ExchangeRatesHttpClient;
        public ExchangeRatesService(IMemoryCache cache, IHttpClient exchangeRatesHttpClient, string key, int expiration)
        {
            m_Cache = cache ?? throw new ArgumentNullException(nameof(cache));
            m_ExchangeRatesHttpClient = exchangeRatesHttpClient ?? throw new ArgumentNullException(nameof(exchangeRatesHttpClient));
            m_ApiKey = key;
            m_CacheExpirationMinutes = expiration;
        }

        public async Task<ExchangeRatesResultModel> GetRatesAsync(string baseCurrency, string targetCurrency)
        {
            if (!Symbols.IsValid(baseCurrency) || !Symbols.IsValid(targetCurrency))
            {
                return new ExchangeRatesResultModel
                {
                    Success = false,
                    ErrorInfo = $"Invalid value for {nameof(baseCurrency)} and/or {nameof(targetCurrency)}"
                };
            }
            m_Cache.TryGetValue(baseCurrency, out List<ExchangeRatesResultModel> latestRates);
            if (latestRates != null)
            {
                var found = latestRates.Find(m => m.Target == targetCurrency);
                return found != null ? found : new ExchangeRatesResultModel
                {
                    Success = false,
                    ErrorInfo = "Unable to retrieve exchange rates."
                };
            }

            var endpointUrl = $"exchangerates_data/latest?apikey={m_ApiKey}&base={baseCurrency}&symbols={string.Join(',', Symbols.ValidSymbols)}";
            var responseModels = (await m_ExchangeRatesHttpClient.GetAsync<ExchangeRatesModel>(endpointUrl));

            if (responseModels?.Success ?? false)
            {
                var response = ConvertExchangeRates(responseModels);
                MemoryCacheEntryOptions memoryCacheEntryOptions = new MemoryCacheEntryOptions();
                memoryCacheEntryOptions.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(m_CacheExpirationMinutes);

                var expiration = DateTimeOffset.Now.ToUnixTimeSeconds() - responseModels.Timestamp;
                m_Cache.Set(baseCurrency, response, memoryCacheEntryOptions);
                var targetFound = response.Find(m => m.Target == targetCurrency);
                return targetFound != null ? targetFound : new ExchangeRatesResultModel
                {
                    Success = false,
                    ErrorInfo = "Unable to retrieve exchange rates."
                };
            }
            return new ExchangeRatesResultModel
            {
                Success = false,
                ErrorInfo = "Unable to retrieve exchange rates."
            };
        }

        private List<ExchangeRatesResultModel> ConvertExchangeRates(ExchangeRatesModel response)
        {
            var result = new List<ExchangeRatesResultModel>();
            foreach (KeyValuePair<string, decimal> ele1 in response.Rates!)
            {
                result.Add(new ExchangeRatesResultModel
                {
                    Success = response.Success,
                    Base = response.Base!,
                    Target = ele1.Key,
                    Rate = ele1.Value,
                    Timestamp = DateTimeOffset.FromUnixTimeSeconds(response.Timestamp),
                    ErrorInfo = !response.Success! ? response.Error!.Info! : null,
                }
                );
            }

            return result;
        }

    }
}