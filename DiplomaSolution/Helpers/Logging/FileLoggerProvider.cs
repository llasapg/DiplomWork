using System;
using Microsoft.Extensions.Logging;

namespace DiplomaSolution.Helpers.Logging
{
    public class FileLoggerProvider : ILoggerProvider
    {
        public FileLoggerProvider()
        {
        }

        public ILogger CreateLogger(string categoryName)
        {
            return new FileLogger();
        }

        public void Dispose()
        {
            // Add some logic to clear logging file ( or remove it when month passed )
        }
    }
}
