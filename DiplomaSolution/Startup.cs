using System.Security.Claims;
using DiplomaSolution.Extensions;
using DiplomaSolution.Models;
using DiplomaSolution.Security;
using DiplomaSolution.Services.Classes;
using DiplomaSolution.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace DiplomaSolution
{
    public class Startup
    {
        public IConfiguration Configuration { get; set; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            #region DataBase stuff

            var connection = Configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<CustomerContext>(options => options.UseMySql(connection));

            #endregion

            #region Identity

            services.AddIdentity<ServiceUser, IdentityRole>(options =>
            {
                options.Password.RequireNonAlphanumeric = false;
                options.Tokens.EmailConfirmationTokenProvider = "CustomEmailConfirmationProvider";
            })
            .AddEntityFrameworkStores<CustomerContext>()
            .AddDefaultTokenProviders()
            .AddTokenProvider<Security.EmailTokenProvider<ServiceUser>>("CustomEmailConfirmationProvider");

            #endregion

            #region Authorization

            services.AddAuthorization(options =>
            {
                options.InvokeHandlersAfterFailure = false;

                options.DefaultPolicy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();

                options.AddPolicy("DefaultMainPolicy", policy => policy.AddRequirements(new DefaultRequirement()));
            });

            #endregion

            #region Authentication

            services.AddAuthentication().AddGoogle(googleOptions =>
            {
                googleOptions.ClientId = "105610985766-9mues2sn2uess0lcl7n51ns1aulil515.apps.googleusercontent.com";
                googleOptions.ClientSecret = "ZZUIFTkPVqWLSVY7bXUApt4h";
            })
            .AddFacebook(facebookOpt =>
            {
                facebookOpt.AppId = "125314042121210";
                facebookOpt.AppSecret = "160e6d9646ace2f5ab2e979e8a437213";
            });

            #endregion

            #region General MVC stuff

            services.AddControllersWithViews();
            services.AddRazorPages();

            #endregion

            #region Custom servises injection and configuring

            services.AddTransient<IFileManagerService, FileManagerService>();
            services.AddTransient<ISendEmailService, SendGridEmailSender>();
            services.AddTransient<IAuthorizationHandler, DefaultHandler>();

            services.ConfigureApplicationCookie(options =>
            {
                options.AccessDeniedPath = "Account/AccessDenied";
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
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles(); // To increase performance we should alloocate this is the begining of the pipeline

            app.UseExceptionHandler("/Error/ExceptionHandler");

            app.UseStatusCodePagesWithReExecute("/Error/{0}");

            app.UseRouting(); // its a endpoint middleware, which desides where this request will be handled + can add some data ( meta ) and etc...

            app.UseAuthentication();

            app.UseAuthorization(); // About attributes like [authorized and allowanonum...]

            app.UseEndpoints(endp => // before was app.UseMvc with configuring routs inside this method, for now we have this
            {
                // This method is used to add new routs

                endp.AddVersioning();

                endp.MapControllerRoute("default", "{Controller=HomePage}/{Action=Index}").RequireAuthorization();
            });
        }
    }
}
