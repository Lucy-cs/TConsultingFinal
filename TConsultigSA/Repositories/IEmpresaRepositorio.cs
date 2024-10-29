using System.Collections.Generic;
using System.Threading.Tasks;
using TConsultigSA.Models;

namespace TConsultigSA.Repositories
{
    public interface IEmpresaRepositorio
    {
        Task<IEnumerable<Empresa>> GetAllEmpresasAsync();
        Task<Empresa> GetEmpresaByIdAsync(int id);
        Task<int> AddEmpresaAsync(Empresa empresa);
        Task<int> UpdateEmpresaAsync(Empresa empresa);
        Task<int> DeleteEmpresaAsync(int id);
    }
}
