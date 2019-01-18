using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using CloudPlus.Database.Common.Attributes;
using CloudPlus.Entities;
using CloudPlus.Entities.Catalog;
using CloudPlus.Entities.Office365;

namespace CloudPlus.Database
{
    public class CldpDbContext : DbContext
    {
        public CldpDbContext()
          //: base("name=CloudPlusDb")
          : base("CloudPlusDb")
        {
            //Enable eager loading
            Configuration.ProxyCreationEnabled = false;
            Configuration.LazyLoadingEnabled = false;
        }

        public virtual DbSet<AnnouncementCompany> AnnouncementCompanies { get; set; }
        public virtual DbSet<Announcement> Announcements { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Company> Companies { get; set; }
        public virtual DbSet<CompanyProductExclusion> CompanyProductExclusions { get; set; }
        public virtual DbSet<EmailTemplate> EmailTemplates { get; set; }
        public virtual DbSet<Domain> Domains { get; set; }
        public virtual DbSet<OrderDetail> OrderDetails { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<ProductConstraint> ProductConstraints { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<PublisherAnnouncment> PublisherAnnouncments { get; set; }
        public virtual DbSet<SubscriptionAudit> SubscriptionAudits { get; set; }
        public virtual DbSet<SubscriptionContract> SubscriptionContracts { get; set; }
        public virtual DbSet<Subscription> Subscriptions { get; set; }
        public virtual DbSet<WorkflowActivity> WorkflowActivity { get; set; }
        public virtual DbSet<Provision> Provisions { get; set; }

        public virtual DbSet<Office365Customer> Office365Customers { get; set; }
        public virtual DbSet<Office365Domain> Office365Domains { get; set; }
        public virtual DbSet<Office365Subscription> Office365Subscriptions { get; set; }
        public virtual DbSet<Office365User> Office365Users { get; set; }
        public virtual DbSet<Office365License> Office365Licenses { get; set; }
        public virtual DbSet<Office365Offer> Office365Offers { get; set; }
        public virtual DbSet<Office365Role> Office365Roles { get; set; }


        // Catalog V2
        public virtual DbSet<Catalog> Catalogs { get; set; }
        public virtual DbSet<CatalogProductItem> CatalogProductItems { get; set; }
        public virtual DbSet<CompanyCatalog> CompanyCatalogs { get; set; }
        public virtual DbSet<ProductItem> ProductItems { get; set; }
        public virtual DbSet<ProductCategory> ProductCategories { get; set; }

        public virtual DbSet<VendorMetrics_Office365_getEmailActivityCounts> VendorMetrics_Office365_getEmailActivityCounts { get; set; }
        public virtual DbSet<VendorMetrics_Office365_getEmailActivityUserDetail> VendorMetrics_Office365_getEmailActivityUserDetail { get; set; }
        public virtual DbSet<VendorMetrics_Office365_getOffice365ActivationCounts> VendorMetrics_Office365_getOffice365ActivationCounts { get; set; }
        public virtual DbSet<VendorMetrics_Office365_getOffice365ActivationsUserDetail> VendorMetrics_Office365_getOffice365ActivationsUserDetail { get; set; }
        public virtual DbSet<VendorMetrics_Office365_getOffice365ActiveUserCounts> VendorMetrics_Office365_getOffice365ActiveUserCounts { get; set; }
        public virtual DbSet<VendorMetrics_Office365_getOffice365ActiveUserDetail> VendorMetrics_Office365_getOffice365ActiveUserDetail { get; set; }
        public virtual DbSet<VendorMetrics_Office365_getOneDriveUsageAccountDetail> VendorMetrics_Office365_getOneDriveUsageAccountDetail { get; set; }
        public virtual DbSet<VendorMetrics_Office365_getOneDriveUsageStorage> VendorMetrics_Office365_getOneDriveUsageStorage { get; set; }
        public virtual DbSet<VendorMetrics_Office365_getSharePointActivityUserCounts> VendorMetrics_Office365_getSharePointActivityUserCounts { get; set; }
        public virtual DbSet<VendorMetrics_Office365_getSharePointActivityUserDetail> VendorMetrics_Office365_getSharePointActivityUserDetail { get; set; }
        public virtual DbSet<VendorMetrics_Office365_getSkypeForBusinessActivityUserCounts> VendorMetrics_Office365_getSkypeForBusinessActivityUserCounts { get; set; }
        public virtual DbSet<VendorMetrics_Office365_getSkypeForBusinessActivityUserDetail> VendorMetrics_Office365_getSkypeForBusinessActivityUserDetail { get; set; }
        public virtual DbSet<VendorMetrics_Office365_getTeamsUserActivityUserCounts> VendorMetrics_Office365_getTeamsUserActivityUserCounts { get; set; }
        public virtual DbSet<VendorMetrics_Office365_getTeamsUserActivityUserDetail> VendorMetrics_Office365_getTeamsUserActivityUserDetail { get; set; }
        public virtual DbSet<VendorMetricsConfiguration> VendorMetricsConfiguration { get; set; }

        public virtual DbSet<VendorMetrics> VendorMetrics { get; set; }
        public virtual DbSet<VendorMetricsAdminConfig> VendorMetricsAdminConfig { get; set; }
        public virtual DbSet<VendorMetricsUserConfig> VendorMetricsUserConfig { get; set; }
        public virtual DbSet<VendorMetricsChartTypes> VendorMetricsChartTypes { get; set; }
        public virtual DbSet<VendorMetricsReportConfig> VendorMetricsReportConfig { get; set; }
        public virtual DbSet<CustomSecureControlPanel> CustomSecureControlPanel { get; set; }
        public virtual DbSet<CustomSecureControlPanelStatus> CustomSecureControlPanelStatus { get; set; }
        public virtual DbSet<Office365InCompatibleService> Office365InCompatibleService { get; set; }
        public virtual DbSet<Office365OfferAddon> Office365OfferAddons { get; set; }
        public virtual DbSet<ProductAddon> ProductAddons { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            var softDelete = new AttributeToTableAnnotationConvention<SoftDeleteAttribute, string>(
                "SoftDeleteColumnName",
                (type, attributes) => attributes.Single().ColumnName);

            var createDate = new AttributeToTableAnnotationConvention<CreateDateAttribute, string>(
                "CreateDateColumnName",
                (type, attributes) => attributes.Single().ColumnName);

            var updateDate = new AttributeToTableAnnotationConvention<UpdateDateAttribute, string>(
                "UpdateDateColumnName",
                (type, attributes) => attributes.Single().ColumnName);

            modelBuilder.Conventions.Add(softDelete);
            modelBuilder.Conventions.Add(createDate);
            modelBuilder.Conventions.Add(updateDate);
        }
    }
}