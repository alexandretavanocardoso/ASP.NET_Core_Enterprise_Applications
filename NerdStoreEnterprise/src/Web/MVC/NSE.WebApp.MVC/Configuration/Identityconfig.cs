using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace NSE.WebApp.MVC.Configuration
{
    public static class Identityconfig
    {
        public static void AddIdentityConfiguration(this IServiceCollection services)
        {
            // Configurando que a Aplicação vai trabalhar com autenticação
            // Cookies
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options => 
                {
                    options.LoginPath = "/login"; // Se não tiver logado, encaminha pra area de autenticacao
                    options.AccessDeniedPath = "/acessoNegado"; // Se usuario navega para area que nao tem acesso
                });
        }

        public static void UseIdentityConfiguration(this IApplicationBuilder app)
        {
            // Configurando que a Aplicação vai trabalhas com autenticação
            app.UseAuthentication();
            app.UseAuthentication();
        }
    }
}
