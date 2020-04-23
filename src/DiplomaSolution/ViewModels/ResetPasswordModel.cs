using System.ComponentModel.DataAnnotations;

namespace DiplomaSolution.ViewModels
{
    public class ResetPasswordModel
    { 
        [Required]
        [EmailAddress]
        public string EmailAddress { get; set; }
    }
}
