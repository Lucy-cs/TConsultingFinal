namespace TConsultigSA.Models
{
    public class EmpleadoViewModel
    {
        public Empleado Empleado { get; set; } = new Empleado();  // Inicializado
        public string NombrePuesto { get; set; } = string.Empty;  // Inicializado
        public string NombreDepartamento { get; set; } = string.Empty;  // Inicializado
    }

}
