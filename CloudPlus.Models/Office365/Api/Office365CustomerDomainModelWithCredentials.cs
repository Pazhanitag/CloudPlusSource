namespace CloudPlus.Models.Office365.Api
{
    public class Office365CustomerDomainModelWithCredentials : IOffice365CustomerDomainModel
    {
        public string Office365CustomerId { get; set; }
        public string Domain { get; set; }
        public string AdminUserName { get; set; }
        public string AdminPassword { get; set; }
    }
}
