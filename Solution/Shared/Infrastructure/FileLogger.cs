﻿using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Shared.Infrastructure
{
    public class FileLogger : ILogger
    {
        protected readonly FileLoggerProvider _provider;
        private readonly object _lock = new object();
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
            Task.Run(() =>
            {
                lock (formatter)
                {
                    var logsFolderPath = Utils.GetApplicationRootPath() + _provider.Options.FolderPath + "\\";
                    var fullFilePath = logsFolderPath + _provider.Options.FilePath.Replace("{date}", DateTimeOffset.UtcNow.ToString("yyyyMMdd"));
                    var logRecord =
                        $"{"[" + DateTimeOffset.UtcNow.ToString("yyyy-MM-dd HH:mm:ss+00:00") + "]"} [{logLevel.ToString()}] {formatter(state, exception)} {(exception != null ? exception.StackTrace : "")}";

                    while (exception != null)
                    {
                        logRecord += Environment.NewLine;
                        logRecord += exception.Message;
                        logRecord += exception.StackTrace;
                        exception = exception.InnerException;
                    }
            
                    using var streamWriter = new StreamWriter(fullFilePath, true);
                    streamWriter.WriteLine(logRecord);
                }
            });
        }
    }
}
