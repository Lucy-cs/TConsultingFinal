using System;
using System.ComponentModel.DataAnnotations;

namespace TConsultigSA.Models
{
    public class HorasTrabajo
    {
        public int Id { get; set; }

        public int IdEmpleado { get; set; }

        public virtual Empleado? Empleado { get; set; }

        [Required(ErrorMessage = "La fecha es obligatoria.")]
        [DataType(DataType.Date)]
        public DateTime Fecha { get; set; } = DateTime.Now; // Valor predeterminado dentro del rango permitido

        [Required(ErrorMessage = "Las horas trabajadas son obligatorias.")]
        public decimal TotalHoras { get; set; }

        [Required(ErrorMessage = "Las observaciones son requeridas.")]
        public string Observaciones { get; set; }

        public bool Aprobado { get; set; }
    }
}
