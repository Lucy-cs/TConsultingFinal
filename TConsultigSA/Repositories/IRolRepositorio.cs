using System.Collections.Generic;
using System.Threading.Tasks;
using TConsultigSA.Models;

namespace TConsultigSA.Repositories
{
    public interface IRolRepositorio
    {
        Task<IEnumerable<Rol>> GetAll();
        Task<Rol> GetById(int id);
        Task Add(Rol rol);
        Task Update(Rol rol);
        Task Delete(int id);
    }
}
