using System;
using System.ComponentModel.DataAnnotations;

namespace DiplomaSolution.ViewModels
{
    public class RegisterViewData
    {
        [Required]
        [Compare("")]
        [DataType(DataType.Password)] // check that password was hidden
        [EmailAddress]
        [Display(Name = "Email")]
        public string EmailAddress { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
