namespace CloudPlus.Database.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure.Annotations;
    using System.Data.Entity.Migrations;
    
    public partial class AddVendorMetricsConfiguration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.VendorMetricsConfigurations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CustomerId = c.Int(nullable: false),
                        ShowActiveUsers = c.Boolean(nullable: false),
                        ShowEmailActivity = c.Boolean(nullable: false),
                        ShowOneDriveStorage = c.Boolean(nullable: false),
                        ShowSharePointActivity = c.Boolean(nullable: false),
                        ShowSkypeForBusinessActivity = c.Boolean(nullable: false),
                        ShowOfficeActivations = c.Boolean(nullable: false),
                        ShowTeamsActivity = c.Boolean(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        CreateDate = c.DateTime(nullable: false),
                        UpdateDate = c.DateTime(nullable: false),
                    },
                annotations: new Dictionary<string, object>
                {
                    { "CreateDateColumnName", "CreatedAt" },
                    { "SoftDeleteColumnName", "IsDeleted" },
                    { "UpdateDateColumnName", "UpdatedAt" },
                })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.VendorMetricsConfigurations",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "CreateDateColumnName", "CreatedAt" },
                    { "SoftDeleteColumnName", "IsDeleted" },
                    { "UpdateDateColumnName", "UpdatedAt" },
                });
        }
    }
}
