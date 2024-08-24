using Backend;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class ConductoresController : ControllerBase
{
    private readonly ConductorService _conductorService;

    public ConductoresController(IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("MySqlConnection");
        _conductorService = new ConductorService(connectionString);
    }

    [HttpGet]
    public ActionResult<IEnumerable<Conductor>> Get()
    {
        var conductores = _conductorService.GetConductores();
        return Ok(conductores);
    }
}
