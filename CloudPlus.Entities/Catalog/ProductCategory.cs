using System;
using System.ComponentModel.DataAnnotations;

namespace CloudPlus.Entities.Catalog
{
	public class ProductCategory : IBaseEntity
	{
		public ProductCategory()
		{
			CreateDate = DateTime.UtcNow;
			UpdateDate = DateTime.UtcNow;
		}

		[Required]
		public string Name { get; set; }

		public int Id { get; set; }
		public bool IsDeleted { get; set; }
		public DateTime CreateDate { get; set; }
		public DateTime UpdateDate { get; set; }
	}
}
