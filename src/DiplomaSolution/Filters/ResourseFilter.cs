using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
namespace DiplomaSolution.Filters
{
    public class ResourseFilter : IAsyncResourceFilter
    {
        public ResourseFilter() // runs rigth before MB and can be used to restrict content type, that action can handle - check that content is in json format ( for ex )
        {
        }

        public async Task OnResourceExecutionAsync(ResourceExecutingContext context, ResourceExecutionDelegate next)
        {
            Trace.WriteLine("ResourceFilter"); // add some kind of logic there ( like chek before MB and etc )

            await next();
        }
    }

    public class ResourseFilterAttribute : TypeFilterAttribute
    {
        public ResourseFilterAttribute() : base(typeof(ResourseFilter))
        {

        }
    }
}
