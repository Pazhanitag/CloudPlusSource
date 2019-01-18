using System;
using CloudPlus.Entities.Catalog;

namespace CloudPlus.Entities
{
    public class OrderDetail : IBaseEntity
    {
        public OrderDetail()
        {
            CreateDate = DateTime.UtcNow;
            UpdateDate = DateTime.UtcNow;
        }

        public int? Quantity { get; set; }

        public Order Order { get; set; }

        public Product Product { get; set; }

        public int Id { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
    }
}