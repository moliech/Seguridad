using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Seguridad.Models
{
    public class Log
    {
        [Key]
        public int Id { get; set; }

        public DateTime FechaInicio { get; set; }

        public int UsuarioId { get; set; }

        public Usuario Usuario { get; set; }
    }
}
