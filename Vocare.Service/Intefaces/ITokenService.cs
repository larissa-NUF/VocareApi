using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vocare.Model;

namespace Vocare.Service.Intefaces
{
    public interface ITokenService
    {
        Task<Token> GerarTokenAsync(Usuario usuario);
        Task<Token> Login(UsuarioLogin request);
        Task<Usuario> ObterCredenciaisPorToken(Token request);
    }
}
