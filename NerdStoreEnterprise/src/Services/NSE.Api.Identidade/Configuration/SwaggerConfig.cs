using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NSE.Api.Identidade.Configuration
{
    public static class SwaggerConfig
    {
        public static IServiceCollection AddSwaggerConfiguration(this IServiceCollection services)
        {
            // Swagger
            services.AddSwaggerGen(options => {
                options.SwaggerDoc("v1", new OpenApiInfo()
                {
                    Title = "NerdStore Enterprise Identity API",
                    Description = "Loja Online, desenvolvido com AspNet 3.1",
                    Contact = new OpenApiContact() { Name = "Alexandre Tavano", Email = "AlexandreTavanoDeveloper@outlook.com" },
                    License = new OpenApiLicense() { Name = "MIT", Url = new Uri("https://opensource.org/licenses/MIT") }
                });
            });

            return services;
        }

        public static IApplicationBuilder UseSwaggerConfiguration(this IApplicationBuilder app)
        {
            // Swagger
            app.UseSwagger();
            app.UseSwaggerUI(options => {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
            });

            return app;
        }
    }
}
