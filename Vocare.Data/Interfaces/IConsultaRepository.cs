
using System.Collections.Generic;
using Vocare.Model;

namespace Vocare.Data.Interfaces
{
    public interface IConsultaRepository
    {
        void Insert(Consulta consulta);
        List<ConsultaResponse> GetSolicitacoes();
        void Update(Consulta consulta);
        List<Consulta> GetAll();
        List<ConsultaResponse> GetConsultasByPsicologo(int id);
        List<Consulta> MinhasConsultas(int idCliente);
    }
}
