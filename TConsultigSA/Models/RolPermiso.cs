namespace TConsultigSA.Models
{
    public class RolPermiso
    {
        public int IdRol { get; set; }
        public Rol Rol { get; set; }

        public int IdPermiso { get; set; }
        public Permiso Permiso { get; set; }
    }
}
