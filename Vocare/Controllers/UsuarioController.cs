using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Vocare.Model;
using System.Collections.Generic;
using Vocare.Service.Intefaces;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using System;

namespace Vocare.Controllers
{
    /// <summary>
    /// Usuario Controller
    /// </summary>
    [ApiController]
    [Route("/usuario")]
    public class UsuarioController : ControllerBase
    {
        
        #region Dependências
        private readonly IConfiguration _config;
        private readonly ILogger<UsuarioController> _logger;
        private readonly IUsuarioService _usuarioService;

        /// <summary>
        /// Construtor
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="artigoService"></param>
        /// <param name="analiseLegibilidadeService"></param>
        public UsuarioController(
            IConfiguration config,
            ILoggerFactory loggerFactory,
            IUsuarioService usuarioService)
        {
            _config = config;
            _logger = loggerFactory.CreateLogger<UsuarioController>();
            _usuarioService = usuarioService;
        }
        #endregion

        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<Usuario>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet]
        public IActionResult GetAll()
        {
            try
            {
                List<Usuario> usuario = _usuarioService.GetAll();
                return Ok(usuario);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error ao executar o método GetAll!", ex);
                throw;
            }
            
        }

        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<Usuario>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost]
        public IActionResult Add([FromBody] Usuario request)
        {
            try
            {
                var usuario = _usuarioService.Add(request);
                return CreatedAtAction(nameof(GetAll), new { usuario.Id }, usuario);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error ao executar o método Add! usuário : {request}", ex);
                throw;
            }
            
        }

        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<Usuario>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPut]
        public IActionResult Update([FromBody] Usuario request)
        {
            try
            {
                var usuario = _usuarioService.Update(request);
                return CreatedAtAction(nameof(GetAll), new { usuario.Id }, usuario);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error ao executar o método Update! Usuario : {request}", ex);
                throw;
            }

        }

        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<Usuario>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpDelete("{id}")]
        public IActionResult Delete([FromRoute] int id)
        {
            try
            {
                _usuarioService.Delete(id);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error ao executar o método Delete! Id : {id}", ex);
                throw;
            }

        }

        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<Usuario>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("{id}")]
        public IActionResult GetById( int id)
        {
            try
            {
                var usuario = _usuarioService.GetById(id);
                return CreatedAtAction(nameof(GetAll), new { usuario.Id }, usuario);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error ao executar o método Add! getById : {id}", ex);
                throw;
            }
            
        }

        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<Usuario>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("/usuario/perfil/{perfil}")]
        public IActionResult GetByPerfil([FromRoute] string perfil)
        {
            try
            {
                var usuario = _usuarioService.GetByPerfil(perfil);
                return Ok(usuario);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error ao executar o método GetByPerfil! perfil : {perfil}", ex);
                throw;
            }

        }

    }
}
