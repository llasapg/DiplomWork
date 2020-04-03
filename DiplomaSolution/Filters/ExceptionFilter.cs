using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;
namespace DiplomaSolution.Filters
{
    public class ExceptionFilter : Attribute, IAsyncExceptionFilter
    {
        public ExceptionFilter()
        {
        }

        public async Task OnExceptionAsync(ExceptionContext context)
        {
            Trace.WriteLine("ExceptionFilter");
        }
    }
}
