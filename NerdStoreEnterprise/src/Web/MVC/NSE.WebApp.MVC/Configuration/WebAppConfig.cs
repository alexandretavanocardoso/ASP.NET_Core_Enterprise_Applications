using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NSE.WebApp.MVC.Extensions;

namespace NSE.WebApp.MVC.Configuration
{
    public static class WebAppConfig
    {
        public static void AddWebAppConfig(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddControllersWithViews();

            services.Configure<AppSettings>(configuration);
        }

        public static void UseWebAppConfig(this IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // 500 - erros de servidor
                app.UseExceptionHandler("/erro/500"); // Usado quando não sabe qual erro foi
                app.UseStatusCodePagesWithRedirects("/erro/{0}"); // Usado quando sabe o erro que foi (Erro tratado)
                app.UseHsts();
            }

            // Questão de teste para erro que não é em dev
            // 500 - erros de servidor
            //app.UseExceptionHandler("/erro/500"); // Usado quando não sabe qual erro foi
            //app.UseStatusCodePagesWithRedirects("/erro/{0}"); // Usado quando sabe o erro que foi (Erro tratado)
            //app.UseHsts();

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseIdentityConfiguration();

            app.UseMiddleware<ExceptionMiddleware>();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
