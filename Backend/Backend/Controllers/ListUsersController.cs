using Backend;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class ListUsersController : ControllerBase
{
    private readonly UsuariosService _listUsuariosService;

    public ListUsersController(IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("MySqlConnection");
        _listUsuariosService = new UsuariosService(connectionString);
    }

    [HttpGet]
    public ActionResult<IEnumerable<Usuario>> Get()
    {
        var usuarios = _listUsuariosService.GetUsuarios();
        return Ok(usuarios);
    }
}
