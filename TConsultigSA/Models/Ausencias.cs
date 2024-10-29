using System.ComponentModel.DataAnnotations;

namespace TConsultigSA.Models
{
    public class Ausencia
    {
        public int Id { get; set; }

        [Required]
        public int IdEmpleado { get; set; }

        [Required(ErrorMessage = "La fecha de solicitud es obligatoria.")]
        public DateTime FechaSolicitud { get; set; } = DateTime.Now;  // Fecha actual por defecto

        [Required(ErrorMessage = "La fecha de inicio es obligatoria.")]
        public DateTime FechaInicio { get; set; }

        [Required(ErrorMessage = "La fecha de fin es obligatoria.")]
        public DateTime FechaFin { get; set; }

        public int TotalDias { get; set; }

        [Required]
        public bool Autorizado { get; set; }

        [Required]
        public bool Deducible { get; set; }
    }
}
