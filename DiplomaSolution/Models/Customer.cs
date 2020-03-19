using System.ComponentModel.DataAnnotations;
using DiplomaSolution.Helpers.Attributes;

namespace DiplomaSolution.Models
{
    /// <summary>
    /// //todo - add DataAnnotation to specify the label name using param Display
    /// </summary>
    public class Customer
    {
        public int Id { get; set; }
        [Required]
        [EmailPattern(RigthTemplates = new string[] { "gmail.com","yahoo.com","softheme.com"})]
        public string EmailAddress { get; set; }
        public string Password { get; set; }
        [Required]
        [Display(Name ="Input your firstName")]
        public string FirstName { get; set; }
        [Required]
        [Display(Name = "Input your lastName")]
        public string LastName { get; set; }
        [Required]
        public int SomeFild { get; set; }
        public int? FileId { get; set; } = null;
    }
}
