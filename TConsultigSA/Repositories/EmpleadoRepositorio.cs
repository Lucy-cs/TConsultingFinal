using Dapper;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Threading.Tasks;
using TConsultigSA.Models;

namespace TConsultigSA.Repositories
{
    public class EmpleadoRepositorio
    {
        private readonly string _connectionString;

        public EmpleadoRepositorio(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        // Obtener todos los empleados
        public async Task<IEnumerable<Empleado>> GetAll()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var query = "SELECT * FROM Empleados";
                return await connection.QueryAsync<Empleado>(query);
            }
        }

        // Obtener un empleado por ID
        public async Task<Empleado> GetById(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var query = "SELECT * FROM Empleados WHERE Id = @Id";
                return await connection.QueryFirstOrDefaultAsync<Empleado>(query, new { Id = id });
            }
        }

        // Insertar un nuevo empleado
        public async Task<int> Add(Empleado empleado)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var query = @"INSERT INTO Empleados (DPI, Nombre, FechaContratado, IdUsuario, IdPuesto, IdDepartamento, Email, Telefono, Direccion, Salario) 
                              VALUES (@DPI, @Nombre, @FechaContratado, @IdUsuario, @IdPuesto, @IdDepartamento, @Email, @Telefono, @Direccion, @Salario)";
                return await connection.ExecuteAsync(query, empleado);
            }
        }

        // Actualizar un empleado existente
        public async Task<int> Update(Empleado empleado)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var query = @"UPDATE Empleados SET 
                              DPI = @DPI, 
                              Nombre = @Nombre, 
                              FechaContratado = @FechaContratado, 
                              IdUsuario = @IdUsuario, 
                              IdPuesto = @IdPuesto, 
                              IdDepartamento = @IdDepartamento,
                              Email = @Email,
                              Telefono = @Telefono,
                              Direccion = @Direccion,
                              Salario = @Salario
                              WHERE Id = @Id";
                return await connection.ExecuteAsync(query, empleado);
            }
        }

        // Eliminar un empleado
        public async Task<int> Delete(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var query = "DELETE FROM Empleados WHERE Id = @Id";
                return await connection.ExecuteAsync(query, new { Id = id });
            }
        }
    }
}
