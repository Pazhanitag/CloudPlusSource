using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace CloudPlus.PowerShell
{
    public class PowerShellManager : IPowerShellManager
    {
        private Dictionary<string, object> _parameters = new Dictionary<string, object>();

        public void AttachParameters(Dictionary<string, object> parameters)
        {
            foreach (var param in parameters)
            {
                _parameters.Add(param.Key, param.Value);
            }
        }

        public IPowerShellManager AddParameter(string key, object value)
        {
            if (_parameters.ContainsKey(key))
            {
                _parameters[key] = value;
                return this;
            }
            _parameters.Add(key, value);
            return this;
        }

        private static void CheckErrors(System.Management.Automation.PowerShell powerShellInstance)
        {
            var exceptions = powerShellInstance.Streams.Error.Select(error => error.Exception).ToList();

            if (exceptions.Count > 0)
                throw new AggregateException(exceptions);
        }

        public Collection<T> ExecuteScript<T>(string psScript)
        {
            using (var powerShellInstance = System.Management.Automation.PowerShell.Create())
            {
                powerShellInstance.AddScript(psScript)
                    .AddParameters(_parameters);

                var result = powerShellInstance.Invoke<T>();

                CheckErrors(powerShellInstance);

                return result;
            }
        }
        public T ExecuteScriptAndReturnFirst<T>(string psScript)
        {
            using (var powerShellInstance = System.Management.Automation.PowerShell.Create())
            {
                powerShellInstance.AddScript(psScript)
                    .AddParameters(_parameters);

                var result = powerShellInstance.Invoke<T>();

                CheckErrors(powerShellInstance);

                return result.FirstOrDefault();
            }
        }
        public void ExecuteScript(string psScript)
        {
            using (var powerShellInstance = System.Management.Automation.PowerShell.Create())
            {
                powerShellInstance.AddScript(psScript)
                    .AddParameters(_parameters);

                powerShellInstance.Invoke();

                CheckErrors(powerShellInstance);
            }
        }
    }
}
