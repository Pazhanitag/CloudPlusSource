using System.Threading.Tasks;

namespace CloudPlus.Workflows.Common.Workflow
{
    public interface IWorkflow<in T> where T : class
    {
        Task Execute(T context);
    }
}
