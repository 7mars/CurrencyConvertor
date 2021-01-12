using CurrencyConvertor.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CurrencyConvertor.Services
{
    public interface IService
    {
        Task<IList<ConversionRate>> Process();
        string[] ConversionRateNames { get; }
    }
}