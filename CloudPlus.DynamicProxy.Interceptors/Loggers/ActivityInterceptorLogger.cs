using System;
using Castle.DynamicProxy;
using CloudPlus.DynamicProxy.Interceptors.Serialization.Json;
using CloudPlus.Logging;
using MassTransit.Courier;
using Newtonsoft.Json;

namespace CloudPlus.DynamicProxy.Interceptors.Loggers
{
    public class ActivityInterceptorLogger : IActivityInterceptorLogger
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

                if (invocation.Method.Name.ToLower().Equals("execute"))
                {
                    var executeContext = invocation.GetArgumentValue(0) as ExecuteContext<object>;
                    var arguments = executeContext?.Arguments;
                    this.Log().Info($"===> {invocation.TargetType.FullName}.${invocation.Method.Name} {Environment.NewLine}Routingslip ExecutionId: {executeContext?.ExecutionId}{Environment.NewLine}Message: { JsonConvert.SerializeObject(arguments, Formatting.Indented, settings)}  {Environment.NewLine} <===");

                }
                else
                {
                    var compensateContext = invocation.GetArgumentValue(0) as CompensateContext<object>;
                    var arguments = compensateContext?.Log;
                    this.Log().Info($"===> {invocation.TargetType.FullName}.${invocation.Method.Name}{Environment.NewLine}Routingslip ExecutionId: {compensateContext?.ExecutionId} {Environment.NewLine}Message: { JsonConvert.SerializeObject(arguments, Formatting.Indented, settings)}  {Environment.NewLine} <===");
                }
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
