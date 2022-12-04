using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
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
        #endregion

        #region Métodos públicos
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

        public Usuario Update(Usuario usuario)
        {
            try
            {
                _usuarioRepository.Update(usuario);
                return usuario;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error ao executar o método Update! usuário: {usuario}", ex);
                throw;
            }
        }

        public void Delete(int id)
        {
            try
            {
                _usuarioRepository.Delete(id);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error ao executar o método Delete! Id: {id}", ex);
                throw;
            }
        }

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

        public List<Usuario> GetByPerfil(string perfil)
        {
            try
            {
                var usuario = _usuarioRepository.GetByPeril(perfil);
                return usuario;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error ao executar o método GetByPerfil!", ex);
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
        

        public async Task<List<Perfil>> GetTypesById(int[] idsTipo)
        {
            try
            {
                return await _usuarioRepository.GetTypesById(idsTipo);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error em deletar dados do usuario!", ex);
                throw;
            }
        }

        #endregion
    }
}
