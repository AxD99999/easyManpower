using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration.CommandLine;

[Route("api/[controller]")]
[ApiController]
public class ChangePasswordController : ControllerBase
{
    private readonly IChangePassword _changePassword;

    public ChangePasswordController(IChangePassword changePassword)
    {
        _changePassword = changePassword;
    }

    [HttpPost, Route("~/api/admin/change-pwd")]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePassword changePassword)
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

        var data = await _changePassword.ChangePasswordAsync(changePassword);
        return Ok(data);


    }
}