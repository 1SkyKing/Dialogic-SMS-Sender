using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DialogicNetCoreTest
{
    public class Startup
    {
        public Startup(IConfiguration config)
        {
            // Configuration from appsettings.json has already been loaded by
            // CreateDefaultBuilder on WebHost in Program.cs. Use DI to load
            // the configuration into the Configuration property.
            Configuration = config;
        }

        public IConfiguration Configuration { get; set; }


        public void ConfigureServices(IServiceCollection services)
        {
            #region snippet_Example1
            // Example #1: General configuration
            // Register the Configuration instance which MyOptions binds against.
            //services.Configure<SkyConfig>(Configuration);
            #endregion
        }
    }
}
