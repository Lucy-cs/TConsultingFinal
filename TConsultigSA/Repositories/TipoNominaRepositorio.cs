using Dapper;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Threading.Tasks;
using TConsultigSA.Models;

namespace TConsultigSA.Repositories
{
    public class TipoNominaRepositorio
    {
        private readonly string _connectionString;

        public TipoNominaRepositorio(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        // Obtener todos los tipos de nómina
        public async Task<IEnumerable<TipoNomina>> GetAll()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var query = "SELECT * FROM TiposNomina";
                return await connection.QueryAsync<TipoNomina>(query);
            }
        }

        // Obtener un tipo de nómina por ID
        public async Task<TipoNomina> GetById(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var query = "SELECT * FROM TiposNomina WHERE Id = @Id";
                return await connection.QueryFirstOrDefaultAsync<TipoNomina>(query, new { Id = id });
            }
        }

        // Insertar un nuevo tipo de nómina
        public async Task<int> Add(TipoNomina tipoNomina)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var query = "INSERT INTO TiposNomina (Descripcion) VALUES (@Descripcion)";
                return await connection.ExecuteAsync(query, tipoNomina);
            }
        }

        // Actualizar un tipo de nómina existente
        public async Task<int> Update(TipoNomina tipoNomina)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var query = "UPDATE TiposNomina SET Descripcion = @Descripcion WHERE Id = @Id";
                return await connection.ExecuteAsync(query, tipoNomina);
            }
        }

        // Eliminar un tipo de nómina
        public async Task<int> Delete(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var query = "DELETE FROM TiposNomina WHERE Id = @Id";
                return await connection.ExecuteAsync(query, new { Id = id });
            }
        }
    }
}
