using System.Collections.Generic;

namespace DiplomaSolution.Models
{
    /// <summary>
    /// General response model for services that provide some operations with account
    /// </summary>
    public class AccountResponseCheckData
    {
        /// <summary>
        /// List with returned validation errors in case of unvalid validation
        /// </summary>
        public  List<string> ValidationErrors { get; set; }

        /// <summary>
        /// Provided status code by service ( 200 - OK, 30. - redirect, etc... )
        /// </summary>
        public int StatusCode { get; set; } // todo - add enum with status codes, that can be returned ( 404 - error with provided data, 300 - redirect to fulfill some info, 200 - OK )

        /// <summary>
        /// Response data to next redirect, or to provide viewModel
        /// </summary>
        public object ResponseData { get; set; } // More like redirect data

        /// <summary>
        /// Can be null in case of no redirect need ( if 300 status code )
        /// </summary>
        public string RedirectUrl { get; set; }
    }
}
