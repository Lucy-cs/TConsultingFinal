using Dapper;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Threading.Tasks;
using TConsultigSA.Models;

namespace TConsultigSA.Repositories
{
    public class DepartamentoRepositorio
    {
        private readonly string _connectionString;

        public DepartamentoRepositorio(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        // Obtener todos los departamentos
        public async Task<IEnumerable<Departamento>> GetAll()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var query = @"
            SELECT D.Id, D.Departamento as DepartamentoNombre, D.IdLider, D.IdEmpresa,
                   E.Nombre as EmpresaNombre, L.Nombre as LiderNombre
            FROM Departamentos D
            LEFT JOIN Empresas E ON D.IdEmpresa = E.Id
            LEFT JOIN Empleados L ON D.IdLider = L.Id";
                return await connection.QueryAsync<Departamento>(query);
            }
        }



        // Obtener departamento por ID
        public async Task<Departamento> GetById(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var query = "SELECT * FROM Departamentos WHERE Id = @Id";
                return await connection.QueryFirstOrDefaultAsync<Departamento>(query, new { Id = id });
            }
        }

        // Agregar un nuevo departamento
        public async Task<int> Add(Departamento departamento)
        {
            using var connection = new SqlConnection(_connectionString);
            var query = @"INSERT INTO Departamentos (Departamento, IdLider, IdEmpresa)
                  VALUES (@DepartamentoNombre, @IdLider, @IdEmpresa)";
            return await connection.ExecuteAsync(query, departamento);
        }

        public async Task<int> Update(Departamento departamento)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var query = @"UPDATE Departamentos 
                      SET Departamento = @DepartamentoNombre, 
                          IdEmpresa = @IdEmpresa, 
                          IdLider = @IdLider 
                      WHERE Id = @Id";
                return await connection.ExecuteAsync(query, new
                {
                    departamento.DepartamentoNombre, // Asegúrate de que el nombre del campo en el modelo y en la consulta coinciden
                    departamento.IdEmpresa,
                    departamento.IdLider,
                    departamento.Id
                });
            }
        }


        // Eliminar un departamento
        public async Task<int> Delete(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var query = "DELETE FROM Departamentos WHERE Id = @Id";
                return await connection.ExecuteAsync(query, new { Id = id });
            }
        }
    }
}