using Microsoft.Extensions.DependencyInjection;
using Vocare.Data;
using Vocare.Data.Interfaces;
using Vocare.Service;
using Vocare.Service.Intefaces;

namespace Vocare.Configuration
{
    public static class DependencyInjection
    {
        /// <summary>
        /// Método para Injeção de Dependência de Services e Repositories
        /// </summary>
        /// <param name="services"></param>
        public static void AddDependencyInjection(this IServiceCollection services)
        {
            services.AddServices();
            services.AddRepositories();
        }

        private static void AddServices(this IServiceCollection services)
        {
            services.AddTransient<IUsuarioService, UsuarioService>();
            services.AddTransient<ITokenService, TokenService>();
            
        }

        private static void AddRepositories(this IServiceCollection services)
        {
            services.AddTransient<IUsuarioRepository, UsuarioRepository>();
            services.AddTransient<IRefreshTokenRepository, RefreshTokenRepository>();
            services.AddTransient<ITesteRepository, TesteRepository>();
            services.AddTransient<IPerguntaTesteRepository, PerguntaTesteRepository>();
            services.AddTransient<ITesteRepository, TesteRepository>();

        }
    }
}
