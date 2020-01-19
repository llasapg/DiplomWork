using DiplomaSolution.Models;
using DiplomaSolution.Services;
using DiplomaSolution.Services.Classes;
using DiplomaSolution.Services.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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

            services.AddTransient<IRegistrationService, RegistrationService>();

            services.AddTransient<ILogInService, LogInService>();

            services.AddDbContext<CustomerContext>(options => options.UseMySql(connection));

            services.AddIdentity<IdentityUser, IdentityRole>(
                options =>
                {
                    options.Password.RequireNonAlphanumeric = false;
                }).AddEntityFrameworkStores<CustomerContext>(); // Just for work with Db + registers all the identity services ==> we specify a context

            services.AddMvc(options => options.EnableEndpointRouting = false);
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();

            app.UseExceptionHandler("/Error/ExceptionHandler");

            app.UseStatusCodePagesWithReExecute("/Error/{0}");

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    template: "{Controller=HomePage}/{Action=Index}",
                    name: "default"
                    );
            });
        }
    }
}
