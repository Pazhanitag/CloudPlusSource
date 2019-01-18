using System;

namespace CloudPlus.Constants
{
    public static class CompanyServiceConstants
    {
        public static string RabbitMqUri = System.Configuration.ConfigurationManager.AppSettings["RabbitMqUri"];
        public static string CompensateSuffix = "compensate";

        // Events
        public static string RoutingSlipEventObserverRoute = "company-service-routing-slip-event-observer";
        public static Uri RoutingSlipEventObserverUri = new Uri($"{RabbitMqUri}{RoutingSlipEventObserverRoute}");

        public static string RoutingSlipCompanyStartedEventRoute = "comapny-routing-slip-started";
        public static Uri RoutingSlipCompanyStartedEventUri = new Uri($"{RabbitMqUri}{RoutingSlipCompanyStartedEventRoute}");

        // Create Company
        public static string QueueCreateCompany = "create-company-flow";
        public static string ActivityCreateAdCompany = "CreateAdCompany";
        public static string ActivityCreateDatabaseCompany = "CreateDatabaseCompany";
        public static string ActivityAddCallbackRedirectUri = "ActivityAddCallbackRedirectUri";
        public static string ActivityRemoveCallbackRedirectUri = "ActivityRemoveCallbackRedirectUri";
        public static string ActivityCreateAdCompanyRoute = "create-ad-company-activity";
        public static string ActivityCreateDatabaseCompanyRoute = "create-database-company-activity";
        public static string ActivityAddCallbackRedirectUriRoute = "activity-add-callback-redirect-uri-route";
        public static string ActivityRemoveCallbackRedirectUriRoute = "activity-add-callback-redirect-uri-route";
        public static string CompensateCreateAdCompanyRoute = $"{ActivityCreateAdCompanyRoute}-{CompensateSuffix}";
        public static string CompensateCreateDatabaseCompanyRoute = $"{ActivityCreateDatabaseCompanyRoute}-{CompensateSuffix}";
        public static string CompensateAddCallbackRedirectUriRoute = $"{ActivityAddCallbackRedirectUriRoute}-{CompensateSuffix}";
        public static string CompensateRemoveCallbackRedirectUriRoute = $"{ActivityRemoveCallbackRedirectUriRoute}-{CompensateSuffix}";
        public static Uri CreateAdCompanyUri = new Uri($"{RabbitMqUri}{ActivityCreateAdCompanyRoute}");
        public static Uri CreateDatabaseCompanyUri = new Uri($"{RabbitMqUri}{ActivityCreateDatabaseCompanyRoute}");
        public static Uri AddCallbackRedirectUriUri = new Uri($"{RabbitMqUri}{ActivityAddCallbackRedirectUriRoute}");
        public static Uri RemoveCallbackRedirectUriUri = new Uri($"{RabbitMqUri}{ActivityRemoveCallbackRedirectUriRoute}");
        public static Uri CompensateCreateAdCompanyUri = new Uri($"{RabbitMqUri}{CompensateCreateAdCompanyRoute}");
        public static Uri CompensateCreateDatabaseCompanyUri = new Uri($"{RabbitMqUri}{CompensateCreateDatabaseCompanyRoute}");
        public static Uri CompensateAddCallbackRedirectUriUri = new Uri($"{RabbitMqUri}{CompensateAddCallbackRedirectUriRoute}");
        public static Uri CompensateRemoveCallbackRedirectUriUri = new Uri($"{RabbitMqUri}{CompensateRemoveCallbackRedirectUriRoute}");

        // Created Company send email
        public static string ActivityCompanyCreated = "CompanyCreated";
        public static string ActivityCompanyCreatedRoute = "company-created";
        public static Uri CompanyCreatedUri = new Uri($"{RabbitMqUri}{ActivityCompanyCreatedRoute}");

        // Update Company
        public static string QueueUpdateCompany = "update-company-flow";

        // Create price catalog
        public static string ActivityAssignCatalog = "AssignCatalog";
        public static string ActivityAssignCatalogRoute = "assign-catalog-activity";
        public static string CompensateAssignCatalogRoute = $"{ActivityAssignCatalogRoute}-{CompensateSuffix}";
        public static Uri AssignCatalogUri = new Uri($"{RabbitMqUri}{ActivityAssignCatalogRoute}");
        public static Uri CompensateAssignCatalogUri = new Uri($"{RabbitMqUri}{CompensateAssignCatalogRoute}");
    }
}
