using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using Vocare.Data.Interfaces;
using Vocare.Model;

namespace Vocare.Controllers
{
    [ApiController]
    [Route("/teste")]
    public class TesteController : Controller
    {
        #region Dependências
        private readonly ILogger<TesteController> _logger;
        private readonly ITesteRepository _testeRepository;

        public TesteController(
            ILoggerFactory loggerFactory,
            ITesteRepository testeRepository)
        {
            _logger = loggerFactory.CreateLogger<TesteController>();
            _testeRepository = testeRepository;
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
                var testeSalvo = _testeRepository.Insert(teste);
                return Ok(testeSalvo);
            }
            catch (UnauthorizedAccessException ex)
            {
                return BadRequest(new { Message = ex.Message});
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error ao executar o método Insert! teste : {teste}", ex);
                throw;
            }

        }
      
    }
}
