using System.Collections.Generic;

namespace CloudPlus.Models.Catalog
{
    public class CustomerProductItemModel
    {
        public int ProductId { get; set; }
        public int ProductItemId { get; set; }
        public string Name { get; set; }
        public decimal Cost { get; set; }
        public bool IsAddon { get; set; }
        public string Description { get; set; }
        public string Identifier { get; set; }
        public IEnumerable<ServiceIdentifier> InCompatibleServices { get; set; }
        public IEnumerable<AddonServiceIdentifier> AddonServices { get; set; }
    }
}