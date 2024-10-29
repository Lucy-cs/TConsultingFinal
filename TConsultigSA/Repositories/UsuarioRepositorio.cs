using Dapper;
using Microsoft.Data.SqlClient;
using TConsultigSA.Models;

namespace TConsultigSA.Repositories
{
    public class UsuarioRepositorio
    {
        private readonly string _connectionString;

        public UsuarioRepositorio(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<Usuario> ObtenerPorNombre(string nombre)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                string sql = "SELECT * FROM Usuarios WHERE Nombre = @Nombre";
                return await connection.QueryFirstOrDefaultAsync<Usuario>(sql, new { Nombre = nombre });
            }
        }

        public async Task Registrar(Usuario usuario)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                string sql = "INSERT INTO Usuarios (Nombre, Contrasenia, IdRol) VALUES (@Nombre, @Contrasenia, @IdRol)";
                await connection.ExecuteAsync(sql, usuario);
            }
        }
    }

}
