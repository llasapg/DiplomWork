using DiplomaSolution.Middlewares;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;

namespace DiplomaSolution.Extensions
{
    public static class VersionExtension
    {
        public static IEndpointConventionBuilder AddVersioning(this IEndpointRouteBuilder endpoint)
        {
            return endpoint.Map("/version", endpoint.CreateApplicationBuilder().UseMiddleware<VersionMiddleware>().Build());
        }
    }
}
