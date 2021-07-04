using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Shared.Infrastructure;
using System;

namespace Shared.Configuration
{
    public static class FileLoggerExtensions
    {
        public static ILoggingBuilder AddFileLogger(this ILoggingBuilder builder, Action<FileLoggerProviderOptions> configure)
        {
            builder.Services
                .AddSingleton<ILoggerProvider, FileLoggerProvider>()
                .AddScoped(typeof(ILogger), (s) =>
                {
                    var provider = (ILoggerProvider) s.GetService(typeof(ILoggerProvider));
                    return provider.CreateLogger("FileLogger");
                })
                .AddScoped(typeof(ILogger<>), typeof(LoggerWrapper<>))
                .Configure(configure);
            return builder;
        }
    }
}
