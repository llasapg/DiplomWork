using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace DiplomaSolution.Middlewares
{
    public class VersionMiddleware
    {
        private const string VERSION = "3.0 ASP.NET CORE app";
        private RequestDelegate RequestDelegate { get; set; }

        public VersionMiddleware(RequestDelegate request)
        {
            RequestDelegate = request;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            await context.Response.WriteAsync($"Current version is - {VERSION}");
        }
    }
}
