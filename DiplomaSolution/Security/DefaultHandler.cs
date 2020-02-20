using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;

namespace DiplomaSolution.Security
{
    public class DefaultHandler : AuthorizationHandler<DefaultRequirement>
    {
        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context, // migth be the context of the request ( we can get what user is comming for us and check if we what him to proceed )
            DefaultRequirement requirement)
        {
            var userClaims = context.User.Claims;

            //var query = context.Resource as AuthorizationOptions;

            foreach (var item in userClaims)
            { 
                if (item.Type == "UploadPhoto" && item.Value == "true")
                {
                    context.Succeed(requirement);
                }
            }

            return Task.CompletedTask; // its not like must-have option to exit from method
        }
    }
}
