using DiplomaSolution.Models;
using DiplomaSolution.Services;
using DiplomaSolution.Services.Classes;
using DiplomaSolution.Services.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
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

            services.AddDbContext<CustomerContext>(options => options.UseMySQL(connection));

            services.AddMvc();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseStaticFiles();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    template: "SmartX/{Controller=HomePage}/{Action=Index}",
                    name: "default"
                    );
            });
        }
    }
}
