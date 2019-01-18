using System;
using Castle.DynamicProxy;
using CloudPlus.DynamicProxy.Interceptors.Serialization.Json;
using CloudPlus.Logging;
using MassTransit;
using Newtonsoft.Json;

namespace CloudPlus.DynamicProxy.Interceptors.Loggers
{
    public class ConsumerInterceptorLogger : IConsumerInterceptorLogger
    {
        public void Intercept(IInvocation invocation)
        {
            try
            {
                var argValue = invocation.GetArgumentValue(0) as ConsumeContext<object>;

                var settings = new JsonSerializerSettings
                {
                    Formatting = Formatting.Indented,
                    ContractResolver = new MaskedPasswordPropertyResolver()
                };

                this.Log().Info($"===> {invocation.TargetType.FullName}.${invocation.Method.Name} {Environment.NewLine} Message: { JsonConvert.SerializeObject(argValue?.Message, Formatting.Indented, settings)}  {Environment.NewLine} <===");

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