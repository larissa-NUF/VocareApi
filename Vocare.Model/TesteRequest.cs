using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vocare.Model
{
    public class TesteRequest
    {
        public Teste Teste { get; set; }
        public IList<TesteResposta> Respostas { get; set; }
    }
}
