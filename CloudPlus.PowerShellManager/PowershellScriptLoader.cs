using System.IO;
using System.Text;
using CloudPlus.Resources;

namespace CloudPlus.PowerShell
{
    public class PowershellScriptLoader : IPowershellScriptLoader
    {
        private readonly IConfigurationManager _configurationManager;

        public PowershellScriptLoader(IConfigurationManager configurationManager)
        {
            _configurationManager = configurationManager;
        }

        public string LoadScript(string scriptName)
        {
            string psScript;

            var path = string.Concat(_configurationManager.GetByKey("PowerShellScriptsFolder"), scriptName);

            if (File.Exists(path))
                psScript = File.ReadAllText(path, Encoding.UTF8);
            else
                throw new FileNotFoundException("Wrong path for the script file");

            return psScript;
        }
    }
}
