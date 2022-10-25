using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PetaPoco;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Vocare.Data.Interfaces;
using Vocare.Model;

namespace Vocare.Data
{
    public class RefreshTokenRepository : IRefreshTokenRepository
    {
        private readonly ILogger _logger;
        private readonly string _connectionString;
        public RefreshTokenRepository(IConfiguration iconfiguration, ILoggerFactory loggerFactory)
        {
            _connectionString = iconfiguration.GetConnectionString("Vocare");
            _logger = loggerFactory.CreateLogger<UsuarioRepository>();
        }
        private IDatabase Connection => new Database(_connectionString, SqlClientFactory.Instance);

        public async Task<RefreshToken> ObterPorToken(string token)
        {
            try
            {
                using IDatabase Db = Connection;
                var result = await Db.FirstOrDefaultAsync<RefreshToken>("WHERE Token = @token", new { token });
                return result;
            }
            catch (SqlException ex)
            {
                _logger.LogError($"Error ao executar o método GetById! id: {token}", ex);
                throw;
            }
        }

        public void Insert(RefreshToken refreshToken)
        {
            try
            {

                using (IDatabase Db = Connection)
                {
                    Db.Insert(refreshToken);
                }
            }
            catch (SqlException ex)
            {
                _logger.LogError($"Error ao executar o método Insert! empresa : {refreshToken}", ex);
                throw;
            }
        }

        public async Task Update(RefreshToken refreshTokenValidado)
        {
            try
            {
                using IDatabase Db = Connection;

                await Db.UpdateAsync(refreshTokenValidado);
            }
            catch (SqlException ex)
            {
                _logger.LogError($"Error ao executar o método Insert! Token : {refreshTokenValidado.AccessToken}", ex);
                throw;
            }
        }
    }
}
