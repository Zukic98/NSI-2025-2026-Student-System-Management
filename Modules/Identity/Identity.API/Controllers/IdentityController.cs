using Microsoft.AspNetCore.Mvc;

namespace Identity.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class IdentityController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get() => Ok("Hello from Identity API!");
    }
}