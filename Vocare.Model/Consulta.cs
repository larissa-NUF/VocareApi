

using System;

namespace Vocare.Model
{
    public class Consulta
    {
        public int Id { get; set; }
        public int IdCliente { get; set; }
        public int? IdPsicologo { get; set; }
        public DateTime DataCadastro { get; set; }
        public DateTime DataConsulta { get; set; }
        public bool Aceita { get; set; }
        public bool Finalizada { get; set; }
        public string IdSala { get; set; }
    }
}
