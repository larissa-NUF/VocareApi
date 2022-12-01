using Vocare.Model;

namespace Vocare.Data.Interfaces
{
    public interface ITesteRepository
    {
        int Insert(Teste teste);
        Teste GetById(int id);
    }
}
