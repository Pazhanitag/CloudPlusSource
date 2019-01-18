using System.Collections.Generic;
using CloudPlus.Models.Office365.Transition;

namespace CloudPlus.QueueModels.Office365.Transition.Commands
{
    public interface IOffice365TransitionReportCommand
    {
        int CompanyId { get; set; }
        IEnumerable<Office365TransitionProductItemModel> ProductItems { get; set; }
    }
}
