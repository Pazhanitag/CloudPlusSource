using System.Collections.Generic;
using CloudPlus.Models.Office365.Transition;

namespace CloudPlus.QueueModels.Office365.Transition.Commands
{
    public interface IOffice365TransitionCommand
    {
        int CompanyId { get; set; }
        string Office365CustomerId { get; set; }
        int ProductId { get; set; }
        IEnumerable<string> Domains { get; set; }
        IEnumerable<Office365TransitionProductItemModel> ProductItems { get; set; }
    }
}
