using System;
using Vocare.Util;
using System.Collections.Generic;
using PetaPoco;
using System.Linq;

namespace Vocare.Model
{
    public class Usuario
    {
        private readonly char[] SEPARADORES = { ',', '|', ';' };

        public int Id { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Login { get; set; }
        public string Senha { get; set; }
        public string Perfis { get; set; }
        public DateTime DataCadastro { get; set; }
        public DateTime DataAtualizacao { get; set; }

        [Ignore]
        public List<string> ListPerfis
        {
            get { return Perfis.ToListString(); }
            set
            {
                if (Perfis.IsNullOrEmpty())
                {
                    Perfis = string.Join(",", value.ToArray());
                }
            }
        }

    }
}
