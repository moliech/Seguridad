using System.ComponentModel.DataAnnotations;

namespace Seguridad.Models
{
    public class Rol
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre del rol es requerido.")]
        public string Nombre { get; set; }
    }
}
