using CloudPlus.Api.ActiveDirectory.Attributes;

namespace CloudPlus.Api.ActiveDirectory.Models.User
{
    public class ChangeUserPassword : IActiveDirectoryModel
    {
        [ActiveDirecotryName(AdPropertyName = "upn")]
        public string Upn { get; set; }
        [ActiveDirecotryName(AdPropertyName = "newPassword")]
        public string NewPassword { get; set; }
    }
}