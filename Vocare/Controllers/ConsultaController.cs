using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using Vocare.Data.Interfaces;
using Vocare.Model;
using Vocare.Service;
using Vocare.Service.Intefaces;

namespace Vocare.Controllers
{
    [ApiController]
    [Route("/consulta")]
    public class ConsultaController : Controller
    {
        #region Dependências
        private readonly ILogger<ConsultaController> _logger;
        private readonly IConsultaService _consultaService;

        public ConsultaController(
            ILoggerFactory loggerFactory,
            ITesteRepository testeRepository,
            IConsultaService consultaService)
        {
            _logger = loggerFactory.CreateLogger<ConsultaController>();
            _consultaService = consultaService;
        }

        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<Usuario>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet]
        public IActionResult GetAll()
        {
            try
            {
                List<Consulta> consulta = _consultaService.GetAll();
                return Ok(consulta);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error ao executar o método GetAll!", ex);
                throw;
            }

        }

        #endregion
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("/consulta/solicitacoes")]
        [AllowAnonymous]
        public IActionResult GetSolicitacoes()
        {
            try
            {
                var consultas = _consultaService.GetSolicitacoes();
                return Ok(consultas);
            }
            catch (UnauthorizedAccessException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error ao executar o método GetSolicitacoes!", ex);
                throw;
            }

        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("/consulta/{id}")]
        [AllowAnonymous]
        public IActionResult GetConsultasByPsicologo([FromRoute] int id)
        {
            try
            {
                var consultas = _consultaService.GetConsultasByPsicologo(id);
                return Ok(consultas);
            }
            catch (UnauthorizedAccessException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error ao executar o método GetConsultasByPsicologo!", ex);
                throw;
            }

        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("/consulta/cliente/aceito/{id}")]
        [AllowAnonymous]
        public IActionResult GetConsultasByClienteAceito([FromRoute] int id)
        {
            try
            {
                var consultas = _consultaService.GetConsultasByClienteAceito(id);
                return Ok(consultas);
            }
            catch (UnauthorizedAccessException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error ao executar o método GetConsultasByClienteAceito!", ex);
                throw;
            }

        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost("/consulta/data")]
        [AllowAnonymous]
        public IActionResult GetConsultasByData([FromBody] ConsultaRequest request)
        {
            try
            {
                var consultas = _consultaService.GetConsultasByData(request.Id, request.DataConsulta);
                return Ok(consultas);
            }
            catch (UnauthorizedAccessException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error ao executar o método GetConsultasByPsicologo!", ex);
                throw;
            }

        }

        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<Usuario>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPut]
        public IActionResult Update([FromBody] Consulta request)
        {
            try
            {
                var consulta = _consultaService.Update(request);
                return CreatedAtAction(nameof(GetAll), new { consulta.Id }, consulta);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error ao executar o método Update! Usuario : {request}", ex);
                throw;
            }

        }
    }
}
