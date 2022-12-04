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
        public Usuario Add(Usuario usuario);
        Usuario Update(Usuario usuario);
        void Delete(int id);
        public Usuario GetById (int id);
        List<Usuario> GetByPerfil(string perfil);


        Task<List<Perfil>> GetTypesById(int[] idsTipo);

    }
}
