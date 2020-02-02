using DiplomaSolution.Extensions;
using DiplomaSolution.Middlewares;
using DiplomaSolution.Models;
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
            var connection = Configuration.GetConnectionString("DefaultConnection");
            services.AddTransient<IFileManagerService, FileManagerService>();
            services.AddDbContext<CustomerContext>(options => options.UseMySql(connection));
            services.AddIdentity<ServiceUser, IdentityRole>(options => { options.Password.RequireNonAlphanumeric = false;})
                .AddEntityFrameworkStores<CustomerContext>();

            services.AddAuthorization(options => 
            {
                options.DefaultPolicy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();

                options.AddPolicy("DefaultUserPolicy", policy => policy.RequireClaim("UserAction").RequireRole("User"));
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

            app.UseRouting(); // its a endpoint middleware, which desides where this request will be handled + can add some data ( meta ) and etc...

            app.UseExceptionHandler("/Error/ExceptionHandler");

            app.UseStatusCodePagesWithReExecute("/Error/{0}");

            app.UseAuthentication(); // Haha its just a new name for obsolete middleware ( identity )

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
