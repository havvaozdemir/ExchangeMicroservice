using Microsoft.AspNetCore.Mvc;
using ExchangeAPI.Models;
using ExchangeAPI.Controllers.Data;
using ExchangeAPI.Services;
using ExchangeAPI.Containers;
using ExchangeAPI.Services.Contracts;

namespace ExchangeAPI.Controllers;

[ApiController]
[Route("/exchange")]
public class ExchangeController : ControllerBase
{
    private readonly ILogger<ExchangeController> m_Logger;
    private readonly IExchangeRateContainer m_DBContext;
    private readonly IExchangeRatesService m_ExchangeRatesService;

    public ExchangeController(ILogger<ExchangeController> logger, IExchangeRateContainer dbContext, IExchangeRatesService exchangeRatesService)
    {
        m_Logger = logger;
        m_DBContext = dbContext;
        m_ExchangeRatesService = exchangeRatesService;
    }

    [HttpPost("customers/{id}/currency/convert")]
    public async Task<ActionResult<RatesResponse>> Post(int id, [FromBody] RatesRequest request)
    {
        var result = await m_ExchangeRatesService.GetRatesAsync(request.Base, request.Target);
        if (result.Success)
        {
            await this.m_DBContext.Save(convertToModel(result, id));
        }
        return Ok(convertToResponse(result));
    }

    private RatesResponse convertToResponse(ExchangeRatesResultModel model)
    {
        if (model.Success)
        {
            return new RatesResponse
            {
                BaseCurrency = model.Base!,
                TargetCurrency = model.Target!,
                ExchangeRate = model.Rate ?? model.Rate!.Value!,
                Timestamp = model.Timestamp,
            };
        }
        else
        {
            return new RatesResponse
            {
                Success = model.Success,
                Info = model.ErrorInfo!
            };
        }
    }
    private CustomerCurrency convertToModel(ExchangeRatesResultModel model, int customerId)
    {
        return new CustomerCurrency
        {
            CustomerId = customerId,
            Base = model.Base!,
            Target = model.Target!,
            Rate = model.Rate ?? model.Rate!.Value!,
            Timestamp = model.Timestamp,
            CreateDate = DateTime.Now
        };
    }
}
