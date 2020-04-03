using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;
namespace DiplomaSolution.Filters
{
    public class ResourseFilter : Attribute , IAsyncResourceFilter
    {
        public ResourseFilter()
        {
        }

        public async Task OnResourceExecutionAsync(ResourceExecutingContext context, ResourceExecutionDelegate next)
        {
            Trace.WriteLine("ResourceFilter");

            await next();
        }
    }
}
