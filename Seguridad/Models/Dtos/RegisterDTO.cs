namespace Seguridad.Models.Dtos
{
    public class RegisterDTO
    {
        public string Nombre { get; set; }
        public string Email { get; set; }
        public string Contrasena { get; set; }
        public int RolId { get; set; }
    }
}
