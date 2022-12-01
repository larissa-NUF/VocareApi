using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PetaPoco;
using System;
using System.Data.SqlClient;
using Vocare.Data.Interfaces;
using Vocare.Model;
using static System.Net.Mime.MediaTypeNames;

namespace Vocare.Data
{
    public class PerguntaTesteRepository : IPerguntaTesteRepository
    {
        private readonly ILogger _logger;
        private readonly string _connectionString;
        public PerguntaTesteRepository(IConfiguration iconfiguration, ILoggerFactory loggerFactory)
        {
            _connectionString = iconfiguration.GetConnectionString("Vocare");
            _logger = loggerFactory.CreateLogger<PerguntaTesteRepository>();
        }
		private IDatabase Connection => new Database(_connectionString, SqlClientFactory.Instance);

		public void Insert(PerguntaTeste perguntaTeste)
		{
			try
			{
				using (IDatabase Db = Connection)
				{
					Db.Insert("PerguntaTeste", "Id", true, perguntaTeste);
				}
			}
			catch (Exception ex)
			{
				_logger.LogError($"Error ao executar o método Insert! PerguntaTeste : {perguntaTeste}", ex);
				throw;
			}
		}
	}
}
