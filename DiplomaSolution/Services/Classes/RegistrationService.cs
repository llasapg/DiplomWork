using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using DiplomaSolution.Helpers.ErrorResponseMessages;
using DiplomaSolution.Models;
using DiplomaSolution.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DiplomaSolution.Services.Classes
{
    /// <summary>
    /// Service for completing registering  customers that are registered by our web-site
    /// </summary>
    public class RegistrationService : IRegistrationService
    {
        /// <summary>
        /// Ctor to get all needed DI services
        /// </summary>
        public RegistrationService(IUrlHelper urlHelper, IHttpContextAccessor httpContextAccessor, UserManager<ServiceUser> userManager, RoleManager<IdentityRole> roleManager, ISendEmailService sendEmailService)
        {
            UserManager = userManager;
            RoleManager = roleManager;
            SendEmailService = sendEmailService;
            Context = httpContextAccessor;
            UrlHelper = urlHelper;
        }

        #region DI services
        /// <summary>
        /// Identity service with func to create users / update data and other stuff...
        /// </summary>
        private UserManager<ServiceUser> UserManager { get; set; }
        /// <summary>
        /// Identity service with func to create roles and etc...
        /// </summary>
        private RoleManager<IdentityRole> RoleManager { get; set; }
        /// <summary>
        /// Custom email service to send emails using SendGrid provider
        /// </summary>
        private ISendEmailService SendEmailService { get; set; }
        /// <summary>
        /// Prop to get access to the httpcontext object and configure response
        /// </summary>
        private IHttpContextAccessor Context { get; set; }
        /// <summary>
        /// Url helper to create dynamic ULRS
        /// </summary>
        private IUrlHelper UrlHelper { get; set; }
        #endregion

        /// <summary>
        /// Method to perform customer registration in case, that he provided correct data to us
        /// </summary>
        /// <returns></returns>
        public async Task<DefaultServiceResponse> CompleteRegistration(CustomerViewModel customer)
        {
            var responseCheckData = new DefaultServiceResponse { ActionName = "Index", ControllerName = "HomePage", ResponseData = null, StatusCode = StatusCodesEnum.BadDataProvided, ValidationErrors = new List<string>() };

            var userSearchResult = await UserManager.FindByEmailAsync(customer.EmailAddress);

            if (userSearchResult == null) // customer has no data in our DB --> he can be registered
            {
                var user = new ServiceUser
                {
                    Email = customer.EmailAddress,
                    UserName = customer.FirstName
                };

                var result = await UserManager.CreateAsync(user, customer.Password);

                if (result.Succeeded) // errors will be displayed on validation-summary
                {
                    if (CheckIfWeHaveRole("User"))
                        await UserManager.AddToRoleAsync(user, "User");
                    else
                    {
                        await RoleManager.CreateAsync(new IdentityRole { Name = "User" });
                        await UserManager.AddToRoleAsync(user, "User");
                    }

                    var currentUser = await UserManager.FindByEmailAsync(user.Email);

                    await UserManager.AddClaimAsync(currentUser, new Claim("UploadPhoto", "true"));

                    var token = await UserManager.GenerateEmailConfirmationTokenAsync(currentUser);

                    var emailUrlConfirmation = UrlHelper.Action("ConfirmEmail", "Account", new { Token = token, UserId = currentUser.Id }, Context.HttpContext.Request.Scheme);

                    var response = await SendEmailService.SendEmail(new ServiceEmail //todo - add there logging to get the response about loggined customers
                    {
                        FromEmail = "testEmailAddress@gmail.com",
                        FromName = "Yevhen",
                        ToEmail = currentUser.Email,
                        ToName = currentUser.UserName,
                        EmailSubject = "Thank you for register!!!",
                        EmailHtmlText = $"<strong>Hello there! Thank you for registering, please confirm your email using this link : {emailUrlConfirmation}</strong>",
                        EmailText = $"Hello there! Thank you for registering, please confirm your email using this link : {emailUrlConfirmation}",
                    });

                    responseCheckData.ResponseData = customer.FirstName;

                    responseCheckData.ActionName = "ConfirmationPage";

                    responseCheckData.ControllerName = "Registration";

                    responseCheckData.StatusCode = StatusCodesEnum.RedirectNeeded;

                    return responseCheckData;
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        responseCheckData.ValidationErrors.Add(error.Description);
                    }

                    responseCheckData.StatusCode = StatusCodesEnum.BadDataProvided;

                    return responseCheckData;
                }
            }
            else
            {
                responseCheckData.ValidationErrors.Add(DefaultResponseMessages.AllreadyHasAccount);

                responseCheckData.StatusCode = StatusCodesEnum.BadDataProvided;

                return responseCheckData;
            }
        }

        #region Helpers

        /// <summary>
        /// Method to verify, that we have needed role, before appling it to customer
        /// </summary>
        /// <param name="roleName"></param>
        /// <returns></returns>
        private bool CheckIfWeHaveRole(string roleName)
        {
            var defaultUserRole = new IdentityRole { Name = roleName };

            foreach (var role in RoleManager.Roles)
            {
                if (role.Name == defaultUserRole.Name) // that means that we have this role in DB
                {
                    return true;
                }
            }
            return false;
        }

        #endregion
    }
}
