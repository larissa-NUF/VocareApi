using System.Collections.Generic;

namespace Vocare.Model
{
    public class UserCredentialsResponse
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public IEnumerable<Perfil> Perfis { get; set; }
   

    }
}
