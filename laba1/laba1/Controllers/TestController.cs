using Microsoft.AspNetCore.Mvc;

namespace laba1.Controllers;

[ApiController]
[Route("laba1")]
public class TestController : ControllerBase
{
    [HttpGet("test/{input}")]
    public ActionResult TestApi(string input)
    {
        return Ok(input);
    }
}
