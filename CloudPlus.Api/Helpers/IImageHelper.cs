namespace CloudPlus.Api.Helpers
{
    public interface IImageHelper
    {
        string SaveCompanyLogo(string base64String);
        string SaveProfilePicture(string base64String);
    }
}