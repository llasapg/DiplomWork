using DiplomaSolution;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Xunit;

namespace DiplomaTests.IntegrationTests
{
    /// <summary>
    /// Simple class to hold simple integration test to check version middleware
    /// </summary>
    public class VersionMiddlewareIntegrationTests
    {
        /// <summary>
        /// Current version of our app ( shoud be removed to some general place )
        /// </summary>
        private const string VERSION = "3.0 ASP.NET CORE app";

        [Fact]
        public async void VersionMiddlewareIntegrationTest()
        {
            var webhost = CreateWebHostBuilder(new string[0]);

            using (var server = new TestServer(webhost))
            {
                var client = server.CreateClient();

                var response = await client.GetAsync("/version");

                System.Console.WriteLine($"response status code - {response.StatusCode}");

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();

                    System.Console.WriteLine($"response message - {content}");

                    Assert.Contains(VERSION, content);
                }
            }
        }

        /// <summary>
        /// Custom method to return configured web-host ( IWebHost ) instanse
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        private IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args).UseStartup<Startup>();
    }
}
