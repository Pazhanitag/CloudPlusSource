using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace CloudPlus.PowerShell
{
    public interface IPowerShellManager
    {
        void AttachParameters(Dictionary<string, object> parameters);
        IPowerShellManager AddParameter(string key, object value);
        Collection<T> ExecuteScript<T>(string psScript);
        T ExecuteScriptAndReturnFirst<T>(string psScript);
        void ExecuteScript(string psScript);
    }
}