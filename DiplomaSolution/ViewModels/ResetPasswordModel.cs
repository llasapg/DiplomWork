using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Runtime.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace DiplomaSolution.ViewModels
{
    public class ResetPasswordModel
    { 
        [Required]
        [EmailAddress]
        public string EmailAddress { get; set; }
    }
}
