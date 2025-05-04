using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration.CommandLine;

[Route("api/[controller]")]
[ApiController]
public class PostSetController : ControllerBase
{
    private readonly IPostSet _postSet;

    public PostSetController(IPostSet postSet)
    {
        _postSet = postSet;
    }

    [HttpPost, Route("~/api/admin/post-set")]
    public async Task<IActionResult> PostSet([FromBody] PostSet postSet)
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

        var data = await _postSet.PostSetAsync(postSet);
        return Ok(data);


    }
}