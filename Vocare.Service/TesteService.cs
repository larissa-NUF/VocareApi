using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using Vocare.Data.Interfaces;
using Vocare.Model;
using Vocare.Service.Intefaces;

namespace Vocare.Service
{
    public class TesteService : ITesteService
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
            _testeRepository = testeRepository;
            _testeRespostaRepository = testeRespostaRepository;
            _perguntaTesteRepository = perguntaTesteRepository;
        }
        #endregion

        #region Métodos públicos
        public TesteRequest Insert(TesteRequest testeRequest)
        {
            try
            {
                testeRequest.Teste.DataCadastro = DateTime.Now;
                testeRequest.Teste.IdPsicologo = null;
                var idTeste = _testeRepository.Insert(testeRequest.Teste);
                var testeSalvo = _testeRepository.GetById(idTeste);
                foreach (var item in testeRequest.Respostas)
                {
                    item.IdTeste = idTeste;
                    _testeRespostaRepository.Insert(item);
                }
                return testeRequest;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error ao executar o método Insert! teste: {testeRequest}", ex);
                throw;
            }
        }
        #endregion
    }
}
