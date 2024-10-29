namespace TConsultigSA.Models
{
    public class Departamento
    {
        public int Id { get; set; }  // Clave primaria del departamento
        public string DepartamentoNombre { get; set; }  // Nombre del departamento
        public int? IdLider { get; set; }  // ID del líder del departamento (opcional)
        public int IdEmpresa { get; set; }  // Relación con la tabla Empresas (clave externa)
    }
}
