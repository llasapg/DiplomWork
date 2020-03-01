using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace DiplomaSolution.Helpers.Logging
{
    public class FileLogger : ILogger
    {
        private IConfiguration Configuration { get; set; }
        private static object Lock { get; set; } = new object();

        public FileLogger()
        {

        }

        protected FileLogger(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            throw new NotImplementedException();
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return Convert.ToBoolean(Configuration["Logging:IsEnabled"]);
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            lock (Lock)
            {
                if (IsEnabled(logLevel))
                {
                    File.AppendAllText(Configuration["Logging:FilePath"], formatter(state, exception) + Environment.NewLine);
                }
            }
        }
    }
}
