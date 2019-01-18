using System;
using System.Collections.Generic;
using CloudPlus.Entities;

namespace CloudPlus.Database.Utilities
{
    public class CompanyUtilities
    {
        private readonly CldpDbContext _dbContext;
        public CompanyUtilities(CldpDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public CompanyUtilities SeedCompanies()
        {
            var cldp = new Company
            {
                CompanyOu = 56001241,
                Name = "CloudPlus",
                Status = 1,
                UniqueIdentifier = Guid.NewGuid().ToString(),
                Type = 0,
                Website = "www.cloudplus.net",
                SupportSiteUrl = "cloudplus.support.com",
                Email = "cloudplus@cloudplus.com",
                PhoneNumber = "1 813 719 1000",
                Country = "United States",
                StreetAddress = "Henderson Blvd., Ste 208-3089",
                City = "Tampa",
                ZipCode = "98052",
                State = "Florida",
                BrandColorText = "#ffffff",
                ControlPanelSiteUrl = "coudplus.cp.site.url",
                BrandColorPrimary = "#AF3333",
                BrandColorSecondary = "#2ba0a3",
                LogoUrl = "8be7c380ee0647ca9102cab750a30e54.png"
            };

            _dbContext.Companies.Add(cldp);

            var mistral = new Company
            {
                Parent = cldp,
                CompanyOu = 9200052,
                Name = "Mistral",
                Status = 1,
                UniqueIdentifier = Guid.NewGuid().ToString(),
                Type = 0,
                Website = "http://mistral.ba/",
                LogoUrl = "8be7c380ee0647ca9102cab750a30e55.png",
                SupportSiteUrl = "suportmistralsolutions.com",
                Email = "mistralmail@mistralsolutions.co",
                PhoneNumber = "12345678",
                StreetAddress = "Milana Preloga 12",
                City = "Sarajevo",
                State = "BiH",
                Country = "BiH",
                BrandColorText = "#ffffff",
                ZipCode = "98052",
                ControlPanelSiteUrl = "mistral.cp.site.url",
                BrandColorPrimary = "#5bb7d6",
                BrandColorSecondary = "#6f1036"
            };

            _dbContext.Companies.Add(mistral);
            
            
            var domains = new List<Domain>
            {
                new Domain
                {
                    Company = mistral,
                    IsPrimary = true,
                    Name = "maestralsolutions.com"
                },
                new Domain
                {
                    Company = mistral,
                    IsPrimary = false,
                    Name = "mistraltechnologies.ba"
                },
                new Domain
                {
                    Company = cldp,
                    IsPrimary = true,
                    Name = "cloudplus.net"
                }
            };
            
            _dbContext.Domains.AddRange(domains);
            
            _dbContext.SaveChanges();

            return this;
        }
    }
}

