namespace CloudPlus.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCanAccessColumnForMetrics : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.VendorMetrics", "CanAccess", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.VendorMetrics", "CanAccess");
        }
    }
}
