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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task InvokeAsync(HttpContext context)
        {
            context.Response.Headers.Add("Generalheader", "Value");

            await context.Response.WriteAsync($"Current version is - {context.Response.Headers.Count}");

            context.Response.OnStarting(()=>
            {
                context.Response.Headers.Add("ssss", "iuuu");

                return Task.FromResult(0);
            });
        }
    }
}
