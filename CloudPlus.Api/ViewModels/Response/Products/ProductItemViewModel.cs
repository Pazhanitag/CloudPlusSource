namespace CloudPlus.Api.ViewModels.Response.Products
{
	public class ProductItemViewModel
	{
		public int Id { get; set; }
	    public string Identifier { get; set; }
        public string Name { get; set; }
		public string Description { get; set; }
		public int Status { get; set; }
		public bool IsAddon { get; set; }

		public int Quantity { get; set; }
		public decimal Cost { get; set; }
		public decimal RetailPrice { get; set; }
		public decimal ResellPrice { get; set; }
		public bool Msrp { get; set; }
		public bool Available { get; set; }
        public int ProductId { get; set; }
        public int CompanyId { get; set; }
        public int TemplateId { get; set; }
    }
}