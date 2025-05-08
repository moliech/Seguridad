using System.ComponentModel.DataAnnotations;

namespace Seguridad.Models.Dtos
{
    public class UsuarioEditarDTO
    {
        [Required]
        public string Nombre { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public int RolId { get; set; }
    }
}
