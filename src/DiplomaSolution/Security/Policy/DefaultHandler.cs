using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace DiplomaSolution.Security
{
    /// <summary>
    /// Default handler to check if whe have needed claims
    /// </summary>
    public class DefaultHandler : AuthorizationHandler<DefaultRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, DefaultRequirement requirement)
        {
            var values = requirement.Age;

            var userClaims = context.User.Claims;

            foreach (var item in userClaims)
            { 
                if (item.Type == "UploadPhoto" && item.Value == "true")
                {
                    context.Succeed(requirement);
                }
            }

            return Task.CompletedTask;
        }
    }
}
