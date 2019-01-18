using System;
using System.Data.Entity.Core.Metadata.Edm;
using System.Linq;

namespace CloudPlus.Database.Common.Attributes
{
    public class UpdateDateAttribute : Attribute
    {
        public UpdateDateAttribute(string column)
        {
            ColumnName = column;
        }

        public string ColumnName { get; set; }

        public static string GetUpdateDateColumnName(EdmType type)
        {
            var annotation = type.MetadataProperties
                .SingleOrDefault(p => p.Name.EndsWith("customannotation:UpdateDateColumnName"));

            return (string)annotation?.Value;
        }
    }
}