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
    public class ConsultaRepository : IConsultaRepository
    {
        private readonly ILogger _logger;
        private readonly string _connectionString;
        public ConsultaRepository(IConfiguration iconfiguration, ILoggerFactory loggerFactory)
        {
            _connectionString = iconfiguration.GetConnectionString("Vocare");
            _logger = loggerFactory.CreateLogger<ConsultaRepository>();
        }
        private IDatabase Connection => new Database(_connectionString, SqlClientFactory.Instance);

        public void Insert(Consulta consulta)
        {
            try
            {
                using IDatabase Db = Connection;
                Db.Insert(consulta);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro ao executar o método Insert!", ex);
                throw;
            }
        }

        public List<Consulta> GetAll()
        {
            try
            {
                using IDatabase Db = Connection;
                return Db.Fetch<Consulta>();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro ao executar o método GetAll!", ex);
                throw;
            }
        }

        public List<ConsultaResponse> GetSolicitacoes()
        {
            try
            {
                using IDatabase Db = Connection;
                return Db.Fetch<ConsultaResponse>("select c.*, u.nome, u.email from consulta as c, usuario as u WHERE Aceita = 0 and u.id = c.idcliente");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error ao executar o método GetSolicitacoes!", ex);
                throw;
            }
        }

        public void Update(Consulta consulta)
        {
            try
            {
                using (IDatabase Db = Connection)
                {
                    Db.Update("Consulta", "Id", consulta, consulta.Id);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error ao executar o método Update! consulta : {consulta}", ex);
                throw;
            }
        }

        public List<ConsultaResponse> GetConsultasByPsicologo(int id)
        {
            try
            {
                using IDatabase Db = Connection;
                return Db.Fetch<ConsultaResponse>("select c.*, u.nome, u.email from consulta as c, usuario as u WHERE Aceita is not NULL and u.id = c.idcliente and c.idPsicologo = @id", new { id });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error ao executar o método GetConsultasByPsicologo! id: {id}", ex);
                throw;
            }
        }

        public List<Consulta> MinhasConsultas(int idCliente)
        {
            using IDatabase Db = Connection;
            return Db.Fetch<Consulta>("SELECT * FROM Consulta WHERE IdCliente = @idCliente", new { idCliente });
        }

        public List<ConsultaResponse> GetConsultasByClienteAceito(int id)
        {
            try
            {
                using IDatabase Db = Connection;
                return Db.Fetch<ConsultaResponse>("select c.*, u.nome from consulta as c, usuario as u WHERE Aceita=1 and u.id = c.idcliente and c.idCliente = @id", new { id });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error ao executar o método GetConsultasByPsicologo! id: {id}", ex);
                throw;
            }
        }

        public List<ConsultaResponse> GetConsultasByData(int id, DateTime data)
        {
            try
            {
                using IDatabase Db = Connection;
                return Db.Fetch<ConsultaResponse>("select c.*, u.nome from consulta as c, usuario as u WHERE Aceita is not NULL and u.id = c.idcliente and c.idPsicologo = @id and cast(dataConsulta as date) = cast(@data as date)", new { id, data });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error ao executar o método GetConsultasByPsicologo! id: {id}", ex);
                throw;
            }
        }
    }
}
