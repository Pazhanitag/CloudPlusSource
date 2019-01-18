using System.Collections.Generic;
using CloudPlus.Models.Office365.Transition;

namespace CloudPlus.Api.ViewModels.Request.Transition
{
    public class Office365TransitionViewModel
    {
        public int CompanyId { get; set; }
        public string Office365CustomerId { get; set; }
        public int ProductId { get; set; }
        public IEnumerable<string> Domains { get; set; }
        public IEnumerable<Office365TransitionProductItemModel> ProductItems { get; set; }
    }
}
