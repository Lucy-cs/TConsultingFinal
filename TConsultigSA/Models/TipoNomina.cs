using System.ComponentModel.DataAnnotations;

namespace TConsultigSA.Models
{
    public class TipoNomina
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "La descripción es obligatoria")]
        [StringLength(50, ErrorMessage = "La descripción no puede tener más de 50 caracteres")]
        public string Descripcion { get; set; }
    }
}

