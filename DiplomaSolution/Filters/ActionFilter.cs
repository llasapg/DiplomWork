using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace DiplomaSolution.Filters
{
    public class ActionFilter : IAsyncActionFilter
    {
        public ActionFilter()
        {
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if(!context.ModelState.IsValid) // there can be model state check 
            {
                 
            }

            Trace.WriteLine("ActionFilter");

            await next();
        }
    }

    public class ActionFilterAttribute : TypeFilterAttribute
    {
        public ActionFilterAttribute() : base(typeof(ActionFilter))
        {

        }
    }
}
