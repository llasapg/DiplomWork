using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using DiplomaSolution.Helpers.ErrorResponseMessages;
using DiplomaSolution.Models;
using DiplomaSolution.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

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
        /// <param name="logger"></param>
        /// <param name="userManager"></param>
        /// <param name="signInManager"></param>
        /// <param name="roleManager"></param>
        /// <param name="sendEmailService"></param>
        public RegistrationService(ILogger<RegistrationService> logger, UserManager<ServiceUser> userManager, SignInManager<ServiceUser> signInManager, RoleManager<IdentityRole> roleManager, ISendEmailService sendEmailService)
        {
            UserManager = userManager;
            SignInManager = signInManager;
            RoleManager = roleManager;
            Logger = logger;
            SendEmailService = sendEmailService;
        }

        #region DI services
        private ILogger<RegistrationService> Logger { get; set; }
        private UserManager<ServiceUser> UserManager { get; set; }
        private SignInManager<ServiceUser> SignInManager { get; set; }
        private RoleManager<IdentityRole> RoleManager { get; set; }
        private ISendEmailService SendEmailService { get; set; }
        #endregion

        /// <summary>
        /// Method to perform customer registration in case, that he provided correct data to us
        /// </summary>
        /// <returns></returns>
        public async Task<AccountResponseCheckData> CompleteRegistration(Customer customer)
        {
            var responseCheckData = new AccountResponseCheckData { RedirectUrl = null, ResponseData = null, StatusCode = StatusCodesEnum.BadDataProvided, ValidationErrors = new List<string>() };

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

                    var emailUrlConfirmation = $"https://localhost:5001/Account/ConfirmEmail/{new { UserId = currentUser.Id, Token = token }}";

                    var response = await SendEmailService.SendEmail(new ServiceEmail
                    {
                        FromEmail = "testEmailAddress@gmail.com",
                        FromName = "Yevhen",
                        ToEmail = currentUser.Email,
                        ToName = currentUser.UserName,
                        EmailSubject = "Thank you for register!!!",
                        EmailHtmlText = $"<strong>Hello there! Thank you for registering, please confirm your email using this link : {emailUrlConfirmation}</strong>",
                        EmailText = $"Hello there! Thank you for registering, please confirm your email using this link : {emailUrlConfirmation}",
                    });

                    responseCheckData.ResponseData = emailUrlConfirmation;

                    responseCheckData.RedirectUrl = $"https://localhost:5001/Registration/ConfirmationPage?{customer.FirstName}";

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
