namespace CloudPlus.Database.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure.Annotations;
    using System.Data.Entity.Migrations;
    
    public partial class AddMetricsReport : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.VendorMetricsReportConfigs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CompanyId = c.Int(nullable: false),
                        UserId = c.Int(nullable: false),
                        ReportName = c.String(),
                        ReportPeriod = c.Int(nullable: false),
                        Widgets = c.String(),
                        ReportFrequency = c.Int(nullable: false),
                        Runtime = c.DateTime(nullable: false),
                        DayFrequency = c.Int(),
                        WeekFrequency = c.Int(),
                        MonthFrequency = c.Int(),
                        EmailList = c.String(),
                        LastRunTime = c.DateTime(),
                        LatRunStatus = c.String(),
                        NextRunTime = c.DateTime(),
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
            DropTable("dbo.VendorMetricsReportConfigs",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "CreateDateColumnName", "CreatedAt" },
                    { "SoftDeleteColumnName", "IsDeleted" },
                    { "UpdateDateColumnName", "UpdatedAt" },
                });
        }
    }
}
