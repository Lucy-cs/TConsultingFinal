using System.ComponentModel.DataAnnotations;

namespace TConsultigSA.Models
{
    public class TipoPrestamo
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "La descripción es obligatoria")]
        public string Descripcion { get; set; }
    }
}
