using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vocare.Model;

namespace Vocare.Service.Intefaces
{
    public interface IUsuarioService
    {
        public List<Usuario> GetAll();
    }
}
