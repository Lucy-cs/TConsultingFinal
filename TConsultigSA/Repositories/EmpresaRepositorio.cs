using Dapper;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Threading.Tasks;
using TConsultigSA.Models;

namespace TConsultigSA.Repositories
{
    public class EmpresaRepositorio : IEmpresaRepositorio
    {
        private readonly string _connectionString;

        public EmpresaRepositorio(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<IEnumerable<Empresa>> GetAllEmpresasAsync()
        {
            using var connection = new SqlConnection(_connectionString);
            var query = "SELECT * FROM Empresas";
            return await connection.QueryAsync<Empresa>(query);
        }

        public async Task<Empresa> GetEmpresaByIdAsync(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            var query = "SELECT * FROM Empresas WHERE Id = @Id";
            return await connection.QueryFirstOrDefaultAsync<Empresa>(query, new { Id = id });
        }

        public async Task<int> AddEmpresaAsync(Empresa empresa)
        {
            using var connection = new SqlConnection(_connectionString);
            var query = @"INSERT INTO Empresas (Nombre, Direccion, Telefono, Email)
                          VALUES (@Nombre, @Direccion, @Telefono, @Email)";
            return await connection.ExecuteAsync(query, empresa);
        }

        public async Task<int> UpdateEmpresaAsync(Empresa empresa)
        {
            using var connection = new SqlConnection(_connectionString);
            var query = @"UPDATE Empresas
                          SET Nombre = @Nombre, Direccion = @Direccion, Telefono = @Telefono, Email = @Email
                          WHERE Id = @Id";
            return await connection.ExecuteAsync(query, empresa);
        }

        public async Task<int> DeleteEmpresaAsync(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            var query = "DELETE FROM Empresas WHERE Id = @Id";
            return await connection.ExecuteAsync(query, new { Id = id });
        }
    }
}
