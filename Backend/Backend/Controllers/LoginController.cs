using Microsoft.AspNetCore.Mvc;
using Backend;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly UsuariosService _usuariosService;

        public LoginController(UsuariosService usuariosService)
        {
            _usuariosService = usuariosService;
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            try
            {
                var response = await _usuariosService.LoginAsync(request.Email, request.Password);

                // Si la respuesta es null, se considera un error
                if (response == null)
                {
                    return Unauthorized("Error de autenticación.");
                }

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
    }


    public class LoginRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
