using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddPublicationsController : ControllerBase
    {
        private readonly PublicacionesService _publicacionesService;

        public AddPublicationsController(PublicacionesService publicacionesService)
        {
            _publicacionesService = publicacionesService;
        }

        [HttpPost]
        public async Task<IActionResult> AddPublication([FromBody] PublicationRequest request)
        {
            // Llamar al servicio para agregar la publicación
            var result = await _publicacionesService.AddPublication(
                request.Title,
                request.Body,
                request.Date,
                request.Comments,
                request.Qualification,
                request.Status,
                request.Author
            );

            if (result == "Registro exitoso.")
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }
    }

    // Clase de solicitud para agregar una publicación
    public class PublicationRequest
    {
        public string Title { get; set; }
        public string Body { get; set; }
        public DateTime Date { get; set; }
        public int Comments { get; set; }
        public int Qualification { get; set; }
        public string Status { get; set; }
        public int Author { get; set; }
    }
}
