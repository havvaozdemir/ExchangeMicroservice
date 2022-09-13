using System;
using System.Collections.Generic;

namespace ExchangeAPI.Services.Contracts
{
    public class ExchangeRatesModel
    {
        public bool Success { get; set; }
        public long Timestamp { get; set; }
        public string? Base { get; set; }
        public DateTime Date { get; set; }
        public IDictionary<string, decimal>? Rates { get; set; }
        public ErrorModel? Error { get; set; }
    }

    public class ErrorModel
    {
        public int? Code { get; set; }
        public string? Info { get; set; }
    }
}

