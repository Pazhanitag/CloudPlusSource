namespace CloudPlus.Database.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure.Annotations;
    using System.Data.Entity.Migrations;
    
    public partial class AddVendorMetricsTables : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.VendorMetrics_Office365_getEmailActivityCounts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CompanyId = c.Int(nullable: false),
                        ReportPeriod = c.Int(nullable: false),
                        ReportRefreshDate = c.DateTime(nullable: false),
                        Send = c.Int(nullable: true),
                        Receive = c.Int(nullable: true),
                        Read = c.Int(nullable: true),
                        ReportDate = c.DateTime(nullable: true),
                        LastReportRetrieval = c.DateTime(nullable: false, defaultValueSql: "GETDATE()"),
                    IsDeleted = c.Boolean(nullable: false, defaultValue: false),
                    CreateDate = c.DateTime(nullable: false, defaultValueSql: "GETDATE()"),
                    UpdateDate = c.DateTime(nullable: false, defaultValueSql: "GETDATE()"),
                },
                annotations: new Dictionary<string, object>
                {
                    { "CreateDateColumnName", "CreatedAt" },
                    { "SoftDeleteColumnName", "IsDeleted" },
                    { "UpdateDateColumnName", "UpdatedAt" },
                })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.VendorMetrics_Office365_getEmailActivityUserDetail",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CompanyId = c.Int(nullable: false),
                        ReportPeriod = c.Int(nullable: false),
                        ReportRefreshDate = c.DateTime(nullable: false),
                        UserPrincipalName = c.String(),
                        DisplayName = c.String(),
                        DeletedDate = c.DateTime(nullable: true),
                        LastActivityDate = c.DateTime(nullable: true),
                        SendCount = c.Int(nullable: true),
                        ReceiveCount = c.Int(nullable: true),
                        ReadCount = c.Int(nullable: true),
                        AssignedProducts = c.String(),
                        LastReportRetrieval = c.DateTime(nullable: false, defaultValueSql: "GETDATE()"),
                    IsDeleted = c.Boolean(nullable: false, defaultValue: false),
                    CreateDate = c.DateTime(nullable: false, defaultValueSql: "GETDATE()"),
                    UpdateDate = c.DateTime(nullable: false, defaultValueSql: "GETDATE()"),
                },
                annotations: new Dictionary<string, object>
                {
                    { "CreateDateColumnName", "CreatedAt" },
                    { "SoftDeleteColumnName", "IsDeleted" },
                    { "UpdateDateColumnName", "UpdatedAt" },
                })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.VendorMetrics_Office365_getOffice365ActivationCounts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CompanyId = c.Int(nullable: false),
                        ReportRefreshDate = c.DateTime(nullable: false),
                        ProductType = c.String(),
                        Windows = c.Int(nullable: true),
                        Mac = c.Int(nullable: true),
                        Android = c.Int(nullable: true),
                        iOS = c.Int(nullable: true),
                        Windows10Mobile = c.Int(nullable: true),
                        LastReportRetrieval = c.DateTime(nullable: false, defaultValueSql: "GETDATE()"),
                    IsDeleted = c.Boolean(nullable: false, defaultValue: false),
                    CreateDate = c.DateTime(nullable: false, defaultValueSql: "GETDATE()"),
                    UpdateDate = c.DateTime(nullable: false, defaultValueSql: "GETDATE()"),
                },
                annotations: new Dictionary<string, object>
                {
                    { "CreateDateColumnName", "CreatedAt" },
                    { "SoftDeleteColumnName", "IsDeleted" },
                    { "UpdateDateColumnName", "UpdatedAt" },
                })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.VendorMetrics_Office365_getOffice365ActivationsUserDetail",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CompanyId = c.Int(nullable: false),
                        ReportRefreshDate = c.DateTime(nullable: false),
                        UserPrincipalName = c.String(),
                        DisplayName = c.String(),
                        ProductType = c.String(),
                        LastActivatedDate = c.DateTime(nullable: true),
                        Windows = c.Int(nullable: true),
                        Mac = c.Int(nullable: true),
                        iOS = c.Int(nullable: true),
                        Android = c.Int(nullable: true),
                        Windows10Mobile = c.Int(nullable: true),
                        LastReportRetrieval = c.DateTime(nullable: false, defaultValueSql: "GETDATE()"),
                    IsDeleted = c.Boolean(nullable: false, defaultValue: false),
                    CreateDate = c.DateTime(nullable: false, defaultValueSql: "GETDATE()"),
                    UpdateDate = c.DateTime(nullable: false, defaultValueSql: "GETDATE()"),
                },
                annotations: new Dictionary<string, object>
                {
                    { "CreateDateColumnName", "CreatedAt" },
                    { "SoftDeleteColumnName", "IsDeleted" },
                    { "UpdateDateColumnName", "UpdatedAt" },
                })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.VendorMetrics_Office365_getOffice365ActiveUserCounts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CompanyId = c.Int(nullable: false),
                        ReportPeriod = c.Int(nullable: false),
                        ReportRefreshDate = c.DateTime(nullable: false),
                        Office365 = c.Int(nullable: true),
                        Exchange = c.Int(nullable: true),
                        OneDrive = c.Int(nullable: true),
                        SharePoint = c.Int(nullable: true),
                        SkypeForBusiness = c.Int(nullable: true),
                        Yammer = c.Int(nullable: true),
                        Teams = c.Int(nullable: true),
                        ReportDate = c.DateTime(nullable: true),
                        LastReportRetrieval = c.DateTime(nullable: false, defaultValueSql: "GETDATE()"),
                    IsDeleted = c.Boolean(nullable: false, defaultValue: false),
                    CreateDate = c.DateTime(nullable: false, defaultValueSql: "GETDATE()"),
                    UpdateDate = c.DateTime(nullable: false, defaultValueSql: "GETDATE()"),
                },
                annotations: new Dictionary<string, object>
                {
                    { "CreateDateColumnName", "CreatedAt" },
                    { "SoftDeleteColumnName", "IsDeleted" },
                    { "UpdateDateColumnName", "UpdatedAt" },
                })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.VendorMetrics_Office365_getOffice365ActiveUserDetail",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CompanyId = c.Int(nullable: false),
                        ReportPeriod = c.Int(nullable: false),
                        ReportRefreshDate = c.DateTime(nullable: false),
                        UserPrincipalName = c.String(),
                        DisplayName = c.String(),
                        DeletedDate = c.DateTime(nullable: true),
                        HasExchangeLicense = c.Boolean(nullable: true),
                        HasOneDriveLicense = c.Boolean(nullable: true),
                        HasSharePointLicense = c.Boolean(nullable: true),
                        HasSkypeForBusinessLicense = c.Boolean(nullable: true),
                        HasYammerLicense = c.Boolean(nullable: true),
                        HasTeamsLicense = c.Boolean(nullable: true),
                        ExchangeLastActivityDate = c.DateTime(nullable: true),
                        OneDriveLastActivityDate = c.DateTime(nullable: true),
                        SharePointLastActivityDate = c.DateTime(nullable: true),
                        SkypeForBusinessLastActivityDate = c.DateTime(nullable: true),
                        YammerLastActivityDate = c.DateTime(nullable: true),
                        TeamsLastActivityDate = c.DateTime(nullable: true),
                        ExchangeLicenseAssignDate = c.DateTime(nullable: true),
                        OneDriveLicenseAssignDate = c.DateTime(nullable: true),
                        SharePointLicenseAssignDate = c.DateTime(nullable: true),
                        SkypeForBusinessLicenseAssignDate = c.DateTime(nullable: true),
                        YammerLicenseAssignDate = c.DateTime(nullable: true),
                        TeamsLicenseAssignDate = c.DateTime(nullable: true),
                        AssignedProducts = c.String(),
                        LastReportRetrieval = c.DateTime(nullable: false, defaultValueSql: "GETDATE()"),
                    IsDeleted = c.Boolean(nullable: false, defaultValue: false),
                    CreateDate = c.DateTime(nullable: false, defaultValueSql: "GETDATE()"),
                    UpdateDate = c.DateTime(nullable: false, defaultValueSql: "GETDATE()"),
                },
                annotations: new Dictionary<string, object>
                {
                    { "CreateDateColumnName", "CreatedAt" },
                    { "SoftDeleteColumnName", "IsDeleted" },
                    { "UpdateDateColumnName", "UpdatedAt" },
                })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.VendorMetrics_Office365_getOneDriveUsageAccountDetail",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CompanyId = c.Int(nullable: false),
                        ReportPeriod = c.Int(nullable: false),
                        ReportRefreshDate = c.DateTime(nullable: false),
                        SiteURL = c.String(),
                        OwnerDisplayName = c.String(),
                        LastActivityDate = c.DateTime(nullable: true),
                        FileCount = c.Int(nullable: true),
                        ActiveFileCount = c.Int(nullable: true),
                        StorageUsed = c.Double(nullable: true),
                        StorageAllocated = c.Double(nullable: true),
                        LastReportRetrieval = c.DateTime(nullable: false, defaultValueSql: "GETDATE()"),
                    IsDeleted = c.Boolean(nullable: false, defaultValue: false),
                    CreateDate = c.DateTime(nullable: false, defaultValueSql: "GETDATE()"),
                    UpdateDate = c.DateTime(nullable: false, defaultValueSql: "GETDATE()"),
                },
                annotations: new Dictionary<string, object>
                {
                    { "CreateDateColumnName", "CreatedAt" },
                    { "SoftDeleteColumnName", "IsDeleted" },
                    { "UpdateDateColumnName", "UpdatedAt" },
                })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.VendorMetrics_Office365_getOneDriveUsageStorage",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CompanyId = c.Int(nullable: false),
                        ReportPeriod = c.Int(nullable: false),
                        ReportRefreshDate = c.DateTime(nullable: false),
                        SiteType = c.String(),
                        StorageUsed = c.Double(nullable: true),
                        ReportDate = c.DateTime(nullable: true),
                        LastReportRetrieval = c.DateTime(nullable: false, defaultValueSql: "GETDATE()"),
                    IsDeleted = c.Boolean(nullable: false, defaultValue: false),
                    CreateDate = c.DateTime(nullable: false, defaultValueSql: "GETDATE()"),
                    UpdateDate = c.DateTime(nullable: false, defaultValueSql: "GETDATE()"),
                },
                annotations: new Dictionary<string, object>
                {
                    { "CreateDateColumnName", "CreatedAt" },
                    { "SoftDeleteColumnName", "IsDeleted" },
                    { "UpdateDateColumnName", "UpdatedAt" },
                })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.VendorMetrics_Office365_getSharePointActivityUserCounts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CompanyId = c.Int(nullable: false),
                        ReportPeriod = c.Int(nullable: false),
                        ReportRefreshDate = c.DateTime(nullable: false),
                        VisitedPage = c.Int(nullable: true),
                        ViewedOrEdited = c.Int(nullable: true),
                        Synced = c.Int(nullable: true),
                        SharedInternally = c.Int(nullable: true),
                        SharedExternally = c.Int(nullable: true),
                        ReportDate = c.DateTime(nullable: true),
                        LastReportRetrieval = c.DateTime(nullable: false, defaultValueSql: "GETDATE()"),
                    IsDeleted = c.Boolean(nullable: false, defaultValue: false),
                    CreateDate = c.DateTime(nullable: false, defaultValueSql: "GETDATE()"),
                    UpdateDate = c.DateTime(nullable: false, defaultValueSql: "GETDATE()"),
                },
                annotations: new Dictionary<string, object>
                {
                    { "CreateDateColumnName", "CreatedAt" },
                    { "SoftDeleteColumnName", "IsDeleted" },
                    { "UpdateDateColumnName", "UpdatedAt" },
                })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.VendorMetrics_Office365_getSharePointActivityUserDetail",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CompanyId = c.Int(nullable: false),
                        ReportPeriod = c.Int(nullable: false),
                        ReportRefreshDate = c.DateTime(nullable: false),
                        UserPrincipalName = c.String(),
                        DeletedDate = c.DateTime(nullable: true),
                        LastActivityDate = c.DateTime(nullable: true),
                        ViewedOrEditedFileCount = c.Int(nullable: true),
                        SyncedFileCount = c.Int(nullable: true),
                        SharedInternallyFileCount = c.Int(nullable: true),
                        SharedExternallyFileCount = c.Int(nullable: true),
                        VisitedPageCount = c.Int(nullable: true),
                        AssignedProducts = c.String(),
                        LastReportRetrieval = c.DateTime(nullable: false, defaultValueSql: "GETDATE()"),
                    IsDeleted = c.Boolean(nullable: false, defaultValue: false),
                    CreateDate = c.DateTime(nullable: false, defaultValueSql: "GETDATE()"),
                    UpdateDate = c.DateTime(nullable: false, defaultValueSql: "GETDATE()"),
                },
                annotations: new Dictionary<string, object>
                {
                    { "CreateDateColumnName", "CreatedAt" },
                    { "SoftDeleteColumnName", "IsDeleted" },
                    { "UpdateDateColumnName", "UpdatedAt" },
                })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.VendorMetrics_Office365_getSkypeForBusinessActivityUserCounts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CompanyId = c.Int(nullable: false),
                        ReportPeriod = c.Int(nullable: false),
                        ReportRefreshDate = c.DateTime(nullable: false),
                        ReportDate = c.DateTime(nullable: true),
                        PeerToPeer = c.Int(nullable: true),
                        Organized = c.Int(nullable: true),
                        Participated = c.Int(nullable: true),
                        LastReportRetrieval = c.DateTime(nullable: false, defaultValueSql: "GETDATE()"),
                    IsDeleted = c.Boolean(nullable: false, defaultValue: false),
                    CreateDate = c.DateTime(nullable: false, defaultValueSql: "GETDATE()"),
                    UpdateDate = c.DateTime(nullable: false, defaultValueSql: "GETDATE()"),
                },
                annotations: new Dictionary<string, object>
                {
                    { "CreateDateColumnName", "CreatedAt" },
                    { "SoftDeleteColumnName", "IsDeleted" },
                    { "UpdateDateColumnName", "UpdatedAt" },
                })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.VendorMetrics_Office365_getSkypeForBusinessActivityUserDetail",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CompanyId = c.Int(nullable: false),
                        ReportPeriod = c.Int(nullable: false),
                        ReportRefreshDate = c.DateTime(nullable: false),
                        UserPrincipalName = c.String(),
                        DeletedDate = c.DateTime(nullable: true),
                        LastActivityDate = c.DateTime(nullable: true),
                        TotalPeerToPeerSessionCount = c.Int(nullable: true),
                        TotalOrganizedConferenceCount = c.Int(nullable: true),
                        TotalParticipatedConferenceCount = c.Int(nullable: true),
                        PeerToPeerLastActivityDate = c.DateTime(nullable: true),
                        OrganizedConferenceLastActivityDate = c.DateTime(nullable: true),
                        ParticipatedConferenceLastActivityDate = c.DateTime(nullable: true),
                        PeerToPeerIMCount = c.Int(nullable: true),
                        PeerToPeerAudioCount = c.Int(nullable: true),
                        PeerToPeerAudioMinutes = c.Int(nullable: true),
                        PeerToPeerVideoCount = c.Int(nullable: true),
                        PeerToPeerVideoMinutes = c.Int(nullable: true),
                        PeerToPeerAppSharingCount = c.Int(nullable: true),
                        PeerToPeerFileTransferCount = c.Int(nullable: true),
                        OrganizedConferenceIMCount = c.Int(nullable: true),
                        OrganizedConferenceAudioVideoCount = c.Int(nullable: true),
                        OrganizedConferenceAudioVideoMinutes = c.Int(nullable: true),
                        OrganizedConferenceAppSharingCount = c.Int(nullable: true),
                        OrganizedConferenceWebSharingCount = c.Int(nullable: true),
                        OrganizedConferenceDialInOut3rdPartyCount = c.Int(nullable: true),
                        OrganizedConferenceDialInOutMicrosoftCount = c.Int(nullable: true),
                        OrganizedConferenceDialInMicrosoftMinutes = c.Int(nullable: true),
                        OrganizedConferenceDialOutMicrosoftMinutes = c.Int(nullable: true),
                        ParticipatedConferenceIMCount = c.Int(nullable: true),
                        ParticipatedConferenceAudioVideoCount = c.Int(nullable: true),
                        ParticipatedConferenceAudioVideoMinutes = c.Int(nullable: true),
                        ParticipatedConferenceAppSharingCount = c.Int(nullable: true),
                        ParticipatedConferenceWebCount = c.Int(nullable: true),
                        ParticipatedConferenceDialInOut3rdPartyCount = c.Int(nullable: true),
                        AssignedProducts = c.String(),
                        LastReportRetrieval = c.DateTime(nullable: false, defaultValueSql: "GETDATE()"),
                    IsDeleted = c.Boolean(nullable: false, defaultValue: false),
                    CreateDate = c.DateTime(nullable: false, defaultValueSql: "GETDATE()"),
                    UpdateDate = c.DateTime(nullable: false, defaultValueSql: "GETDATE()"),
                },
                annotations: new Dictionary<string, object>
                {
                    { "CreateDateColumnName", "CreatedAt" },
                    { "SoftDeleteColumnName", "IsDeleted" },
                    { "UpdateDateColumnName", "UpdatedAt" },
                })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.VendorMetrics_Office365_getTeamsUserActivityUserCounts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CompanyId = c.Int(nullable: false),
                        ReportPeriod = c.Int(nullable: false),
                        ReportRefreshDate = c.DateTime(nullable: false),
                        ReportDate = c.DateTime(nullable: true),
                        TeamChatMessages = c.Int(nullable: true),
                        PrivateChatMessages = c.Int(nullable: true),
                        Calls = c.Int(nullable: true),
                        Meetings = c.Int(nullable: true),
                        OtherActions = c.Int(nullable: true),
                        LastReportRetrieval = c.DateTime(nullable: false, defaultValueSql: "GETDATE()"),
                    IsDeleted = c.Boolean(nullable: false, defaultValue: false),
                    CreateDate = c.DateTime(nullable: false, defaultValueSql: "GETDATE()"),
                    UpdateDate = c.DateTime(nullable: false, defaultValueSql: "GETDATE()"),
                },
                annotations: new Dictionary<string, object>
                {
                    { "CreateDateColumnName", "CreatedAt" },
                    { "SoftDeleteColumnName", "IsDeleted" },
                    { "UpdateDateColumnName", "UpdatedAt" },
                })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.VendorMetrics_Office365_getTeamsUserActivityUserDetail",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CompanyId = c.Int(nullable: false),
                        ReportPeriod = c.Int(nullable: false),
                        ReportRefreshDate = c.DateTime(nullable: false),
                        UserPrincipalName = c.String(),
                        LastActivityDate = c.DateTime(nullable: true),
                        DeletedDate = c.DateTime(nullable: true),
                        AssignedProducts = c.String(),
                        TeamChatMessageCount = c.Int(nullable: true),
                        PrivateChatMessageCount = c.Int(nullable: true),
                        CallCount = c.Int(nullable: true),
                        MeetingCount = c.Int(nullable: true),
                        HasOtherAction = c.String(),
                        LastReportRetrieval = c.DateTime(nullable: false, defaultValueSql: "GETDATE()"),
                    IsDeleted = c.Boolean(nullable: false, defaultValue: false),
                    CreateDate = c.DateTime(nullable: false, defaultValueSql: "GETDATE()"),
                    UpdateDate = c.DateTime(nullable: false, defaultValueSql: "GETDATE()"),
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
            DropTable("dbo.VendorMetrics_Office365_getTeamsUserActivityUserDetail",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "CreateDateColumnName", "CreatedAt" },
                    { "SoftDeleteColumnName", "IsDeleted" },
                    { "UpdateDateColumnName", "UpdatedAt" },
                });
            DropTable("dbo.VendorMetrics_Office365_getTeamsUserActivityUserCounts",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "CreateDateColumnName", "CreatedAt" },
                    { "SoftDeleteColumnName", "IsDeleted" },
                    { "UpdateDateColumnName", "UpdatedAt" },
                });
            DropTable("dbo.VendorMetrics_Office365_getSkypeForBusinessActivityUserDetail",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "CreateDateColumnName", "CreatedAt" },
                    { "SoftDeleteColumnName", "IsDeleted" },
                    { "UpdateDateColumnName", "UpdatedAt" },
                });
            DropTable("dbo.VendorMetrics_Office365_getSkypeForBusinessActivityUserCounts",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "CreateDateColumnName", "CreatedAt" },
                    { "SoftDeleteColumnName", "IsDeleted" },
                    { "UpdateDateColumnName", "UpdatedAt" },
                });
            DropTable("dbo.VendorMetrics_Office365_getSharePointActivityUserDetail",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "CreateDateColumnName", "CreatedAt" },
                    { "SoftDeleteColumnName", "IsDeleted" },
                    { "UpdateDateColumnName", "UpdatedAt" },
                });
            DropTable("dbo.VendorMetrics_Office365_getSharePointActivityUserCounts",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "CreateDateColumnName", "CreatedAt" },
                    { "SoftDeleteColumnName", "IsDeleted" },
                    { "UpdateDateColumnName", "UpdatedAt" },
                });
            DropTable("dbo.VendorMetrics_Office365_getOneDriveUsageStorage",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "CreateDateColumnName", "CreatedAt" },
                    { "SoftDeleteColumnName", "IsDeleted" },
                    { "UpdateDateColumnName", "UpdatedAt" },
                });
            DropTable("dbo.VendorMetrics_Office365_getOneDriveUsageAccountDetail",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "CreateDateColumnName", "CreatedAt" },
                    { "SoftDeleteColumnName", "IsDeleted" },
                    { "UpdateDateColumnName", "UpdatedAt" },
                });
            DropTable("dbo.VendorMetrics_Office365_getOffice365ActiveUserDetail",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "CreateDateColumnName", "CreatedAt" },
                    { "SoftDeleteColumnName", "IsDeleted" },
                    { "UpdateDateColumnName", "UpdatedAt" },
                });
            DropTable("dbo.VendorMetrics_Office365_getOffice365ActiveUserCounts",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "CreateDateColumnName", "CreatedAt" },
                    { "SoftDeleteColumnName", "IsDeleted" },
                    { "UpdateDateColumnName", "UpdatedAt" },
                });
            DropTable("dbo.VendorMetrics_Office365_getOffice365ActivationsUserDetail",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "CreateDateColumnName", "CreatedAt" },
                    { "SoftDeleteColumnName", "IsDeleted" },
                    { "UpdateDateColumnName", "UpdatedAt" },
                });
            DropTable("dbo.VendorMetrics_Office365_getOffice365ActivationCounts",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "CreateDateColumnName", "CreatedAt" },
                    { "SoftDeleteColumnName", "IsDeleted" },
                    { "UpdateDateColumnName", "UpdatedAt" },
                });
            DropTable("dbo.VendorMetrics_Office365_getEmailActivityUserDetail",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "CreateDateColumnName", "CreatedAt" },
                    { "SoftDeleteColumnName", "IsDeleted" },
                    { "UpdateDateColumnName", "UpdatedAt" },
                });
            DropTable("dbo.VendorMetrics_Office365_getEmailActivityCounts",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "CreateDateColumnName", "CreatedAt" },
                    { "SoftDeleteColumnName", "IsDeleted" },
                    { "UpdateDateColumnName", "UpdatedAt" },
                });
        }
    }
}
