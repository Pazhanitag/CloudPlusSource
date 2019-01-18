using System;
using CloudPlus.Entities.Catalog;

namespace CloudPlus.Entities
{
    public class Subscription : IBaseEntity
    {
        public Subscription()
        {
            CreateDate = DateTime.UtcNow;
            UpdateDate = DateTime.UtcNow;
        }

        public int Quantity { get; set; }

        public int UserId { get; set; }

        public int Status { get; set; }

        public Product Product { get; set; }

        public int Id { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
    }
}