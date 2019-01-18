namespace CloudPlus.Api.ActiveDirectory.Utils
{
    public interface ISamAccountNameGenerator
    {
        string GenerateSamAccountName(string upn);
    }
}