using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DiplomaSolution.Models.TrainModels
{
    /// <summary>
    /// General model to perform all needed validation and other TAG helpers actions
    /// </summary>
    public class TrainModel
    {
        [DataType(DataType.Password)] // like this
        public string JustTest { get; set; }

        public string SelectedResult { get; set; }

        public IEnumerable<string> SelectedItems { get; set; }

        [Required]
        [Display(Name = "Choose right variant")]
        public IEnumerable<SelectListItem> ListValues { get; set; } = new List<SelectListItem>
        {
            new SelectListItem
            {
                Value= "csharp",
                Text="C#",
                Group = new SelectListGroup { Name = "Retards"}
            },
            new SelectListItem{Value= "python", Text= "Python", Group = new SelectListGroup { Name = "Retards"}},
            new SelectListItem{Value= "cpp", Text="C++"},
            new SelectListItem{Value= "java", Text="Java"},
            new SelectListItem{Value= "js", Text="JavaScript"},
            new SelectListItem{Value= "ruby", Text="Ruby", Group = new SelectListGroup { Name = "Retards"}},
        };
    }
}
