using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Seguridad.Models.Dtos
{
    public class RolCrearDTO
    {
        public string Nombre { get; set; }
    }
}
