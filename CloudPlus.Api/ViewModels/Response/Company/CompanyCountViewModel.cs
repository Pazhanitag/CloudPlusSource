using CloudPlus.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CloudPlus.Api.ViewModels.Response.Company
{
    public class CompanyCountViewModel
    {
        public int Id { get; set; }
        public int NumberOfResellers { get; set; }
        public int NumberOfCustomers { get; set; }
        public int NumberOfUsers { get; set; }
        public string Type { get; set; }
    }
}