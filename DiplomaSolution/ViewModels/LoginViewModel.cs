using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DiplomaSolution.Helpers.Attributes;
using Microsoft.AspNetCore.Authentication;

namespace DiplomaSolution.ViewModels
{
    public class LoginViewModel
    {
        [Required]
        [EmailPattern(RigthTemplates = new string[] { "gmail.com", "yahoo.com", "softheme.com" })]
        public string EmailAddress { get; set; }
        [Required]
        public string Password { get; set; }

        public string ReturnUrl { get; set; }
        public IEnumerable<AuthenticationScheme> ListOfProviders { get; set; }
    }
}
