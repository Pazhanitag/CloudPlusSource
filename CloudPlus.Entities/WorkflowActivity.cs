using System;
using System.ComponentModel.DataAnnotations;

namespace CloudPlus.Entities
{
    public class WorkflowActivity : IBaseEntity
    {
        public WorkflowActivity()
        {
            CreateDate = DateTime.UtcNow;
            UpdateDate = DateTime.UtcNow;
        }

        public string UniqueId { get; set; }

        [MaxLength]
        public string Context { get; set; }

        public int Id { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
    }
}