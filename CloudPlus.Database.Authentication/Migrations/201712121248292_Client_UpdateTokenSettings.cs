namespace CloudPlus.Database.Authentication.Migrations
{
    using CloudPlus.Database.Authentication.Utilities;
    using CloudPlus.Resources;
    using System.Data.Entity.Migrations;
    
    public partial class Client_UpdateTokenSettings : DbMigration, IMigrationSeed<CloudPlusAuthDbContext>
    {
        public override void Up()
        {
        }
        
        public override void Down()
        {
        }

        public void Seed(CloudPlusAuthDbContext context)
        {
            var clientUtilities = new ClientUtilities(context, new ConfigurationManager());

            clientUtilities.UpdateTokenSettings();
        }
    }
}
