using System;
using System.ComponentModel.DataAnnotations;
using ExchangeAPI.Services.Contracts;

namespace ExchangeAPI.Controllers.Data;

public class RatesResponse
{
    public bool Success { get; set; } = true;
    public string? Info { get; set; } = null;
    public string BaseCurrency { get; set; } = string.Empty;
    public string TargetCurrency { get; set; } = string.Empty;
    [DisplayFormat(DataFormatString = "{0:#.#####")]
    public decimal ExchangeRate { get; set; }
    public DateTimeOffset Timestamp { get; set; }
}
