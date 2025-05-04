public class RefOutCountrySet
{
    public int? UserID { get; set; }
    public string? AuthCode { get; set; }
    public string? Device { get; set; }
    public int? ID { get; set; }
    public string? Name { get; set; }
    public string? Address { get; set; }
    public string? Phone { get; set; }
    public string? Mobile { get; set; }
    public string? Mail { get; set; }
    public string? Status { get; set; }
    public string? Flag { get; set; }
}

public class RefOutCountrySetRes : CommonResponse
{
    public List<dynamic>? RefOutCountrySetList { get; set; }
}