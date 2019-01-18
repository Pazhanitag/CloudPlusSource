using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CloudPlus.Services.Database.Product
{
    public interface IProductService
    {
        Task<ProductModel> GetProduct(int productId);
    }

    public class ProductItemModel
    {
        public int Id { get; set; }
        public string Identifier { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }

        //Active/Hidden/Pending approval
        public int Status { get; set; }

        //Onetime or Recurring/License
        public int BillingType { get; set; }
        public int BillingCycle { get; set; }
        public bool IsAddon { get; set; }
        public bool ProductSuppressible { get; set; }
        public string KnowledgebaseLink { get; set; }
        public string VideoLink { get; set; }
        public decimal DefaultMarkupPercentage { get; set; }

        //API integration or Form-to-email
        public int IntegrationType { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
    }

    public class ProductModel
    {
        
        public int Id { get; set; }
        public string Name { get; set; }
        public int Ord { get; set; }
        public string Description { get; set; }
        public string Vendor { get; set; }
        public string ImgUrl { get; set; }
        public IEnumerable<ProductItemModel> ProductItems { get; set; }
    }
    
}