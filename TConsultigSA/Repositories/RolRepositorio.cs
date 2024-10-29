using Dapper;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Threading.Tasks;
using TConsultigSA.Models;

namespace TConsultigSA.Repositories
{
    public class RolRepositorio : IRolRepositorio
    {
        private readonly string _connectionString;

        public RolRepositorio(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<IEnumerable<Rol>> GetAll()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var query = "SELECT Id, Descripcion FROM Roles";
                return await connection.QueryAsync<Rol>(query);
            }
        }

        public async Task<Rol> GetById(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var query = "SELECT Id, Descripcion FROM Roles WHERE Id = @Id";
                return await connection.QueryFirstOrDefaultAsync<Rol>(query, new { Id = id });
            }
        }

        public async Task Add(Rol rol)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var query = "INSERT INTO Roles (Descripcion) VALUES (@Descripcion)";
                await connection.ExecuteAsync(query, new { Descripcion = rol.Descripcion });
            }
        }

        public async Task Update(Rol rol)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var query = "UPDATE Roles SET Descripcion = @Descripcion WHERE Id = @Id";
                await connection.ExecuteAsync(query, new { Descripcion = rol.Descripcion, Id = rol.Id });
            }
        }

        public async Task Delete(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var query = "DELETE FROM Roles WHERE Id = @Id";
                await connection.ExecuteAsync(query, new { Id = id });
            }
        }
    }
}
