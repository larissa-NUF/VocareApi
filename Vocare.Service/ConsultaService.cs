﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vocare.Data;
using Vocare.Data.Interfaces;
using Vocare.Model;
using Vocare.Service.Intefaces;

namespace Vocare.Service
{
    public class ConsultaService : IConsultaService
    {
        #region Dependências
        private readonly IConfiguration _config;
        private readonly ILogger<ConsultaService> _logger;
        private readonly IConsultaRepository _consultaRepository;


        public ConsultaService(
            IConfiguration config,
            ILoggerFactory loggerFactory,
            IConsultaRepository consultaRepository
            )
        {
            _config = config;
            _logger = loggerFactory.CreateLogger<ConsultaService>();
            _consultaRepository= consultaRepository;
        }
        #endregion

        #region Métodos públicos

        public Consulta Insert(Consulta consulta)
        {
            try
            {
                consulta.DataCadastro = DateTime.Now;
                consulta.IdPsicologo = null;
                _consultaRepository.Insert(consulta);
                return consulta;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error ao executar o método GetAll!", ex);
                throw;
            }
        }

        public List<Consulta> GetAll()
        {
            try
            {
                var consultas = _consultaRepository.GetAll();
                return consultas;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error ao executar o método GetAll!", ex);
                throw;
            }

        }

        public List<ConsultaResponse> GetSolicitacoes()
        {
            try
            {
                var consultas = _consultaRepository.GetSolicitacoes();

                return consultas;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error ao executar o método GetSolicitacoes!", ex);
                throw;
            }
        }

        public List<ConsultaResponse> GetConsultasByPsicologo(int id)
        {
            try
            {
                var consultas = _consultaRepository.GetConsultasByPsicologo(id);

                return consultas;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error ao executar o método GetConsultasByPsicologo!", ex);
                throw;
            }
        }

        public List<ConsultaResponse> GetConsultasByClienteAceito(int id)
        {
            try
            {
                var consultas = _consultaRepository.GetConsultasByClienteAceito(id);

                return consultas;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error ao executar o método GetConsultasByClienteAceito!", ex);
                throw;
            }
        }

        public List<ConsultaResponse> GetConsultasByData(int id, DateTime data)
        {
            try
            {
                var consultas = _consultaRepository.GetConsultasByData(id, data);

                return consultas;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error ao executar o método GetConsultasByData!", ex);
                throw;
            }
        }

        public Consulta Update(Consulta consulta)
        {
            try
            {
                _consultaRepository.Update(consulta);
                return consulta;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error ao executar o método Update! consulta: {consulta}", ex);
                throw;
            }
        }

        public List<Consulta> MinhasConsultas(int idUsuario)
        {
            return _consultaRepository.MinhasConsultas(idUsuario);
        }
        #endregion
    }
}
