using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace CurrencyConvertor.Services
{
    public class ExchangeRatesAPIService : Base
    {
        private readonly string _baseAddress = "https://api.exchangeratesapi.io";
        private readonly string _params = "/latest?base={0}&symbols={1}";

        protected override async Task<double> GetResource(string conversionRateName)
        {
            var httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(_baseAddress);
            httpClient.Timeout = new TimeSpan(0, 0, 30);

            var symbols = ParseSymbols(conversionRateName);
            var response = await httpClient.GetAsync(string.Format(_params, symbols[0], symbols[1]));

            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            return JObject.Parse(content)["rates"].Value<double>($"{symbols[1]}");
        }
    }
}
