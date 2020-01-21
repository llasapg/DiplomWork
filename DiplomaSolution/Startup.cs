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
            services.AddIdentity<IdentityUser, IdentityRole>(
                options =>
                {
                    options.Password.RequireNonAlphanumeric = false;
                }).AddEntityFrameworkStores<CustomerContext>(); // Just for work with Db + registers all the identity services ==> we specify a context

            //-- Nothing changed in the top code

            services.AddAuthorization(options => // before we didnt have this functionality, but now we can use it to register authencation service with speacial params
            {
                options.DefaultPolicy = new AuthorizationPolicyBuilder()
                  .RequireAuthenticatedUser()
                  .Build();
            }); // There is 


            services.AddControllersWithViews(); // before was AddMvc --> now we can choose wich option to add ( like we can set-up only with controllers or with controllers and views )
            services.AddRazorPages(); // New
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //-- Old

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseStaticFiles();

            //-- Old


            //--New

            app.UseRouting();

            app.UseExceptionHandler("/Error/ExceptionHandler");

            app.UseStatusCodePagesWithReExecute("/Error/{0}");

            app.UseAuthentication(); // Haha its just a new name for obsolete middleware ( identity )

            app.UseAuthorization(); // About attributes like [authorized and allowanonum...]

            // this component is new!!!

            app.UseEndpoints(endp => // before was app.UseMvc with configuring routs inside this method, for now we have this
            {
                // we can map there lots of usefull stuff

                endp.MapControllerRoute("default", "{Controller=HomePage}/{Action=Index}").RequireAuthorization();
            });

            //--New
        }
    }
}
