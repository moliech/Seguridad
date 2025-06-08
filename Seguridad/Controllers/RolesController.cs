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
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Rol>>> GetRoles()
        {
            return await _context.Roles.ToListAsync();
        }

        // GET: api/roles/5 - Obtener un rol por ID
        [HttpGet("{id}")]
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
        [HttpPost]
        public async Task<ActionResult<Rol>> PostRol([FromBody] RolCrearDTO dto)
        {
            var rol = new Rol { Nombre = dto.Nombre };
            _context.Roles.Add(rol);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetRol), new { id = rol.Id }, rol);
        }

        // PUT: api/roles/5 - Editar un rol
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRol(int id, [FromBody] RolCrearDTO dto)
        {
            var rol = await _context.Roles.FindAsync(id);
            if (rol == null)
            {
                return NotFound();
            }

            rol.Nombre = dto.Nombre; // Actualizamos solo la propiedad que nos interesa
                                     // _context.Entry(rol).State = EntityState.Modified; // Esto es una forma alternativa

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/roles/5 - Eliminar un rol
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRol(int id)
        {
            var rol = await _context.Roles.FindAsync(id);
            if (rol == null)
            {
                return NotFound();
            }

            // --- LÓGICA DE VALIDACIÓN AÑADIDA ---
            // Verificamos si algún usuario está usando este rol ANTES de intentar borrar.
            var isRolInUse = await _context.Usuarios.AnyAsync(u => u.RolId == id);
            if (isRolInUse)
            {
                // Devolvemos un 409 Conflict, que es más específico que un 400.
                // Significa que la petición es válida, pero no se puede completar por un conflicto de estado.
                return Conflict(new { message = "No se puede eliminar el rol porque está asignado a uno o más usuarios." });
            }
            // --- FIN DE LA LÓGICA AÑADIDA ---

            _context.Roles.Remove(rol);
            await _context.SaveChangesAsync();

            return NoContent(); // Esto significa 204 No Content, que es correcto para un DELETE exitoso.
        }
    }
}
