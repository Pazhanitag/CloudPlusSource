using System;
using System.Collections.Generic;
using CloudPlus.Models.Enums;

namespace CloudPlus.Api.ViewModels.Response.Company
{
    public class CompanyViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? ParentId { get; set; }
        public int? CatalogId { get; set; }
        public int CompanyOu { get; set; }
        public string UniqueIdentifier { get; set; }
        public CompanyType Type { get; set; }
        public CompanyStatus Status { get; set; }
        public string Website { get; set; }
        public string SupportSite { get; set; }
        public string ControlPanelSiteUrl { get; set; }
        public string LogoUrl { get; set; }
        public string LogoBase64 { get; set; }
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
        public DateTime CreateDate { get; set; }
        public IEnumerable<DomainViewModel> Domains { get; set; }
        public IEnumerable<DomainViewModel> NewDomains { get; set; }
        public bool WebsiteSameAsPrimaryDomain { get; set; }
        public bool SendWelcomeLetters { get; set; }
        public int NumberOfResellers { get; set; }
        public int NumberOfCustomers { get; set; }
        public int NumberOfUsers { get; set; }
        public int NumberOfTotalResellers { get; set; }
        public int NumberOfTotalCustomers { get; set; }
        
            
    }
}