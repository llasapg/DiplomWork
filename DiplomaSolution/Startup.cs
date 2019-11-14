using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DiplomaSolution.Models;
using DiplomaSolution.Services;
using DiplomaSolution.Services.Classes;
using DiplomaSolution.Services.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DiplomaSolution
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            var connection = Configuration.GetConnectionString("DefaultConnection");

            services.AddTransient<IRegistrationService, RegistrationService>();

            services.AddTransient<ILogInService, LogInService>();

            services.AddDbContext<CustomerContext>(options => options.UseMySQL(connection));

            services.AddMvc();
        }

        public IConfiguration Configuration { get; set; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    template : "{Controller=HomePage}/{Action=Index}",
                    name: "default"
                    );
            });
        }
    }
}
