namespace TConsultigSA.Models
{
    public class HorasTrabajoViewModel
    {
        public int Id { get; set; }
        public string NombreEmpleado { get; set; }
        public DateTime Fecha { get; set; }
        public decimal? TotalHoras { get; set; }
    }
}
