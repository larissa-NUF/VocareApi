using Vocare.Model;
using System;
using System.Collections.Generic;
using System.Text;
using Vocare.Model;

namespace Vocare.Data.Interfaces
{
    public interface IUsuarioRepository
    {
        List<Usuario> GetAll();
    }
}
