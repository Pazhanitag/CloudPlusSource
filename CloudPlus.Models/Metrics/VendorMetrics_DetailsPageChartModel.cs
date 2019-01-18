using System;
using System.Collections.Generic;
using CloudPlus.Models.Domain;
using CloudPlus.Models.Enums;

namespace CloudPlus.Models.Metrics
{
    public class VendorMetrics_DetailsPageChartModel
    {
        public string[] Legends { get; set; }
        public string[] Props { get; set; }
       public List<List<string>> Values { get; set; }
        public Others Others { get; set; }
    }

    public class Others
    {
        public string Metrics { get; set; }
        public int ChartType { get; set; }
    }  
}
