using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using CloudPlus.Entities.Identity;
using CloudPlus.Enums.User;
using CloudPlus.Resources;
using IdentityServer3.Core.Models;
using IdentityServer3.EntityFramework.Entities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace CloudPlus.Database.Authentication.Utilities
{
    public class SeedUtilities
    {
        private readonly ConfigurationManager _configurationManager;
        private readonly CloudPlusAuthDbContext _context;
        private readonly RolesUtility _rolesUtility;
        private readonly PermissionsUtility _permissionsUtility;
        public SeedUtilities(ConfigurationManager configurationManager, CloudPlusAuthDbContext context)
        {
            _configurationManager = configurationManager;
            _context = context;
            _rolesUtility = new RolesUtility(context);
            _permissionsUtility = new PermissionsUtility(context);
        }
        public SeedUtilities SeedClients()
        {
            var adminPortalClient = new IdentityServer3.EntityFramework.Entities.Client
            {
                ClientName = _configurationManager.GetByKey("AdminPortal.ClientName"),
                ClientId = _configurationManager.GetByKey("AdminPortal.ClientId"),
                Flow = Flows.Implicit,
                EnableLocalLogin = true,
                Enabled = true,
                AllowAccessTokensViaBrowser = true,
                RequireConsent = false,
                AllowRememberConsent = true,
                AccessTokenType = AccessTokenType.Jwt,
                AccessTokenLifetime = 3600,
                IdentityTokenLifetime = 3600,
                AuthorizationCodeLifetime = 3600,

                // refresh token settings
                AbsoluteRefreshTokenLifetime = 86400,
                SlidingRefreshTokenLifetime = 43200,
                RefreshTokenUsage = TokenUsage.OneTimeOnly,
                RefreshTokenExpiration = TokenExpiration.Sliding,
                PostLogoutRedirectUris = new List<ClientPostLogoutRedirectUri>
                {
                    new ClientPostLogoutRedirectUri
                    {
                        Uri = _configurationManager.GetByKey("AdminPortal.PostLogoutRedirectUri")
                    }
                },

                RedirectUris = new List<ClientRedirectUri>
                {
                    new ClientRedirectUri
                    {
                        Uri = _configurationManager.GetByKey("AdminPortal.RedirectUri")
                    },
                    new ClientRedirectUri
                    {
                        Uri = _configurationManager.GetByKey("AdminPortal.RedirectUri") + "silent.html"
                    }
                },
                AllowedCorsOrigins = new List<ClientCorsOrigin>
                {
                    new ClientCorsOrigin {Origin = _configurationManager.GetByKey("AdminPortal.RedirectUri")}
                },
                AllowedScopes = new List<ClientScope>
                {
                    new ClientScope
                    {
                        Scope = IdentityServer3.Core.Constants.StandardScopes.OpenId
                    },
                    new ClientScope
                    {
                        Scope = IdentityServer3.Core.Constants.StandardScopes.Email
                    },
                    new ClientScope
                    {
                        Scope = IdentityServer3.Core.Constants.StandardScopes.Roles
                    },
                    new ClientScope
                    {
                        Scope = IdentityServer3.Core.Constants.StandardScopes.Profile
                    },
                    new ClientScope
                    {
                        Scope = "read"
                    },
                    new ClientScope
                    {
                        Scope = "write"
                    }
                }
            };

            //Client: CloudPlus Portal API
            //Client type: API
            var cloudPlusPortalApiClient = new IdentityServer3.EntityFramework.Entities.Client
            {
                ClientName = _configurationManager.GetByKey("CloudPlusPortalApi.ClientName"),
                ClientId = _configurationManager.GetByKey("CloudPlusPortalApi.ClientId"),
                Flow = Flows.ClientCredentials,
                Enabled = true,
                AccessTokenLifetime = 3600,

                AllowedScopes = new List<ClientScope>
                {
                    new ClientScope
                    {
                        Scope = "trustedAPI"
                    },
                    new ClientScope
                    {
                        Scope = "read"
                    },
                    new ClientScope
                    {
                        Scope = "write"
                    }
                },
                ClientSecrets = new List<ClientSecret>
                {
                    new ClientSecret { Value = _configurationManager.GetByKey("CloudPlusPortalApi.ClientSecret").Sha256(), Type= IdentityServer3.Core.Constants.SecretTypes.SharedSecret}
                }
            };

            _context.Clients.AddOrUpdate(x => x.ClientId, adminPortalClient);
            _context.Clients.AddOrUpdate(x => x.ClientId, cloudPlusPortalApiClient);

            _context.Scopes.AddOrUpdate(x => x.Name, new IdentityServer3.EntityFramework.Entities.Scope
            {
                Name = IdentityServer3.Core.Constants.StandardScopes.OpenId,
                DisplayName = IdentityServer3.Core.Constants.StandardScopes.OpenId,
                Type = (int)ScopeType.Identity,
                Emphasize = false,
                Enabled = true
            });

            _context.Scopes.AddOrUpdate(x => x.Name, new IdentityServer3.EntityFramework.Entities.Scope
            {
                Name = IdentityServer3.Core.Constants.StandardScopes.Profile,
                DisplayName = IdentityServer3.Core.Constants.StandardScopes.Profile,
                Type = (int)ScopeType.Identity,
                Emphasize = false,
                Enabled = true
            });

            _context.Scopes.AddOrUpdate(x => x.Name, new IdentityServer3.EntityFramework.Entities.Scope
            {
                Name = IdentityServer3.Core.Constants.StandardScopes.Email,
                DisplayName = IdentityServer3.Core.Constants.StandardScopes.Email,
                Type = (int)ScopeType.Resource,
                Emphasize = false,
                Enabled = true
            });

            _context.Scopes.AddOrUpdate(x => x.Name, new IdentityServer3.EntityFramework.Entities.Scope
            {
                Name = IdentityServer3.Core.Constants.StandardScopes.Roles,
                DisplayName = IdentityServer3.Core.Constants.StandardScopes.Roles,
                Type = (int)ScopeType.Resource,
                Emphasize = false,
                Enabled = true
            });

            _context.Scopes.Add(new IdentityServer3.EntityFramework.Entities.Scope
            {
                Name = "trustedAPI",
                DisplayName = "trustedAPI",
                Type = (int)ScopeType.Resource,
                Emphasize = false,
                Enabled = true
            });

            _context.Scopes.AddOrUpdate(x => x.Name, new IdentityServer3.EntityFramework.Entities.Scope
            {
                Name = "read",
                DisplayName = "read",
                Type = (int)ScopeType.Resource,
                Emphasize = false,
                Enabled = true
            });

            _context.Scopes.AddOrUpdate(x => x.Name, new IdentityServer3.EntityFramework.Entities.Scope
            {
                Name = "write",
                DisplayName = "write",
                Type = (int)ScopeType.Resource,
                Emphasize = false,
                Enabled = true
            });

            return this;
        }
        public SeedUtilities SeedUsers()
        {
            var userManager =
                new UserManager<User, int>(
                    new UserStore<User, Role, int, UserLogin, UserRole, UserClaim>(_context));

            var tony = new User
            {
                UserName = "tony@cloudplus.net",
                Email = "tony@cloudplus.net",
                EmailConfirmed = true,
                FirstName = "Tony",
                LastName = "Francisco",
                UserStatus = UserStatus.Active,
                AlternativeEmail = "tony@test.test",
                CompanyId = 1,
                LockoutEnabled = true,
            };

            var jill = new User
            {
                UserName = "jhagenkovach@cloudplus.net",
                Email = "jhagenkovach@cloudplus.net",
                EmailConfirmed = true,
                FirstName = "Jill",
                LastName = "Hagen-Kovach",
                UserStatus = UserStatus.Active,
                AlternativeEmail = "jhagenkovach@cloudplus.net",
                CompanyId = 1,
                LockoutEnabled = true,
            };

            var bill = new User
            {
                UserName = "bvasser@cloudplus.net",
                Email = "bvasser@cloudplus.net",
                EmailConfirmed = true,
                FirstName = "Bill",
                LastName = "Vasser",
                UserStatus = UserStatus.Active,
                AlternativeEmail = "bvasser@cloudplus.net",
                CompanyId = 1,
                LockoutEnabled = true,
            };

            var emirCldp = new User
            {
                UserName = "emir@cloudplus.net",
                Email = "emir@cloudplus.net",
                EmailConfirmed = true,
                FirstName = "Emir",
                LastName = "Kljucanin",
                UserStatus = UserStatus.Active,
                AlternativeEmail = "emir@maestralsolutions.com",
                CompanyId = 1,
                LockoutEnabled = true,
                ProfilePicture = "34ab63ad83a84c39bdec016111dff368.png"
            };

            var irhadCldb = new User
            {
                UserName = "irhad@cloudplus.net",
                Email = "irhad@cloudplus.net",
                EmailConfirmed = true,
                FirstName = "Irhad",
                LastName = "Babic",
                UserStatus = UserStatus.Active,
                AlternativeEmail = "irhadba@maestralsolutions.com",
                CompanyId = 1,
                LockoutEnabled = true,
                ProfilePicture = "34ab63ad83a84c39bdec016111dff368.png"
            };

            var seidCldp = new User
            {
                UserName = "seid@cloudplus.net",
                Email = "seid@cloudplus.net",
                EmailConfirmed = true,
                FirstName = "Seid",
                LastName = "Solak",
                UserStatus = UserStatus.Active,
                AlternativeEmail = "sejo@test.test",
                CompanyId = 1,
                LockoutEnabled = true,
                ProfilePicture = "278ddf410bf84ddc8c8ef0cfd72add4a.png"
            };
            var adnanCldp = new User
            {
                UserName = "adnan@cloudplus.net",
                Email = "adnan@cloudplus.net",
                EmailConfirmed = true,
                FirstName = "Adnan",
                LastName = "Mulalic",
                UserStatus = UserStatus.Active,
                AlternativeEmail = "adnan@test.test",
                CompanyId = 1,
                LockoutEnabled = true,
                ProfilePicture = "1a2b41e5c57b4e0986599ff3a609c597.png"
            };
            var dalilaCldp = new User
            {
                UserName = "dalila@cloudplus.net",
                Email = "dalila@cloudplus.net",
                EmailConfirmed = true,
                FirstName = "Dalila",
                LastName = "Avdukic",
                UserStatus = UserStatus.Active,
                AlternativeEmail = "dalila@test.test",
                CompanyId = 1,
                LockoutEnabled = true,
                ProfilePicture = "06b85e848f23462ab13a7fbc1dc6df80.png"
            };
            var amerCldp = new User
            {
                UserName = "amer@cloudplus.net",
                Email = "amer@cloudplus.net",
                EmailConfirmed = true,
                FirstName = "Amer",
                LastName = "Ratkovic",
                UserStatus = UserStatus.Active,
                AlternativeEmail = "amer@test.test",
                CompanyId = 1,
                LockoutEnabled = true,
                ProfilePicture = "5fd88872a466468386c6ffb57fe3ed8d.png"
            };
            var farisCldp = new User
            {
                UserName = "faris@cloudplus.net",
                Email = "faris@cloudplus.net",
                EmailConfirmed = true,
                FirstName = "Faris",
                LastName = "Fetahagic",
                UserStatus = UserStatus.Active,
                AlternativeEmail = "faris@test.test",
                CompanyId = 1,
                LockoutEnabled = true,
                ProfilePicture = "339353576c5a419080851ab867b22ae8.png"
            };
            var mersihaCldp = new User
            {
                UserName = "mersiha@cloudplus.net",
                Email = "mersiha@cloudplus.net",
                EmailConfirmed = true,
                FirstName = "Mersiha",
                LastName = "Seljpic",
                UserStatus = UserStatus.Active,
                AlternativeEmail = "mersiha@test.test",
                CompanyId = 1,
                LockoutEnabled = true,
                ProfilePicture = "d68a7be3aed740c481e32a180c072461.png"
            };

            var uzeirCldp = new User
            {
                UserName = "uzeir@cloudplus.net",
                Email = "uzeir@cloudplus.net",
                EmailConfirmed = true,
                FirstName = "Uzeir",
                LastName = "Basic",
                UserStatus = UserStatus.Active,
                AlternativeEmail = "uzeir@test.test",
                CompanyId = 1,
                LockoutEnabled = true,
                ProfilePicture = "503a28af4ea34b598405906c1b1761f1.png"
            };

            var emirMistral = new User
            {
                UserName = "emir@maestralsolutions.com",
                Email = "emir@maestralsolutions.com",
                EmailConfirmed = true,
                FirstName = "Emir",
                LastName = "Kljucanin",
                UserStatus = UserStatus.Active,
                AlternativeEmail = "kljuco@test.test",
                CompanyId = 2,
                LockoutEnabled = true,
                ProfilePicture = "34ab63ad83a84c39bdec016111dff368.png"
            };
            var irhadMistral = new User
            {
                UserName = "irhadba@maestralsolutions.com",
                Email = "irhadba@maestralsolutions.com",
                EmailConfirmed = true,
                FirstName = "Irhad",
                LastName = "Babic",
                UserStatus = UserStatus.Active,
                AlternativeEmail = "irhad@test.test",
                CompanyId = 2,
                LockoutEnabled = true,
                ProfilePicture = "34ab63ad83a84c39bdec016111dff368.png"
            };
            var seidMistral = new User
            {
                UserName = "seidso@maestralsolutions.com",
                Email = "seidso@maestralsolutions.com",
                EmailConfirmed = true,
                FirstName = "Seid",
                LastName = "Solak",
                UserStatus = UserStatus.Active,
                AlternativeEmail = "sejo@test.test",
                CompanyId = 2,
                LockoutEnabled = true,
                ProfilePicture = "278ddf410bf84ddc8c8ef0cfd72add4a.png"
            };
            var adnanMistral = new User
            {
                UserName = "adnanmu@maestralsolutions.com",
                Email = "adnanmu@maestralsolutions.com",
                EmailConfirmed = true,
                FirstName = "Adnan",
                LastName = "Mulalic",
                UserStatus = UserStatus.Active,
                AlternativeEmail = "adnan@test.test",
                CompanyId = 2,
                LockoutEnabled = true,
                ProfilePicture = "1a2b41e5c57b4e0986599ff3a609c597.png"
            };
            var dalilaMistral = new User
            {
                UserName = "dalilaav@maestralsolutions.com",
                Email = "dalilaav@maestralsolutions.com",
                EmailConfirmed = true,
                FirstName = "Dalila",
                LastName = "Avdukic",
                UserStatus = UserStatus.Active,
                AlternativeEmail = "dalila@test.test",
                CompanyId = 2,
                LockoutEnabled = true,
                ProfilePicture = "06b85e848f23462ab13a7fbc1dc6df80.png"
            };
            var amerMistral = new User
            {
                UserName = "amerra@maestralsolutions.com",
                Email = "amerra@maestralsolutions.com",
                EmailConfirmed = true,
                FirstName = "Amer",
                LastName = "Ratkovic",
                UserStatus = UserStatus.Active,
                AlternativeEmail = "amer@test.test",
                CompanyId = 2,
                LockoutEnabled = true,
                ProfilePicture = "5fd88872a466468386c6ffb57fe3ed8d.png"
            };
            var farisMistral = new User
            {
                UserName = "farisfe@maestralsolutions.com",
                Email = "farisfe@maestralsolutions.com",
                EmailConfirmed = true,
                FirstName = "Faris",
                LastName = "Fetahagic",
                UserStatus = UserStatus.Active,
                AlternativeEmail = "faris@test.test",
                CompanyId = 2,
                LockoutEnabled = true,
                ProfilePicture = "339353576c5a419080851ab867b22ae8.png"
            };
            var mersihaMistral = new User
            {
                UserName = "mersihase@maestralsolutions.com",
                Email = "mersihase@maestralsolutions.com",
                EmailConfirmed = true,
                FirstName = "Mersiha",
                LastName = "Seljpic",
                UserStatus = UserStatus.Active,
                AlternativeEmail = "mersiha@test.test",
                CompanyId = 2,
                LockoutEnabled = true,
                ProfilePicture = "d68a7be3aed740c481e32a180c072461.png"
            };

            var uzeirMistral = new User
            {
                UserName = "uzeir@maestralsolutions.com",
                Email = "uzeir@maestralsolutions.com",
                EmailConfirmed = true,
                FirstName = "Uzeir",
                LastName = "Basic",
                UserStatus = UserStatus.Active,
                AlternativeEmail = "uzeir@test.test",
                CompanyId = 2,
                LockoutEnabled = true,
                ProfilePicture = "503a28af4ea34b598405906c1b1761f1.png"
            };
            
            userManager.Create(adnanMistral);
            userManager.Create(farisMistral);
            userManager.Create(dalilaMistral);
            userManager.Create(uzeirMistral);
            userManager.Create(mersihaMistral);
            userManager.Create(amerMistral);
            userManager.Create(seidMistral);
            userManager.Create(emirMistral);
            userManager.Create(irhadMistral);

            userManager.Create(tony);
            userManager.Create(jill);
            userManager.Create(bill);
            userManager.Create(adnanCldp);
            userManager.Create(farisCldp);
            userManager.Create(dalilaCldp);
            userManager.Create(uzeirCldp);
            userManager.Create(mersihaCldp);
            userManager.Create(amerCldp);
            userManager.Create(seidCldp);
            userManager.Create(emirCldp);
            userManager.Create(irhadCldb);
            
            return this;
        }
        public SeedUtilities SeedRoles()
        {
            var masterAdmin = new Role
            {
                Name = "MasterAdmin",
                FriendlyName = "Master Administrator",
                Description = ""
            };
            var resellerAdmin = new Role
            {
                Name = "ResellerAdmin",
                FriendlyName = "Reseller Administrator",
                Description = "Reseller Administrators have the ability to add, edit and delete customer accounts and users as well as edit available services and pricing."
            };
            var customerAdmin = new Role
            {
                Name = "CustomerAdmin",
                FriendlyName = "Customer Administrator",
                Description = "Customer Administrators have the ability to add, edit, and delete users and services, as well as edit company information."
            };
       
            var user = new Role
            {
                Name = "User",
                FriendlyName = "User",
                Description = "Users can edit their own profile information, as well as view company information and Support menu items. Users do not have administrative capabilities."
            };

            _context.Roles.Add(masterAdmin);
            _context.Roles.Add(resellerAdmin);
            _context.Roles.Add(customerAdmin);
            _context.Roles.Add(user);

            _context.SaveChanges();

            SeedRolesRestrictions();
            return this;
        }
        
        public SeedUtilities SeedRolesRestrictions()
        {
            var jsonSerializer = new JsonSerializer();

            _rolesUtility.MasterAdminRole.AvailableRoles = jsonSerializer.Serialize(new List<Role>
            {
                _rolesUtility.MasterAdminRole,
                _rolesUtility.ResellerAdminRole,
                _rolesUtility.CustomerAdminRole,
                _rolesUtility.UserRole
            });
            _rolesUtility.ResellerAdminRole.AvailableRoles = jsonSerializer.Serialize(new List<Role>
            {
                _rolesUtility.ResellerAdminRole,
                _rolesUtility.CustomerAdminRole,
                _rolesUtility.UserRole
            });
            _rolesUtility.CustomerAdminRole.AvailableRoles = jsonSerializer.Serialize(new List<Role>
            {
                _rolesUtility.CustomerAdminRole,
                _rolesUtility.UserRole
            });

            _rolesUtility.UserRole.AvailableRoles = jsonSerializer.Serialize(new List<Role>());

            _context.SaveChanges();

            return this;
        }
        public SeedUtilities SeedPermissions()
        {
            _context.Permissions.AddOrUpdate(x => x.Name,
                new Permission
                {
                    Name = "ViewUsers"
                },
                new Permission
                {
                    Name = "EditUsers"
                },
                new Permission
                {
                    Name = "AddUsers"
                },
                new Permission
                {
                    Name = "DeleteUsers"
                },
                new Permission
                {
                    Name = "ViewAccounts"
                },
                new Permission
                {
                    Name = "EditAccounts"
                },
                new Permission
                {
                    Name = "AddAccounts"
                },
                new Permission
                {
                    Name = "DeleteAccounts"
                },
                new Permission
                {
                    Name = "ViewPriceCatalog"
                }, new Permission
                {
                    Name = "SetMsrpFixed"
                }, 
                new Permission
                {
                    Name = "ViewProductCatalog"
                });

            return this;
        }

        public SeedUtilities SeedRolePermissions()
        {
            _context.RolePermissions.AddOrUpdate(
                new RolePermission
                {
                    Role = _rolesUtility.MasterAdminRole,
                    Permission = _permissionsUtility.ViewUsersPermission
                },
                new RolePermission
                {
                    Role = _rolesUtility.ResellerAdminRole,
                    Permission = _permissionsUtility.ViewUsersPermission
                },
                new RolePermission
                {
                    Role = _rolesUtility.CustomerAdminRole,
                    Permission = _permissionsUtility.ViewUsersPermission
                },
                new RolePermission
                {
                    Role = _rolesUtility.MasterAdminRole,
                    Permission = _permissionsUtility.EditUsersPermission
                },
                new RolePermission
                {
                    Role = _rolesUtility.ResellerAdminRole,
                    Permission = _permissionsUtility.EditUsersPermission
                },
                new RolePermission
                {
                    Role = _rolesUtility.CustomerAdminRole,
                    Permission = _permissionsUtility.EditUsersPermission
                },
                new RolePermission
                {
                    Role = _rolesUtility.MasterAdminRole,
                    Permission = _permissionsUtility.AddUsersPermission
                },
                new RolePermission
                {
                    Role = _rolesUtility.ResellerAdminRole,
                    Permission = _permissionsUtility.AddUsersPermission
                },
                new RolePermission
                {
                    Role = _rolesUtility.CustomerAdminRole,
                    Permission = _permissionsUtility.AddUsersPermission
                },
                new RolePermission
                {
                    Role = _rolesUtility.MasterAdminRole,
                    Permission = _permissionsUtility.DeleteUsersPermission
                },
                new RolePermission
                {
                    Role = _rolesUtility.ResellerAdminRole,
                    Permission = _permissionsUtility.DeleteUsersPermission
                },
                new RolePermission
                {
                    Role = _rolesUtility.CustomerAdminRole,
                    Permission = _permissionsUtility.DeleteUsersPermission
                },
                new RolePermission
                {
                    Role = _rolesUtility.MasterAdminRole,
                    Permission = _permissionsUtility.ViewAccountsPermission
                },
                new RolePermission
                {
                    Role = _rolesUtility.ResellerAdminRole,
                    Permission = _permissionsUtility.ViewAccountsPermission
                },
                new RolePermission
                {
                    Role = _rolesUtility.MasterAdminRole,
                    Permission = _permissionsUtility.EditAccountsPermission
                },
                new RolePermission
                {
                    Role = _rolesUtility.ResellerAdminRole,
                    Permission = _permissionsUtility.EditAccountsPermission
                },
                new RolePermission
                {
                    Role = _rolesUtility.MasterAdminRole,
                    Permission = _permissionsUtility.AddAccountsPermission
                },
                new RolePermission
                {
                    Role = _rolesUtility.ResellerAdminRole,
                    Permission = _permissionsUtility.AddAccountsPermission
                },
                new RolePermission
                {
                    Role = _rolesUtility.MasterAdminRole,
                    Permission = _permissionsUtility.DeleteAccountsPermission
                },
                new RolePermission
                {
                    Role = _rolesUtility.ResellerAdminRole,
                    Permission = _permissionsUtility.DeleteAccountsPermission
                },
                new RolePermission
                {
                    Role = _rolesUtility.MasterAdminRole,
                    Permission = _permissionsUtility.ViewProductCatalogPermission
                },
                new RolePermission
                {
                    Role = _rolesUtility.ResellerAdminRole,
                    Permission = _permissionsUtility.ViewProductCatalogPermission
                },
                new RolePermission
                {
                    Role = _rolesUtility.CustomerAdminRole,
                    Permission = _permissionsUtility.ViewProductCatalogPermission
                },
                new RolePermission
                {
                    Role = _rolesUtility.MasterAdminRole,
                    Permission = _permissionsUtility.ViewPriceCatalogPermission
                },
                new RolePermission
                {
                    Role = _rolesUtility.ResellerAdminRole,
                    Permission = _permissionsUtility.ViewPriceCatalogPermission
                },
                new RolePermission
                {
                    Role = _rolesUtility.MasterAdminRole,
                    Permission = _permissionsUtility.SetMsrpFixedPermission
                });
            return this;
        }
        public SeedUtilities SeedUserRoles()
        {
            var emir = _context.Users.FirstOrDefault(x => x.UserName == "emir@maestralsolutions.com");
            var seid = _context.Users.FirstOrDefault(x => x.UserName == "seidso@maestralsolutions.com");
            var amer = _context.Users.FirstOrDefault(x => x.UserName == "amerra@maestralsolutions.com");
            var faris = _context.Users.FirstOrDefault(x => x.UserName == "farisfe@maestralsolutions.com");
            var uzeir = _context.Users.FirstOrDefault(x => x.UserName == "uzeir@maestralsolutions.com");
            var adnan = _context.Users.FirstOrDefault(x => x.UserName == "adnanmu@maestralsolutions.com");
            var dalila = _context.Users.FirstOrDefault(x => x.UserName == "dalilaav@maestralsolutions.com");
            var mersiha = _context.Users.FirstOrDefault(x => x.UserName == "mersihase@maestralsolutions.com");
            var irhad = _context.Users.FirstOrDefault(x => x.UserName == "irhadba@maestralsolutions.com");
            var irhadCldp = _context.Users.FirstOrDefault(x => x.UserName == "irhad@cloudplus.net");
            var emirCldp = _context.Users.FirstOrDefault(x => x.UserName == "emir@cloudplus.net");
            var seidCldp = _context.Users.FirstOrDefault(x => x.UserName == "seid@cloudplus.net");
            var amerCldp = _context.Users.FirstOrDefault(x => x.UserName == "amer@cloudplus.net");
            var farisCldp = _context.Users.FirstOrDefault(x => x.UserName == "faris@cloudplus.net");
            var uzeirCldp = _context.Users.FirstOrDefault(x => x.UserName == "uzeir@cloudplus.net");
            var adnanCldp = _context.Users.FirstOrDefault(x => x.UserName == "adnan@cloudplus.net");
            var dalilaCldp = _context.Users.FirstOrDefault(x => x.UserName == "dalila@cloudplus.net");
            var mersihaCldp = _context.Users.FirstOrDefault(x => x.UserName == "mersiha@cloudplus.net");
            var tony = _context.Users.FirstOrDefault(x => x.UserName == "tony@cloudplus.net");
            var jill = _context.Users.FirstOrDefault(x => x.UserName == "jhagenkovach@cloudplus.net");
            var bill = _context.Users.FirstOrDefault(x => x.UserName == "bvasser@cloudplus.net");

            _context.UserRoles.AddOrUpdate(
                new UserRole
                {
                    User = seid,
                    Role = _rolesUtility.ResellerAdminRole
                },
                new UserRole
                {
                    User = amer,
                    Role = _rolesUtility.ResellerAdminRole
                },
                new UserRole
                {
                    User = faris,
                    Role = _rolesUtility.ResellerAdminRole
                },
                new UserRole
                {
                    User = uzeir,
                    Role = _rolesUtility.ResellerAdminRole
                },
                new UserRole
                {
                    User = adnan,
                    Role = _rolesUtility.ResellerAdminRole
                },
                new UserRole
                {
                    User = dalila,
                    Role = _rolesUtility.ResellerAdminRole
                },
                new UserRole
                {
                    User = mersiha,
                    Role = _rolesUtility.ResellerAdminRole
                },
                new UserRole
                {
                    User = emir,
                    Role = _rolesUtility.ResellerAdminRole
                },
                new UserRole
                {
                    User = seidCldp,
                    Role = _rolesUtility.MasterAdminRole
                },
                new UserRole
                {
                    User = amerCldp,
                    Role = _rolesUtility.MasterAdminRole
                },
                new UserRole
                {
                    User = farisCldp,
                    Role = _rolesUtility.MasterAdminRole
                },
                new UserRole
                {
                    User = uzeirCldp,
                    Role = _rolesUtility.MasterAdminRole
                },
                new UserRole
                {
                    User = adnanCldp,
                    Role = _rolesUtility.MasterAdminRole
                },
                new UserRole
                {
                    User = dalilaCldp,
                    Role = _rolesUtility.MasterAdminRole
                },
                new UserRole
                {
                    User = mersihaCldp,
                    Role = _rolesUtility.MasterAdminRole
                },
                new UserRole
                {
                    User = emirCldp,
                    Role = _rolesUtility.MasterAdminRole
                },
                new UserRole
                {
                    User = tony,
                    Role = _rolesUtility.MasterAdminRole
                },
                new UserRole
                {
                    User = jill,
                    Role = _rolesUtility.MasterAdminRole
                },
                new UserRole
                {
                    User = bill,
                    Role = _rolesUtility.MasterAdminRole
                },
                new UserRole
                {
                   User = irhadCldp,
                   Role = _rolesUtility.MasterAdminRole
                },
                new UserRole
                {
                    User = irhad,
                    Role = _rolesUtility.ResellerAdminRole
                }
            );
            
            return this;
        }

	    public SeedUtilities AddPermissions()
	    {
		    _context.Permissions.AddOrUpdate(x => x.Name,
			    new Permission
			    {
				    Name = "ViewMyCompany"
				},
			    new Permission
			    {
				    Name = "EditMyProfile"
				}
		    );

		    return this;
	    }

		public SeedUtilities AddRolePermissions()
	    {
		    _context.RolePermissions.AddOrUpdate(
			    new RolePermission
			    {
				    Role = _rolesUtility.MasterAdminRole,
				    Permission = _permissionsUtility.EditMyProfile
			    },
			    new RolePermission
			    {
				    Role = _rolesUtility.ResellerAdminRole,
				    Permission = _permissionsUtility.EditMyProfile
			    },
				new RolePermission
				{
					Role = _rolesUtility.CustomerAdminRole,
					Permission = _permissionsUtility.EditMyProfile
				},
			    new RolePermission
			    {
				    Role = _rolesUtility.UserRole,
				    Permission = _permissionsUtility.EditMyProfile
			    },
				new RolePermission
			    {
				    Role = _rolesUtility.UserRole,
				    Permission = _permissionsUtility.ViewMyCompany
			    }
			);
		    return this;
	    }

		public SeedUtilities Save()
        {
            _context.SaveChanges();
            return this;
        }
    }
}
