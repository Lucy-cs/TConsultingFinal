public class Usuario
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty; // Inicializar con valor por defecto
    public string Contrasenia { get; set; } = string.Empty; // Inicializar con valor por defecto
    public int IdRol { get; set; }
}
