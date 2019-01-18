using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudPlus.Models.Support
{
    public class SupportModel
    {
        public int id { get; set; }
        public string name { get; set; }
        public string imgUrl { get; set; }
        public DateTime activatedDate { get; set; }
        public IEnumerable<CustomControlPanelModel> CustomControlPanel { get; set; }
    }

    public class CustomControlPanelModel
    {
        public string urls { get; set; }
        public Status Status { get; set; }
    }
    public class Status
    {
        public int statusId { get; set; }
        public string statusValue { get; set; }
        public string statusIcon { get; set; }
    }

}
