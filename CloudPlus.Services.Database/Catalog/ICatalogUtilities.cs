namespace CloudPlus.Services.Database.Catalog
{
    public interface ICatalogUtilities
    {
        decimal CalculateResellerPrice(decimal retailPrice, decimal resellerPrice);
    }
}