using System.Collections.Generic;
using Vocare.Model;

namespace Vocare.Service.Intefaces
{
    public interface IConsultaService
    {
        Consulta Insert(Consulta consulta);
        List<ConsultaResponse> GetSolicitacoes();
        Consulta Update(Consulta consulta);
        List<Consulta> GetAll();
        List<ConsultaResponse> GetConsultasByPsicologo(int id);
        List<Consulta> MinhasConsultas(int idUsuario);
    }
}
