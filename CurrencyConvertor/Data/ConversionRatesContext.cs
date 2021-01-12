using CurrencyConvertor.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CurrencyConvertor.Data
{
    public class ConversionRatesContext : DbContext
    {
        public ConversionRatesContext(DbContextOptions<ConversionRatesContext> options) : base(options)
        {
        }

        public DbSet<ConversionRate> ConversionRates { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ConversionRate>().ToTable("ConversionRates");
        }
    }
}
