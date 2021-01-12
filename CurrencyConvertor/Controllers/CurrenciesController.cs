using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CurrencyConvertor.Data;
using CurrencyConvertor.Models;
using CurrencyConvertor.Services;
using System.Net.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace CurrencyConvertor.Controllers
{
    public class CurrenciesController : Controller
    {
        private readonly ConversionRatesContext _context; 
        private readonly IConfiguration _config;
        private readonly ILogger _logger;
        private readonly Dictionary<string,Type> dispatch = new Dictionary<string, Type>();
        private readonly int _refreshMinutes = 5;

        public CurrenciesController(ConversionRatesContext context, IConfiguration config, ILogger<CurrenciesController> logger)
        {
            _context = context;
            _config = config;
            _logger = logger;

            dispatch.Add("CurrConv", typeof(CurrConvService));
            dispatch.Add("ExchangeRatesAPI", typeof(ExchangeRatesAPIService));
        }

        public async Task<IActionResult> Index()
        {
            if (!_context.ConversionRates.Any() || NeedsUpdate(_context.ConversionRates.First().LastUpdate))
            {
                await RefreshData();
            }
            return View(await _context.ConversionRates.ToListAsync());
        }

        public async Task<IActionResult> Refresh()
        {
            await RefreshData();
            return RedirectToAction("Index");
        }

        private async Task RefreshData()
        {
            IService service = (IService)Activator.CreateInstance(dispatch[_config["Services:Active"]]);

            try
            {
                await GetCurrentData(service);
            }
            catch (HttpRequestException e)
            {
                _logger.LogError(e.Message);
            }
        }

        private bool NeedsUpdate(DateTime lastUpdate)
        {
            return (DateTime.UtcNow - lastUpdate) > TimeSpan.FromMinutes(_refreshMinutes);
        }

        private async Task GetCurrentData(IService service)
        {
            var data = await service.Process();
            if (data.Count == service.ConversionRateNames.Length)
            {
                _context.RemoveRange(_context.ConversionRates);
                await _context.AddRangeAsync(data);
                await _context.SaveChangesAsync();
            }
        }
    }
}
