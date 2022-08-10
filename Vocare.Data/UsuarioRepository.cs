using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Vocare.Model;
using PetaPoco;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Vocare.Data.Interfaces;
using Vocare.Model;

namespace Data
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly ILogger _logger;
        private readonly string _connectionString;
        public UsuarioRepository(IConfiguration iconfiguration, ILoggerFactory loggerFactory)
        {
            _connectionString = iconfiguration.GetConnectionString("Vocare");
            _logger = loggerFactory.CreateLogger<UsuarioRepository>();
        }
        public IDatabase Connection
        {
            get
            {
                return new Database(_connectionString, SqlClientFactory.Instance);
            }
        }

        public List<Usuario> GetAll()
        {
            try
            {
                using IDatabase Db = Connection;
                return Db.Fetch<Usuario>();
            }
            catch (SqlException ex)
            {
                _logger.LogError($"Erro ao executar o método GetAll!", ex);
                throw;
            }
        }
    }
}
