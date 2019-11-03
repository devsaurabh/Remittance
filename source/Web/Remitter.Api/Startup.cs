using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Remitter.Api.Framework;
using Remitter.Api.Framework.Middleware;
using Serilog;
using System.IO;

namespace Remitter.Api
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
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Remitter API", Version = "v1" });
            });
            JsonConvert.DefaultSettings = () => new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };
            //services.AddMemoryCache();
            ServiceRegistration.Register(services, Configuration);
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.Authority = "{yourAuthorizationServerAddress}";
                    options.Audience = "{yourAudience}";
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            string logFilePath = Path.Combine(env.ContentRootPath, "Logs/Remitter-{Date}.txt");

            Log.Logger = new LoggerConfiguration()
                .WriteTo.RollingFile(logFilePath)
                .ReadFrom.Configuration(Configuration)
               .CreateLogger();

            loggerFactory.AddSerilog();
            app.ConfigureExceptionHandler();
            app.UseHsts();
            app.UseSerilogRequestLogging();
            app.UseHttpsRedirection();
            app.UseMvc();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });

            if(!env.IsDevelopment())
            {
                app.UseAuthentication();
            }
            

        }
    }
}
