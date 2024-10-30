using TConsultigSA.Models;

namespace TConsultingSA.Repositories
{
    public interface IHorasTrabajoRepositorio
    {
        Task<IEnumerable<HorasTrabajo>> GetAll();
        Task<int> Add(HorasTrabajo horasTrabajo);
        Task<HorasTrabajo> GetById(int id);
        Task<int> Delete(int id);
        Task<int> Update(HorasTrabajo horasTrabajo);
        // Puedes agregar más métodos según sea necesario, como Update, Delete, etc.
        Task<IEnumerable<HorasTrabajo>> ObtenerHorasPorEmpleadoYMes(int idEmpleado, int mes, int año);
        // Puedes agregar más métodos según sea necesario, como Update, Delete, etc.

    }
}
