using System.Linq;
using CloudPlus.Api.ViewModels.Request.User;
using CloudPlus.Api.ViewModels.Response.Office365;
using CloudPlus.Api.ViewModels.Response.User;
using CloudPlus.Models.Identity;

namespace CloudPlus.Api.Mappers
{
    public static class UserMapper
    {
        public static UserViewModel ToUserViewModel(this UserModel user, string urlContent)
        {
            if (user == null)
                return null;

            return new UserViewModel
            {
                Id = user.Id,
                CompanyId = user.CompanyId,
                ProfilePicture = !string.IsNullOrWhiteSpace(user.ProfilePicture)
                    ? $"{urlContent}Static/Images/ProfilePicture/{user.ProfilePicture}"
                    : "",
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                UserName = user.UserName?.Split('@').ElementAtOrDefault(0),
                Domain = user.UserName?.Split('@').ElementAtOrDefault(1),
                DisplayName = user.DisplayName,
                City = user.City,
                Country = user.Country,
                AlternativeEmail = user.AlternativeEmail,
                State = user.State,
                ZipCode = user.ZipCode,
                StreetAddress = user.StreetAddress,
                JobTitle = user.JobTitle,
                CompanyName = user.CompanyName,
                PhoneNumber = user.PhoneNumber,
                UserStatus = user.UserStatus,
                Roles = user.Roles.Select(r => r.ToRoleViewModel())
            };
        }

        public static UserProfileViewModel ToUserProfileViewModel(this UserModel user, string urlContent)
        {
            if (user == null)
                return null;

            return new UserProfileViewModel
            {
                Id = user.Id,
                ProfilePicture = !string.IsNullOrWhiteSpace(user.ProfilePicture)
                    ? $"{urlContent}Static/Images/ProfilePicture/{user.ProfilePicture}"
                    : "",
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                UserName = user.UserName?.Split('@').ElementAtOrDefault(0),
                Domain = user.UserName?.Split('@').ElementAtOrDefault(1),
                DisplayName = user.DisplayName,
                City = user.City,
                Country = user.Country,
                AlternativeEmail = user.AlternativeEmail,
                State = user.State,
                ZipCode = user.ZipCode,
                StreetAddress = user.StreetAddress,
                JobTitle = user.JobTitle,
                CompanyName = user.CompanyName,
                PhoneNumber = user.PhoneNumber,
                UserStatus = user.UserStatus,
            };
        }

        public static UserEmailsViewModel ToUserEmailsViewModel(this UserModel user)
        {
            if (user == null)
                return null;

            return new UserEmailsViewModel
            {
                Id = user.Id,
                Email = user.Email,
                AlternativeEmail = user.AlternativeEmail
            };
        }

        public static UserModel ToUserModel(this CreateUserViewModel user)
        {
            if (user == null)
                return null;

            return new UserModel
            {
                AlternativeEmail = user.AlternativeEmail,
                City = user.City,
                CompanyName = user.CompanyName,
                Country = user.Country,
                CountryCode = user.CountryCode,
                DisplayName = user.DisplayName,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                JobTitle = user.JobTitle,
                PhoneNumber = user.PhoneNumber,
                State = user.State,
                StreetAddress = user.StreetAddress,
                UserName = user.UserName,
                ZipCode = user.ZipCode,
                ProfilePicture = user.ProfilePicture,
                UserStatus = user.UserStatus,
                Roles = user.Roles.Select(roleId => new RoleModel {Id = roleId})
            };
        }

        public static dynamic ToCreateUserCommand(this CreateUserViewModel user)
        {
            if (user == null)
                return null;

            return new
            {
                user.Email,
                user.UserName,
                user.FirstName,
                user.LastName,
                user.AlternativeEmail,
                user.JobTitle,
                user.DisplayName,
                user.PhoneNumber,
                user.StreetAddress,
                user.City,
                user.ZipCode,
                user.State,
                user.Country,
                user.CountryCode,
                user.Roles,
                user.PasswordSetupMethod,
                user.PasswordSetupEmail,
                user.SendPlainPasswordViaEmail,
                user.Password,
                user.ProfilePicture,
                CompanyDomain = user.Domain,
                user.CompanyName,
                user.CompanyId,
                user.UserStatus
            };
        }

        public static dynamic ToUpdateUserCommand(this UpdateUserViewModel user)
        {
            if (user == null)
                return null;

            return new
            {
                UserId = user.Id,
                user.CompanyName,
                user.FirstName,
                user.LastName,
                user.AlternativeEmail,
                user.CompanyId,
                user.JobTitle,
                user.DisplayName,
                user.PhoneNumber,
                user.StreetAddress,
                user.City,
                user.ZipCode,
                user.State,
                user.Country,
                user.CountryCode,
                user.Roles,
                user.ProfilePicture,
                user.UserStatus
            };
        }

        public static dynamic ToUpdateUserCommand(this UpdateUserProfileViewModel user)
        {
            if (user == null)
                return null;

            return new
            {
                UserId = user.Id,
                user.CompanyName,
                user.FirstName,
                user.LastName,
                user.AlternativeEmail,
                user.CompanyId,
                user.JobTitle,
                user.DisplayName,
                user.PhoneNumber,
                user.StreetAddress,
                user.City,
                user.ZipCode,
                user.State,
                user.Country,
                user.CountryCode,
                user.ProfilePicture,
                user.UserStatus
            };
        }

        public static dynamic ToChangeUserPasswordCommand(this ChangePasswordViewModel user)
        {
            if (user == null)
                return null;

            return new
            {
                user.UserId,
                user.Password,
                user.PasswordSetupMethod,
                user.SendPlainPasswordViaEmail,
                user.PasswordSetupEmail
            };
        }

        public static Office365DomainUserViewModel ToOffice365DomainUserViewModel(this UserModel user, string urlContent)
        {
            if (user == null)
                return null;

            return new Office365DomainUserViewModel
            {
                Id = user.Id,
                Email = user.Email,
                DisplayName = user.DisplayName,
                IsProvisioned = user.IsProvisioned,
                AssignedLicense = user.AssignedLicense,
                 AssignedLicenses=user.AssignedLicenses,
                ProfilePicture = !string.IsNullOrWhiteSpace(user.ProfilePicture)
                    ? $"{urlContent}Static/Images/ProfilePicture/{user.ProfilePicture}"
                    : "",
            };
        }
    }
}