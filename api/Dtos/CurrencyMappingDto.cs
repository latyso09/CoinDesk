using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Dtos
{
    public class CurrencyMappingDto
    {
        public required string Code { get; set; }
        public required string Name { get; set; }
        public required string Rate { get; set; }
        public required string UpdateTime { get; set; }
    }
}