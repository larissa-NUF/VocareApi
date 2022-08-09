using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PetaPoco;
using System;
using System.Data.SqlClient;
using Vocare.Data.Interfaces;

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
    }
}
