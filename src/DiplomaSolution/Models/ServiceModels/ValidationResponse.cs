using System.Collections.Generic;

namespace DiplomaSolution.Models
{
    /// <summary>
    /// General response model for services that provide some operations with account
    /// </summary>
    public class DefaultServiceResponse
    {
        /// <summary>
        /// List with returned validation errors in case of unvalid validation
        /// </summary>
        public  List<string> ValidationErrors { get; set; }

        /// <summary>
        /// Returned from the service status code to specify what should be done next
        /// </summary>
        public StatusCodesEnum StatusCode { get; set; }

        /// <summary>
        /// Response data to next redirect, or to provide viewModel
        /// </summary>
        public object ResponseData { get; set; }

        /// <summary>
        /// Redirect controller name
        /// </summary>
        public string ControllerName { get; set; }

        /// <summary>
        /// Redirect action name
        /// </summary>
        public string ActionName { get; set; }
    }
}
