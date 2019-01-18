using CloudPlus.Database.Utilities;

namespace CloudPlus.Database.Migrations
{
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration, IMigrationSeed<CldpDbContext>
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AnnouncementCompanies",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IsDeleted = c.Boolean(nullable: false),
                        CreateDate = c.DateTime(nullable: false),
                        UpdateDate = c.DateTime(nullable: false),
                        Announcement_Id = c.Int(),
                        Company_Id = c.Int(),
                    },
                annotations: new Dictionary<string, object>
                {
                    { "CreateDateColumnName", "CreatedAt" },
                    { "SoftDeleteColumnName", "IsDeleted" },
                    { "UpdateDateColumnName", "UpdatedAt" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Announcements", t => t.Announcement_Id)
                .ForeignKey("dbo.Companies", t => t.Company_Id)
                .Index(t => t.Announcement_Id)
                .Index(t => t.Company_Id);
            
            CreateTable(
                "dbo.Announcements",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false),
                        ShortDescription = c.String(nullable: false),
                        Text = c.String(nullable: false),
                        PublishDate = c.DateTime(),
                        FinishDate = c.DateTime(),
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
                "dbo.Companies",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        CompanyOu = c.Int(nullable: false),
                        UniqueIdentifier = c.String(),
                        Type = c.Int(nullable: false),
                        ParentId = c.Int(),
                        Website = c.String(),
                        LogoUrl = c.String(),
                        SupportSiteUrl = c.String(),
                        ControlPanelSiteUrl = c.String(),
                        Email = c.String(),
                        PhoneNumber = c.String(),
                        StreetAddress = c.String(),
                        City = c.String(),
                        ZipCode = c.String(),
                        State = c.String(),
                        Country = c.String(),
                        BrandColorPrimary = c.String(),
                        BrandColorSecondary = c.String(),
                        BrandColorText = c.String(),
                        Status = c.Int(nullable: false),
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
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Companies", t => t.ParentId)
                .Index(t => t.ParentId);
            
            CreateTable(
                "dbo.CompanyCatalogs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IsDeleted = c.Boolean(nullable: false),
                        CreateDate = c.DateTime(nullable: false),
                        UpdateDate = c.DateTime(nullable: false),
                        CompanyId = c.Int(nullable: false),
                        CatalogId = c.Int(nullable: false),
                        CatalogType = c.Int(nullable: false),
                        Default = c.Boolean(nullable: false),
                    },
                annotations: new Dictionary<string, object>
                {
                    { "CreateDateColumnName", "CreatedAt" },
                    { "SoftDeleteColumnName", "IsDeleted" },
                    { "UpdateDateColumnName", "UpdatedAt" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Catalogs", t => t.CatalogId, cascadeDelete: true)
                .ForeignKey("dbo.Companies", t => t.CompanyId, cascadeDelete: true)
                .Index(t => t.CompanyId)
                .Index(t => t.CatalogId);
            
            CreateTable(
                "dbo.Catalogs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IsDeleted = c.Boolean(nullable: false),
                        CreateDate = c.DateTime(nullable: false),
                        UpdateDate = c.DateTime(nullable: false),
                        Name = c.String(),
                        Description = c.String(),
                    },
                annotations: new Dictionary<string, object>
                {
                    { "CreateDateColumnName", "CreatedAt" },
                    { "SoftDeleteColumnName", "IsDeleted" },
                    { "UpdateDateColumnName", "UpdatedAt" },
                })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.CatalogProductItems",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IsDeleted = c.Boolean(nullable: false),
                        CreateDate = c.DateTime(nullable: false),
                        UpdateDate = c.DateTime(nullable: false),
                        CatalogId = c.Int(nullable: false),
                        ProductItemId = c.Int(nullable: false),
                        Available = c.Boolean(nullable: false),
                        ResellerPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        RetailPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        FixedRetailPrice = c.Boolean(nullable: false),
                    },
                annotations: new Dictionary<string, object>
                {
                    { "CreateDateColumnName", "CreatedAt" },
                    { "SoftDeleteColumnName", "IsDeleted" },
                    { "UpdateDateColumnName", "UpdatedAt" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Catalogs", t => t.CatalogId, cascadeDelete: true)
                .ForeignKey("dbo.ProductItems", t => t.ProductItemId, cascadeDelete: true)
                .Index(t => t.CatalogId)
                .Index(t => t.ProductItemId);
            
            CreateTable(
                "dbo.ProductItems",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Identifier = c.String(),
                        Name = c.String(nullable: false),
                        Description = c.String(),
                        Status = c.Int(nullable: false),
                        BillingType = c.Int(nullable: false),
                        BillingCycle = c.Int(nullable: false),
                        IsAddon = c.Boolean(nullable: false),
                        ProductSuppressible = c.Boolean(nullable: false),
                        KnowledgebaseLink = c.String(),
                        VideoLink = c.String(),
                        DefaultMarkupPercentage = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Ord = c.Int(nullable: false),
                        IntegrationType = c.Int(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        CreateDate = c.DateTime(nullable: false),
                        UpdateDate = c.DateTime(nullable: false),
                        Product_Id = c.Int(),
                    },
                annotations: new Dictionary<string, object>
                {
                    { "CreateDateColumnName", "CreatedAt" },
                    { "SoftDeleteColumnName", "IsDeleted" },
                    { "UpdateDateColumnName", "UpdatedAt" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Products", t => t.Product_Id)
                .Index(t => t.Product_Id);
            
            CreateTable(
                "dbo.Products",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Ord = c.Int(nullable: false),
                        Description = c.String(),
                        Vendor = c.String(),
                        ImgUrl = c.String(),
                        IsDeleted = c.Boolean(nullable: false),
                        CreateDate = c.DateTime(nullable: false),
                        UpdateDate = c.DateTime(nullable: false),
                        Category_Id = c.Int(),
                    },
                annotations: new Dictionary<string, object>
                {
                    { "CreateDateColumnName", "CreatedAt" },
                    { "SoftDeleteColumnName", "IsDeleted" },
                    { "UpdateDateColumnName", "UpdatedAt" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ProductCategories", t => t.Category_Id)
                .Index(t => t.Category_Id);
            
            CreateTable(
                "dbo.ProductCategories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
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
                "dbo.Domains",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        IsPrimary = c.Boolean(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        CreateDate = c.DateTime(nullable: false),
                        UpdateDate = c.DateTime(nullable: false),
                        Company_Id = c.Int(nullable: false),
                    },
                annotations: new Dictionary<string, object>
                {
                    { "CreateDateColumnName", "CreatedAt" },
                    { "SoftDeleteColumnName", "IsDeleted" },
                    { "UpdateDateColumnName", "UpdatedAt" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Companies", t => t.Company_Id, cascadeDelete: true)
                .Index(t => t.Company_Id);
            
            CreateTable(
                "dbo.Categories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Ord = c.Int(nullable: false),
                        Vendor = c.String(),
                        ImgUrl = c.String(),
                        IsDeleted = c.Boolean(nullable: false),
                        CreateDate = c.DateTime(nullable: false),
                        UpdateDate = c.DateTime(nullable: false),
                        Parent_Id = c.Int(),
                    },
                annotations: new Dictionary<string, object>
                {
                    { "CreateDateColumnName", "CreatedAt" },
                    { "SoftDeleteColumnName", "IsDeleted" },
                    { "UpdateDateColumnName", "UpdatedAt" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Categories", t => t.Parent_Id)
                .Index(t => t.Parent_Id);
            
            CreateTable(
                "dbo.CompanyProductExclusions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ProductId = c.Int(nullable: false),
                        CompanyId = c.Int(nullable: false),
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
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Companies", t => t.CompanyId, cascadeDelete: true)
                .ForeignKey("dbo.Products", t => t.ProductId, cascadeDelete: true)
                .Index(t => t.ProductId)
                .Index(t => t.CompanyId);
            
            CreateTable(
                "dbo.EmailTemplates",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Type = c.String(nullable: false),
                        Subject = c.String(nullable: false),
                        Template = c.String(nullable: false),
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
                "dbo.Office365Customer",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IsDeleted = c.Boolean(nullable: false),
                        CreateDate = c.DateTime(nullable: false),
                        UpdateDate = c.DateTime(nullable: false),
                        Office365Id = c.String(),
                        CompanyId = c.Int(nullable: false),
                    },
                annotations: new Dictionary<string, object>
                {
                    { "CreateDateColumnName", "CreatedAt" },
                    { "SoftDeleteColumnName", "IsDeleted" },
                    { "UpdateDateColumnName", "UpdatedAt" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Companies", t => t.CompanyId, cascadeDelete: true)
                .Index(t => t.CompanyId);
            
            CreateTable(
                "dbo.Office365Domain",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IsDeleted = c.Boolean(nullable: false),
                        CreateDate = c.DateTime(nullable: false),
                        UpdateDate = c.DateTime(nullable: false),
                        DomainName = c.String(),
                        IsFederated = c.Boolean(nullable: false),
                        Office365DomainStaus = c.Int(nullable: false),
                        Office365Customer_Id = c.Int(),
                    },
                annotations: new Dictionary<string, object>
                {
                    { "CreateDateColumnName", "CreatedAt" },
                    { "SoftDeleteColumnName", "IsDeleted" },
                    { "UpdateDateColumnName", "UpdatedAt" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Office365Customer", t => t.Office365Customer_Id)
                .Index(t => t.Office365Customer_Id);
            
            CreateTable(
                "dbo.Office365Subscription",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IsDeleted = c.Boolean(nullable: false),
                        CreateDate = c.DateTime(nullable: false),
                        UpdateDate = c.DateTime(nullable: false),
                        Office365SubscriptionId = c.String(),
                        Office365OrderId = c.String(),
                        Office365FriendlyName = c.String(),
                        Quantity = c.Int(nullable: false),
                        SubscriptionState = c.Int(nullable: false),
                        Suspended = c.Boolean(nullable: false),
                        Office365Customer_Id = c.Int(),
                        Office365Offer_Id = c.Int(),
                    },
                annotations: new Dictionary<string, object>
                {
                    { "CreateDateColumnName", "CreatedAt" },
                    { "SoftDeleteColumnName", "IsDeleted" },
                    { "UpdateDateColumnName", "UpdatedAt" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Office365Customer", t => t.Office365Customer_Id)
                .ForeignKey("dbo.Office365Offer", t => t.Office365Offer_Id)
                .Index(t => t.Office365Customer_Id)
                .Index(t => t.Office365Offer_Id);
            
            CreateTable(
                "dbo.Office365Offer",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IsDeleted = c.Boolean(nullable: false),
                        CreateDate = c.DateTime(nullable: false),
                        UpdateDate = c.DateTime(nullable: false),
                        Office365OfferId = c.String(),
                        Office365OfferName = c.String(),
                        Office365ProductSku = c.String(),
                        CloudPlusProductIdentifier = c.String(),
                    },
                annotations: new Dictionary<string, object>
                {
                    { "CreateDateColumnName", "CreatedAt" },
                    { "SoftDeleteColumnName", "IsDeleted" },
                    { "UpdateDateColumnName", "UpdatedAt" },
                })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Office365User",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IsDeleted = c.Boolean(nullable: false),
                        CreateDate = c.DateTime(nullable: false),
                        UpdateDate = c.DateTime(nullable: false),
                        Office365UserId = c.String(),
                        CloudPlusUserId = c.Int(nullable: false),
                        UserPrincipalName = c.String(),
                        CustomerId = c.Int(nullable: false),
                        Office365UserState = c.Int(nullable: false),
                        Office365SoftDeletionTime = c.DateTime(),
                    },
                annotations: new Dictionary<string, object>
                {
                    { "CreateDateColumnName", "CreatedAt" },
                    { "SoftDeleteColumnName", "IsDeleted" },
                    { "UpdateDateColumnName", "UpdatedAt" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Office365Customer", t => t.CustomerId, cascadeDelete: true)
                .Index(t => t.CustomerId);
            
            CreateTable(
                "dbo.Office365License",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IsDeleted = c.Boolean(nullable: false),
                        CreateDate = c.DateTime(nullable: false),
                        UpdateDate = c.DateTime(nullable: false),
                        Office365Offer_Id = c.Int(),
                        Office365User_Id = c.Int(),
                    },
                annotations: new Dictionary<string, object>
                {
                    { "CreateDateColumnName", "CreatedAt" },
                    { "SoftDeleteColumnName", "IsDeleted" },
                    { "UpdateDateColumnName", "UpdatedAt" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Office365Offer", t => t.Office365Offer_Id)
                .ForeignKey("dbo.Office365User", t => t.Office365User_Id)
                .Index(t => t.Office365Offer_Id)
                .Index(t => t.Office365User_Id);
            
            CreateTable(
                "dbo.Office365Role",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IsDeleted = c.Boolean(nullable: false),
                        CreateDate = c.DateTime(nullable: false),
                        UpdateDate = c.DateTime(nullable: false),
                        Office365Id = c.String(),
                        Name = c.String(),
                        Description = c.String(),
                    },
                annotations: new Dictionary<string, object>
                {
                    { "CreateDateColumnName", "CreatedAt" },
                    { "SoftDeleteColumnName", "IsDeleted" },
                    { "UpdateDateColumnName", "UpdatedAt" },
                })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.OrderDetails",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Quantity = c.Int(),
                        IsDeleted = c.Boolean(nullable: false),
                        CreateDate = c.DateTime(nullable: false),
                        UpdateDate = c.DateTime(nullable: false),
                        Order_Id = c.Int(),
                        Product_Id = c.Int(),
                    },
                annotations: new Dictionary<string, object>
                {
                    { "CreateDateColumnName", "CreatedAt" },
                    { "SoftDeleteColumnName", "IsDeleted" },
                    { "UpdateDateColumnName", "UpdatedAt" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Orders", t => t.Order_Id)
                .ForeignKey("dbo.Products", t => t.Product_Id)
                .Index(t => t.Order_Id)
                .Index(t => t.Product_Id);
            
            CreateTable(
                "dbo.Orders",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        Status = c.Int(),
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
                "dbo.ProductConstraints",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Constraint = c.String(),
                        IsDeleted = c.Boolean(nullable: false),
                        CreateDate = c.DateTime(nullable: false),
                        UpdateDate = c.DateTime(nullable: false),
                        Product_Id = c.Int(),
                    },
                annotations: new Dictionary<string, object>
                {
                    { "CreateDateColumnName", "CreatedAt" },
                    { "SoftDeleteColumnName", "IsDeleted" },
                    { "UpdateDateColumnName", "UpdatedAt" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Products", t => t.Product_Id)
                .Index(t => t.Product_Id);
            
            CreateTable(
                "dbo.Provisions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CompanyId = c.Int(nullable: false),
                        ProductV2Id = c.Int(nullable: false),
                        Status = c.Int(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        CreateDate = c.DateTime(nullable: false),
                        UpdateDate = c.DateTime(nullable: false),
                        Product_Id = c.Int(),
                    },
                annotations: new Dictionary<string, object>
                {
                    { "CreateDateColumnName", "CreatedAt" },
                    { "SoftDeleteColumnName", "IsDeleted" },
                    { "UpdateDateColumnName", "UpdatedAt" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Companies", t => t.CompanyId, cascadeDelete: true)
                .ForeignKey("dbo.Products", t => t.Product_Id)
                .Index(t => t.CompanyId)
                .Index(t => t.Product_Id);
            
            CreateTable(
                "dbo.PublisherAnnouncments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AnnouncementId = c.Int(nullable: false),
                        PublisherId = c.Int(nullable: false),
                        AllowSupression = c.Boolean(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        CreateDate = c.DateTime(nullable: false),
                        UpdateDate = c.DateTime(nullable: false),
                        Company_Id = c.Int(),
                    },
                annotations: new Dictionary<string, object>
                {
                    { "CreateDateColumnName", "CreatedAt" },
                    { "SoftDeleteColumnName", "IsDeleted" },
                    { "UpdateDateColumnName", "UpdatedAt" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Announcements", t => t.AnnouncementId, cascadeDelete: true)
                .ForeignKey("dbo.Companies", t => t.Company_Id)
                .Index(t => t.AnnouncementId)
                .Index(t => t.Company_Id);
            
            CreateTable(
                "dbo.SubscriptionAudits",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        OldQuantity = c.Int(nullable: false),
                        NewQuantity = c.Int(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        CreateDate = c.DateTime(nullable: false),
                        UpdateDate = c.DateTime(nullable: false),
                        Subscription_Id = c.Int(),
                    },
                annotations: new Dictionary<string, object>
                {
                    { "CreateDateColumnName", "CreatedAt" },
                    { "SoftDeleteColumnName", "IsDeleted" },
                    { "UpdateDateColumnName", "UpdatedAt" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Subscriptions", t => t.Subscription_Id)
                .Index(t => t.Subscription_Id);
            
            CreateTable(
                "dbo.Subscriptions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Quantity = c.Int(nullable: false),
                        UserId = c.Int(nullable: false),
                        Status = c.Int(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        CreateDate = c.DateTime(nullable: false),
                        UpdateDate = c.DateTime(nullable: false),
                        Product_Id = c.Int(),
                    },
                annotations: new Dictionary<string, object>
                {
                    { "CreateDateColumnName", "CreatedAt" },
                    { "SoftDeleteColumnName", "IsDeleted" },
                    { "UpdateDateColumnName", "UpdatedAt" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Products", t => t.Product_Id)
                .Index(t => t.Product_Id);
            
            CreateTable(
                "dbo.SubscriptionContracts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        StartDate = c.DateTime(nullable: false),
                        EndDate = c.DateTime(nullable: false),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Status = c.Int(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        CreateDate = c.DateTime(nullable: false),
                        UpdateDate = c.DateTime(nullable: false),
                        Subscription_Id = c.Int(),
                    },
                annotations: new Dictionary<string, object>
                {
                    { "CreateDateColumnName", "CreatedAt" },
                    { "SoftDeleteColumnName", "IsDeleted" },
                    { "UpdateDateColumnName", "UpdatedAt" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Subscriptions", t => t.Subscription_Id)
                .Index(t => t.Subscription_Id);
            
            CreateTable(
                "dbo.WorkflowActivities",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UniqueId = c.String(),
                        Context = c.String(),
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
            DropForeignKey("dbo.SubscriptionContracts", "Subscription_Id", "dbo.Subscriptions");
            DropForeignKey("dbo.SubscriptionAudits", "Subscription_Id", "dbo.Subscriptions");
            DropForeignKey("dbo.Subscriptions", "Product_Id", "dbo.Products");
            DropForeignKey("dbo.PublisherAnnouncments", "Company_Id", "dbo.Companies");
            DropForeignKey("dbo.PublisherAnnouncments", "AnnouncementId", "dbo.Announcements");
            DropForeignKey("dbo.Provisions", "Product_Id", "dbo.Products");
            DropForeignKey("dbo.Provisions", "CompanyId", "dbo.Companies");
            DropForeignKey("dbo.ProductConstraints", "Product_Id", "dbo.Products");
            DropForeignKey("dbo.OrderDetails", "Product_Id", "dbo.Products");
            DropForeignKey("dbo.OrderDetails", "Order_Id", "dbo.Orders");
            DropForeignKey("dbo.Office365License", "Office365User_Id", "dbo.Office365User");
            DropForeignKey("dbo.Office365License", "Office365Offer_Id", "dbo.Office365Offer");
            DropForeignKey("dbo.Office365User", "CustomerId", "dbo.Office365Customer");
            DropForeignKey("dbo.Office365Subscription", "Office365Offer_Id", "dbo.Office365Offer");
            DropForeignKey("dbo.Office365Subscription", "Office365Customer_Id", "dbo.Office365Customer");
            DropForeignKey("dbo.Office365Domain", "Office365Customer_Id", "dbo.Office365Customer");
            DropForeignKey("dbo.Office365Customer", "CompanyId", "dbo.Companies");
            DropForeignKey("dbo.CompanyProductExclusions", "ProductId", "dbo.Products");
            DropForeignKey("dbo.CompanyProductExclusions", "CompanyId", "dbo.Companies");
            DropForeignKey("dbo.Categories", "Parent_Id", "dbo.Categories");
            DropForeignKey("dbo.AnnouncementCompanies", "Company_Id", "dbo.Companies");
            DropForeignKey("dbo.Companies", "ParentId", "dbo.Companies");
            DropForeignKey("dbo.Domains", "Company_Id", "dbo.Companies");
            DropForeignKey("dbo.CompanyCatalogs", "CompanyId", "dbo.Companies");
            DropForeignKey("dbo.CompanyCatalogs", "CatalogId", "dbo.Catalogs");
            DropForeignKey("dbo.CatalogProductItems", "ProductItemId", "dbo.ProductItems");
            DropForeignKey("dbo.ProductItems", "Product_Id", "dbo.Products");
            DropForeignKey("dbo.Products", "Category_Id", "dbo.ProductCategories");
            DropForeignKey("dbo.CatalogProductItems", "CatalogId", "dbo.Catalogs");
            DropForeignKey("dbo.AnnouncementCompanies", "Announcement_Id", "dbo.Announcements");
            DropIndex("dbo.SubscriptionContracts", new[] { "Subscription_Id" });
            DropIndex("dbo.Subscriptions", new[] { "Product_Id" });
            DropIndex("dbo.SubscriptionAudits", new[] { "Subscription_Id" });
            DropIndex("dbo.PublisherAnnouncments", new[] { "Company_Id" });
            DropIndex("dbo.PublisherAnnouncments", new[] { "AnnouncementId" });
            DropIndex("dbo.Provisions", new[] { "Product_Id" });
            DropIndex("dbo.Provisions", new[] { "CompanyId" });
            DropIndex("dbo.ProductConstraints", new[] { "Product_Id" });
            DropIndex("dbo.OrderDetails", new[] { "Product_Id" });
            DropIndex("dbo.OrderDetails", new[] { "Order_Id" });
            DropIndex("dbo.Office365License", new[] { "Office365User_Id" });
            DropIndex("dbo.Office365License", new[] { "Office365Offer_Id" });
            DropIndex("dbo.Office365User", new[] { "CustomerId" });
            DropIndex("dbo.Office365Subscription", new[] { "Office365Offer_Id" });
            DropIndex("dbo.Office365Subscription", new[] { "Office365Customer_Id" });
            DropIndex("dbo.Office365Domain", new[] { "Office365Customer_Id" });
            DropIndex("dbo.Office365Customer", new[] { "CompanyId" });
            DropIndex("dbo.CompanyProductExclusions", new[] { "CompanyId" });
            DropIndex("dbo.CompanyProductExclusions", new[] { "ProductId" });
            DropIndex("dbo.Categories", new[] { "Parent_Id" });
            DropIndex("dbo.Domains", new[] { "Company_Id" });
            DropIndex("dbo.Products", new[] { "Category_Id" });
            DropIndex("dbo.ProductItems", new[] { "Product_Id" });
            DropIndex("dbo.CatalogProductItems", new[] { "ProductItemId" });
            DropIndex("dbo.CatalogProductItems", new[] { "CatalogId" });
            DropIndex("dbo.CompanyCatalogs", new[] { "CatalogId" });
            DropIndex("dbo.CompanyCatalogs", new[] { "CompanyId" });
            DropIndex("dbo.Companies", new[] { "ParentId" });
            DropIndex("dbo.AnnouncementCompanies", new[] { "Company_Id" });
            DropIndex("dbo.AnnouncementCompanies", new[] { "Announcement_Id" });
            DropTable("dbo.WorkflowActivities",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "CreateDateColumnName", "CreatedAt" },
                    { "SoftDeleteColumnName", "IsDeleted" },
                    { "UpdateDateColumnName", "UpdatedAt" },
                });
            DropTable("dbo.SubscriptionContracts",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "CreateDateColumnName", "CreatedAt" },
                    { "SoftDeleteColumnName", "IsDeleted" },
                    { "UpdateDateColumnName", "UpdatedAt" },
                });
            DropTable("dbo.Subscriptions",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "CreateDateColumnName", "CreatedAt" },
                    { "SoftDeleteColumnName", "IsDeleted" },
                    { "UpdateDateColumnName", "UpdatedAt" },
                });
            DropTable("dbo.SubscriptionAudits",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "CreateDateColumnName", "CreatedAt" },
                    { "SoftDeleteColumnName", "IsDeleted" },
                    { "UpdateDateColumnName", "UpdatedAt" },
                });
            DropTable("dbo.PublisherAnnouncments",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "CreateDateColumnName", "CreatedAt" },
                    { "SoftDeleteColumnName", "IsDeleted" },
                    { "UpdateDateColumnName", "UpdatedAt" },
                });
            DropTable("dbo.Provisions",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "CreateDateColumnName", "CreatedAt" },
                    { "SoftDeleteColumnName", "IsDeleted" },
                    { "UpdateDateColumnName", "UpdatedAt" },
                });
            DropTable("dbo.ProductConstraints",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "CreateDateColumnName", "CreatedAt" },
                    { "SoftDeleteColumnName", "IsDeleted" },
                    { "UpdateDateColumnName", "UpdatedAt" },
                });
            DropTable("dbo.Orders",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "CreateDateColumnName", "CreatedAt" },
                    { "SoftDeleteColumnName", "IsDeleted" },
                    { "UpdateDateColumnName", "UpdatedAt" },
                });
            DropTable("dbo.OrderDetails",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "CreateDateColumnName", "CreatedAt" },
                    { "SoftDeleteColumnName", "IsDeleted" },
                    { "UpdateDateColumnName", "UpdatedAt" },
                });
            DropTable("dbo.Office365Role",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "CreateDateColumnName", "CreatedAt" },
                    { "SoftDeleteColumnName", "IsDeleted" },
                    { "UpdateDateColumnName", "UpdatedAt" },
                });
            DropTable("dbo.Office365License",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "CreateDateColumnName", "CreatedAt" },
                    { "SoftDeleteColumnName", "IsDeleted" },
                    { "UpdateDateColumnName", "UpdatedAt" },
                });
            DropTable("dbo.Office365User",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "CreateDateColumnName", "CreatedAt" },
                    { "SoftDeleteColumnName", "IsDeleted" },
                    { "UpdateDateColumnName", "UpdatedAt" },
                });
            DropTable("dbo.Office365Offer",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "CreateDateColumnName", "CreatedAt" },
                    { "SoftDeleteColumnName", "IsDeleted" },
                    { "UpdateDateColumnName", "UpdatedAt" },
                });
            DropTable("dbo.Office365Subscription",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "CreateDateColumnName", "CreatedAt" },
                    { "SoftDeleteColumnName", "IsDeleted" },
                    { "UpdateDateColumnName", "UpdatedAt" },
                });
            DropTable("dbo.Office365Domain",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "CreateDateColumnName", "CreatedAt" },
                    { "SoftDeleteColumnName", "IsDeleted" },
                    { "UpdateDateColumnName", "UpdatedAt" },
                });
            DropTable("dbo.Office365Customer",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "CreateDateColumnName", "CreatedAt" },
                    { "SoftDeleteColumnName", "IsDeleted" },
                    { "UpdateDateColumnName", "UpdatedAt" },
                });
            DropTable("dbo.EmailTemplates",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "CreateDateColumnName", "CreatedAt" },
                    { "SoftDeleteColumnName", "IsDeleted" },
                    { "UpdateDateColumnName", "UpdatedAt" },
                });
            DropTable("dbo.CompanyProductExclusions",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "CreateDateColumnName", "CreatedAt" },
                    { "SoftDeleteColumnName", "IsDeleted" },
                    { "UpdateDateColumnName", "UpdatedAt" },
                });
            DropTable("dbo.Categories",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "CreateDateColumnName", "CreatedAt" },
                    { "SoftDeleteColumnName", "IsDeleted" },
                    { "UpdateDateColumnName", "UpdatedAt" },
                });
            DropTable("dbo.Domains",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "CreateDateColumnName", "CreatedAt" },
                    { "SoftDeleteColumnName", "IsDeleted" },
                    { "UpdateDateColumnName", "UpdatedAt" },
                });
            DropTable("dbo.ProductCategories",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "CreateDateColumnName", "CreatedAt" },
                    { "SoftDeleteColumnName", "IsDeleted" },
                    { "UpdateDateColumnName", "UpdatedAt" },
                });
            DropTable("dbo.Products",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "CreateDateColumnName", "CreatedAt" },
                    { "SoftDeleteColumnName", "IsDeleted" },
                    { "UpdateDateColumnName", "UpdatedAt" },
                });
            DropTable("dbo.ProductItems",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "CreateDateColumnName", "CreatedAt" },
                    { "SoftDeleteColumnName", "IsDeleted" },
                    { "UpdateDateColumnName", "UpdatedAt" },
                });
            DropTable("dbo.CatalogProductItems",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "CreateDateColumnName", "CreatedAt" },
                    { "SoftDeleteColumnName", "IsDeleted" },
                    { "UpdateDateColumnName", "UpdatedAt" },
                });
            DropTable("dbo.Catalogs",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "CreateDateColumnName", "CreatedAt" },
                    { "SoftDeleteColumnName", "IsDeleted" },
                    { "UpdateDateColumnName", "UpdatedAt" },
                });
            DropTable("dbo.CompanyCatalogs",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "CreateDateColumnName", "CreatedAt" },
                    { "SoftDeleteColumnName", "IsDeleted" },
                    { "UpdateDateColumnName", "UpdatedAt" },
                });
            DropTable("dbo.Companies",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "CreateDateColumnName", "CreatedAt" },
                    { "SoftDeleteColumnName", "IsDeleted" },
                    { "UpdateDateColumnName", "UpdatedAt" },
                });
            DropTable("dbo.Announcements",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "CreateDateColumnName", "CreatedAt" },
                    { "SoftDeleteColumnName", "IsDeleted" },
                    { "UpdateDateColumnName", "UpdatedAt" },
                });
            DropTable("dbo.AnnouncementCompanies",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "CreateDateColumnName", "CreatedAt" },
                    { "SoftDeleteColumnName", "IsDeleted" },
                    { "UpdateDateColumnName", "UpdatedAt" },
                });
        }

        public void Seed(CldpDbContext context)
        {
            var companyUtilities = new CompanyUtilities(context);
            var emailUtilities = new EmailTemplatesUtilities(context);
            var catalogUtilities = new CatalogUtilities(context);
            var office365Utilities = new Office365Utilities(context);

            companyUtilities.SeedCompanies();
            emailUtilities.SeedEmailTemplates();

            catalogUtilities.SeedCategoriesProductsAndProductItems();
            catalogUtilities.SeedCatalogs();

            office365Utilities.SeedOffice365Roles();
            office365Utilities.SeedOffice365Offers();
        }
    }
}
