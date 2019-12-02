using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace DiplomaSolution.Models
{
    public class FormFile
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public byte[] FileData { get; set; }
    }
}
