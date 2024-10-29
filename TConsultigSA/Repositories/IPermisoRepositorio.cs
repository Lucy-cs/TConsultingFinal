using System.Collections.Generic;
using System.Threading.Tasks;
using TConsultigSA.Models;

namespace TConsultigSA.Repositories
{
    public interface IPermisoRepositorio
    {
        Task<IEnumerable<Permiso>> GetAll();
        Task<Permiso> GetById(int id);
        Task Add(Permiso permiso);
        Task Update(Permiso permiso);
        Task Delete(int id);
    }
}
