using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PetaPoco;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vocare.Data.Interfaces;
using Vocare.Model;

namespace Vocare.Data
{
    public class TesteRespostaRepository : ITesteRespostaRepository
    {
        private readonly ILogger _logger;
        private readonly string _connectionString;
        public TesteRespostaRepository(IConfiguration iconfiguration, ILoggerFactory loggerFactory)
        {
            _connectionString = iconfiguration.GetConnectionString("Vocare");
            _logger = loggerFactory.CreateLogger<TesteRespostaRepository>();
        }
        private IDatabase Connection => new Database(_connectionString, SqlClientFactory.Instance);

        public void Insert(TesteResposta testeResposta)
        {
            try
            {
                using (IDatabase Db = Connection)
                {
                    Db.Insert("TesteResposta", testeResposta);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error ao executar o método Insert! TesteResposta : {testeResposta}", ex);
                throw;
            }
        }
    }
}
