using System.IO;
using System.Reflection;

namespace KeyReceiverService.Infrastructure
{
    public class Utils
    {
        public static string GetApplicationRootPath()
        {
            var assemblyPath = Assembly.GetExecutingAssembly().CodeBase;
            var current = Path.GetDirectoryName(assemblyPath);
            if (current.StartsWith("file:\\"))
                current = current.Substring(6, current.Length - 6);
            return current;
        }
    }
}
