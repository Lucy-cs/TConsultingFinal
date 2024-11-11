namespace TConsultigSA.Models
{
    public class AumentoSalarial
    {
        public int Id { get; set; }
        public int EmpleadoId { get; set; }  // Relación con el empleado
        public DateTime FechaAumento { get; set; }
        public decimal CantidadAumento { get; set; }  // Monto del aumento
        public decimal SalarioFinal { get; set; }      // Salario después del aumento

        // Relación con el empleado
        public Empleado Empleado { get; set; }
    }
}
