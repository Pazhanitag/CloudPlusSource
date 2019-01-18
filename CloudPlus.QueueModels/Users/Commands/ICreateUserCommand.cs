using CloudPlus.Enums.User;
using System.Collections.Generic;

namespace CloudPlus.QueueModels.Users.Commands
{
    public interface ICreateUserCommand : IQueueModel
    {
        int UserId { get; set; }
        string Email { get; set; }
        string DisplayName { get; set; }
        int CompanyId { get; set; }
        string CompanyName { get; set; }
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
        string CountryCode { get; set; }
        List<int> Roles { get; set; }
        string CompanyDomain { get; set; }
        string ProfilePicture { get; set; }
        string Password { get; set; }
        PasswordSetupMethod PasswordSetupMethod { get; set; }
        bool SendPlainPasswordViaEmail { get; set; }
        string PasswordSetupEmail { get; set; }
        UserStatus UserStatus { get; set; }
    }
}
