namespace CloudPlus.Database.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure.Annotations;
    using System.Data.Entity.Migrations;
    
    public partial class CustomSecureControlPanel : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CustomSecureControlPanels",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CompanyID = c.Int(nullable: false),
                        CustomSecureControlPanelURL = c.String(),
                        CompanyName = c.String(),
                        Email = c.String(),
                        CompanyAddressStreet = c.String(),
                        CompanyAddressCity = c.String(),
                        CompanyAddressState = c.String(),
                        CompanyAddressZipCode = c.String(),
                        CompanyAddressCountry = c.String(),
                        ContactPerson = c.String(),
                        ContactPhone = c.String(),
                        IsDeleted = c.Boolean(nullable: false),
                        CreateDate = c.DateTime(nullable: false),
                        UpdateDate = c.DateTime(nullable: false),
                        StatusId = c.Int(nullable: false),
                    },
                annotations: new Dictionary<string, object>
                {
                    { "CreateDateColumnName", "CreatedAt" },
                    { "SoftDeleteColumnName", "IsDeleted" },
                    { "UpdateDateColumnName", "UpdatedAt" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CustomSecureControlPanelStatus", t => t.StatusId, cascadeDelete: true)
                .Index(t => t.StatusId);
            
            CreateTable(
                "dbo.CustomSecureControlPanelStatus",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Status = c.String(),
                        StatusIcon = c.String(),
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
            
            AddColumn("dbo.Products", "GroupId", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CustomSecureControlPanels", "StatusId", "dbo.CustomSecureControlPanelStatus");
            DropIndex("dbo.CustomSecureControlPanels", new[] { "StatusId" });
            DropColumn("dbo.Products", "GroupId");
            DropTable("dbo.CustomSecureControlPanelStatus",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "CreateDateColumnName", "CreatedAt" },
                    { "SoftDeleteColumnName", "IsDeleted" },
                    { "UpdateDateColumnName", "UpdatedAt" },
                });
            DropTable("dbo.CustomSecureControlPanels",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "CreateDateColumnName", "CreatedAt" },
                    { "SoftDeleteColumnName", "IsDeleted" },
                    { "UpdateDateColumnName", "UpdatedAt" },
                });
        }
    }
}
