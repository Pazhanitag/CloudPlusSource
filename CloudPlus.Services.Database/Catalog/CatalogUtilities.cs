using System;
using CloudPlus.Resources;

namespace CloudPlus.Services.Database.Catalog
{
    public class CatalogUtilities : ICatalogUtilities
    {
        private readonly IConfigurationManager _configurationManager;

        public CatalogUtilities(IConfigurationManager configurationManager)
        {
            _configurationManager = configurationManager;
        }
        public decimal CalculateResellerPrice(decimal retailPrice, decimal resellerPrice)
        {
            var resellerMarkup = Convert.ToDecimal(_configurationManager.GetByKey("DefaultProductMarkupPercentage"));
            return (retailPrice - resellerPrice) * resellerMarkup / 100 + resellerPrice;
        }
    }
}