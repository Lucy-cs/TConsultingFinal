using Dapper;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Threading.Tasks;
using TConsultigSA.Models;

namespace TConsultigSA.Repositories
{
    public class TipoPrestamoRepositorio
    {
        private readonly string _connectionString;

        public TipoPrestamoRepositorio(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        // Obtener todos los tipos de préstamo
        public async Task<IEnumerable<TipoPrestamo>> GetAll()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var query = "SELECT * FROM TiposPrestamo";
                return await connection.QueryAsync<TipoPrestamo>(query);
            }
        }

        // Obtener un tipo de préstamo por ID
        public async Task<TipoPrestamo> GetById(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var query = "SELECT * FROM TiposPrestamo WHERE Id = @Id";
                return await connection.QueryFirstOrDefaultAsync<TipoPrestamo>(query, new { Id = id });
            }
        }

        // Insertar un nuevo tipo de préstamo
        public async Task<int> Add(TipoPrestamo tipoPrestamo)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var query = "INSERT INTO TiposPrestamo (Descripcion) VALUES (@Descripcion)";
                return await connection.ExecuteAsync(query, tipoPrestamo);
            }
        }

        // Actualizar un tipo de préstamo existente
        public async Task<int> Update(TipoPrestamo tipoPrestamo)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var query = @"UPDATE TiposPrestamo SET 
                              Descripcion = @Descripcion 
                              WHERE Id = @Id";
                return await connection.ExecuteAsync(query, tipoPrestamo);
            }
        }

        // Eliminar un tipo de préstamo
        public async Task<int> Delete(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var query = "DELETE FROM TiposPrestamo WHERE Id = @Id";
                return await connection.ExecuteAsync(query, new { Id = id });
            }
        }
    }
}
