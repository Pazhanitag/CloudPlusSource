using CloudPlus.PowerShell;
using CloudPlus.Resources;

namespace CloudPlus.Api.Office365.Utils
{
    public class PowerShellUtility : IPowerShellUtility
    {
        private readonly IConfigurationManager _configurationManager;

        public PowerShellUtility(IConfigurationManager configurationManager)
        {
            _configurationManager = configurationManager;
        }

        public void AttachOffice365Credentials(IPowerShellManager powerShellManager)
        {
            powerShellManager
                .AddParameter("AdminUsername", _configurationManager.GetByKey("PowerShellAdminUsername"))
                .AddParameter("AdminPassword", _configurationManager.GetByKey("PowerShellAdminPassword"));
        }

        public void AttachSqlHostAndCredentials(IPowerShellManager powerShellManager)
        {
            powerShellManager
                .AddParameter("SQLServerIP", _configurationManager.GetByKey("PowerShellSQLServerIP"))
                .AddParameter("AuthDBName", _configurationManager.GetByKey("PowerShellAuthDBName"))
                .AddParameter("CPDBName", _configurationManager.GetByKey("PowerShellCPDBName"))
                .AddParameter("SQLServerUsername", _configurationManager.GetByKey("PowerShellSQLServerUsername"))
                .AddParameter("SQLServerPassword", _configurationManager.GetByKey("PowerShellSQLServerPassword"));
        }
    }
}
