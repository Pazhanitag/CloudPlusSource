using CloudPlus.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudPlus.Models.Company
{
    public class CompanyCountModel
    {
        public int Id { get; set; }
        public int? ParentId { get; set; }
        public List<CompanyCountModel> Children { get; set; } = new List<CompanyCountModel>();
        public CompanyCountModel Parent { get; set; }
        public int NumberOfResellers { get; set; } = 0;
        public int NumberOfCustomers { get; set; } = 0;
        public int NumberOfUsers { get; set; } = 0;
        public bool Root { get; set; } = false;
        public bool Visited { get; set; } = false;
        public CompanyType Type { get; set; }
    }
}
