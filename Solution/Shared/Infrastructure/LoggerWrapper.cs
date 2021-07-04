using System;
using Microsoft.Extensions.Logging;

namespace Shared.Infrastructure
{
    public class LoggerWrapper<T> : ILogger<T>
    {
        private readonly ILogger _innerLogger;
        public LoggerWrapper(ILogger innerLogger)
        {
            _innerLogger = innerLogger;
        }
        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            _innerLogger.Log(logLevel, eventId, state, exception, formatter);
        }

        public bool IsEnabled(LogLevel logLevel) => logLevel != LogLevel.None;

        public IDisposable BeginScope<TState>(TState state) => null;
    }
}