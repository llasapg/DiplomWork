using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace DiplomaSolution.Controllers
{
    public class ErrorController : Controller
    {
        [Route("Error/{statuscode}")]
        public IActionResult StatusCodeErrorHandler(int statuscode)
        {
            var errorData = HttpContext.Features.Get<IStatusCodeReExecuteFeature>();

            ViewBag.QS = errorData.OriginalQueryString;

            ViewBag.OP = errorData.OriginalPath;

            return View("ErrorHandler", statuscode);
        }

        [Route("Error/ExceptionHandler")]
        public IActionResult ExceptionHandler()
        {
            var exceptionDetails = HttpContext.Features.Get<IExceptionHandlerPathFeature>();

            ViewBag.EM = exceptionDetails.Error.Message;

            ViewBag.Path = exceptionDetails.Path;

            return View();
        }
    }
}
