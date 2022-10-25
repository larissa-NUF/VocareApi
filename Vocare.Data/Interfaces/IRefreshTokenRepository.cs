using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vocare.Model;

namespace Vocare.Data.Interfaces
{
    public interface IRefreshTokenRepository
    {
        void Insert(RefreshToken refreshToken);
        Task<RefreshToken> ObterPorToken(string token);
        Task Update(RefreshToken refreshTokenValidado);
    }
}
