using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CurrencyConvertor.Models
{
    public class ConversionRate
    {
        [MaxLength(7)]
        [Required]
        [Key]
        public string Name { get; set; }
        public double Value { get; set; }
        public DateTime LastUpdate { get; set; }
    }
}
