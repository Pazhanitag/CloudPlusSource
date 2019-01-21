namespace CloudPlus.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddIsAddoninoffice365offer : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Office365Offer", "IsAddon", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Office365Offer", "IsAddon");
        }
    }
}
