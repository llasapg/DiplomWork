using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace DiplomaSolution
{
    /// <summary>
    /// Class to perform configuration and build of the web-host
    /// </summary>
    public class Program
    {
        /// <summary>
        /// entery point in the application ( app starts as console app ) --> we have class program and method main
        /// </summary>
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        /// <summary>
        /// Custom method to return configured web-host ( IWebHost ) instanse
        /// </summary>
        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
            .UseUrls("http://*:5000")
            .ConfigureAppConfiguration((hostingContext, config) =>
            {
                config.AddEnvironmentVariables();

                var enviroment = config.Build();

                config.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

                config.AddJsonFile($"appconfig.{enviroment["ASPNETCORE_ENVIRONMENT"]}.json", optional: true, reloadOnChange: true);

                config.AddJsonFile($"appsecrets.json", optional: true, reloadOnChange: true);

                if (args != null)
                {
                    config.AddCommandLine(args);
                }
            })
            .ConfigureLogging((context, logging) =>
            {
                logging.AddConfiguration(context.Configuration.GetSection("Logging"));
                logging.AddConsole();
                logging.AddDebug();
                logging.AddEventSourceLogger();
            }) // Add configuration features there
            // think about adding extra config files ( remove everithing there )
            .UseStartup<Startup>();
    }
}
