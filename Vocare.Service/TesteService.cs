using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vocare.Data;
using Vocare.Data.Interfaces;
using Vocare.Model;

namespace Vocare.Service
{
    public class TesteService
    {
        #region Dependências
        private readonly IConfiguration _config;
        private readonly ILogger<TesteService> _logger;
        private readonly ITesteRepository _testeRepository;
        private readonly ITesteRespostaRepository _testeRespostaRepository;
        private readonly IPerguntaTesteRepository _perguntaTesteRepository;

        public TesteService(
            IConfiguration config,
            ILoggerFactory loggerFactory,
            ITesteRepository testeRepository,
            ITesteRespostaRepository testeRespostaRepository,
            IPerguntaTesteRepository perguntaTesteRepository)
        {
            _config = config;
            _logger = loggerFactory.CreateLogger<TesteService>();
            _testeRepository= testeRepository;
            _testeRespostaRepository = testeRespostaRepository;
            _perguntaTesteRepository = perguntaTesteRepository;
        }
        #endregion

        #region Métodos públicos
        public Teste Insert(Teste teste)
        {
            try
            {
                var idTeste = _testeRepository.Insert(teste);
                var testeSalvo = _testeRepository.GetById(idTeste);
                foreach(var item in teste.Respostas)
                {
                    item.IdTeste = idTeste;
                    _testeRespostaRepository.Insert(item);
                }
                return teste;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error ao executar o método Insert! teste: {teste}", ex);
                throw;
            }
        }
        #endregion
    }
}
