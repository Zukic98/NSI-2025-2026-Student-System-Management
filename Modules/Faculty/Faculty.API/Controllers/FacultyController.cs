using Microsoft.AspNetCore.Mvc;

namespace Faculty.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FacultyController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get() => Ok("Hello from Faculty API!");
    }
}