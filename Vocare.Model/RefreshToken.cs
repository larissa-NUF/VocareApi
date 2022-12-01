using System;
namespace Vocare.Model
{
    public class RefreshToken
    {
        public int Id { get; set; }
        public int IdUsuario { get; set; }
        public string Token { get; set; }
        public string AccessToken { get; set; }
        public DateTime DataCadastro { get; set; }
        public DateTime? DataAtualizacao { get; set; }
        public DateTime DataExpiracao { get; set; }
        public bool IsRevoked { get; set; }
    }
}
