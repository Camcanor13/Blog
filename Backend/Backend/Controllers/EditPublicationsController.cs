using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EditPublicationsController : ControllerBase
    {
        private readonly PublicacionesService _publicacionesService;

        public EditPublicationsController(PublicacionesService publicacionesService)
        {
            _publicacionesService = publicacionesService;
        }

        // Método para actualizar una publicación
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePublication(int id, [FromBody] UpdatePublicationRequest request)
        {
            if (request == null)
            {
                return BadRequest("El cuerpo de la solicitud no puede estar vacío.");
            }

            if (string.IsNullOrWhiteSpace(request.Title))
            {
                return BadRequest("El título es obligatorio.");
            }

            var result = await _publicacionesService.UpdatePublication(
                id,
                request.Title,
                request.Body,
                request.Date,
                request.Comments,
                request.Qualification,
                request.Status
            );

            if (result == "Actualización exitosa.")
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }
    }

    // Clase de solicitud para actualizar la publicación
    public class UpdatePublicationRequest
    {
        public string Title { get; set; }
        public string Body { get; set; }
        public DateTime Date { get; set; }
        public int Comments { get; set; }
        public int Qualification { get; set; }
        public string Status { get; set; }
    }
}
