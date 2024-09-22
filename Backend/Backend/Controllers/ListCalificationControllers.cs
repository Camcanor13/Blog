using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ListCalificationController : ControllerBase
    {
        private readonly CalificacionesService _calificacionesService;

        public ListCalificationController(CalificacionesService calificacionesService)
        {
            _calificacionesService = calificacionesService;
        }

        [HttpGet("{postId}")]
        public async Task<IActionResult> GetCalifications(int postId)
        {
            // Verificar si el postId es válido
            if (postId <= 0)
            {
                return BadRequest("El id de la publicación no es válido.");
            }

            try
            {
                // Llamar al servicio para obtener las calificaciones
                IEnumerable<CalificationResult> califications = await _calificacionesService.GetCalificationsByPublication(postId);

                // Verificar si se encontraron calificaciones
                if (califications == null || !califications.Any())
                {
                    return NotFound("No se encontraron calificaciones para esta publicación.");
                }

                // Retornar la lista de calificaciones
                return Ok(califications);
            }
            catch (Exception ex)
            {
                // Manejo de errores
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }
    }
}
