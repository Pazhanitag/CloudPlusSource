using System;
using Castle.DynamicProxy;
using CloudPlus.DynamicProxy.Interceptors.Serialization.Json;
using CloudPlus.Logging;
using Newtonsoft.Json;

namespace CloudPlus.DynamicProxy.Interceptors.Loggers
{
    public class ServiceInterceptorLogger : IServiceInterceptorLogger
    {
        public void Intercept(IInvocation invocation)
        {
            try
            {
                var settings = new JsonSerializerSettings
                {
                    Formatting = Formatting.Indented,
                    ContractResolver = new MaskedPasswordPropertyResolver()
                };
                var args = "";
                var methodParameters = invocation.GetConcreteMethod()?.GetParameters();

                for (var index = 0; index < methodParameters?.Length; index++)
                {
                    var methodParameter = methodParameters[index];

                    var argument = invocation.GetArgumentValue(index);
                    if (argument == null) continue;

                    var argValue = !methodParameter.Name.ToLower().Contains("password")
                        ? JsonConvert.SerializeObject(argument, Formatting.Indented, settings)
                        : "*****";

                    args += $"{methodParameter.Name}: { argValue }, {Environment.NewLine}";
                }

                this.Log().Info($"===> {invocation.TargetType.FullName}.${invocation.Method.Name} {Environment.NewLine} Arguments: {args} <===");
            }
            catch (Exception ex)
            {
                this.Log().Error(ex);
                this.Log().Info($"===> {invocation.TargetType.FullName}.${invocation.Method.Name}");
            }

            invocation.Proceed();
        }
    }
}