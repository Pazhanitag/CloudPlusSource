using System;
using System.Collections.Generic;
using CloudPlus.Models.Domain;
using CloudPlus.Models.Enums;

namespace CloudPlus.Models.Company
{
    public class CompanyModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? ParentId { get; set; }
        public int CompanyOu { get; set; }
        public string UniqueIdentifier { get; set; }
        public CompanyType Type { get; set; }
        public CompanyStatus Status { get; set; }
        public string Website { get; set; }
        public string SupportSiteUrl { get; set; }
        public string ControlPanelSiteUrl { get; set; }
        public string LogoUrl { get; set; }
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
        public int? CatalogId { get; set; }
        public DateTime CreateDate { get; set; }
        public IEnumerable<DomainModel> Domains { get; set; }
        public bool SendWelcomeLetters { get; set; }
		public int NumberOfCustomers { get; set; }
		public int NumberOfResellers { get; set; }
		public int NumberOfUsers { get; set; }
        public int numberOfTotalResellers { get; set; }
        public int numberOfTotalCustomers { get; set; }
    }
}
