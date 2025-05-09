using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Seguridad.Datos;
using Seguridad.Models;
using Seguridad.Models.Dtos;


namespace Seguridad.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LogController : Controller
    {
        private readonly ApplicationDbContext _context;

        public LogController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("Mostrar Logs")]
        public async Task<ActionResult<IEnumerable<LogDto>>> GetLogs()
        {
            var logs=await _context.Logs
                .Include(l => l.Usuario)
                .Select(l => new LogDto
                {
                   Id = l.Id,
                    Nombre = l.Usuario.Nombre,
                    FechaInicio = l.FechaInicio
                })
                .ToListAsync();
            return Ok(logs);
        }



    }
}
