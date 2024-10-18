using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ComentariosController : ControllerBase
    {
        private readonly ComentariosService _comentariosService;

        public ComentariosController(ComentariosService comentariosService)
        {
            _comentariosService = comentariosService;
        }

        // Endpoint para agregar un comentario
        [HttpPost("add")]
        public async Task<IActionResult> AddComment([FromBody] CommentRequest request)
        {
            if (request == null)
            {
                return BadRequest("Datos del comentario inválidos.");
            }
            var result = await _comentariosService.AddComment(
                request.id_publicacion,
                request.id_usuario,
                request.comentario
            );

            if (result == "Comentario agregado exitosamente :)")
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }

        // Endpoint para listar los comentarios de una publicación
        [HttpGet("list")]
        public async Task<IActionResult> GetComments([FromQuery] int idPublicacion)
        {
            try
            {
                var comments = await _comentariosService.GetCommentsByPublication(idPublicacion);
                return Ok(comments);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
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

    // Clase de resultado para los comentarios
    public class CommentResult
    {
        public string Comment { get; set; }
        public string UserName { get; set; }
    }
}
