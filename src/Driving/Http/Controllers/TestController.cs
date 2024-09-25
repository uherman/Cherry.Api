using Domain.Ports.Driven;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Driving.Http.Controllers;

[Authorize]
[ApiController]
[Route("[controller]/{key}")]
public class TestController(IRedisClient client) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<string>> Get(string key)
    {
        var response = await client.GetString(key);
        if (response is null)
        {
            return NotFound();
        }

        return Ok(response);
    }

    [HttpPost]
    public async Task<ActionResult> Post(string key, [FromBody] string value)
    {
        await client.SetString(key, value);

        return Created($"/api/test/${key}", new { Message = "New entry created", Data = value });
    }

    [HttpDelete]
    public async Task<ActionResult> Delete(string key)
    {
        var response = await client.Delete(key);
        if (!response)
        {
            return NotFound();
        }

        return NoContent();
    }
}