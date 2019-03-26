using DialogicNetCoreTest.Config;
using DialogicNetCoreTest.Services;
using Dlgc.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SkyLogger;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DialogicNetCoreTest
{
    internal class Program
    {

        //Severity	Code	Description	Project	File	Line	Suppression State
        //Error CS1061	'IConfigurationBuilder' does not contain a definition for 'SetBasePath' and no accessible extension method 'SetBasePath' accepting a first argument of type 'IConfigurationBuilder' could be found(are you missing a using directive or an assembly reference?)	DialogicNetCoreTest D:\Projects\NETCore\GwTest\Console_GW\DialogicNetCoreTest\Program.cs	19	Active
        //public static Action<SkyConfig> Configuration { get; set; }
        private static async Task Main(string[] args)
        {
            var isService = !(Debugger.IsAttached || args.Contains("--console"));
            var host = new HostBuilder()
                .ConfigureAppConfiguration((hostContext, config) =>
                {
                    config.SetBasePath(Directory.GetCurrentDirectory());//Microsoft.Extensions.Configuration.FileExtensions
                    config.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);//Microsoft.Extensions.Configuration.Json
                    //config.AddEnvironmentVariables();

                    if (args != null)
                    {
                        config.AddCommandLine(args);
                    }
                })
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddOptions();
                    services.Configure<AppConfig>(hostContext.Configuration.GetSection("AppConfig"));//Microsoft.Extensions.Options.ConfigurationExtensions
                    services.AddDbContext<MessageContext>(options => options.UseSqlServer(hostContext.Configuration.GetConnectionString("DefaultConnection")));
                    services.AddDbContext<SkyLoggerContext>(options => options.UseSqlServer(hostContext.Configuration.GetConnectionString("SkyLogger")));

                    //Hangi servis aktif edilecek
                    //services.Configure<SkyConfig>(hostContext.Configuration.GetSection("SkyConfig"));
                    services.AddHostedService<MessageSendService>();
                    //services.AddSingleton<IHostedService, PrintTextToConsoleService>();

                }).ConfigureLogging((hostingContext, logging) => {
                    logging.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
                    logging.AddConsole();
                }); //.UseStart;
            //.UseConsoleLifetime()
            //.Build(); 
            //Configuration = builder.Build();
            //var connectionString = Configuration["ConnectionStrings:YourConnectionString"]
                     

            if (isService)
            {
                await host.RunAsServiceAsync();
            }
            else
            {
                await host.RunConsoleAsync();
            }
        }


       
    }
}
