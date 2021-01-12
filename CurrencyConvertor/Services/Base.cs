using CurrencyConvertor.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace CurrencyConvertor.Services
{
    public abstract class Base : IService
    {
        protected string[] ParseSymbols(string symbols)
        {
            return symbols.Split('/');
        }

        public string[] ConversionRateNames => new[] { "USD/ILS", "GBP/EUR", "EUR/JPY", "EUR/USD" };
        protected abstract Task<double> GetResource(string conversionRateName);

        public async  Task<IList<ConversionRate>> Process()
        {
            var data = new List<ConversionRate>();

            foreach (var name in ConversionRateNames)
            {
                var response = await GetResource(name);
                data.Add(new ConversionRate
                {
                    LastUpdate = DateTime.UtcNow,
                    Name = name,
                    Value = response
                });
            }

            return data;
        }
    }
}
