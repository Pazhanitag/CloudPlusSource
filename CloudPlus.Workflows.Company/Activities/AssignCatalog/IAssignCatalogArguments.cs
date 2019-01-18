
using CloudPlus.Models.Company;

namespace CloudPlus.Workflows.Company.Activities.AssignCatalog
{
    public interface IAssignCatalogArguments
    {
        int? ParentId { get; set; }
        int CompanyId { get; set; }
        int? CatalogId { get; set; }
    }
}
