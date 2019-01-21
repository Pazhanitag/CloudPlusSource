using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CloudPlus.Entities.Catalog
{
	public class ProductItem : IBaseEntity
	{
		public ProductItem()
		{
			CreateDate = DateTime.UtcNow;
			UpdateDate = DateTime.UtcNow;
            

        }

		public int Id { get; set; }
		public string Identifier { get; set; }

		[Required]
		public string Name { get; set; }
		public string Description { get; set; }

		//Active/Hidden/Pending approval
		public int Status { get; set; }

		//Onetime or Recurring/License
		public int BillingType { get; set; }
		public int BillingCycle { get; set; }
		public bool IsAddon { get; set; }
		public Product Product { get; set; }
		public bool ProductSuppressible { get; set; }
		public string KnowledgebaseLink { get; set; }
		public string VideoLink { get; set; }
		public decimal DefaultMarkupPercentage { get; set; }
	    public int Ord { get; set; }

		//API integration or Form-to-email
		public int IntegrationType { get; set; }
		public bool IsDeleted { get; set; }
		public DateTime CreateDate { get; set; }
		public DateTime UpdateDate { get; set; }
        
    }
}
