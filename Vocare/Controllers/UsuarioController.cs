using Microsoft.AspNetCore.Mvc;

namespace Vocare.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsuarioController : ControllerBase
    {
        [HttpGet]
        public IActionResult Index()
        {
            return Ok("fdfsd");
        }
    }
}
