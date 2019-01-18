using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using CloudPlus.Entities.Catalog;

namespace CloudPlus.Entities
{
    public class Company : IBaseEntity
    {
        public Company()
        {
            CreateDate = DateTime.UtcNow;
            UpdateDate = DateTime.UtcNow;
            Domains = new List<Domain>();
            CompanyCatalogs = new List<CompanyCatalog>();
            MyCompanies = new List<Company>();
        }

        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public int CompanyOu { get; set; }
        public string UniqueIdentifier { get; set; }
        [Required]
        public int Type { get; set; }
        public Company Parent { get; set; }
        public int? ParentId { get; set; }        
        public string Website { get; set; }
        public string LogoUrl { get; set; }
        public string SupportSiteUrl { get; set; }
        public string ControlPanelSiteUrl { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string StreetAddress { get; set; }
        public string City { get; set; }
        public string ZipCode { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string BrandColorPrimary { get; set; }
        public string BrandColorSecondary { get; set; }
        public string BrandColorText { get; set; }
        public int Status { get; set; }
        public List<Domain> Domains { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }

        public List<CompanyCatalog> CompanyCatalogs { get; set; }
        public List<Company> MyCompanies { get; set; }
    }
}
