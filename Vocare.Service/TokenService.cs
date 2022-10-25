using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Threading.Tasks;
using Vocare.Model;
using Vocare.Service.Intefaces;
using Microsoft.Extensions.Options;
using Vocare.Data.Interfaces;

namespace Vocare.Service
{
    public class TokenService: ITokenService
    {
        #region Dependências
        private readonly IConfiguration _config;
        private readonly ILogger<TokenService> _logger;
        private readonly JwtSettings _jwtSettings;
        private readonly TokenValidationParameters _tokenValidationParameters;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly IUsuarioService _usuarioService;
        private readonly IUsuarioRepository _usuarioRepository;

        public TokenService(
            IConfiguration config,
            ILoggerFactory loggerFactory,
            IOptions<JwtSettings> jwtSettings,
            IRefreshTokenRepository refreshTokenRepository,
            IUsuarioService usuarioService,
            IUsuarioRepository usuarioRepository)
        {
            _config = config;
            _logger = loggerFactory.CreateLogger<TokenService>();
            _jwtSettings = jwtSettings.Value;
            _refreshTokenRepository = refreshTokenRepository;
            _usuarioService = usuarioService;
            _usuarioRepository = usuarioRepository;

            var key = Encoding.ASCII.GetBytes(_jwtSettings.Secret);
            _tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = false
            };
        }
        #endregion
        public async Task<Token> GerarTokenAsync(Usuario usuario)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var signinCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_jwtSettings.Secret)), SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
                {
                new Claim("Id", usuario.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, usuario.Login),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                };

            int[] idsTipo = usuario.ListPerfis.Select(x => int.Parse(x)).ToArray();
            List<Perfil> roles = await _usuarioService.GetTypesById(idsTipo);
            claims.AddRange(roles.Select(perfil => new Claim(ClaimTypes.Role, perfil.Nome)));

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.Add(_jwtSettings.ExpiryTimeFrame),
                SigningCredentials = signinCredentials
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var jwtToken = tokenHandler.WriteToken(token);
            var refreshToken = new RefreshToken
            {
                Token = $"{RandomString(25)}_{Guid.NewGuid()}",
                DataExpiracao = DateTime.UtcNow.AddMonths(6),
                DataCadastro = DateTime.Now,
                DataAtualizacao = null,
                IdUsuario = usuario.Id,
                IsRevoked = false,
                AccessToken = token.Id,
               
            };

            _refreshTokenRepository.Insert(refreshToken);

            var tokenData = new Token
            {
                AccessToken = jwtToken,
                RefreshToken = refreshToken.Token
            };

            return tokenData;
        }

        public async Task<Usuario> ObterCredenciaisPorToken(Token request)
        {
            var response = new UserCredentialsResponse();

            var claims = ValidarJWT(request.AccessToken);
            bool isExpirado = IsJWTExpired(claims);

            RefreshToken refreshTokenValidado = await ValidarRefreshToken(request.RefreshToken, claims);
            var usuario = _usuarioService.GetById(refreshTokenValidado.IdUsuario);
            if (usuario == null) throw new Exception();

            response.Nome = usuario.Nome;
            response.Email = usuario.Email;

            if (isExpirado)
            {
                await _refreshTokenRepository.Update(refreshTokenValidado);
                var token = await GerarTokenAsync(usuario);

                response.RefreshToken = token.RefreshToken;
                response.Token = token.AccessToken;
            }
            else
            {
                response.RefreshToken = request.RefreshToken;
                response.Token = request.AccessToken;
            }

            return usuario;
        }

        public async Task<Token> Login(UsuarioLogin request)
        {
            try
            {
                var usuario = _usuarioRepository.GetByLogin(request.Login);
                if (usuario is null || usuario.Login == request.Login || usuario.Senha == request.Senha)
                    return await GerarTokenAsync(usuario);
                else
                    throw new UnauthorizedAccessException("Senha e/ou usuário incorretos!");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error ao executar o método GetByLogin! Login: {request.Login}", ex);
                throw;
            }
        }

        #region Métodos Privados

        private static DateTime UnixTimestampToDatetime(long unixDate)
        {
            var dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dateTime = dateTime.AddSeconds(unixDate).ToUniversalTime();
            return dateTime;
        }

        private bool IsJWTExpired(ClaimsPrincipal principal)
        {
            try
            {
                var utcExpiryDate = long.Parse(principal.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Exp).Value);
                DateTime expiryDate = UnixTimestampToDatetime(utcExpiryDate);

                return expiryDate < DateTime.UtcNow;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro ao executar o método IsJWTExpired! principal:{principal}", ex);
                throw;
            }
        }

        private async Task<RefreshToken> ValidarRefreshToken(string token, ClaimsPrincipal principal)
        {
            try
            {
                var refreshTokenExists = await _refreshTokenRepository.ObterPorToken(token);
                if (refreshTokenExists == null)
                    throw new ApplicationException("RefreshToken inválido!");

                if (refreshTokenExists.DataExpiracao < DateTime.UtcNow)
                    throw new ApplicationException("RefreshToken expirado!");

                if (refreshTokenExists.IsRevoked)
                    throw new ApplicationException("RefreshToken já foi revogado!");

                var jti = principal.Claims.SingleOrDefault(x => x.Type == JwtRegisteredClaimNames.Jti).Value;
                if (refreshTokenExists.AccessToken != jti)
                    throw new ApplicationException("RefreshToken inválido!");

                return refreshTokenExists;
            }
            catch (ApplicationException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro ao executar o método ValidarRefreshToken! token:{token}", ex);
                throw;
            }
        }
        private static string RandomString(int length)
        {
            var random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        private ClaimsPrincipal ValidarJWT(string jwt)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var principal = tokenHandler.ValidateToken(jwt, _tokenValidationParameters, out var validatedToken);

                if (validatedToken is JwtSecurityToken jwtSecurityToken)
                {
                    var result = jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase);
                    if (!result)
                        throw new ApplicationException("Token inválido!");
                }

                return principal;
            }
            catch (ApplicationException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro ao executar o método ValidatJWT! jwt:{jwt}", ex);
                throw;
            }
        }
        #endregion
    }
}
