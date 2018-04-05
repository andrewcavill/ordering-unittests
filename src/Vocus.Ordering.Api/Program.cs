using System.Diagnostics;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.WindowsServices;
using Vocus.Common;

namespace Vocus.Ordering.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var isWindowsService = !(Debugger.IsAttached || args.Contains("--console"));

            var contentRoot = Directory.GetCurrentDirectory();
            if (isWindowsService)
            {
                var pathToExe = Process.GetCurrentProcess().MainModule.FileName;
                contentRoot = Path.GetDirectoryName(pathToExe);
            }

            var host = new WebHostBuilder()
                .UseKestrel()
                .UseContentRoot(contentRoot)
                .UseIISIntegration()
                .UseLocalIpWithRandomPort()
                .UseStartup<Startup>()
                .Build();

            if (isWindowsService)
                host.RunAsService();
            else
                host.Run();
        }
    }
}
