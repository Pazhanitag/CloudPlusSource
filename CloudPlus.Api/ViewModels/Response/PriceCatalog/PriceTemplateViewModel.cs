using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CloudPlus.Api.ViewModels.Response.PriceCatalog
{
	public class PriceTemplateViewModel
	{
		public int Id { get; set; }
		[Required]
		public string Name { get; set; }
		[Required]
		public DateTime ValidFrom { get; set; }
		
		public DateTime? ValidTo { get; set; }

		public List<string> Accounts { get; set; }

		public string Status { get; set; }

		public bool IsGlobal { get; set; }
	}
}