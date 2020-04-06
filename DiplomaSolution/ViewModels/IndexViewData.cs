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
            new SelectListItem {Value = "CycleColorMap", Text = "Cycle color map" },
            new SelectListItem {Value = "FloodFill", Text = "Flood fill"},
            new SelectListItem {Value = "Flop", Text = "Flop" },
            new SelectListItem {Value = "GammaCorrect", Text = "Gamma correct"},
            new SelectListItem {Value = "GausiianBlur", Text = "Blur" },
            new SelectListItem {Value = "MedianFilter", Text = "Median filter"},
            new SelectListItem {Value = "MotionBlur", Text = "Motion blur"},
            new SelectListItem {Value = "Negate", Text = "Negate"}
        };
        /// <summary>
        /// From the begining is null, but when we process an image can be fullfiled
        /// </summary>
        public string PathToTheResultImage { get; set; }
        /// <summary>
        /// To avoid passind all the form image data again decided to save path to the image like this
        /// </summary>
        public string PathToTheInputImage { get; set; }
    }
}
