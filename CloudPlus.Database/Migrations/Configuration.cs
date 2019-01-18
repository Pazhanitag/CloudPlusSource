namespace CloudPlus.Database.Migrations
{
	using System;
	using System.Data.Entity.Migrations;
	using System.Linq;

	internal sealed class Configuration : DbMigrationsConfiguration<CldpDbContext>
	{
		public Configuration()
		{
			AutomaticMigrationsEnabled = false;

			//TODO: We don't need this if we are going to recreate initial migration everytime. Later on this needs to be implemented
			var dbMigrator = new DbMigrator(this);
			var pendingMigrations = dbMigrator.GetPendingMigrations().ToArray();

			if (pendingMigrations.Any())
				dbMigrator.Update();

			foreach (var pendingMigration in pendingMigrations)
			{
				var migrationName = pendingMigration.Substring(pendingMigration.IndexOf('_') + 1).Replace("-", "");
				var dbMigration = typeof(Configuration).Assembly.GetType(
					typeof(Configuration).Namespace + "." + migrationName);

				if (!dbMigration.GetInterfaces()
					.Any(x =>
						x.IsGenericType &&
						x.GetGenericTypeDefinition() == typeof(IMigrationSeed<>)))
					continue;

				var context = new CldpDbContext();
				var seedMigration = (IMigrationSeed<CldpDbContext>)Activator.CreateInstance(dbMigration);

				seedMigration.Seed(context);

				context.SaveChanges();
			}
		}
	}
}
