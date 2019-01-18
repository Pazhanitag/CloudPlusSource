using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CloudPlus.Entities.Catalog
{
	public class Product : IBaseEntity
	{
		public Product()
		{
			CreateDate = DateTime.UtcNow;
			UpdateDate = DateTime.UtcNow;
			ProductItems = new List<ProductItem>();
		}

		[Required]
		public string Name { get; set; }

		public int Ord { get; set; }
		public ProductCategory Category { get; set; }
		public List<ProductItem> ProductItems { get; set; }
        public string Description { get; set; }
        public string Vendor { get; set; }
		public string ImgUrl { get; set; }

		public int Id { get; set; }
		public bool IsDeleted { get; set; }
		public DateTime CreateDate { get; set; }
		public DateTime UpdateDate { get; set; }
        public int GroupId { get; set; }
    }
}
