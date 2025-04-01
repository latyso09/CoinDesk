using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Dtos
{
    public class CoinDeskMockDataDto
    {
        public Time? Time { get; set; }
        public string? Disclaimer { get; set; }
        public string? CharName { get; set; }
        public Bpi Bpi { get; set; }
    }

    public class Time
    {
        public string? Updated { get; set; }
        public string? UpdatedISO { get; set; }
        public string? Updateduk { get; set; }
    }

    public class Bpi
    {
        public CurrencyInfoDto? USD { get; set; }
        public CurrencyInfoDto? GBP { get; set; }
        public CurrencyInfoDto? EUR { get; set; }
    }

    public class CurrencyInfoDto
    {
        public string? Code { get; set; }
        public string? Symbol { get; set; }
        public string? Rate { get; set; }
        public string? Description { get; set; }
        public string? Rate_Float { get; set; }
    }
}