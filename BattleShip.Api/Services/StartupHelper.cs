using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using BattleShip.Api.Filters;
using BattleShip.Api.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace BattleShip.Api.Services
{
    public static class StartupHelper
    {
        public static IServiceCollection AddFilters(IServiceCollection services)
        {
            services.AddMvc(opt =>
            {
                opt.Filters.AddService(typeof(IAsyncExceptionFilter));
            });
            services.AddMvcCore()
                .AddDataAnnotations()
                .AddMvcOptions(opt =>
                {
                    opt.Filters.Add<ValidateModelFilter>();
                });
            return services;
        }

        public static void SetupSwagger(IServiceCollection services, EnvironmentVariable envVariables)
        {
            var apiVersion = Assembly.GetExecutingAssembly().GetName().Version;
            const string title = "BattleShip: Game API";
            var description = FormatDescription(apiVersion.ToString(), envVariables.EnvironmentName, envVariables.ContainerId);
            services.AddSwaggerGen(c =>
            {
                c.CustomSchemaIds(x => x.FullName);
                c.IgnoreObsoleteProperties();
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = title,
                    Version = $"{apiVersion}",
                    Description = description
                });
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "BattleShip.Api.xml");
                c.IncludeXmlComments(filePath);
            });
            services.AddSwaggerGenNewtonsoftSupport(); // Adds support for Json.Net to Swagger.
        }

        public static void SetupApiDocumentation(IApplicationBuilder app)
        {
            // Creates Swagger JSON
            app.UseSwagger(c =>
            {
                c.RouteTemplate = "api/docs/{documentName}/swagger.json";
            });

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/api/docs/v1/swagger.json", "BattleShip: Game API");
                c.RoutePrefix = "api/docs";
            });
        }


        private static string FormatDescription(string version, string env, string container)
        {
            var sb = new StringBuilder();
            sb.AppendLine($"##### Version:      {version}");
            sb.AppendLine($"##### Env:            {env}");
            sb.AppendLine($"##### Container:  {container}");
            sb.AppendLine($"##### TZ:              {TimeZoneInfo.Local.Id} | {TimeZoneInfo.Local.DisplayName} | {TimeZoneInfo.Local.StandardName}</br>");
            return sb.ToString();
        }
    }
}
