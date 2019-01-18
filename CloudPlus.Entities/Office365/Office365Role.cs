using System;

namespace CloudPlus.Entities.Office365
{
    public class Office365Role : IBaseEntity
    {
        public Office365Role()
        {
            CreateDate = DateTime.UtcNow;
            UpdateDate = DateTime.UtcNow;
        }
        public int Id { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }

        public string Office365Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string DisplayName { get; set; }
        public int Ord { get; set; }
    }
}