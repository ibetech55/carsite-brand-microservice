using Microsoft.AspNetCore.Mvc;

namespace BrandMicroservice.src.Controllers
{
    [ApiController]
    [Route("api/brand-microservice")]
    public class HealthController : ControllerBase
    {
        [HttpGet("healthcheck")]
        public IActionResult GetHealthcheck() 
        {
            return Ok(new
            {
                success = true,
            });
        }
    }
}
