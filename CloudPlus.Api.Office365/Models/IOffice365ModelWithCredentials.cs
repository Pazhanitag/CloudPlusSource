namespace CloudPlus.Api.Office365.Models
{
    public interface IOffice365ModelWithCredentials : IOffice365Model
    {
        string AdminUserName { get; set; }
        string AdminPassword { get; set; }
    }
}