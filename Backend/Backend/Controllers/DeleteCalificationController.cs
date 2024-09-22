using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeleteCalificationController : ControllerBase
    {
        private readonly CalificacionesService _calificacionesService;

        public DeleteCalificationController(CalificacionesService calificacionesService)
        {
            _calificacionesService = calificacionesService;
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
    }
}
