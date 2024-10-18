using Microsoft.AspNetCore.Mvc;
using Backend.Controllers;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MeGustaController : ControllerBase
    {
        private readonly MeGustaService _meGustaService;

        public MeGustaController(MeGustaService meGustaService)
        {
            _meGustaService = meGustaService;
        }

        // Get me gusta
        [HttpGet]
        [Route("publicacion/{publicationId}")]
        public async Task<ActionResult<IEnumerable<MeGustaResult>>> GetMeGustaByPublication(int publicationId)
        {
            var result = await _meGustaService.GetMeGustaByPublication(publicationId);
            return Ok(result);
        }

        // Add me gusta
        [HttpPost]
        public async Task<ActionResult<string>> AddMeGusta([FromBody] MeGustaModel model)
        {
            return await _meGustaService.AddMeGusta(model.PublicationId, model.UserId);
        }

        // Eliminar me gusta
        [HttpDelete]
        [Route("{publicationId}/{userId}")]
        public async Task<ActionResult<string>> DeleteMeGusta(int publicationId, int userId)
        {
            return await _meGustaService.DeleteMeGusta(publicationId, userId);
        }

        // Obtener las publicaciones a las que un usuario ha dado me gusta
        [HttpGet]
        [Route("usuario/{userId}")]
        public async Task<ActionResult<IEnumerable<PublicacionMeGustaResult>>> GetPublicacionesMeGusta(int userId)
        {
            var result = await _meGustaService.GetPublicacionesMeGusta(userId);
            return Ok(result);
        }
    }

    public class MeGustaModel
    {
        public int PublicationId { get; set; }
        public int UserId { get; set; }
    }

    public class MeGustaResult
    {
        public string UserName { get; set; }
    }

    public class PublicacionMeGustaResult
    {
        public string Title { get; set; }
        public string Body { get; set; }
        public DateTime Date { get; set; }
        public string Author { get; set; }
    }
}