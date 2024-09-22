using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddCalificationController : ControllerBase
    {
        private readonly CalificacionesService _calificacionesService;

        public AddCalificationController(CalificacionesService calificacionesService) // Cambié el tipo y nombre del servicio
        {
            _calificacionesService = calificacionesService;
        }

        [HttpPost]
        public async Task<IActionResult> AddCalification([FromBody] CalificationRequest request) // Cambié el tipo de solicitud
        {
            if (request == null)
            {
                return BadRequest("Datos de calificación inválidos.");
            }

            // Llamar al servicio para agregar la calificación
            var result = await _calificacionesService.AddCalification(
                request.id_publicacion,
                request.id_usuario,
                request.calificacion
            );

            if (result == "Calificación agregada exitosamente.")
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }
    }

    public class CalificationRequest // Cambié el nombre de la clase
    {
        public int id_publicacion { get; set; }
        public int id_usuario { get; set; }
        public int calificacion { get; set; }
    }
}
