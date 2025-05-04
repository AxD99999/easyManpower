public class CompanySet
{
    public int? UserID { get; set; }
    public string? AuthCode { get; set; }
    public string? Device { get; set; }
    public int? ID { get; set; }
    public string? Company { get; set; }
    public string? Cell { get; set; }
    public string? Mail { get; set; }
    public string? City { get; set; }
    public string? Contact { get; set; }
    public string? Owner { get; set; }
    public string? Address { get; set; }
    public string? CR { get; set; }
    public string? Location { get; set; }
    public string? Status { get; set; }
    public string? Flag { get; set; }
}

public class CompanySetRes : CommonResponse
{
    public List<dynamic>? CompanySetList { get; set; }
}