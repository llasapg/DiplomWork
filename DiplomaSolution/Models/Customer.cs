using System;
namespace DiplomaSolution.Models
{
    public class Customer
    {
        public int Id { get; set; }
        public string EmailAddress { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int? FileId { get; set; } = null;
    }
}
