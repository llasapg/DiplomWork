using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace DiplomaSolution.Controllers
{
    /// <summary>
    /// Error handling controller ( handles exceptions and 400-599 error codes )
    /// </summary>
    public class ErrorController : Controller
    {
        public ErrorController()
        {

        }

        //todo rewrite
        /// <summary>
        /// Action to handle status codes between 400 and 599 ( currently no error handling stuff )
        /// </summary>
        /// <param name="statuscode"></param>
        /// <returns></returns>
        [Route("Error/{statuscode}")]
        public IActionResult StatusCodeErrorHandler(int statuscode)
        {
            var errorData = HttpContext.Features.Get<IStatusCodeReExecuteFeature>();

            ViewBag.QS = errorData.OriginalQueryString;

            ViewBag.OP = errorData.OriginalPath;

            return View("ErrorHandler", statuscode);
        }

        //todo rewrite this shit
        /// <summary>
        /// In case of exception this route will be executed, so we can see error list 
        /// </summary>
        /// <returns></returns>
        [Route("Error/ExceptionHandler")]
        public IActionResult ExceptionHandler()
        {
            var exceptionDetails = HttpContext.Features.Get<IExceptionHandlerPathFeature>();

            ViewBag.EM = exceptionDetails.Error.Message;

            ViewBag.Path = exceptionDetails.Path;

            return View("ExceptionHandler");
        }
    }
}
