using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Seguridad.Models.Dtos
{
    public class UsuarioCrearDTO
    {
        [Required(ErrorMessage = "El Nombre es requerido")]
        public string Nombre { get; set; }
        [Required(ErrorMessage = "El Correo es requerido")]
        [EmailAddress(ErrorMessage = "Correo no valido")]
        public string Email { get; set; }
        [Required(ErrorMessage = "La Contraseña es requerida")]
        public string Contrasena { get; set; }
        [Required(ErrorMessage = "El Rol es requerido")]
        public int RolId { get; set; }

        public string estado { get; set; }
    }
}
