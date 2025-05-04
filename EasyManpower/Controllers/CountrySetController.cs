using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration.CommandLine;

[Route("api/[controller]")]
[ApiController]
public class CountrySetController : ControllerBase
{
    private readonly ICountrySet _countrySet;

    public CountrySetController(ICountrySet countrySet)
    {
        _countrySet = countrySet;
    }

    [HttpPost, Route("~/api/admin/country-set")]
    public async Task<IActionResult> CountrySet([FromBody] CountrySet countrySet)
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

        var data = await _countrySet.CountrySetAsync(countrySet);
        return Ok(data);


    }
}