using System.ComponentModel.DataAnnotations;
using DiplomaSolution.Helpers.Attributes;

namespace DiplomaSolution.Models
{
    public class CustomerViewModel
    {
        /// <summary>
        /// Customer email
        /// </summary>
        [Required]
        [EmailPattern(RigthTemplates = new string[] { "gmail.com","yahoo.com","softheme.com"})]
        public string EmailAddress { get; set; }
        /// <summary>
        /// Customer password
        /// </summary>
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        /// <summary>
        /// Customer firstName
        /// </summary>
        [Required]
        [Display(Name ="Firstname")]
        public string FirstName { get; set; }
        /// <summary>
        /// Customer lastName
        /// </summary>
        [Required]
        [Display(Name = "Lastname")]
        public string LastName { get; set; }
    }
}
