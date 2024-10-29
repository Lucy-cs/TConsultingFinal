using Dapper;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Threading.Tasks;
using TConsultigSA.Models;

namespace TConsultigSA.Repositories
{
    public class PermisoRepositorio : IPermisoRepositorio
    {
        private readonly string _connectionString;

        public PermisoRepositorio(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<IEnumerable<Permiso>> GetAll()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var query = "SELECT * FROM Permisos";
                return await connection.QueryAsync<Permiso>(query);
            }
        }

        public async Task<Permiso> GetById(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var query = "SELECT * FROM Permisos WHERE Id = @Id";
                return await connection.QueryFirstOrDefaultAsync<Permiso>(query, new { Id = id });
            }
        }

        public async Task Add(Permiso permiso)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var query = "INSERT INTO Permisos (Nombre, Descripcion) VALUES (@Nombre, @Descripcion)";
                await connection.ExecuteAsync(query, permiso);
            }
        }

        public async Task Update(Permiso permiso)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var query = "UPDATE Permisos SET Nombre = @Nombre, Descripcion = @Descripcion WHERE Id = @Id";
                await connection.ExecuteAsync(query, permiso);
            }
        }

        public async Task Delete(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var query = "DELETE FROM Permisos WHERE Id = @Id";
                await connection.ExecuteAsync(query, new { Id = id });
            }
        }
    }
}
