using System;
using System.ComponentModel.DataAnnotations;

namespace DiplomaSolution.ViewModels
{
    public class ResetPasswordConfirmationModel
    {
        public string UserId { get; set; }
        public string Token { get; set; }
        [Required]
        public string Password { get; set; }
        [Compare("Password",ErrorMessage ="Passwords are not same")]
        [Required]
        public string ConfirmPassword { get; set; }
    }
}
