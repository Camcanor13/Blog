using Microsoft.AspNetCore.Mvc;

using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PublicationsController : ControllerBase
    {
        private readonly PublicacionesService _publicationsService;

        public PublicationsController(PublicacionesService publicationsService)
        {
            _publicationsService = publicationsService;
        }

        [HttpGet]
        public async Task<IActionResult> GetPublications()
        {
            try
            {
                var publications = await _publicationsService.GetPublicationsAsync();
                return Ok(publications);
            }
            catch (Exception ex)
            {
                // Manejo de errores
                return StatusCode(500, "Error al procesar la solicitud.");
            }
        }
    }
}