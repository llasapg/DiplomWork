using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
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
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        /// <summary>
        /// Custom method to return configured web-host ( IWebHost ) instanse
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
            .ConfigureLogging((context, logging) =>
            {
                logging.AddConfiguration(context.Configuration.GetSection("Logging"));
                logging.AddConsole();
                logging.AddDebug();
                logging.AddEventSourceLogger();
            }) // Add configuration features there
            .UseStartup<Startup>();
    }
}
