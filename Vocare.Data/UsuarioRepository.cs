using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Vocare.Model;
using PetaPoco;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Vocare.Data.Interfaces;
using System.Threading.Tasks;

namespace Vocare.Data
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
        private IDatabase Connection => new Database(_connectionString, SqlClientFactory.Instance);

        public List<Usuario> GetAll()
        {
            try
            {
                using IDatabase Db = Connection;
                return Db.Fetch<Usuario>();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro ao executar o método GetAll!", ex);
                throw;
            }
        }

        public Usuario GetById(int id)
        {
            try
            {
                using IDatabase Db = Connection;
                return Db.FirstOrDefault<Usuario>("WHERE id = @id", new { id });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error ao executar o método GetById! id: {id}", ex);
                throw;
            }
        }

        public List<Usuario> GetByPeril(string perfil)
        {
            try
            {
                using IDatabase Db = Connection;
                return Db.Fetch<Usuario>("WHERE Perfis = @perfil", new { perfil });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error ao executar o método GetByPeril! id: {perfil}", ex);
                throw;
            }
        }

        public Usuario GetByLogin(string login)
        {
            try
            {
                using IDatabase Db = Connection;
                return Db.FirstOrDefault<Usuario>("WHERE login = @login", new { login });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error ao executar o método GetByIdLogin! login: {login}", ex);
                throw;
            }
        }

        public void Add(Usuario usuario)
        {
            try
            {
                usuario.DataCadastro = DateTime.Now;
                usuario.DataAtualizacao = DateTime.Now;

                using (IDatabase Db = Connection)
                {
                    Db.Insert(usuario);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error ao executar o método Add! empresa : {usuario}", ex);
                throw;
            }
        }

        public void Update(Usuario usuario)
        {
            try
            {
                usuario.DataAtualizacao = DateTime.Now;

                using (IDatabase Db = Connection)
                {
                    Db.Update("Usuario", "Id", usuario, usuario.Id);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error ao executar o método Update! Usuario : {usuario}", ex);
                throw;
            }
        }

        public void Delete(int id)
        {
            try
            {
                using (IDatabase Db = Connection)
                {
                    Db.Delete("Usuario", "Id", null, id);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error ao executar o método Delete! Id : {id}", ex);
                throw;
            }
        }

        public async Task<List<Perfil>> GetTypesById(int[] idsTipo)
        {
            try
            {
                using (IDatabase Db = Connection)
                {
                    return await Db.FetchAsync<Perfil>("WHERE Id in (@arraysId)", new { arraysId = idsTipo });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error ao tentar GetTypesById {idsTipo}", ex);
                throw;
            }
        }
    }
}
