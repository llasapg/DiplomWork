using DiplomaSolution.Helpers.Logging;
using Microsoft.Extensions.Logging;

namespace DiplomaSolution.Extensions
{
    /// <summary>
    /// 
    /// </summary>
    public static class FileLoggerExtension
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="factory"></param>
        /// <returns></returns>
        public static ILoggerFactory AddFileLogger(this ILoggerFactory factory)
        {
            factory.AddProvider(new FileLoggerProvider());

            return factory;
        }
    }
}
