using DiplomaSolution.ConfigurationModels;
using DiplomaSolution.Extensions;
using DiplomaSolution.Models;
using DiplomaSolution.Security;
using DiplomaSolution.Services.Classes;
using DiplomaSolution.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;

namespace DiplomaSolution
{
    /// <summary>
    /// Basic class, what creates request handling pipeline and DI provider
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// Basic configuration prop what stores all provided configurations
        /// </summary>
        public IConfiguration Configuration { get; set; }
        public ILoggerFactory LoggerFactory { get; set; }

        /// <summary>
        /// Ctop to get configurations
        /// </summary>
        /// <param name="configuration"></param>
        public Startup(IConfiguration configuration, ILoggerFactory loggerFactory)
        {
            Configuration = configuration;
            LoggerFactory = loggerFactory;
        }

        /// <summary>
        /// Basic Asp.net core DI method ( Reqister services there )
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            #region DataBase stuff

            var connection = Configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<CustomerContext>(options => options.UseMySql(connection, builder =>
            {
                builder.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null);
            }));

            #endregion

            #region Identity

            services.AddIdentity<ServiceUser, IdentityRole>(options =>
            {
                #region Password options ( Service name is - PasswordValidator )
                options.Password.RequireNonAlphanumeric = false;
                #endregion

                #region Custom token providers
                options.Tokens.EmailConfirmationTokenProvider = "CustomEmailConfirmationProvider";
                #endregion

                #region Lockout options
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
                options.Lockout.MaxFailedAccessAttempts = 5;
                #endregion
            })
            .AddEntityFrameworkStores<CustomerContext>() // Adds UserStore and RoleStore to make awailable to UserManager and other services work with tables representation
            .AddDefaultTokenProviders() // Adds default token providers
            .AddTokenProvider<Security.EmailTokenProvider<ServiceUser>>("CustomEmailConfirmationProvider");

            #endregion

            #region Authorization

            services.AddAuthorization(options =>
            {
                options.InvokeHandlersAfterFailure = false;

                options.DefaultPolicy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();

                options.AddPolicy("DefaultMainPolicy", policy =>
                {
                    policy.AddRequirements(new DefaultRequirement());
                });
            });

            #endregion

            #region Authentication

            var settings = Configuration.GetSection("Authentication");

            // Currently we have this call in AddIdentity method, but we need to specify what provider exactly we want to add
            services.AddAuthentication().AddGoogle(googleOptions =>
            {
                googleOptions.ClientId = settings["GoogleAuthentication:ClientId"];
                googleOptions.ClientSecret = settings["GoogleAuthentication:ClientSecret"];
                googleOptions.RemoteAuthenticationTimeout = TimeSpan.FromHours(1);
            })
            .AddFacebook(facebookOpt =>
            {
                facebookOpt.AppId = settings["FacebookAuthentication:AppId"];
                facebookOpt.AppSecret = settings["FacebookAuthentication:AppSecret"];
                facebookOpt.RemoteAuthenticationTimeout = TimeSpan.FromHours(1);
            });

            #endregion

            #region General MVC stuff

            services.AddControllersWithViews();
            services.AddRazorPages();

            #endregion

            #region Custom servises injection and configuring

            services.Configure<Authentication>(Configuration.GetSection("Authentication"));
            services.Configure<FileConfiguration>(Configuration.GetSection("FileConfigurations"));
            services.AddTransient<IFileManagerService, FileManagerService>();
            services.AddTransient<ISendEmailService, SendGridEmailSender>();
            services.AddTransient<IAuthorizationHandler, DefaultHandler>();
            services.AddTransient<IAccountService, AccountService>();
            services.AddTransient<IRegistrationService, RegistrationService>();
            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
            services.AddHttpContextAccessor();
            services.AddScoped(urlHelperOptions => {
                var actionContext = urlHelperOptions.GetRequiredService<IActionContextAccessor>().ActionContext;
                var factory = urlHelperOptions.GetRequiredService<IUrlHelperFactory>();
                return factory.GetUrlHelper(actionContext);
            });

            services.ConfigureApplicationCookie(options =>
            {
                options.AccessDeniedPath = "/Account/AccessDenied";
            });

            services.Configure<DataProtectionTokenProviderOptions>(opt => // All token lifeSpan
            {
                opt.TokenLifespan = TimeSpan.FromHours(36);
            });

            services.Configure<EmailTokenOptions>(opt =>
            {
                opt.TokenLifespan = TimeSpan.FromDays(1); // Email token lifeSpan
            });

            #endregion

            #region Logging

            LoggerFactory.AddFileLogger();

            #endregion
        }

        /// <summary>
        /// Request handling pipeline
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseExceptionHandler("/Error/ExceptionHandler");

            app.UseStatusCodePagesWithReExecute("/Error/{0}");

            app.UseStaticFiles(); // To increase performance we should alloocate this is the begining of the pipeline

            app.UseRouting(); // its a endpoint middleware, which desides where this request will be handled + can add some data ( meta ) and etc...

            app.UseAuthentication();

            app.UseAuthorization(); // About attributes like [authorized and allowanonum...]

            app.UseEndpoints(endp => // before was app.UseMvc with configuring routs inside this method, for now we have this
            {
                // This method is used to add new routs

                endp.AddVersioning();

                endp.MapControllerRoute(
                    name : "default",
                    pattern : "{Controller}/{Action}/{value?}",
                    defaults : new { Controller = "HomePage", Action = "Index" });
            });
        }
    }
}
