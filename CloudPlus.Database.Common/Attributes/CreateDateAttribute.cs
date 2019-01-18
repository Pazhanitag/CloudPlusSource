using System;
using System.Data.Entity.Core.Metadata.Edm;
using System.Linq;

namespace CloudPlus.Database.Common.Attributes
{
    public class CreateDateAttribute : Attribute
    {
        public CreateDateAttribute(string column)
        {
            ColumnName = column;
        }

        public string ColumnName { get; set; }

        public static string GetCreateDateColumnName(EdmType type)
        {
            var annotation = type.MetadataProperties
                .SingleOrDefault(p => p.Name.EndsWith("customannotation:CreateDateColumnName"));

            return (string)annotation?.Value;
        }
    }
}