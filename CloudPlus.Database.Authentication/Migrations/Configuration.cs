using System;
using System.Data.Entity.Migrations;
using System.Linq;
using CloudPlus.Database.Authentication.Utilities;
using CloudPlus.Resources;

namespace CloudPlus.Database.Authentication.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<CloudPlusAuthDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "CloudPlus.Authentication.Database.CloudPlusAuthDbContext";

            var dbMigrator = new DbMigrator(this);
            var pendingMigrations = dbMigrator.GetPendingMigrations().ToArray();

            if (pendingMigrations.Any())
                dbMigrator.Update();

            foreach (var pendingMigration in pendingMigrations)
            {
                var migrationName = pendingMigration.Substring(pendingMigration.IndexOf('_') + 1).Replace("-", "");
                var dbMigration = typeof(Configuration).Assembly.GetType(
                    typeof(Configuration).Namespace + "." + migrationName);

                if (!dbMigration.GetInterfaces().Any(x =>
                    x.IsGenericType &&
                    x.GetGenericTypeDefinition() == typeof(IMigrationSeed<>)))
                    continue;

                var context = new CloudPlusAuthDbContext();
                var seedMigration = (IMigrationSeed<CloudPlusAuthDbContext>)Activator.CreateInstance(dbMigration);

                seedMigration.Seed(context);

                context.SaveChanges();
            }
        }
    }
}
