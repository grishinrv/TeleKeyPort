using System.IO;
using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Shared.Configuration;

namespace Shared.Infrastructure
{
    public static class Utils
    {
        public static string GetApplicationRootPath()
        {
            var assemblyPath = Assembly.GetExecutingAssembly().CodeBase;
            var current = Path.GetDirectoryName(assemblyPath);
            if (current.StartsWith("file:\\"))
                current = current.Substring(6, current.Length - 6);
            return current;
        }

        public static IHostBuilder ConfigureFileLogging(this IHostBuilder builder)
        {
            return builder.ConfigureLogging((hostBuilderContext, logging) =>
            {
                logging.ClearProviders();
                logging.AddFileLogger(options =>
                {
                    hostBuilderContext.Configuration.GetSection("Logging").GetSection("FileLogger")
                        .GetSection("Options").Bind(options);
                });
            });
        }
    }
}
