using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Vocare.Configuration
{
    public static class HttpProtection
    {
        public static void AddHttpProtection(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("DevPolicy",
                    builder => builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod().WithExposedHeaders("x-pagination"));
                options.AddPolicy("UATPolicy",
                    builder => builder.WithOrigins("https://localhost:44391").AllowAnyHeader().AllowAnyMethod().WithExposedHeaders("x-pagination"));
                // Mudar de https://localhost:44391 para a rota de UAT do front-end
                options.AddPolicy("ProdPolicy",
                    builder => builder.WithOrigins("https://localhost:44391").AllowAnyHeader().AllowAnyMethod().WithExposedHeaders("x-pagination"));
                // Mudar de https://localhost:44391 para a rota de produção do front-end
            });
        }

        public static void UseHttpProtection(this IApplicationBuilder app, IWebHostEnvironment env)
        {

            app.UseHttpsRedirection();
            app.UseRouting();

            if (env.IsEnvironment("PROD"))
            {
                app.UseCors("ProdPolicy");
            }
            else if (env.IsEnvironment("UAT"))
            {
                app.UseCors("UATPolicy");
            }
            else
            {
                app.UseCors("DevPolicy");
            }
        }
    }
}
