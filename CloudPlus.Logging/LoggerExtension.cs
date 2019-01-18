using System;
using log4net;

namespace CloudPlus.Logging
{
    public static class LoggerExtension
    {
        public static ILog Log<T>(this T obj)
        {
            return LogManager.GetLogger(typeof(T));
        }

        public static ILog GetLogger(Type type)
        {
            return LogManager.GetLogger(type);
        }
    }
}