using Microsoft.AspNetCore.Mvc;
using Synced.Backend.Controllers.HttpPayload;
using Synced.Backend.Singletons;

namespace Synced.Backend.Controllers;

[ApiController]
[Route("/api/v1/[controller]/[action]")]
public class Initialization : Controller
{
    [HttpGet]
    [Route("/api/v1/[controller]")]
    [Route("/api/v1/[controller]/[action]")]
    public IActionResult Index([FromBody] InitConfig config)
    {
        return Ok("test"); 
    }
    [HttpGet]
    public IActionResult IsInit()
    {
        if (!Runtime.IsInit)
        {
            return BadRequest("Not in init mode. "); 
        }

        return BadRequest(new NotImplementedException()); 
    }
}