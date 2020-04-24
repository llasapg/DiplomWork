using System;
using System.ComponentModel.DataAnnotations;

namespace DiplomaSolution.Models
{
    /// <summary>
    /// todo - add attributes 
    /// </summary>
    public class AccountLevelFile
    {
        /// <summary>
        /// File id
        /// </summary>
        [Key]
        public Guid FileId { get; set; }
        /// <summary>
        /// Customer accountId to map this with the file
        /// </summary>
        public string CustomerId { get; set; }
        /// <summary>
        /// Custom file name for the data provided by the customer
        /// </summary>
        public string FullName { get; set; }
        /// <summary>
        /// byte representation of the customer files
        /// </summary>
        public byte[] FileData { get; set; }
    }
}
