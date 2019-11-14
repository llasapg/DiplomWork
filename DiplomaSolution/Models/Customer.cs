using System;
namespace DiplomaSolution.Models
{
    /// <summary>
    /// Basic model for customer
    /// </summary>
    public class Customer
    {
        public int Id { get; set; }
        public string EmailAddress { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
