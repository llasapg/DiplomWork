using System.ComponentModel.DataAnnotations;

namespace DiplomaSolution.Models
{
    public class RoleModel
    {
        [Required]
        public string RoleName { get; set; }
    }
}
