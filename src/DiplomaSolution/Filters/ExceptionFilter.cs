using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
namespace DiplomaSolution.Filters
{
    public class ExceptionFilter : IAsyncExceptionFilter
    {
        public ExceptionFilter()
        {
        }

        public async Task OnExceptionAsync(ExceptionContext context)
        {
            Trace.WriteLine("ExceptionFilter");
        }
    }

    public class ExceptionFilterAttribute : TypeFilterAttribute
    {
        public ExceptionFilterAttribute() : base(typeof(ExceptionFilter))
        {

        }
    }
}
