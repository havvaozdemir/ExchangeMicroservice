using System.ComponentModel.DataAnnotations;

namespace ExchangeAPI.Controllers.Data;

public class RatesRequest
{
    [Required(AllowEmptyStrings = false)]
    [DisplayFormat(ConvertEmptyStringToNull = false)]
    public string Base { get; set; } = string.Empty;
    [Required(AllowEmptyStrings = false)]
    [DisplayFormat(ConvertEmptyStringToNull = false)]
    public string Target { get; set; } = string.Empty;
}
