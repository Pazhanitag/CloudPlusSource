using System;

namespace CloudPlus.Entities
{
    public class Order : IBaseEntity
    {
        public Order()
        {
            CreateDate = DateTime.UtcNow;
            UpdateDate = DateTime.UtcNow;
        }

        public int UserId { get; set; }

        public int? Status { get; set; }

        public int Id { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
    }
}