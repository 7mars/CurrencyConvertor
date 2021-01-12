using CurrencyConvertor.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace CurrencyConvertor.Services
{
    public class CurrConvService : Base
    {
        private readonly string _baseAddress = "https://free.currconv.com";
        private readonly string _params = "/api/v7/convert?q={0}_{1}&compact=ultra&apiKey=d6005b092d3dc83a1a31";

        protected override async Task<double> GetResource(string conversionRateName)
        {
            var httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(_baseAddress);
            httpClient.Timeout = new TimeSpan(0, 0, 30);

            var symbols = ParseSymbols(conversionRateName);
            var response = await httpClient.GetAsync(string.Format(_params, symbols[0], symbols[1]));

            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            return JObject.Parse(content).Value<double>($"{symbols[0]}_{symbols[1]}");
        }
    }
}
