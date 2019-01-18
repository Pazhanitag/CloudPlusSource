using System;
using System.Collections.Generic;
using CloudPlus.Enums.Office365;
using CloudPlus.Models.Office365.License;

namespace CloudPlus.Models.Office365.User
{
    public class Office365UserModel
    {
        public int Id { get; set; }
        public string Office365UserId { get; set; }
        public int CloudPlusUserId { get; set; }
        public string UserPrincipalName { get; set; }
        public int CustomerId { get; set; }
        public Office365UserState Office365UserState { get; set; }
        public DateTime? Office365SoftDeletionTime { get; set; }
        public IEnumerable<Office365LicenseModel> Licenses { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string DisplayName { get; set; }
    }
}
