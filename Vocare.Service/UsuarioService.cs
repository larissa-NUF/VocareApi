using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vocare.Data.Interfaces;
using Vocare.Model;
using Vocare.Service.Intefaces;

namespace Vocare.Service
{
    public class UsuarioService: IUsuarioService
    {
        #region Dependências
        private readonly IConfiguration _config;
        private readonly ILogger<UsuarioService> _logger;
        private readonly IUsuarioRepository _usuarioRepository;
        public UsuarioService(
            IConfiguration config,
            ILoggerFactory loggerFactory,
            IUsuarioRepository usuarioRepository)
        {
            _config = config;
            _logger = loggerFactory.CreateLogger<UsuarioService>();
            _usuarioRepository = usuarioRepository;
        }

        public Usuario Add(Usuario usuario)
        {
            try
            {
                _usuarioRepository.Add(usuario);
                return usuario;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error ao executar o método Addd! usuário: {usuario}", ex);
                throw;
            }
        }
        #endregion

        #region Métodos públicos
        public List<Usuario> GetAll()
        {
            try
            {
                var usuario = _usuarioRepository.GetAll();
                return usuario;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error ao executar o método GetAll!", ex);
                throw;
            }
            
        }

        public Usuario GetById(int id)
        {
            try
            {
                var usuario = _usuarioRepository.GetById(id);
                return usuario;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error ao executar o método GetById! id: {id}", ex);
                throw;
            }
        }

        #endregion
    }
}
