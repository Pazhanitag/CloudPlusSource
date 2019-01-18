using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudPlus.Database.Utilities
{
    public class CategoryUtilities
    {
        private readonly CldpDbContext _dbContext;

        public CategoryUtilities(CldpDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public CategoryUtilities SeedCategory()
        {
            var office365 = _dbContext.Categories.FirstOrDefault(category => category.Name == "Office 365");

            if(office365 != null)
            {
                office365.Vendor = "Microsoft Corporation";
                office365.ImgUrl = "https://rickzeleznik.files.wordpress.com/2014/04/officelogoorange_print.png";

                _dbContext.Categories.Attach(office365);
                _dbContext.Entry(office365).State = EntityState.Modified;

                _dbContext.SaveChanges();
            }

            return this;
        }
    }
}
