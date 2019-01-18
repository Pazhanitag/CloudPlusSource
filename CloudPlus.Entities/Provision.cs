using System;
using CloudPlus.Entities.Catalog;
using CloudPlus.Enums.Provisions;

namespace CloudPlus.Entities
{
	public class Provision : IBaseEntity
	{
		public Provision()
		{
			CreateDate = DateTime.UtcNow;
			UpdateDate = DateTime.UtcNow;
		}		
		public int CompanyId { get; set; }
		public int ProductV2Id { get; set; }
		
		public Company Company { get; set; }
		public Product Product { get; set; }
	    public CompanyProvisioningStatus Status { get; set; }

		public int Id { get; set; }
		public bool IsDeleted { get; set; }
		public DateTime CreateDate { get; set; }
		public DateTime UpdateDate { get; set; }
	}
}
