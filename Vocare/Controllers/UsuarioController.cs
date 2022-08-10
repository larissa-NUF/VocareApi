using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Vocare.Model;
using System.Collections.Generic;
using Vocare.Model;
using Vocare.Service.Intefaces;
using Microsoft.AspNetCore.Http;

namespace Vocare.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsuarioController : ControllerBase
    {
        #region Dependências
        private readonly IConfiguration _config;
        private readonly ILogger<UsuarioController> _logger;
        private readonly IUsuarioService _usuarioService;
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
        public IActionResult Index()
        {
            List<Usuario> usuario = _usuarioService.GetAll();
            return Ok(usuario);
        }

    }
}
