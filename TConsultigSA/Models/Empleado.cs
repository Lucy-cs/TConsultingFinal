using System.ComponentModel.DataAnnotations;

namespace TConsultigSA.Models
{
    public class Empleado
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El DPI es obligatorio")]
        [StringLength(13, ErrorMessage = "El DPI debe tener un máximo de 13 caracteres")]
        public string DPI { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "La fecha de contratación es obligatoria")]
        public DateTime FechaContratado { get; set; }

        [Required(ErrorMessage = "El puesto es obligatorio")]
        public int IdPuesto { get; set; }

        [Required(ErrorMessage = "El departamento es obligatorio")]
        public int IdDepartamento { get; set; }

        public int? IdUsuario { get; set; }

        public string Email { get; set; }
        public string Telefono { get; set; }
        public string Direccion { get; set; }

        [Required(ErrorMessage = "El salario es obligatorio")]
        [Range(0, double.MaxValue, ErrorMessage = "El salario debe ser un valor positivo.")]
        public decimal Salario { get; set; }
    }
}
