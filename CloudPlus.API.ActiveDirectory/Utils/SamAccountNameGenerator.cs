using System;
using System.Linq;
using CloudPlus.PowerShell;
using CloudPlus.Extensions;

namespace CloudPlus.Api.ActiveDirectory.Utils
{
    public class SamAccountNameGenerator : ISamAccountNameGenerator
    {
        private readonly IPowerShellManager _powerShellManager;
        private readonly IPowershellScriptLoader _powershellScriptLoader;

        public SamAccountNameGenerator(IPowerShellManager powerShellManager, IPowershellScriptLoader powershellScriptLoader)
        {
            _powerShellManager = powerShellManager;
            _powershellScriptLoader = powershellScriptLoader;
        }
        public string GenerateSamAccountName(string upn)
        {
            string samAccountName;
            if (string.IsNullOrEmpty(upn))
                throw new ArgumentNullException(nameof(upn));
            do
            {
                var splitted = upn.RemoveWhitespaces().Split('@');
                if (splitted.Length != 2)
                    throw new FormatException("Upn was not in correct format");

                var firstPart = splitted.ElementAtOrDefault(1).OnlyAlphaNumeric().TrySubstring(0, 6);
                var secondPart = splitted.FirstOrDefault().OnlyAlphaNumeric().TrySubstring(0, 9);
                var filler = new Random().Next(100, 999);
                samAccountName = $"{firstPart}{filler}_{secondPart}";

            } while (!SamAccountNameAvailable(samAccountName));

            return samAccountName;
        }

        private bool SamAccountNameAvailable(string samAccountName)
        {
            var script = _powershellScriptLoader.LoadScript(PowershellScripts.SamAccountNameAvailable);
            _powerShellManager.AddParameter("samAccountName", samAccountName);
            return _powerShellManager.ExecuteScriptAndReturnFirst<bool>(script);
        }
    }
}