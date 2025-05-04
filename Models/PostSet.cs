public class PostSet
{
    public int? UserID { get; set; }
    public string? AuthCode { get; set; }
    public string? Device { get; set; }
    public int? ID { get; set; }
    public string? Post { get; set; }
    public string? Status { get; set; }
    public string? Flag { get; set; }
}

public class PostSetRes : CommonResponse
{
    public List<dynamic>? PostSetList { get; set; }
}