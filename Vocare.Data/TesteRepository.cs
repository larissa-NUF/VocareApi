using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PetaPoco;
using System;
using System.Data.SqlClient;
using Vocare.Data.Interfaces;
using Vocare.Model;

namespace Vocare.Data
{
    public class TesteRepository : ITesteRepository
    {
        private readonly ILogger _logger;
        private readonly string _connectionString;
        public TesteRepository(IConfiguration iconfiguration, ILoggerFactory loggerFactory)
        {
            _connectionString = iconfiguration.GetConnectionString("Vocare");
            _logger = loggerFactory.CreateLogger<TesteRepository>();
        }
        private IDatabase Connection => new Database(_connectionString, SqlClientFactory.Instance);

        public int Insert(Teste teste)
        {
            try
            {
                using (IDatabase Db = Connection)
                {
                    return (int)Db.Insert(teste);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error ao executar o método Insert! Teste : {teste}", ex);
                throw;
            }
        }

        public Teste GetById(int id)
        {
            try
            {
                using IDatabase Db = Connection;
                return Db.FirstOrDefault<Teste>("WHERE id = @id", new { id });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error ao executar o método GetById! id: {id}", ex);
                throw;
            }
        }
    }
}
