using System.IO;
using System.Threading.Tasks;
using DiplomaSolution.Middlewares;
using Microsoft.AspNetCore.Http;
using Xunit;

namespace DiplomaTests
{
    /// <summary>
    /// Class to verify that version middleware is working correctly in each case ( valid request data and invalid )
    /// </summary>
    public class VersionMiddlewareUnitTests
    {
        /// <summary>
        /// Current version of our app ( shoud be removed to some general place )
        /// </summary>
        private const string VERSION = "3.0 ASP.NET CORE app";

        /// <summary>
        /// Test to check, that when we provide valid request path application version will be returned
        /// </summary>
        [Fact]
        public async void ValidRequestData_Test()
        {
            var memoryStream = new MemoryStream();

            var httpcontext = new DefaultHttpContext();

            httpcontext.Request.Path = "/version";

            httpcontext.Response.Body = memoryStream; // replays default Stream.Null to get some response

            RequestDelegate nextMiddleware = async (context) => // we wont use it, but we should pre-define this variable
            {
                await Task.CompletedTask;
            };

            var middleware = new VersionMiddleware(nextMiddleware);

            await middleware.InvokeAsync(httpcontext);

            var response = "";

            memoryStream.Seek(0, SeekOrigin.Begin); // todo - check was this method actially doing

            using (var stream = new StreamReader(memoryStream))
            {
                response = await stream.ReadToEndAsync();
            }

            Assert.Contains(VERSION, response);
            Assert.Equal(200, httpcontext.Response.StatusCode);
        }

        [Fact]
        public async void InValidRequestData_Test()
        {
            var memoryStream = new MemoryStream();

            var httpcontext = new DefaultHttpContext();

            httpcontext.Request.Path = "/version9283";

            httpcontext.Response.Body = memoryStream; // replays default Stream.Null to get some response

            RequestDelegate nextMiddleware = async (context) => // we wont use it, but we should pre-define this variable
            {
                await Task.CompletedTask;
            };

            var middleware = new VersionMiddleware(nextMiddleware);

            await middleware.InvokeAsync(httpcontext);

            var response = "";

            using (var stream = new StreamReader(memoryStream))
            {
                response = await stream.ReadToEndAsync();
            }

            Assert.Contains("", response);
            Assert.Equal(200, httpcontext.Response.StatusCode);
        }
    }
}
