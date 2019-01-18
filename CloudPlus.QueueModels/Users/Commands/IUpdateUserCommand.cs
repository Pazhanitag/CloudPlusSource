using System.Collections.Generic;
using CloudPlus.Enums.User;

namespace CloudPlus.QueueModels.Users.Commands
{
    public interface IUpdateUserCommand : IQueueModel
    {
        int UserId { get; set; }
        string CountryCode { get; set; }
        string DisplayName { get; set; }
        string CompanyName { get; set; }
        string CompanyId { get; set; }
        string FirstName { get; set; }
        string LastName { get; set; }
        string JobTitle { get; set; }
        string AlternativeEmail { get; set; }
        string Department { get; set; }
        string PhoneNumber { get; set; }
        string StreetAddress { get; set; }
        string City { get; set; }
        string ZipCode { get; set; }
        string State { get; set; }
        string Country { get; set; }
        List<int> Roles { get; set; }
        string ProfilePicture { get; set; }
        UserStatus UserStatus { get; set; }
    }
}