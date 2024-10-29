using Dapper;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Threading.Tasks;
using TConsultigSA.Models;

namespace TConsultigSA.Repositories
{
    public class PuestoRepositorio
    {
        private readonly string _connectionString;

        public PuestoRepositorio(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        // Obtener todos los puestos
        public async Task<IEnumerable<Puesto>> GetAll()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var query = "SELECT * FROM Puestos";
                return await connection.QueryAsync<Puesto>(query);
            }
        }

        // Obtener un puesto por ID
        public async Task<Puesto> GetById(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var query = "SELECT * FROM Puestos WHERE Id = @Id";
                return await connection.QueryFirstOrDefaultAsync<Puesto>(query, new { Id = id });
            }
        }

        // Insertar un nuevo puesto
        public async Task<int> Add(Puesto puesto)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var query = "INSERT INTO Puestos (Descripcion) VALUES (@Descripcion)";
                return await connection.ExecuteAsync(query, puesto);
            }
        }

        // Actualizar un puesto existente
        public async Task<int> Update(Puesto puesto)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var query = "UPDATE Puestos SET Descripcion = @Descripcion WHERE Id = @Id";
                return await connection.ExecuteAsync(query, puesto);
            }
        }

        // Eliminar un puesto
        public async Task<int> Delete(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var query = "DELETE FROM Puestos WHERE Id = @Id";
                return await connection.ExecuteAsync(query, new { Id = id });
            }
        }
    }
}
