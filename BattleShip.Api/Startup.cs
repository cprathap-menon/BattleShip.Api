using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using BattleShip.Api.Models;
using BattleShip.Api.Services;
using Newtonsoft.Json.Converters;

namespace BattleShip.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            var appSettings = new AppSettings();
            var containerId = Configuration.GetValue<string>("HOSTNAME") ?? "N/A";
            var env = Configuration.GetValue<string>("ASPNETCORE_ENVIRONMENT");
            Configuration.Bind("AppSettings", appSettings);
            new ApiBootstrapper().Bootstrap(builder, appSettings, containerId, env);
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddMvc().AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.Converters.Add(new StringEnumConverter());

            });
            var timezoneEnv = Configuration.GetValue<string>("TZ");
            var containerId = Configuration.GetValue<string>("HOSTNAME") ?? "N/A";
            var culture = Configuration.GetValue<string>("CULTURE") ?? "Not setup, default en-AU";
            var ssmSecretPrefix = Configuration.GetValue<string>("SSM_SECRETS_PREFIX");
            var env = Configuration.GetValue<string>("ASPNETCORE_ENVIRONMENT");
            var environmentVariables = new EnvironmentVariable
            {
                ContainerId = containerId,
                Culture = culture,
                EnvironmentName = env,
                SsmSecretPrefix = ssmSecretPrefix,
                TimeZone = timezoneEnv
            };
            StartupHelper.AddFilters(services);
            StartupHelper.SetupSwagger(services, environmentVariables);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
            StartupHelper.SetupApiDocumentation(app);
        }
    }
}
