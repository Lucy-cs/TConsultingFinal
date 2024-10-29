using System.ComponentModel.DataAnnotations;

namespace TConsultigSA.Models
{
    public class Puesto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "La descripción del puesto es obligatoria.")]
        [StringLength(50, ErrorMessage = "La descripción no puede exceder los 50 caracteres.")]
        public string Descripcion { get; set; }
    }
}
