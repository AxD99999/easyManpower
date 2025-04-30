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
        var data = await _changePassword.ChangePasswordAsync(changePassword);
        return Ok(data);


    }
}