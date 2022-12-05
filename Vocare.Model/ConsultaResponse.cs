

using System;

namespace Vocare.Model
{
    public class ConsultaResponse
    {
        public int Id { get; set; }
        public int IdCliente { get; set; }
        public int IdPsicologo { get; set; }
        public DateTime DataCadastro { get; set; }
        public DateTime DataConsulta { get; set; }
        public bool Aceita { get; set; }
        public bool Finalizada { get; set; }
        public string IdSala { get; set; }
        public string? Nome { get; set; }
        public string Email { get; set; }
    }
}
