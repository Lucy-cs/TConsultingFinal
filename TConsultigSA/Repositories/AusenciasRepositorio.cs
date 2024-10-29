using Dapper;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Threading.Tasks;
using TConsultigSA.Models;

namespace TConsultigSA.Repositories
{
    public class AusenciaRepositorio
    {
        private readonly string _connectionString;

        public AusenciaRepositorio(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        // Obtener todas las ausencias, incluyendo el nombre del empleado
        public async Task<IEnumerable<AusenciaViewModel>> GetAll()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var query = @"
                    SELECT A.*, E.Nombre AS EmpleadoNombre
                    FROM Ausencias A
                    LEFT JOIN Empleados E ON A.IdEmpleado = E.Id";

                return await connection.QueryAsync<AusenciaViewModel>(query);
            }
        }

        // Obtener una ausencia por ID
        public async Task<Ausencia> GetById(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var query = "SELECT * FROM Ausencias WHERE Id = @Id";
                return await connection.QueryFirstOrDefaultAsync<Ausencia>(query, new { Id = id });
            }
        }

        // Insertar una nueva ausencia
        public async Task<int> Add(Ausencia ausencia)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var query = @"INSERT INTO Ausencias (IdEmpleado, FechaInicio, FechaFin, TotalDias, Autorizado, Deducible) 
                              VALUES (@IdEmpleado, @FechaInicio, @FechaFin, @TotalDias, @Autorizado, @Deducible)";
                return await connection.ExecuteAsync(query, ausencia);
            }
        }

        // Actualizar una ausencia
        public async Task<int> Update(Ausencia ausencia)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var query = @"UPDATE Ausencias SET 
                              IdEmpleado = @IdEmpleado, 
                              FechaInicio = @FechaInicio, 
                              FechaFin = @FechaFin, 
                              TotalDias = @TotalDias, 
                              Autorizado = @Autorizado, 
                              Deducible = @Deducible 
                              WHERE Id = @Id";
                return await connection.ExecuteAsync(query, ausencia);
            }
        }

        // Eliminar una ausencia
        public async Task<int> Delete(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var query = "DELETE FROM Ausencias WHERE Id = @Id";
                return await connection.ExecuteAsync(query, new { Id = id });
            }
        }
    }
}
