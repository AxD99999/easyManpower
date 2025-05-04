using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration.CommandLine;

[Route("api/[controller]")]
[ApiController]
public class UpdateUserController : ControllerBase
{
    private readonly IUpdateUser _updateUser;

    public UpdateUserController(IUpdateUser updateUser)
    {
        _updateUser = updateUser;
    }

    [HttpPost, Route("~/api/admin/update-user")]
    public async Task<IActionResult> UpdateUser([FromBody] UpdateUser updateUser)
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

        var data = await _updateUser.UpdateUserAsync(updateUser);
        return Ok(data);


    }
}