using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Jewerly.Domain;

namespace Jewerly.Web.Models
{
    public class CurrencyModel

    {
        public CurrencyModel(List<Currency> currencies, int currentCurrencyId)
        {
            Currencies = currencies;
            CurrentCurrencyId = currentCurrencyId;
            _currentCurrency = Currencies.SingleOrDefault(t => t.CurrencyId == CurrentCurrencyId);
        }

        public List<Currency> Currencies { get; set; }
        public int CurrentCurrencyId { get; set; }
        private readonly Currency _currentCurrency;
        public Currency CurrentCurrency
        {
            get { return _currentCurrency; } 
        }

        public SelectList GetSelectList()
        {
            return new SelectList(Currencies, "Name", "CurrencyId", _currentCurrency.CurrencyId);
        }

    }
}