using CloudPlus.Database.Authentication.Utilities;
using CloudPlus.Resources;

namespace CloudPlus.Database.Authentication.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure.Annotations;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration, IMigrationSeed<CloudPlusAuthDbContext>
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ClientCorsOrigins",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Origin = c.String(nullable: false, maxLength: 150),
                        Client_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Clients", t => t.Client_Id)
                .Index(t => t.Client_Id);
            
            CreateTable(
                "dbo.Clients",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Enabled = c.Boolean(nullable: false),
                        ClientId = c.String(nullable: false, maxLength: 200),
                        ClientName = c.String(nullable: false, maxLength: 200),
                        ClientUri = c.String(maxLength: 2000),
                        LogoUri = c.String(),
                        RequireConsent = c.Boolean(nullable: false),
                        AllowRememberConsent = c.Boolean(nullable: false),
                        AllowAccessTokensViaBrowser = c.Boolean(nullable: false),
                        Flow = c.Int(nullable: false),
                        AllowClientCredentialsOnly = c.Boolean(nullable: false),
                        LogoutUri = c.String(),
                        LogoutSessionRequired = c.Boolean(nullable: false),
                        RequireSignOutPrompt = c.Boolean(nullable: false),
                        AllowAccessToAllScopes = c.Boolean(nullable: false),
                        IdentityTokenLifetime = c.Int(nullable: false),
                        AccessTokenLifetime = c.Int(nullable: false),
                        AuthorizationCodeLifetime = c.Int(nullable: false),
                        AbsoluteRefreshTokenLifetime = c.Int(nullable: false),
                        SlidingRefreshTokenLifetime = c.Int(nullable: false),
                        RefreshTokenUsage = c.Int(nullable: false),
                        UpdateAccessTokenOnRefresh = c.Boolean(nullable: false),
                        RefreshTokenExpiration = c.Int(nullable: false),
                        AccessTokenType = c.Int(nullable: false),
                        EnableLocalLogin = c.Boolean(nullable: false),
                        IncludeJwtId = c.Boolean(nullable: false),
                        AlwaysSendClientClaims = c.Boolean(nullable: false),
                        PrefixClientClaims = c.Boolean(nullable: false),
                        AllowAccessToAllGrantTypes = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.ClientId, unique: true);
            
            CreateTable(
                "dbo.ClientCustomGrantTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        GrantType = c.String(nullable: false, maxLength: 250),
                        Client_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Clients", t => t.Client_Id)
                .Index(t => t.Client_Id);
            
            CreateTable(
                "dbo.ClientScopes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Scope = c.String(nullable: false, maxLength: 200),
                        Client_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Clients", t => t.Client_Id)
                .Index(t => t.Client_Id);
            
            CreateTable(
                "dbo.ClientClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Type = c.String(nullable: false, maxLength: 250),
                        Value = c.String(nullable: false, maxLength: 250),
                        Client_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Clients", t => t.Client_Id)
                .Index(t => t.Client_Id);
            
            CreateTable(
                "dbo.ClientSecrets",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Value = c.String(nullable: false, maxLength: 250),
                        Type = c.String(maxLength: 250),
                        Description = c.String(maxLength: 2000),
                        Expiration = c.DateTimeOffset(precision: 7),
                        Client_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Clients", t => t.Client_Id)
                .Index(t => t.Client_Id);
            
            CreateTable(
                "dbo.ClientIdPRestrictions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Provider = c.String(nullable: false, maxLength: 200),
                        Client_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Clients", t => t.Client_Id)
                .Index(t => t.Client_Id);
            
            CreateTable(
                "dbo.ClientPostLogoutRedirectUris",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Uri = c.String(nullable: false, maxLength: 2000),
                        Client_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Clients", t => t.Client_Id)
                .Index(t => t.Client_Id);
            
            CreateTable(
                "dbo.ClientRedirectUris",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Uri = c.String(nullable: false, maxLength: 2000),
                        Client_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Clients", t => t.Client_Id)
                .Index(t => t.Client_Id);
            
            CreateTable(
                "dbo.Impersonates",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CreatedAt = c.DateTime(nullable: false),
                        UpdatedAt = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        ImpersonateUser_Id = c.Int(),
                        ParentUser_Id = c.Int(),
                    },
                annotations: new Dictionary<string, object>
                {
                    { "CreateDateColumnName", "CreatedAt" },
                    { "SoftDeleteColumnName", "IsDeleted" },
                    { "UpdateDateColumnName", "UpdatedAt" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.ImpersonateUser_Id)
                .ForeignKey("dbo.Users", t => t.ParentUser_Id)
                .Index(t => t.ImpersonateUser_Id)
                .Index(t => t.ParentUser_Id);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserName = c.String(maxLength: 50),
                        DisplayName = c.String(),
                        FirstName = c.String(),
                        LastName = c.String(),
                        IsApproved = c.Boolean(),
                        IsLockedOut = c.Boolean(),
                        Discriminator = c.String(),
                        IsDeleted = c.Boolean(nullable: false),
                        UserStatus = c.Int(nullable: false),
                        CreatedAt = c.DateTime(nullable: false),
                        UpdatedAt = c.DateTime(nullable: false),
                        JobTitle = c.String(),
                        Department = c.String(),
                        StreetAddress = c.String(),
                        City = c.String(),
                        ZipCode = c.String(),
                        State = c.String(),
                        Country = c.String(),
                        CompanyId = c.Int(nullable: false),
                        AlternativeEmail = c.String(),
                        ProfilePicture = c.String(),
                        CompanyName = c.String(),
                        Email = c.String(),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                    },
                annotations: new Dictionary<string, object>
                {
                    { "CreateDateColumnName", "CreatedAt" },
                    { "SoftDeleteColumnName", "IsDeleted" },
                    { "UpdateDateColumnName", "UpdatedAt" },
                })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, name: "UserNameIndex");
            
            CreateTable(
                "dbo.UserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatedAt = c.DateTime(nullable: false),
                        UpdatedAt = c.DateTime(nullable: false),
                        UserId = c.Int(nullable: false),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    },
                annotations: new Dictionary<string, object>
                {
                    { "CreateDateColumnName", "CreatedAt" },
                    { "SoftDeleteColumnName", "IsDeleted" },
                    { "UpdateDateColumnName", "UpdatedAt" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.UserLogins",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatedAt = c.DateTime(nullable: false),
                        UpdatedAt = c.DateTime(nullable: false),
                        LoginProvider = c.String(),
                        ProviderKey = c.String(),
                        UserId = c.Int(nullable: false),
                    },
                annotations: new Dictionary<string, object>
                {
                    { "CreateDateColumnName", "CreatedAt" },
                    { "SoftDeleteColumnName", "IsDeleted" },
                    { "UpdateDateColumnName", "UpdatedAt" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.UserRoles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatedAt = c.DateTime(nullable: false),
                        UpdatedAt = c.DateTime(nullable: false),
                        UserId = c.Int(nullable: false),
                        RoleId = c.Int(nullable: false),
                    },
                annotations: new Dictionary<string, object>
                {
                    { "CreateDateColumnName", "CreatedAt" },
                    { "SoftDeleteColumnName", "IsDeleted" },
                    { "UpdateDateColumnName", "UpdatedAt" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Roles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.Roles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AvailableRoles = c.String(),
                        FriendlyName = c.String(),
                        Description = c.String(),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatedAt = c.DateTime(nullable: false),
                        UpdatedAt = c.DateTime(nullable: false),
                        Name = c.String(),
                    },
                annotations: new Dictionary<string, object>
                {
                    { "CreateDateColumnName", "CreatedAt" },
                    { "SoftDeleteColumnName", "IsDeleted" },
                    { "UpdateDateColumnName", "UpdatedAt" },
                })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Permissions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatedAt = c.DateTime(nullable: false),
                        UpdatedAt = c.DateTime(nullable: false),
                    },
                annotations: new Dictionary<string, object>
                {
                    { "CreateDateColumnName", "CreatedAt" },
                    { "SoftDeleteColumnName", "IsDeleted" },
                    { "UpdateDateColumnName", "UpdatedAt" },
                })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.RolePermissions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatedAt = c.DateTime(nullable: false),
                        UpdatedAt = c.DateTime(nullable: false),
                        Permission_Id = c.Int(),
                        Role_Id = c.Int(),
                    },
                annotations: new Dictionary<string, object>
                {
                    { "CreateDateColumnName", "CreatedAt" },
                    { "SoftDeleteColumnName", "IsDeleted" },
                    { "UpdateDateColumnName", "UpdatedAt" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Permissions", t => t.Permission_Id)
                .ForeignKey("dbo.Roles", t => t.Role_Id)
                .Index(t => t.Permission_Id)
                .Index(t => t.Role_Id);
            
            CreateTable(
                "dbo.Scopes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Enabled = c.Boolean(nullable: false),
                        Name = c.String(nullable: false, maxLength: 200),
                        DisplayName = c.String(maxLength: 200),
                        Description = c.String(maxLength: 1000),
                        Required = c.Boolean(nullable: false),
                        Emphasize = c.Boolean(nullable: false),
                        Type = c.Int(nullable: false),
                        IncludeAllClaimsForUser = c.Boolean(nullable: false),
                        ClaimsRule = c.String(maxLength: 200),
                        ShowInDiscoveryDocument = c.Boolean(nullable: false),
                        AllowUnrestrictedIntrospection = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ScopeClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 200),
                        Description = c.String(maxLength: 1000),
                        AlwaysIncludeInIdToken = c.Boolean(nullable: false),
                        Scope_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Scopes", t => t.Scope_Id)
                .Index(t => t.Scope_Id);
            
            CreateTable(
                "dbo.ScopeSecrets",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Description = c.String(maxLength: 1000),
                        Expiration = c.DateTimeOffset(precision: 7),
                        Type = c.String(maxLength: 250),
                        Value = c.String(nullable: false, maxLength: 250),
                        Scope_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Scopes", t => t.Scope_Id)
                .Index(t => t.Scope_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ScopeSecrets", "Scope_Id", "dbo.Scopes");
            DropForeignKey("dbo.ScopeClaims", "Scope_Id", "dbo.Scopes");
            DropForeignKey("dbo.RolePermissions", "Role_Id", "dbo.Roles");
            DropForeignKey("dbo.RolePermissions", "Permission_Id", "dbo.Permissions");
            DropForeignKey("dbo.Impersonates", "ParentUser_Id", "dbo.Users");
            DropForeignKey("dbo.Impersonates", "ImpersonateUser_Id", "dbo.Users");
            DropForeignKey("dbo.UserRoles", "UserId", "dbo.Users");
            DropForeignKey("dbo.UserRoles", "RoleId", "dbo.Roles");
            DropForeignKey("dbo.UserLogins", "UserId", "dbo.Users");
            DropForeignKey("dbo.UserClaims", "UserId", "dbo.Users");
            DropForeignKey("dbo.ClientRedirectUris", "Client_Id", "dbo.Clients");
            DropForeignKey("dbo.ClientPostLogoutRedirectUris", "Client_Id", "dbo.Clients");
            DropForeignKey("dbo.ClientIdPRestrictions", "Client_Id", "dbo.Clients");
            DropForeignKey("dbo.ClientSecrets", "Client_Id", "dbo.Clients");
            DropForeignKey("dbo.ClientClaims", "Client_Id", "dbo.Clients");
            DropForeignKey("dbo.ClientScopes", "Client_Id", "dbo.Clients");
            DropForeignKey("dbo.ClientCustomGrantTypes", "Client_Id", "dbo.Clients");
            DropForeignKey("dbo.ClientCorsOrigins", "Client_Id", "dbo.Clients");
            DropIndex("dbo.ScopeSecrets", new[] { "Scope_Id" });
            DropIndex("dbo.ScopeClaims", new[] { "Scope_Id" });
            DropIndex("dbo.RolePermissions", new[] { "Role_Id" });
            DropIndex("dbo.RolePermissions", new[] { "Permission_Id" });
            DropIndex("dbo.UserRoles", new[] { "RoleId" });
            DropIndex("dbo.UserRoles", new[] { "UserId" });
            DropIndex("dbo.UserLogins", new[] { "UserId" });
            DropIndex("dbo.UserClaims", new[] { "UserId" });
            DropIndex("dbo.Users", "UserNameIndex");
            DropIndex("dbo.Impersonates", new[] { "ParentUser_Id" });
            DropIndex("dbo.Impersonates", new[] { "ImpersonateUser_Id" });
            DropIndex("dbo.ClientRedirectUris", new[] { "Client_Id" });
            DropIndex("dbo.ClientPostLogoutRedirectUris", new[] { "Client_Id" });
            DropIndex("dbo.ClientIdPRestrictions", new[] { "Client_Id" });
            DropIndex("dbo.ClientSecrets", new[] { "Client_Id" });
            DropIndex("dbo.ClientClaims", new[] { "Client_Id" });
            DropIndex("dbo.ClientScopes", new[] { "Client_Id" });
            DropIndex("dbo.ClientCustomGrantTypes", new[] { "Client_Id" });
            DropIndex("dbo.Clients", new[] { "ClientId" });
            DropIndex("dbo.ClientCorsOrigins", new[] { "Client_Id" });
            DropTable("dbo.ScopeSecrets");
            DropTable("dbo.ScopeClaims");
            DropTable("dbo.Scopes");
            DropTable("dbo.RolePermissions",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "CreateDateColumnName", "CreatedAt" },
                    { "SoftDeleteColumnName", "IsDeleted" },
                    { "UpdateDateColumnName", "UpdatedAt" },
                });
            DropTable("dbo.Permissions",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "CreateDateColumnName", "CreatedAt" },
                    { "SoftDeleteColumnName", "IsDeleted" },
                    { "UpdateDateColumnName", "UpdatedAt" },
                });
            DropTable("dbo.Roles",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "CreateDateColumnName", "CreatedAt" },
                    { "SoftDeleteColumnName", "IsDeleted" },
                    { "UpdateDateColumnName", "UpdatedAt" },
                });
            DropTable("dbo.UserRoles",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "CreateDateColumnName", "CreatedAt" },
                    { "SoftDeleteColumnName", "IsDeleted" },
                    { "UpdateDateColumnName", "UpdatedAt" },
                });
            DropTable("dbo.UserLogins",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "CreateDateColumnName", "CreatedAt" },
                    { "SoftDeleteColumnName", "IsDeleted" },
                    { "UpdateDateColumnName", "UpdatedAt" },
                });
            DropTable("dbo.UserClaims",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "CreateDateColumnName", "CreatedAt" },
                    { "SoftDeleteColumnName", "IsDeleted" },
                    { "UpdateDateColumnName", "UpdatedAt" },
                });
            DropTable("dbo.Users",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "CreateDateColumnName", "CreatedAt" },
                    { "SoftDeleteColumnName", "IsDeleted" },
                    { "UpdateDateColumnName", "UpdatedAt" },
                });
            DropTable("dbo.Impersonates",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "CreateDateColumnName", "CreatedAt" },
                    { "SoftDeleteColumnName", "IsDeleted" },
                    { "UpdateDateColumnName", "UpdatedAt" },
                });
            DropTable("dbo.ClientRedirectUris");
            DropTable("dbo.ClientPostLogoutRedirectUris");
            DropTable("dbo.ClientIdPRestrictions");
            DropTable("dbo.ClientSecrets");
            DropTable("dbo.ClientClaims");
            DropTable("dbo.ClientScopes");
            DropTable("dbo.ClientCustomGrantTypes");
            DropTable("dbo.Clients");
            DropTable("dbo.ClientCorsOrigins");
        }

        public void Seed(CloudPlusAuthDbContext context)
        {
            var seedUtilities = new SeedUtilities(new ConfigurationManager(), context);

            seedUtilities.SeedClients()
                .SeedUsers()
                .SeedRoles()
                .SeedRolesRestrictions()
                .SeedPermissions().Save()
                .SeedRolePermissions().Save()
                .SeedUserRoles().Save();
        }
    }
}
