using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace KeyReceiverService.Infrastructure
{
    public class FileLogger : ILogger
    {
        protected readonly FileLoggerProvider _provider;

        public FileLogger([NotNull] FileLoggerProvider provider)
        {
            _provider = provider;
        }

        public IDisposable BeginScope<TState>(TState state) => null;

        public bool IsEnabled(LogLevel logLevel) => logLevel != LogLevel.None;

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (!IsEnabled(logLevel))
                return;
            var logsFolderPath = Utils.GetApplicationRootPath() + _provider.Options.FolderPath + "\\";
            var fullFilePath = logsFolderPath + _provider.Options.FilePath.Replace("{date}", DateTimeOffset.UtcNow.ToString("yyyyMMdd"));
            var logRecord =
                $"{"[" + DateTimeOffset.UtcNow.ToString("yyyy-MM-dd HH:mm:ss+00:00") + "]"} [{logLevel.ToString()}] {formatter(state, exception)} {(exception != null ? exception.StackTrace : "")}";

            using var streamWriter = new StreamWriter(fullFilePath, true);
            streamWriter.WriteLine(logRecord);
        }
    }
}
