using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CurrencyConvertor.Data
{
    public static class DbInitializer
    {
        public static void Initialize(ConversionRatesContext context)
        {
            context.Database.EnsureCreated();
        }
    }
}