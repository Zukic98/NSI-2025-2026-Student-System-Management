using Microsoft.AspNetCore.Mvc;

namespace Support.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SupportController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get() => Ok("Hello from Support API!");

    }
}
