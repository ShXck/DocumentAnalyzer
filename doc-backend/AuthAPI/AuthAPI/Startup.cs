using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;

using Microsoft.EntityFrameworkCore;

using APIAuthLibrary;

using AuthLibrary.Factory;
using AuthLibrary.Configuration;

using DataHandlerSQL.Configuration;
using DataHandlerSQL.Factory;

using AuthAPI.Services.EncryptionService;

namespace AuthAPI
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

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "AuthAPI", Version = "v1" });
            });

            /*services.AddAuthentication("BasicAuthentication").
                AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>
                ("BasicAuthentication", null);*/

            services.AddHttpContextAccessor();


            /*------------------------------------------------------------------------*/
            // Configuration related to the APIAuthLibrary & AuthLibrary
            /*------------------------------------------------------------------------*/
            AuthServiceConfig.Config.SecretKey = Environment.GetEnvironmentVariable("DOCANALYZER_TOKEN_SECRETKEY");
            AuthServiceConfig.Config.IssuerToken = Environment.GetEnvironmentVariable("DOCANALYZER_TOKEN_ISSUERTOKEN");
            AuthServiceConfig.Config.AuthType = Environment.GetEnvironmentVariable("DOCANALYZER_TOKEN_AUTHTYPE");

            string expTime = Environment.GetEnvironmentVariable("DOCANALYZER_TOKEN_EXPIRATIONTIME");
            AuthServiceConfig.Config.ExpirationTime = int.Parse(expTime);

            services.AddAuthentication("Authorized")
                .AddScheme<AuthenticationSchemeOptions, AuthHandler>("Authorized", "Authorized", opts => { });

            services.AddScoped<IAuthServiceFactory, AuthServiceFactory>();


            /*------------------------------------------------------------------------*/
            // Configuration related to the DataHandlerSQL
            /*------------------------------------------------------------------------*/
            string connStringPostgreSQL = Configuration["ConnectionStrings:PostgreSQL_DB"];
            DataHandlerSQLConfig.Config.ConnectionString = connStringPostgreSQL;
            services.AddScoped<IUnitOfWorkFactory, UnitOfWorkFactory>();


            /*------------------------------------------------------------------------*/
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "AuthAPI v1"));
            }

            app.UseRouting();

            // TODO: Adjust CORS settings appropriately
            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

        }
    }
}
