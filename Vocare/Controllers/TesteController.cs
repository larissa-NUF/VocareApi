using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using Vocare.Model;
using Vocare.Service.Intefaces;

namespace Vocare.Controllers
{
    [ApiController]
    [Route("/teste")]
    public class TesteController : Controller
    {
        #region Dependências
        private readonly ILogger<TesteController> _logger;
        private readonly ITesteService _testeService;

        public TesteController(
            ILoggerFactory loggerFactory,
            ITesteService testeService)
        {
            _logger = loggerFactory.CreateLogger<TesteController>();
            _testeService = testeService;
        }
        #endregion
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost]
        [AllowAnonymous]
        public IActionResult Insert([FromBody] Teste teste)
        {
            try
            {
                var testeSalvo = _testeService.Insert(teste);
                return Ok(testeSalvo);
            }
            catch (UnauthorizedAccessException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error ao executar o método Insert! teste : {teste}", ex);
                throw;
            }

        }

    }
}
