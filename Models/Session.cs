public class Session
{
    public string? ComID { get; set; }
    public int? UserID { get; set; }
    public string? AuthCode { get; set; }
    public string? Device { get; set; }
    public string? ImgPath { get; set; }
}

public class SessionRes : CommonResponse
{
    public List<dynamic>? SessionList { get; set; }
}