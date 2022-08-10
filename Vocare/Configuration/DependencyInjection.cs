using Data;
using Microsoft.Extensions.DependencyInjection;
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
            
        }

        private static void AddRepositories(this IServiceCollection services)
        {
            services.AddTransient<IUsuarioRepository, UsuarioRepository>();
           
        }
    }
}
