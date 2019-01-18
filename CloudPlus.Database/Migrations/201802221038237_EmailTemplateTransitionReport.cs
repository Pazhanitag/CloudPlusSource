using CloudPlus.Database.Utilities;

namespace CloudPlus.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EmailTemplateTransitionReport : DbMigration, IMigrationSeed<CldpDbContext>
    {
        public override void Up()
        {
        }
        
        public override void Down()
        {
        }

        public void Seed(CldpDbContext context)
        {
            var utilities = new EmailTemplatesUtilities(context);

            utilities.SeedTransitionReportEmailTemplate();
        }
    }
}
