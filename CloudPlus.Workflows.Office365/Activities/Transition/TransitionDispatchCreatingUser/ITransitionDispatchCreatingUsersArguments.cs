using System.Collections.Generic;
using CloudPlus.Models.Office365.Transition;

namespace CloudPlus.Workflows.Office365.Activities.Transition.TransitionDispatchCreatingUser
{
    public interface ITransitionDispatchCreatingUsersArguments
    {
        int CompanyId { get; set; }
        string Office365CustomerId { get; set; }
        IEnumerable<string> Domains { get; set; }
        IEnumerable<Office365TransitionProductItemModel> ProductItems { get; set; }
    }
}
