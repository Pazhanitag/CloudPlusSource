namespace CloudPlus.Database.Migrations
{
    using CloudPlus.Database.Utilities;
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Office365Roles_AddDisplayNameAndOrd_Seed : DbMigration, IMigrationSeed<CldpDbContext>
    {
        public override void Up()
        {
            AddColumn("dbo.Office365Role", "DisplayName", c => c.String());
            AddColumn("dbo.Office365Role", "Ord", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Office365Role", "Ord");
            DropColumn("dbo.Office365Role", "DisplayName");
        }

        public void Seed(CldpDbContext context)
        {
            var office365Utilities = new Office365Utilities(context);

            office365Utilities.SeedOffice365RolesDisplayNamesAndOrd();
        }
    }
}
