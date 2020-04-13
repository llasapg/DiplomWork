using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using DiplomaSolution.Helpers.ErrorResponseMessages;
using DiplomaSolution.Models;
using DiplomaSolution.Services.Interfaces;
using DiplomaSolution.ViewModels;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DiplomaSolution.Services.Classes
{
    public class AccountService : IAccountService
    {
        /// <summary>
        /// Main service from Identity to perform signIn of the user
        /// </summary>
        private SignInManager<ServiceUser> SignInManager { get; set; }
        /// <summary>
        /// Main service from Identity to perform user register / remove / verification and etc...
        /// </summary>
        private UserManager<ServiceUser> UserManager { get; set; }
        /// <summary>
        /// Custom service to perform email sending using SendGrind functional
        /// </summary>
        private ISendEmailService SendEmailService { get; set; }
        /// <summary>
        /// System interface for security purpose
        /// </summary>
        private IDataProtectionProvider DataProtectionProvider { get; set; }
        /// <summary>
        /// Data protector to hide request data provided in query string
        /// </summary>
        private IDataProtector Protector { get; set; }
        /// <summary>
        /// Url helper to create dynamic ULRS
        /// </summary>
        private IUrlHelper UrlHelper { get; set; }
        /// <summary>
        /// Prop to get HttpContext object to configure the request and etc...
        /// </summary>
        private IHttpContextAccessor Context { get; set; }
        /// <summary>
        /// Manage the roles
        /// </summary>
        private RoleManager<IdentityRole> RoleManager { get; set; }
        /// <summary>
        /// Basic construstor to perform DI
        /// </summary>
        public AccountService(IHttpContextAccessor context, SignInManager<ServiceUser> signInManager, UserManager<ServiceUser> userManager, ISendEmailService sendEmailService, IDataProtectionProvider dataProtecttionProvider, IUrlHelper urlHelper, RoleManager<IdentityRole> roleManager)
        {
            SignInManager = signInManager;
            UserManager = userManager;
            SendEmailService = sendEmailService;
            DataProtectionProvider = dataProtecttionProvider;
            Protector = DataProtectionProvider.CreateProtector("DataProtection");
            UrlHelper = urlHelper;
            Context = context;
            RoleManager = roleManager;
        }

        /// <summary>
        /// Main method to perform login of the customer
        /// </summary>
        /// <param name="customer"></param>
        /// <param name="returnUrl"></param>
        public async Task<DefaultServiceResponse> LoginCustomer(LoginViewModel customer, string returnUrl = null)
        {
            var validationResponse = new DefaultServiceResponse { StatusCode = StatusCodesEnum.Ok, ValidationErrors = new List<string>(), ResponseData = null };

            var userData = await UserManager.FindByEmailAsync(customer.EmailAddress);

            if (userData != null)
            {
                var providedData = await SignInManager.CheckPasswordSignInAsync(userData, customer.Password, true);

                if (providedData.ToString() == "Failed")
                {
                    validationResponse.StatusCode = StatusCodesEnum.BadDataProvided; // in case of this response --> reload view with validation errors

                    validationResponse.ValidationErrors.Add(DefaultResponseMessages.WrongPasswordAndEmailCombination);

                    return validationResponse;
                }
                else if (providedData.IsLockedOut)
                {
                    validationResponse.StatusCode = StatusCodesEnum.BadDataProvided;

                    validationResponse.ValidationErrors.Add(DefaultResponseMessages.AccountIsLockOut);

                    return validationResponse;
                }
                else if (providedData.Succeeded) // Check that customer has a verified email
                {
                    if (userData.EmailConfirmed == false)
                    {
                        validationResponse.StatusCode = StatusCodesEnum.BadDataProvided;

                        validationResponse.ValidationErrors.Add(DefaultResponseMessages.EmailIsNotVerified);

                        return validationResponse;
                    }
                    else
                    {
                        var loginResponse = await SignInManager.PasswordSignInAsync(userData, customer.Password, false, true);

                        if (loginResponse.Succeeded && returnUrl != null)
                        {
                            validationResponse.StatusCode = StatusCodesEnum.RedirectNeeded; // Can be changed in future to valid one ( for now --> simple redirect to home page )

                            return validationResponse;
                        }
                        else if (loginResponse.Succeeded)
                        {
                            validationResponse.StatusCode = StatusCodesEnum.Ok; // no redirect needed --> return URL was not provided

                            return validationResponse;
                        }
                    }
                }
            }

            validationResponse.StatusCode = StatusCodesEnum.BadDataProvided;

            validationResponse.ValidationErrors.Add(DefaultResponseMessages.CustomerIsNotFoundInDb);

            return validationResponse;
        }

        /// <summary>
        /// Method to perform login of the external customer
        /// </summary>
        /// <param name="returnUrl"></param>
        /// <param name="remoteError"></param>
        /// <returns></returns>
        public async Task<DefaultServiceResponse> ExternalLoginCallBack(string returnUrl = null, string remoteError = null)
        {
            var loginCallBackResult = new DefaultServiceResponse { ActionName = "Index", ControllerName = "HomePage", ResponseData = null, StatusCode = StatusCodesEnum.Ok, ValidationErrors = new List<string>() };

            if (remoteError == null)
            {
                var accountDetailsFromProvider = await SignInManager.GetExternalLoginInfoAsync();

                #region Check, that data is provided

                if (accountDetailsFromProvider != null)
                {
                    // Search for the customer in our db

                    var accountData = await UserManager.FindByEmailAsync(accountDetailsFromProvider.Principal.FindFirstValue(ClaimTypes.Email));

                    #region New account for our DB

                    if (accountData == null) // new account
                    {
                        var serviceUser = new ServiceUser
                        {
                            Email = accountDetailsFromProvider.Principal.FindFirstValue(ClaimTypes.Email),
                            UserName = accountDetailsFromProvider.Principal.FindFirstValue(ClaimTypes.GivenName)
                        };

                        var userResponse = await UserManager.CreateAsync(serviceUser);

                        #region Customer created without any error --> send him email to verify email and add password ( or not )

                        if (userResponse.Succeeded)
                        {
                            await UserManager.AddClaimAsync(serviceUser, new Claim("UploadPhoto", "true"));

                            if(CheckIfWeHaveRole("User"))
                                await UserManager.AddToRoleAsync(serviceUser, "User");
                            else
                            {
                                await RoleManager.CreateAsync(new IdentityRole { Name = "User", NormalizedName = "USER"});
                                await UserManager.AddToRoleAsync(serviceUser, "User");
                            }

                            await UserManager.AddLoginAsync(serviceUser, accountDetailsFromProvider); // Creates point in AspNetUserLogins

                            var token = await UserManager.GenerateEmailConfirmationTokenAsync(serviceUser);

                            var emailUrlConfirmation = UrlHelper.Action("ConfirmEmail", "Account", new { Token = token, UserId = serviceUser.Id }, Context.HttpContext.Request.Scheme);

                            await SendEmailService.SendEmail(new ServiceEmail
                            {
                                FromEmail = "testEmailAddress@gmail.com",
                                FromName = "Yevhen",
                                ToEmail = serviceUser.Email,
                                ToName = serviceUser.UserName,
                                EmailSubject = "Thank you for register!!!",
                                EmailHtmlText = $"<strong>Hello, {serviceUser.UserName}! Thank you for registering, please confirm your email using this link : {emailUrlConfirmation}</strong>",
                                EmailText = $"Hello there! Thank you for registering, please confirm your email using this link : {emailUrlConfirmation}",
                            });

                            loginCallBackResult.ActionName = "ConfirmPartnerRegister";

                            loginCallBackResult.ControllerName = "Registration";

                            loginCallBackResult.ResponseData = serviceUser.UserName;

                            loginCallBackResult.StatusCode = StatusCodesEnum.RedirectNeeded;

                            return loginCallBackResult;
                        }

                        #endregion

                        else
                        {
                            loginCallBackResult.ValidationErrors.Add(DefaultResponseMessages.ExternalLoginFailed);

                            loginCallBackResult.StatusCode = StatusCodesEnum.BadDataProvided;

                            return loginCallBackResult;
                        }
                    }

                    #endregion

                    else
                    {
                        #region Customer already has account on our db, but email is not confirmed

                        if (!accountData.EmailConfirmed)
                        {
                            var loginsData = await UserManager.GetLoginsAsync(accountData);

                            if (loginsData == null)
                            {
                                await UserManager.AddLoginAsync(accountData, accountDetailsFromProvider);
                            }
                            var token = await UserManager.GenerateEmailConfirmationTokenAsync(accountData);

                            var emailUrlConfirmation = UrlHelper.Action("ConfirmEmail", "Account", new { Token = token, UserId = accountData.Id }, Context.HttpContext.Request.Scheme);

                            await SendEmailService.SendEmail(new ServiceEmail
                            {
                                FromEmail = "testEmailAddress@gmail.com",
                                FromName = "Yevhen",
                                ToEmail = accountData.Email,
                                ToName = accountData.UserName,
                                EmailSubject = "Thank you for register!!!",
                                EmailHtmlText = $"<strong>Hello, {accountData.UserName}! Thank you for registering, please confirm your email using this link : {emailUrlConfirmation}</strong>",
                                EmailText = $"Hello there! Thank you for registering, please confirm your email using this link : {emailUrlConfirmation}",
                            });

                            loginCallBackResult.StatusCode = StatusCodesEnum.RedirectNeeded;

                            loginCallBackResult.ResponseData = accountData.UserName;

                            loginCallBackResult.ActionName = "ConfirmPartnerRegister";

                            loginCallBackResult.ControllerName = "Registration";

                            return loginCallBackResult;
                        }

                        #endregion

                        #region Email is confirmed but we need to check, that customer has row in ASPNETUSERLOGINS

                        else
                        {
                            var loginReponse = await SignInManager.ExternalLoginSignInAsync(accountDetailsFromProvider.LoginProvider, accountDetailsFromProvider.ProviderKey, false);

                            if (loginReponse.Succeeded)
                            {
                                loginCallBackResult.StatusCode = StatusCodesEnum.Ok;
                            }
                            else
                            {
                                await UserManager.AddLoginAsync(accountData, accountDetailsFromProvider);

                                await SignInManager.SignInAsync(accountData, false);
                            }

                            loginCallBackResult.ActionName = "Index";

                            loginCallBackResult.ControllerName = "HomePage";

                            return loginCallBackResult;
                        }

                        #endregion
                    }
                }

                #endregion

                #region No data provided from external provider

                else
                {
                    loginCallBackResult.ValidationErrors.Add(DefaultResponseMessages.ExternalLoginFailed);

                    loginCallBackResult.StatusCode = StatusCodesEnum.BadDataProvided;

                    return loginCallBackResult;
                }

                #endregion
            }

            #region Error from external provider

            else
            {
                loginCallBackResult.ValidationErrors.Add(DefaultResponseMessages.ExternalLoginFailed);

                loginCallBackResult.StatusCode = StatusCodesEnum.RedirectNeeded;

                loginCallBackResult.ActionName = "Login";

                loginCallBackResult.ControllerName = "Account";

                return loginCallBackResult;
            }

            #endregion
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
