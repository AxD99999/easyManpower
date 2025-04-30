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
        var data = await _session.SessionAsync(session);
        return Ok(data);

     
    }
}