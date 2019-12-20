using System;
using System.ComponentModel.DataAnnotations;

namespace DiplomaSolution.Models
{
    public class Customer
    {
        public int Id { get; set; }
        [Required]
        public string EmailAddress { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        public int? FileId { get; set; } = null;
    }
}
