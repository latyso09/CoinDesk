using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace api.Dtos
{
    public class UpdateCurrencyRequestDto : CommonDto
    {
        public required string Code { get; set; }
        public required string Name { get; set; }
    }
}