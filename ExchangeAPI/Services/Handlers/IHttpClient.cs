using System.Threading.Tasks;

namespace ExchangeAPI.Services.Handlers
{
    public interface IHttpClient
    {
        Task<TResponse> GetAsync<TResponse>(string endpointUrl);
    }
}