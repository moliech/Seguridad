using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Seguridad.Models
{
    public class Usuario
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "El Nombre es requerido")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El Correo es requerido")]
        [EmailAddress(ErrorMessage ="Correo no valido")]
        public string Email { get; set;}

        [Required(ErrorMessage = "La Contraseña es requerida")]
        public string Contrasena { get; set; }

        public string estado { get; set; }

        [ForeignKey("Rol")]
        public int RolId { get; set; }
        public Rol Rol { get; set; }

        
    }
}
