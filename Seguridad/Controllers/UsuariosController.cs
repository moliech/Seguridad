using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Seguridad.Datos;
using Seguridad.Models;
using Seguridad.Models.Dtos;


namespace Seguridad.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class UsuariosController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public UsuariosController(ApplicationDbContext context)
        {
            _context = context;
        }

        //Get: api/usuarios - Obtener todos los usuarios
        [HttpGet("Mostrar Usuarios")]
        public async Task<ActionResult<IEnumerable<Usuario>>> GetUsuarios()
        {
            return await _context.Usuarios.Include(u => u.Rol).ToListAsync();
        }

        //Get: api/usuarios/5 - Obtener un usuario por ID
        [HttpGet("Buscar por ID")]
        public async Task<ActionResult<Usuario>> GetUsuario(int id)
        {
            var usuario = await _context.Usuarios.Include(u => u.Rol).FirstOrDefaultAsync(u => u.Id == id);
            if (usuario == null)
            {
                return NotFound();
            }
            return usuario;
        }

        //Post: api/usuarios - Crear un nuevo usuario
        [HttpPost("Crear Usuario")]
        public async Task<ActionResult> CrearUsuario([FromBody] UsuarioCrearDTO dto)
        {
            var usuario = new Usuario
            {
                Nombre = dto.Nombre,
                Email = dto.Email,
                Contrasena = dto.Contrasena,
                RolId = dto.RolId
            };

            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();
            return Ok(usuario);
        }

        //Put: api/usuarios/5 - Editar un usuario
        [HttpPut("Editar Usuario")]
        public async Task<IActionResult> PutUsuario(int id, [FromBody] UsuarioEditarDTO dto)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
                return NotFound();

            usuario.Nombre = dto.Nombre;
            usuario.Email = dto.Email;
            usuario.RolId = dto.RolId;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        //Delete: api/usuarios/5 - Eliminar un usuario
        [HttpDelete("Eliminar Usuario")]
        public async Task<IActionResult> DeleteUsuario(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
            {
                return NotFound();
            }
            _context.Usuarios.Remove(usuario);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
