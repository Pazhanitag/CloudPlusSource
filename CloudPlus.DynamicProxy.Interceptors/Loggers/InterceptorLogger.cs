using Castle.DynamicProxy;
using CloudPlus.Logging;

namespace CloudPlus.DynamicProxy.Interceptors.Loggers
{
    public class InterceptorLogger : IInterceptorLogger
    {
        public void Intercept(IInvocation invocation)
        {
            this.Log().Info($"===> {invocation.TargetType.FullName}.${invocation.Method.Name}");
            invocation.Proceed();
        }
    }
}
