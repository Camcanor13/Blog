using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DeletePublicationController : ControllerBase
    {
        private readonly PublicacionesService _publicacionesService;

        public DeletePublicationController(PublicacionesService publicacionesService)
        {
            _publicacionesService = publicacionesService;
        }

        // DELETE: api/DeletePublication/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePublication(int id)
        {
            var result = await _publicacionesService.DeletePublication(id);

            if (result == "Eliminación exitosa.")
            {
                return Ok(result);
            }
            else if (result == "No se puede eliminar la publicación porque tiene comentarios asociados.")
            {
                return Conflict(result); // 409 Conflict
            }
            else
            {
                return BadRequest(result); // 400 Bad Request
            }
        }
    }
}
