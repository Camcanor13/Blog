using Backend.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly UsuariosService _usuariosService;

        public UsuariosController(UsuariosService usuariosService)
        {
            _usuariosService = usuariosService;
        }

        // Método para iniciar sesión
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            try
            {
                var response = await _usuariosService.LoginAsync(request.Email, request.Password);
                return Ok(response);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error al procesar la solicitud. Inténtalo de nuevo más tarde.");
            }
        }

        // Método para registrar un nuevo usuario
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            // Validación básica de entrada
            if (request == null || string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Password) || string.IsNullOrEmpty(request.User) || string.IsNullOrEmpty(request.Rol))
            {
                return BadRequest("Todos los campos son obligatorios.");
            }

            var result = await _usuariosService.RegisterAsync(request.Email, request.Password, request.User, request.Rol);
            if (result == "Registro exitoso.")
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }

        // Método para obtener todos los usuarios
        [HttpGet("list")]
        public ActionResult<IEnumerable<Usuario>> GetUsuarios()
        {
            var usuarios = _usuariosService.GetUsuarios();
            return Ok(usuarios);
        }
    }

    public class LoginRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class RegisterRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string User { get; set; }
        public string Rol { get; set; }
    }
}
