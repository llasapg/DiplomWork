using System;
using System.ComponentModel.DataAnnotations;
using DiplomaSolution.Helpers.Attributes;

namespace DiplomaSolution.ViewModels
{
    public class LoginViewModel
    {
        [Required]
        [EmailPattern(RigthTemplates = new string[] { "gmail.com", "yahoo.com", "softheme.com" })]
        public string EmailAddress { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
