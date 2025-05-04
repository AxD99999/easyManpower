using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration.CommandLine;

[Route("api/[controller]")]
[ApiController]
public class RefCountrySetController : ControllerBase
{
    private readonly IRefCountrySet _refCountrySet;

    public RefCountrySetController(IRefCountrySet refCountrySet)
    {
        _refCountrySet = refCountrySet;
    }

    [HttpPost, Route("~/api/admin/refCountry-set")]
    public async Task<IActionResult> RefCountrySet([FromBody] RefCountrySet refCountrySet)
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

        var data = await _refCountrySet.RefCountrySetAsync(refCountrySet);
        return Ok(data);


    }
}