using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration.CommandLine;

[Route("api/[controller]")]
[ApiController]
public class RefOutCountrySetController : ControllerBase
{
    private readonly IRefOutCountrySet _refOutCountrySet;

    public RefOutCountrySetController(IRefOutCountrySet refOutCountrySet)
    {
        _refOutCountrySet = refOutCountrySet;
    }

    [HttpPost, Route("~/api/admin/refOutCountry-set")]
    public async Task<IActionResult> RefOutCountrySet([FromBody] RefOutCountrySet refOutCountrySet)
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

        var data = await _refOutCountrySet.RefOutCountrySetAsync(refOutCountrySet);
        return Ok(data);


    }
}