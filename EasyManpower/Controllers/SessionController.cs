using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration.CommandLine;

[Route("api/[controller]")]
[ApiController]
public class SessionController : ControllerBase
{
    private readonly ISession _session;

    public SessionController(ISession session)
    {
        _session = session;
    }

    [HttpPost, Route("~/api/admin/check-session")]
    public async Task<IActionResult> Session([FromBody] Session session)
    {

        if (!Request.Headers.TryGetValue("Auth", out var authHeader) || string.IsNullOrEmpty(authHeader))
        {
            return BadRequest(new { status = 1, error = "Auth header is required" });
        }

        string signature = authHeader.ToString();

        if (signature != "Easy")
        {
            return Unauthorized(new { status = 1, error = "Unauthorized" });
        }

        var data = await _session.SessionAsync(session);
        return Ok(data);

     
    }
}