using System;
using CloudPlus.Database.Common.Attributes;
namespace CloudPlus.Entities.Identity
{
    /// <summary>
    /// Base interface for identity domain objects
    /// </summary>
    [SoftDelete("IsDeleted")]
    [CreateDate("CreatedAt")]
    [UpdateDate("UpdatedAt")]
    public interface IBaseEntity
    {
        int Id { get; set; }
        bool IsDeleted { get; set; }
        DateTime CreatedAt { get; set; }
        DateTime UpdatedAt { get; set; }
    }
}
