namespace CloudPlus.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveProductAddOn : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ProductAddons", "ProductItem_Id", "dbo.ProductItems");
            DropIndex("dbo.ProductAddons", new[] { "ProductItem_Id" });
            DropColumn("dbo.ProductAddons", "ProductItem_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ProductAddons", "ProductItem_Id", c => c.Int());
            CreateIndex("dbo.ProductAddons", "ProductItem_Id");
            AddForeignKey("dbo.ProductAddons", "ProductItem_Id", "dbo.ProductItems", "Id");
        }
    }
}
