using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Seguridad.Datos;
using Seguridad.Models;
using Seguridad.Models.Dtos;

namespace Seguridad.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RolesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public RolesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/roles - Obtener todos los roles
        [HttpGet("Mostrar Roles")]
        public async Task<ActionResult<IEnumerable<Rol>>> GetRoles()
        {
            return await _context.Roles.ToListAsync();
        }

        // GET: api/roles/5 - Obtener un rol por ID
        [HttpGet("Buscar por ID")]
        public async Task<ActionResult<Rol>> GetRol(int id)
        {
            var rol = await _context.Roles.FindAsync(id);
            if (rol == null)
            {
                return NotFound();
            }
            return rol;
        }

        // POST: api/roles - Crear un nuevo rol
        [HttpPost("Crear Rol")]
        public async Task<ActionResult> CrearRol([FromBody] RolCrearDTO dto)
        {
            var rol = new Rol
            {
                Nombre = dto.Nombre
            };
            _context.Roles.Add(rol);
            await _context.SaveChangesAsync();
            return Ok(rol);
        }

        // PUT: api/roles/5 - Editar un rol
        [HttpPut("Editar Rol")]
        public async Task<IActionResult> PutRol(int id, Rol rol)
        {
            if (id != rol.Id)
            {
                return BadRequest();
            }
            _context.Entry(rol).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if(!_context.Roles.Any(e => e.Id == id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return NoContent();
        }

        // DELETE: api/roles/5 - Eliminar un rol
        [HttpDelete("Eliminar Rol")]
        public async Task<IActionResult> DeleteRol(int id)
        {
            var rol = await _context.Roles.FindAsync(id);
            if (rol == null)
            {
                return NotFound();
            }
            _context.Roles.Remove(rol);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
