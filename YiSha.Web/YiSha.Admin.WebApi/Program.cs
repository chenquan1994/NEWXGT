using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NLog.Web;
using System.Text.Encodings.Web;
using System.Text.Unicode;


namespace YiSha.Admin.WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //CreateWebHostBuilder(args).Build().Run();

            CreateHostBuilder(args).Build().Run();
        }

        //public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
        // WebHost.CreateDefaultBuilder(args)
        //             .UseUrls("http://*:5001")

        //             .UseStartup<Startup>()
        //             .ConfigureLogging(logging =>
        //             {
        //                 logging.ClearProviders();
        //                 logging.SetMinimumLevel(LogLevel.Trace);
        //             }).UseNLog();





        public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.ConfigureKestrel(serverOptions =>
                {
                    // 配置HTTP端口
                    serverOptions.Listen(System.Net.IPAddress.Any, 80, listenOptions =>
                    {
                        listenOptions.Protocols = Microsoft.AspNetCore.Server.Kestrel.Core.HttpProtocols.Http1AndHttp2;
                    });

                    // 配置HTTPS端口和证书
                    serverOptions.Listen(System.Net.IPAddress.Any, 443, listenOptions =>
                    {

                        listenOptions.Protocols = Microsoft.AspNetCore.Server.Kestrel.Core.HttpProtocols.Http1AndHttp2;
                    });
                });
                webBuilder.UseStartup<Startup>();
            });
    }
}
