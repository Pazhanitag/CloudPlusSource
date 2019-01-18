using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using CloudPlus.Database.Common.Attributes;
using CloudPlus.Entities.Identity;
using IdentityServer3.EntityFramework;
using IdentityServer3.EntityFramework.Entities;
using Microsoft.AspNet.Identity.EntityFramework;
using Client = IdentityServer3.EntityFramework.Entities.Client;
using Scope = IdentityServer3.EntityFramework.Entities.Scope;

namespace CloudPlus.Database.Authentication
{
    public class CloudPlusAuthDbContext : IdentityDbContext<User, Role, int, UserLogin, UserRole, UserClaim>,
        IClientConfigurationDbContext, IScopeConfigurationDbContext
    {
        public CloudPlusAuthDbContext()
            : base("CloudPlusAuthDb")
        {
            //Enable eager loading
            Configuration.ProxyCreationEnabled = false;
            Configuration.LazyLoadingEnabled = false;
        }

        public DbSet<Client> Clients { get; set; }
        public DbSet<Scope> Scopes { get; set; }
        public DbSet<ClientPostLogoutRedirectUri> ClientPostLogoutRedirectUris { get; set; }
        public DbSet<ClientRedirectUri> ClientRedirectUri { get; set; }
        public DbSet<ClientCorsOrigin> ClientCorsOrigins { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<RolePermission> RolePermissions { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<Impersonate> Impersonates { get; set; }

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

            modelBuilder.Entity<User>().Map(m => m.ToTable("Users"));

            modelBuilder.Entity<User>()
                .Property(p => p.UserName).HasMaxLength(50)
                .HasColumnAnnotation(
                    IndexAnnotation.AnnotationName,
                    new IndexAnnotation(new IndexAttribute("UserNameIndex", 1) { IsUnique = false }));

            modelBuilder.Entity<Role>().Map(m => m.ToTable("Roles"));
            modelBuilder.Entity<UserRole>().Map(m => m.ToTable("UserRoles"));
            modelBuilder.Entity<UserLogin>().Map(m => m.ToTable("UserLogins"));
            modelBuilder.Entity<UserClaim>().Map(m => m.ToTable("UserClaims"));
            modelBuilder.Entity<Permission>().Map(m => m.ToTable("Permissions"));
        }
    }
}
