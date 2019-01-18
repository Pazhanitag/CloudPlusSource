namespace CloudPlus.Api.Security
{
    public interface IAuthorizationManager
    {
        bool HasAccessByPermission(string[] permissions, int userId);
    }
}