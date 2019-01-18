using System;
using CloudPlus.Database.Common.Attributes;

namespace CloudPlus.Entities
{
    [SoftDelete("IsDeleted")]
    [CreateDate("CreatedAt")]
    [UpdateDate("UpdatedAt")]
    public interface IBaseEntity
    {
        int Id { get; set; }
        bool IsDeleted { get; set; }
        DateTime CreateDate { get; set; }
        DateTime UpdateDate { get; set; }
    }
}