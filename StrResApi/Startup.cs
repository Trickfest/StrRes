using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;
using StrResData.Entities;
using StrResServices.Interfaces;
using StrResServices.Functional;
using StrResData.Interfaces;
using StrResData.Repositories;
using StrResApi.Middleware;
using StrResApi.Auth;
using static StrResConfiguration.StrResConfiguration;
using static StrResApi.Auth.Constants;


namespace StrResApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            DbPlatforms dbPlatform = DbPlatform();
            string connection = DbConnectionString(dbPlatform);

            switch (dbPlatform)
            {
                case DbPlatforms.SQL_SERVER:
                    services.AddDbContext<StrResDbContext>(options => options.UseSqlServer(connection, b => b.MigrationsAssembly("StrResApi")));
                    break;

                case DbPlatforms.SQLITE:
                    services.AddDbContext<StrResDbContext>(options => options.UseSqlite(connection, b => b.MigrationsAssembly("StrResApi")));
                    break;

                default:
                    throw new Exception("Unknown database platform");
            }

            // services
            services.AddScoped<ITenantService, TenantService>();
            services.AddScoped<IResourceService, ResourceService>();
            services.AddScoped<IAdminService, AdminService>();

            // repositories
            services.AddScoped<ITenantRepository, TenantRepository>();
            services.AddScoped<IResourceRepository, ResourceRepository>();
            services.AddScoped<IAdminRepository, AdminRepository>();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            
            services.AddAuthentication(o =>
            {
                o.DefaultScheme = STR_RES_AUTH_SCHEME;

            }).AddStrResAuth(STR_RES_AUTH_SCHEME, STR_RES_AUTH_SCHEME_DISPLAY_NAME, o => { });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            //app.UseSecurityMiddleware(); // alternatively, app.UseMiddleware<SecurityMiddleware>(); 
            app.UseMvc();
        }
    }
}
