using System.ComponentModel.DataAnnotations;

namespace TConsultigSA.Models
{
    public class Prestamo
    {
        public int Id { get; set; }

        [Required]
        public int IdEmpleado { get; set; }

        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "El total del préstamo debe ser un valor positivo.")]
        public decimal Total { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Debe haber al menos una cuota pendiente.")]
        public int CuotasPendientes { get; set; }

        public DateTime FechaPrestamo { get; set; }

        public int? IdTipo { get; set; }
    }
}
