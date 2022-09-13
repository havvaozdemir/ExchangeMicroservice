using System;

namespace ExchangeAPI.Services.Contracts
{
    public class ExchangeRatesResultModel
    {
        public bool Success { get; set; } = true;
        public string? Base { get; set; } = null;
        public string? Target { get; set; } = null;
        public decimal? Rate { get; set; }
        public DateTimeOffset Timestamp { get; set; }
        public string? ErrorInfo { get; set; } = null;
    }
}