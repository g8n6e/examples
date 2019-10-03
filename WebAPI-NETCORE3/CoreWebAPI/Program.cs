using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Net;

namespace CoreWebAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseKestrel(serverOptions =>
                    {
                        serverOptions.Listen(IPAddress.Loopback, Int32.Parse(args[0]));
                        serverOptions.Listen(IPAddress.Loopback, Int32.Parse(args[1]),
                            listenOptions =>
                            {
                                listenOptions.UseHttps(args[2],
                                    args[3]);
                            });
                    })
                    .UseIISIntegration()
                    .UseStartup<Startup>();
                });
    }
}
