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
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Usuario>>> GetUsuarios()
        {
            return await _context.Usuarios.Include(u => u.Rol).ToListAsync();
        }

        //Get: api/usuarios/5 - Obtener un usuario por ID
        [HttpGet("{id}")]
        public async Task<ActionResult<Usuario>> GetUsuario(int id)
        {
            var usuario = await _context.Usuarios.Include(u => u.Rol).FirstOrDefaultAsync(u => u.Id == id);
            if (usuario == null)
            {
                return NotFound();
            }
            return usuario;
        }

        [HttpPost("activos")]
        public async Task<ActionResult<IEnumerable<Usuario>>> GetUsuariosActivos()
        {
            var usuarios = await _context.Usuarios
                .Include(u => u.Rol)
                .Where(u => u.estado.ToLower()== "activo")
                .ToListAsync();
            return usuarios;
        }

        //Post: api/usuarios - Crear un nuevo usuario
        [HttpPost] // (Recomendación: quitar el string "Crear Usuario" si ya lo hiciste)
        public async Task<ActionResult> CrearUsuario([FromBody] UsuarioCrearDTO dto)
        {
            // Opcional: Verificar si el email ya existe para dar un error más claro
            if (await _context.Usuarios.AnyAsync(u => u.Email == dto.Email))
            {
                return Conflict(new { message = "El email ya está registrado." });
            }

            var usuario = new Usuario
            {
                Nombre = dto.Nombre,
                Email = dto.Email,
                Contrasena = dto.Contrasena, // <-- Recuerda que esto debe ser un hash
                RolId = dto.RolId,

                // --- ¡AQUÍ ESTÁ LA CORRECCIÓN! ---
                // Asignamos un estado por defecto al crear el usuario.
                estado = "activo"
            };

            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetUsuario), new { id = usuario.Id }, usuario);
        }

        //Put: api/usuarios/5 - Editar un usuario
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUsuario(int id, [FromBody] UsuarioEditarDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
                return NotFound(new { mensaje = "Usuario no encontrado" });

            usuario.Nombre = dto.Nombre;
            usuario.Email = dto.Email;
            usuario.RolId = dto.RolId;
            usuario.estado = dto.estado;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return StatusCode(500, new { mensaje = "Error al actualizar el usuario" });
            }

            return NoContent();
        }

        //Delete: api/usuarios/5 - Eliminar un usuario
        [HttpDelete("{id}")]
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
