using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace api.Dtos
{
    public class CommonDto
    {
        [Required]
        public DateTime CreateDate { get; set; } = DateTime.Now;
        [Required]
        public string CreatedBy { get; set; } = "System";
        public DateTime? ModifyDate { get; set; }
        public string? ModifiedBy { get; set; }
    }
}