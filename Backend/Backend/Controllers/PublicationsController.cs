using Backend.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PublicationsController : ControllerBase
    {
        private readonly PublicacionesService _publicacionesService;

        public PublicationsController(PublicacionesService publicacionesService)
        {
            _publicacionesService = publicacionesService; 
        }

        // Get publicaciones
        [HttpGet]
        public async Task<IActionResult> GetPublications()
        {
            try
            {
                var publications = await _publicacionesService.GetPublicationsAsync();
                return Ok(publications);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error");
            }
        }

        //Post publicaciones
        [HttpPost]
        public async Task<IActionResult> AddPublication([FromBody] PublicationRequest request)
        {
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

        // Delete publicaciones
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

        //Put publicaciones
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

        // Put estado de publicaciones
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

    public class UpdatePublicationRequest
    {
        public string Title { get; set; }
        public string Body { get; set; }
        public DateTime Date { get; set; }
        public int Comments { get; set; }
        public int Qualification { get; set; }
        public string Status { get; set; }
    }
    public class UpdateStatusRequest
    {
        public string NewStatus { get; set; }
    }
}
