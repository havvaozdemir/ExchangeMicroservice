using System.Linq;

namespace ExchangeAPI.Services.Contracts
{
    public static class Symbols
    {
        //TODO: we can get currencies from table
        public static string[] ValidSymbols = new string[]
        {
            "TRY",
            "EUR",
            "USD",
            "GBP",
        };


        public static bool IsValid(string symbol)
        {
            return ValidSymbols.Contains(symbol);
        }

    }
}