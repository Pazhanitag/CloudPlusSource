namespace CloudPlus.Database.Authentication.Migrations
{
    using CloudPlus.Database.Authentication.Utilities;
    using CloudPlus.Resources;
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdatePermisionsExternalSignup : DbMigration, IMigrationSeed<CloudPlusAuthDbContext>
    {
        public override void Up()
        {
        }
        
        public override void Down()
        {
        }
        public void Seed(CloudPlusAuthDbContext context)
        {
            var seedPermisions = new SeedPermisions(context);

            seedPermisions.UpdatePermisionsExternalSignup();
        }
    }
}
