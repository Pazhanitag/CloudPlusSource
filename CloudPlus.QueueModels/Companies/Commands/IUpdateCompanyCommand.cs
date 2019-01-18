using CloudPlus.Models.Domain;
using System.Collections.Generic;

namespace CloudPlus.QueueModels.Companies.Commands
{
    public interface IUpdateCompanyCommand : IQueueModel
    {
        int CompanyId { get; set; }
        int? ParentId { get; set; }
        string Name { get; set; }
        string Website { get; set; }
        string LogoBase64 { get; set; }
        string Logo { get; set; }
        string SupportSiteUrl { get; set; }
        string ControlPanelSiteUrl { get; set; }
        string Email { get; set; }
        string PhoneNumber { get; set; }
        string StreetAddress { get; set; }
        string City { get; set; }
        string ZipCode { get; set; }
        string State { get; set; }
        string Country { get; set; }
        string BrandColorPrimary { get; set; }
        string BrandColorSecondary { get; set; }
        string BrandColorText { get; set; }
        int? CatalogId { get; set; }
        List<DomainModel> Domains { get; set; }
    }
}
