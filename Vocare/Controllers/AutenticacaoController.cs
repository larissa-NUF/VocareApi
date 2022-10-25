using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Vocare.Model;
using Vocare.Service.Intefaces;

namespace Vocare.Controllers
{
    [ApiController]
    [Route("/autenticacao")]
    public class AutenticacaoController : Controller
    {
        #region Dependências
        private readonly IConfiguration _config;
        private readonly ILogger<AutenticacaoController> _logger;
        private readonly IUsuarioService _usuarioService;
        private readonly ITokenService _tokenService;

        /// <summary>
        /// Construtor
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="artigoService"></param>
        /// <param name="analiseLegibilidadeService"></param>
        public AutenticacaoController(
            IConfiguration config,
            ILoggerFactory loggerFactory,
            IUsuarioService usuarioService,
            ITokenService tokenService)
        {
            _config = config;
            _logger = loggerFactory.CreateLogger<AutenticacaoController>();
            _usuarioService = usuarioService;
            _tokenService = tokenService;
        }
        #endregion
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<Usuario>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] UsuarioLogin request)
        {
            try
            {
                var token = await _tokenService.Login(request);
                return Ok(token);
            }
            catch (UnauthorizedAccessException ex)
            {
                return BadRequest(new { Message = ex.Message});
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error ao executar o método Login! usuário : {request}", ex);
                throw;
            }

        }
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<Usuario>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost("credenciais")]
        [AllowAnonymous]
        public async Task<IActionResult> ObterCredenciaisPorToken([FromBody] Token request)
        {
            try
            {
                var token = await _tokenService.ObterCredenciaisPorToken(request);
                return Ok(token);
            }
            catch (UnauthorizedAccessException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error ao executar o método ObterCredenciaisPorToken! token : {request}", ex);
                throw;
            }

        }
    }
}
