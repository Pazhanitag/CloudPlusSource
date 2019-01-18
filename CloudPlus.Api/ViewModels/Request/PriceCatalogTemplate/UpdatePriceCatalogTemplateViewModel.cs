using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CloudPlus.Api.ViewModels.Request.PriceCatalogTemplate
{
    public class UpdatePriceCatalogTemplateViewModel
    {
        [Required]
        public string Name { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }
        public string Description { get; set; }
        public IEnumerable<int> Companies { get; set; }
    }
}