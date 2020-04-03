using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DiplomaSolution.Models
{
    /// <summary>
    /// View model for homepage to the file from customer
    /// </summary>
    public class IndexViewData
    {
        /// <summary>
        /// File, that user provided
        /// </summary>
        public IFormFile FormFileData { get; set; }
        /// <summary>
        /// Response from filemanager service in case of wrong data provided
        /// </summary>
        public List<string> ValidationResponse { get; set; }
        /// <summary>
        /// Stores file format choosed by the customer
        /// </summary>
        public string SelectedResponseFileFormat { get; set; }
        /// <summary>
        /// List of available file formats
        /// </summary>
        [Display(Name = "Select out-put file format")]
        public List<SelectListItem> FileFormats { get; set; } = new List<SelectListItem>
        {
            new SelectListItem {Value = "PNG", Text = "PNG" },
            new SelectListItem {Value = "JPG", Text = "JPG"}
        };
        /// <summary>
        /// Selected file operation by the customer
        /// </summary>
        public string SelectedFileOperation { get; set; }
        /// <summary>
        /// List with file operations
        /// </summary>
        [Display(Name = "Select operation")]
        public List<SelectListItem> OpetationList { get; set; } = new List<SelectListItem>
        {
            new SelectListItem {Value = "OperationOne", Text = "OperationOne" },
            new SelectListItem {Value = "OperationTwo", Text = "OperationTwo"},
            new SelectListItem {Value = "OperationTree", Text = "OperationTree" },
            new SelectListItem {Value = "OperationFour", Text = "OperationFour"},
            new SelectListItem {Value = "OperationFive", Text = "OperationFive" },
            new SelectListItem {Value = "OperationSix", Text = "OperationSix"}
        };

    }
}
