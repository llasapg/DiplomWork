using System.ComponentModel.DataAnnotations;
using DiplomaSolution.Helpers.Attributes;

namespace DiplomaSolution.Models
{
    /// <summary>
    /// //todo - add DataAnnotation to specify the label name using param Display
    /// </summary>
    public class Customer
    {
        /// <summary>
        /// 
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Required]
        [EmailPattern(RigthTemplates = new string[] { "gmail.com","yahoo.com","softheme.com"})]
        public string EmailAddress { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Required]
        [Display(Name ="Firstname")]
        public string FirstName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Required]
        [Display(Name = "Lastname")]
        public string LastName { get; set; }
    }
}
