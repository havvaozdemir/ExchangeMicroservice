using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ExchangeAPI.Services.Handlers
{
    public class ExchangeRatesHttpClient : IHttpClient
    {
        private readonly HttpClient _httpClient;

        public ExchangeRatesHttpClient(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("exchangeRatesApi");
        }

        public async Task<TResponse> GetAsync<TResponse>(string endpointUrl)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, endpointUrl);

                var response = await _httpClient.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    return JsonConvert.DeserializeObject<TResponse>(content)!;
                }
                return default(TResponse)!;
            }
            catch (Exception)
            {
                return default(TResponse)!;
            }
        }
    }
}