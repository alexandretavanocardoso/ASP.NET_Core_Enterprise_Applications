using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NSE.Api.Identidade.Configuration;

namespace NSE.Api.Identidade
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IHostEnvironment hostEnvironment)
        {
            // Configuracao para user appsetting.developmente
            var builder = new ConfigurationBuilder()
                .SetBasePath(hostEnvironment.ContentRootPath) // Caminho
                .AddJsonFile("appsettings.json", true, true) // Adiciona arquivo depende do ambiente
                .AddJsonFile($"appsettings.{hostEnvironment.EnvironmentName}.json", true, true) // Adiciona arquivo depende do ambiente
                .AddEnvironmentVariables();

            // Verifica se � ambiente de desenvolvimento
            if (hostEnvironment.IsDevelopment())
            {
                // Segredos do usuario
                // Chave de ambiente
                builder.AddUserSecrets<Startup>();
            }

            // Retorna a instancia da IConfigurantio
            Configuration = builder.Build();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddIdentityConfiguration(Configuration);

            services.AddApiConfiguration();

            services.AddSwaggerConfiguration();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseSwaggerConfiguration();

            app.UseApiConfiguration(env);
        }
    }
}
