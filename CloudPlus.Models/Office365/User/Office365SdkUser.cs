namespace CloudPlus.Models.Office365.User
{
    public class Office365SdkUser
    {
        public string Office365UserId { get; set; }
        public string DisplayName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserPrincipalName { get; set; }
        public string Password { get; set; }
        public string Office365CustomerId { get; set; }
        public string UsageLocation { get; set; }
    }
}