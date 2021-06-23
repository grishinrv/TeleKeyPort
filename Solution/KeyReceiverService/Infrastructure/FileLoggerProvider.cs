using KeyReceiverService.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.IO;

namespace KeyReceiverService.Infrastructure
{
    [ProviderAlias("FileLogger")]
    public class FileLoggerProvider : ILoggerProvider
    {
        public readonly FileLoggerProviderOptions Options;

        public FileLoggerProvider(IOptions<FileLoggerProviderOptions> _options)
        {
            Options = _options.Value;
            var current = Directory.GetCurrentDirectory();

            if (!Directory.Exists(current + Options.FolderPath))
                Directory.CreateDirectory(current + Options.FolderPath);
        }

        public ILogger CreateLogger(string categoryName) => new FileLogger(this);

        public void Dispose()
        {
        }
    }
}
