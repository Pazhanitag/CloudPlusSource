using CloudPlus.Database.Authentication.Utilities;
using CloudPlus.Resources;

namespace CloudPlus.Database.Authentication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPermissions : DbMigration, IMigrationSeed<CloudPlusAuthDbContext>
    {
	    public override void Up()
	    {
	    }

	    public override void Down()
	    {
	    }

	    public void Seed(CloudPlusAuthDbContext context)
	    {
		    var seedUtilities = new SeedUtilities(new ConfigurationManager(), context);
		    seedUtilities.AddPermissions().Save().AddRolePermissions().Save();
	    }
	}
}
