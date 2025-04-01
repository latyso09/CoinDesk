using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace api.Dtos
{
    public class CurrencyDto
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public required string Code { get; set; }
        [Required]
        public required string Name { get; set; }
    }
}