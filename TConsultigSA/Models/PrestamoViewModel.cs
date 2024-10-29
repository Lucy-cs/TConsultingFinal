namespace TConsultigSA.Models
{
public class PrestamoViewModel
{
    public int Id { get; set; }
    public int IdEmpleado { get; set; }
    public string EmpleadoNombre { get; set; }
    public decimal Total { get; set; }
    public DateTime FechaPrestamo { get; set; }
    public int CuotasPendientes { get; set; }
}
}
