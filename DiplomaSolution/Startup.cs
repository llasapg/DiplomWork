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
using Microsoft.AspNetCore.Authentication;

namespace DiplomaSolution
{
    public class Startup
    {
        public IConfiguration Configuration { get; set; }

        public Startup(IConfiguration configuration) // appsettings.json --> secrets.json (Secret manager ) --> EV --> console params
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            var connection = Configuration.GetConnectionString("DefaultConnection");
            services.AddTransient<IFileManagerService, FileManagerService>();
            services.AddTransient<ISendEmailService, SendGridEmailSender>();
            services.AddDbContext<CustomerContext>(options => options.UseMySql(connection));
            services.AddIdentity<ServiceUser, IdentityRole>(options =>
            {
                options.Password.RequireNonAlphanumeric = false;
            }).AddEntityFrameworkStores<CustomerContext>().AddDefaultTokenProviders();

            services.AddAuthorization(options => // FIX IT!!!
            {
                options.InvokeHandlersAfterFailure = false;

                options.DefaultPolicy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();

                options.AddPolicy("DefaultUserPolicy", policy => policy.RequireClaim("UploadPhoto", "true").RequireRole("User"));

                options.AddPolicy("Custom", policy =>
                {
                    policy.RequireAssertion(context =>
                    {
                        return context.User.HasClaim(claim => claim.Value == "SSS");
                    });
                });

                options.AddPolicy("ShitPolicy", policy => policy.AddRequirements(new DefaultRequirement()));
            });

            services.AddTransient<IAuthorizationHandler, DefaultHandler>();

            services.AddAuthentication()
                .AddGoogle(googleOptions => {
                googleOptions.ClientId = "105610985766-9mues2sn2uess0lcl7n51ns1aulil515.apps.googleusercontent.com";
                googleOptions.ClientSecret = "ZZUIFTkPVqWLSVY7bXUApt4h";
            }).AddFacebook(facebookOpt => {
                facebookOpt.AppId = "125314042121210";
                facebookOpt.AppSecret = "160e6d9646ace2f5ab2e979e8a437213";
            });

            services.AddControllersWithViews(); // before was AddMvc --> now we can choose wich option to add ( like we can set-up only with controllers or with controllers and views )
            services.AddRazorPages(); // New
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
