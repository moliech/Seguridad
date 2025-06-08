using Microsoft.AspNetCore.Mvc;
using Seguridad.Datos;
using Seguridad.Models.Dtos;
using Seguridad.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Seguridad.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthController(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        // POST: api/auth/login - Login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO login)
        {
            var usuario = await _context.Usuarios
                .Include(u => u.Rol)
                .FirstOrDefaultAsync(u => u.Email == login.Email);
            // 1. Validar si el usuario existe Y si la contraseña coincide (usando un hasher)
            // Ejemplo conceptual: if (usuario == null || !PasswordHasher.Verify(login.Contrasena, usuario.ContrasenaHash))
            if (usuario == null || usuario.Contrasena != login.Contrasena) // <-- Manteniendo tu lógica actual por ahora
            {
                return Unauthorized(new { message = "Credenciales inválidas" });
            }

            //Guardar log de inicio de sesión
            var log= new Log
            {
                UsuarioId = usuario.Id,
                FechaInicio = DateTime.Now,
            };

            _context.Logs.Add(log);
            await _context.SaveChangesAsync();


            //Crear Claims - información del usuario
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, usuario.Nombre),
                new Claim(ClaimTypes.Email, usuario.Email),
                new Claim(ClaimTypes.Role, usuario.Rol.Nombre)
            };

            //Crear Token
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds
                );

            return Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token),
                usuario = new
                {
                    nombre = usuario.Nombre,
                    email = usuario.Email,
                    rol = usuario.Rol.Nombre
                }
            });
        }

        // POST: api/auth/register - Registro de usuario
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO register)
        {
            var rol = await _context.Roles.FindAsync(register.RolId);
            if (rol == null)
            {
                return BadRequest("Rol no encontrado");
            }
            var usuario = new Usuario
            {
                Nombre = register.Nombre,
                Email = register.Email,
                Contrasena = register.Contrasena,
                Rol = rol
            };
            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();
            return Ok("Usuario registrado exitosamente");
        }

        [HttpPost("validate")]
        public IActionResult ValidarToken([FromBody] string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"])),
                ValidateIssuer = false,
                ValidateAudience = false
            };
            try
            {
                tokenHandler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);
                return Ok("Token válido");
            }
            catch (Exception)
            {
                return Unauthorized("Token inválido");
            }
        }

    } 
}
