using Microsoft.AspNetCore.Authorization;

namespace DiplomaSolution.Security
{
    public class DefaultRequirement : IAuthorizationRequirement
    {
        public int Age { get; set; }

        public DefaultRequirement(int age)
        {
            Age = age;
        }
    }
}
