using System;
using System.Collections.Generic;
using CloudPlus.Models.Domain;
using CloudPlus.Models.Enums;

namespace CloudPlus.Models.Metrics
{
    public class VendorMetricsModel
    {
        public int Id { get; set; }
        public string VendorMetricsName { get; set; }
        public string Description { get; set; }
        public bool CanAccess { get; set; }

    }
}

