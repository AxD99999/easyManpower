public class CountrySet
{
    public int? UserID { get; set; }
    public string? AuthCode { get; set; }
    public string? Device { get; set; }
    public int? ID { get; set; }
    public string? Country { get; set; }
    public string? Status { get; set; }
    public string? Flag { get; set; }
}

public class CountrySetRes : CommonResponse
{
    public List<dynamic>? CountrySetList { get; set; }
}