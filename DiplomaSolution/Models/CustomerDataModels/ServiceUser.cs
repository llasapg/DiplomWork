using Microsoft.AspNetCore.Identity;

namespace DiplomaSolution.Models
{
    public class ServiceUser : IdentityUser
    {
        public string FileId { get; set; }
    }
}
