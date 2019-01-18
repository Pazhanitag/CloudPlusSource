using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudPlus.Models.Company
{
    public class CompanyDetailsResponseDto
    {
        public int Id { get; set; }
        public string Website { get; set; }
        public string LogoUrl { get; set; }
        public string SupportSiteUrl { get; set; }

        #region Contact Informations
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string StreetAddress { get; set; }
        public string City { get; set; }
        public string ZipCode { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        #endregion

        #region Branding
        public string BrandColorPrimary { get; set; }
        public string BrandColorSecondary { get; set; }
        public string BrandColorText { get; set; }
        #endregion
    }
}
