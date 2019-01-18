using CloudPlus.PowerShell;

namespace CloudPlus.Api.Office365.Utils
{
    public interface IPowerShellUtility
    {
        void AttachOffice365Credentials(IPowerShellManager powerShellManager);
        void AttachSqlHostAndCredentials(IPowerShellManager powerShellManager);
    }
}
