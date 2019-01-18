using MassTransit.Courier;

namespace CloudPlus.Workflows.Company.Activities.AssignCatalog
{
    public interface IAssignCatalogActivity: Activity<IAssignCatalogArguments, IAssignCatalogLog>
    {
    }
}
