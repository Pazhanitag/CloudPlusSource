namespace CloudPlus.Database.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure.Annotations;
    using System.Data.Entity.Migrations;
    
    public partial class AddVendorMetricsAdminTables : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.VendorMetrics",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                        DashboardChartType = c.Int(nullable: false),
                        DetailsChartType = c.Int(nullable: false),
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
            
            CreateTable(
                "dbo.VendorMetricsAdminConfigs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CompanyId = c.Int(nullable: false),
                        MetricsId = c.Int(nullable: false),
                        CanAccess = c.Boolean(nullable: false),
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
            
            CreateTable(
                "dbo.VendorMetricsChartTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Type = c.String(),
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
            
            CreateTable(
                "dbo.VendorMetricsUserConfigs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        MetricsId = c.Int(nullable: false),
                        CanAccess = c.Boolean(nullable: false),
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
            DropTable("dbo.VendorMetricsUserConfigs",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "CreateDateColumnName", "CreatedAt" },
                    { "SoftDeleteColumnName", "IsDeleted" },
                    { "UpdateDateColumnName", "UpdatedAt" },
                });
            DropTable("dbo.VendorMetricsChartTypes",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "CreateDateColumnName", "CreatedAt" },
                    { "SoftDeleteColumnName", "IsDeleted" },
                    { "UpdateDateColumnName", "UpdatedAt" },
                });
            DropTable("dbo.VendorMetricsAdminConfigs",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "CreateDateColumnName", "CreatedAt" },
                    { "SoftDeleteColumnName", "IsDeleted" },
                    { "UpdateDateColumnName", "UpdatedAt" },
                });
            DropTable("dbo.VendorMetrics",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "CreateDateColumnName", "CreatedAt" },
                    { "SoftDeleteColumnName", "IsDeleted" },
                    { "UpdateDateColumnName", "UpdatedAt" },
                });
        }
    }
}
