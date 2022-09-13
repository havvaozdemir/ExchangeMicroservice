
using System;
using System.Collections.Generic;

namespace ExchangeAPI.Models
{
    public partial class CustomerCurrency
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public string? Base { get; set; }
        public string? Target { get; set; }
        public decimal? Rate { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTimeOffset? Timestamp { get; set; }

    }
}