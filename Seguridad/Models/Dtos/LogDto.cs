using System.Text.Json.Serialization;

namespace Seguridad.Models.Dtos
{
    public class LogDto
    {
        public int Id { get; set; }

        public string fecha => FechaInicio.ToString("dd-MM-yyyy HH:mm");

        public string Nombre { get; set; }

        [JsonIgnore]
        public DateTime FechaInicio { get; set; }
    }
}
