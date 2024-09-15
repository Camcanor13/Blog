using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddCommentController : ControllerBase
    {
        private readonly ComentariosService _comentariosService;

        public AddCommentController(ComentariosService comentariosService)
        {
            _comentariosService = comentariosService;
        }

        [HttpPost]
        public async Task<IActionResult> AddComment([FromBody] CommentRequest request)
        {
            if (request == null)
            {
                return BadRequest("Datos del comentario inválidos.");
            }

            // Llamar al servicio para agregar el comentario
            var result = await _comentariosService.AddComment(
                request.id_publicacion,
                request.id_usuario,
                request.comentario
            );

            if (result == "Comentario agregado exitosamente.")
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }
    }

    // Clase de solicitud para agregar un comentario
    public class CommentRequest
    {
        public int id_publicacion { get; set; }
        public int id_usuario { get; set; }
        public string comentario { get; set; }
    }
}
