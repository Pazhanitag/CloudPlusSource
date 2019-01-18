using System.Collections.Generic;

namespace CloudPlus.Models.Office365.Transition
{
    public class Office365TransitionMatchingDataModel
    {
        public int CompanyId { get; set; }
        public string Office365CustomerId { get; set; }
        public IEnumerable<string> Domains { get; set; }
        public IEnumerable<Office365TransitionProductItemModel> ProductItems { get; set; }
    }
}
