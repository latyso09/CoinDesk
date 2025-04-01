using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace api.Models
{
    public class Common
    {
        [Required]
         public DateTime CreateDate { get; set; } = DateTime.Now;
         [Required]
         public string CreatedBy { get; set; }
         public DateTime? ModifyDate { get; set; }
         public string? ModifiedBy { get; set; }
    }
}