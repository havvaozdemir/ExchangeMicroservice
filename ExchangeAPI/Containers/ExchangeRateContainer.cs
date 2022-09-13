using Microsoft.EntityFrameworkCore;
using ExchangeAPI.Models;

namespace ExchangeAPI.Containers;

public interface IExchangeRateContainer
{
    Task<bool> Save(CustomerCurrency _customerRate);
}

public class ExchangeRateContainer : IExchangeRateContainer
{
    private readonly Exchange_DBContext m_DBContext;
    public ExchangeRateContainer(Exchange_DBContext dBContext)
    {
        this.m_DBContext = dBContext;
    }

    public async Task<bool> Save(CustomerCurrency customerCurrencyRate)
    {
        this.m_DBContext.CustomerCurrencies.Add(customerCurrencyRate);
        await this.m_DBContext.SaveChangesAsync();
        return true;
    }

}