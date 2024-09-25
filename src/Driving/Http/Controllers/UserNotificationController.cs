using System.Security.Claims;
using Domain.Ports.Driven;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Driving.Http.Controllers;

// TODO: Create a Notification domain model and maybe a domain service?
// Im not really sure yet how this will integrate with users so it might be a good idéa to not make it too complex yet.
// This should also be located at '/user/notifications' or some similar route.

[Authorize]
[ApiController]
[Route("[controller]")]
public class UserNotificationController(IRedisClient client) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<string>> Get()
    {
        var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId is not { Length: > 0 })
        {
            return Forbid();
        }

        var notifications = await client.GetByPattern<Notification>($"{userId}:notifications*");
        return Ok(notifications);
    }

    [HttpPost]
    public async Task<ActionResult> Post([FromBody] NotificationDto notificationDto)
    {
        var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId is not { Length: > 0 })
        {
            return Forbid();
        }

        var notification = new Notification(Guid.NewGuid(),
            notificationDto.Title,
            notificationDto.Message,
            DateTimeOffset.UtcNow);

        await client.Set($"{userId}:notifications:{notification.Id}", notification);

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(string id)
    {
        var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId is not { Length: > 0 })
        {
            return Forbid();
        }

        await client.Delete($"{userId}:notifications:{id}");
        return NoContent();
    }
}

public record NotificationDto(string Title, string Message);

public record Notification(Guid Id, string Title, string Message, DateTimeOffset Timestamp);