using Backend.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CalificacionesController : ControllerBase
    {
        private readonly CalificacionesService _calificacionesService;

        public CalificacionesController(CalificacionesService calificacionesService)
        {
            _calificacionesService = calificacionesService;
        }

        // Método para agregar una nueva calificación
        [HttpPost("add")]
        public async Task<IActionResult> AddCalification([FromBody] CalificationRequest request)
        {
            if (request == null)
            {
                return BadRequest("Datos de calificación inválidos.");
            }

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

        // Método para eliminar una calificación por ID
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCalification(int id)
        {
            var result = await _calificacionesService.DeleteCalification(id);

            if (result == "Calificación eliminada exitosamente.")
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }

        // Método para obtener calificaciones por id de publicación
        [HttpGet("list/{postId}")]
        public async Task<IActionResult> GetCalifications(int postId)
        {
            if (postId <= 0)
            {
                return BadRequest("El id de la publicación no es válido.");
            }

            IEnumerable<CalificationResult> califications = await _calificacionesService.GetCalificationsByPublication(postId);

            if (califications == null || !califications.Any())
            {
                return NotFound("No se encontraron calificaciones para esta publicación.");
            }

            return Ok(califications);
        }
    }

    // Clase para el request de calificación
    public class CalificationRequest
    {
        public int id_publicacion { get; set; }
        public int id_usuario { get; set; }
        public int calificacion { get; set; }
    }
}

