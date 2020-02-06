using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DiplomaSolution.Helpers.Attributes;

namespace DiplomaSolution.Models
{
    public class Customer
    {
        public int Id { get; set; }
        [Required]
        //[EmailPattern(RigthTemplates = new string[] { "gmail.com","yahoo.com","softheme.com"})]
        public string EmailAddress { get; set; }
        public string Password { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public int SomeFild { get; set; }
        public int? FileId { get; set; } = null;
    }
}
