using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration.CommandLine;

[Route("api/[controller]")]
[ApiController]
public class LoginController : ControllerBase
{
    private readonly ILogin _login;

    public LoginController(ILogin login)
    {
        _login = login;
    }

    [HttpPost, Route("~/api/admin/Login")]
    public async Task<IActionResult> Login([FromBody] Login login)
    {
        var data = await _login.LoginAsync(login);
        return Ok(data);

     
    }
}