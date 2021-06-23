using KeyReceiverService.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.IO;
using System.Reflection;

namespace KeyReceiverService.Infrastructure
{
    [ProviderAlias("FileLogger")]
    public class FileLoggerProvider : ILoggerProvider
    {
        public readonly FileLoggerProviderOptions Options;

        public FileLoggerProvider(IOptions<FileLoggerProviderOptions> _options)
        {
            Options = _options.Value;
            var logsFolderPath = Utils.GetApplicationRootPath() + Options.FolderPath;
            if (!Directory.Exists(logsFolderPath))
                Directory.CreateDirectory(logsFolderPath);
        }

        public ILogger CreateLogger(string categoryName) => new FileLogger(this);

        public void Dispose()
        {
        }
    }
}
