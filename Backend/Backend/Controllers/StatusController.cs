using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StatusController : ControllerBase
    {
        private readonly PublicacionesService _publicacionesService;

        public StatusController(PublicacionesService publicacionesService)
        {
            _publicacionesService = publicacionesService;
        }

        [HttpPut("UpdateStatus/{id}")]
        public async Task<IActionResult> UpdateStatus(int id, [FromBody] UpdateStatusRequest request)
        {
            if (request == null || string.IsNullOrEmpty(request.NewStatus))
            {
                return BadRequest("El estado no puede estar vacío.");
            }

            var result = await _publicacionesService.UpdatePublicationStatus(id, request.NewStatus);

            if (result == "Estado actualizado exitosamente.")
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }
    }

    public class UpdateStatusRequest
    {
        public string NewStatus { get; set; }
    }
}
