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
            services.AddAuthorization(options =>
            {
                options.DefaultPolicy = new AuthorizationPolicyBuilder()
                  .RequireAuthenticatedUser()
                  .Build();
            });
            services.AddControllersWithViews();
            services.AddRazorPages();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if(env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseStaticFiles();
            app.UseRouting();
            app.UseExceptionHandler("/Error/ExceptionHandler");
            app.UseStatusCodePagesWithReExecute("/Error/{0}");
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endp =>
            {
                endp.MapControllerRoute("default", "{Controller=HomePage}/{Action=Index}").RequireAuthorization();
            });
        }
    }
}
