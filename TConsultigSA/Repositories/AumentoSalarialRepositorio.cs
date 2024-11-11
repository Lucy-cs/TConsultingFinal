using Dapper;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Threading.Tasks;
using TConsultigSA.Models;
using Microsoft.Extensions.Configuration;

namespace TConsultigSA.Repositories
{
    public class AumentoSalarialRepositorio
    {
        private readonly string _connectionString;

        public AumentoSalarialRepositorio(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        // Obtener el historial de aumentos salariales para un empleado específico
        public async Task<IEnumerable<AumentoSalarial>> ObtenerAumentosPorEmpleado(int empleadoId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var query = @"SELECT Id, EmpleadoId, FechaAumento, CantidadAumento, SalarioFinal
                              FROM AumentosSalariales
                              WHERE EmpleadoId = @EmpleadoId
                              ORDER BY FechaAumento DESC";

                return await connection.QueryAsync<AumentoSalarial>(query, new { EmpleadoId = empleadoId });
            }
        }

        // Insertar un nuevo aumento salarial
        public async Task<int> AgregarAumento(AumentoSalarial aumento)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var query = @"INSERT INTO AumentosSalariales (EmpleadoId, FechaAumento, CantidadAumento, SalarioFinal)
                              VALUES (@EmpleadoId, @FechaAumento, @CantidadAumento, @SalarioFinal)";

                return await connection.ExecuteAsync(query, aumento);
            }
        }
    }
}
