using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration.CommandLine;

[Route("api/[controller]")]
[ApiController]
public class CompanySetController : ControllerBase
{
    private readonly ICompanySet _companySet;

    public CompanySetController(ICompanySet companySet)
    {
        _companySet = companySet;
    }

    [HttpPost, Route("~/api/admin/company-set")]
    public async Task<IActionResult> CompanySet([FromBody] CompanySet companySet)
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

        var data = await _companySet.CompanySetAsync(companySet);
        return Ok(data);


    }
}