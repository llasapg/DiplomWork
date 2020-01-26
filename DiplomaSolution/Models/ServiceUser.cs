using Microsoft.AspNetCore.Identity;

namespace DiplomaSolution.Models
{
    public class ServiceUser : IdentityUser
    {
        public string FileName { get; set; }
    }
}
