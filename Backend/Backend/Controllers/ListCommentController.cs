using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ListCommentController : ControllerBase
    {
        private readonly ComentariosService _comentariosService;

        public ListCommentController(ComentariosService comentariosService)
        {
            _comentariosService = comentariosService;
        }

        [HttpGet]
        
        public async Task<IActionResult> GetComments([FromQuery] int idPublicacion)
        {
            try
            {
                // Llamar al servicio para obtener los comentarios filtrados por idPublicacion
                var comments = await _comentariosService.GetCommentsByPublication(idPublicacion);
                return Ok(comments);
            }
            catch (Exception ex)
            {
                // Manejo de errores
                return StatusCode(500, $"Error al procesar la solicitud: {ex.Message}");
            }
        }
    }

    // Clase de resultado para los comentarios
    public class CommentResult
    {
        public string Comment { get; set; }
        public string UserName { get; set; }
    }
}
