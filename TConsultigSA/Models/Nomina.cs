namespace TConsultigSA.Models
{
    public class NominaResultado
    {
        public int IdEmpleado { get; set; }
        public string NombreEmpleado { get; set; }
        public int Mes { get; set; }
        public int Año { get; set; }
        public decimal SalarioBase { get; set; }
        public decimal HorasTrabajadas { get; set; }
        public decimal HorasExtras { get; set; }
        public decimal Bonificacion { get; set; }
        public decimal DescuentoAusencias { get; set; }
        public decimal DeduccionesPrestamos { get; set; }
        public decimal SalarioBruto { get; set; }
        public decimal SalarioNeto { get; set; }
    }
}
