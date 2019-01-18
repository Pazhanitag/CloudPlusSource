namespace CloudPlus.Api.ViewModels.Response.Catalog
{
    public class CustomerProductItemViewModel
    {
        public int Id { get; set; }
        public decimal Cost { get; set; }
        public bool IsAddon { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Identifier { get; set; }
    }
}