public class ChangePassword
{
    public string? ComID { get; set; }
    public int? UserID { get; set; }
    public string? AuthCode { get; set; }
    public string? OldPass { get; set; }
    public string? NewPass { get; set; }
}

public class ChangePasswordRes : CommonResponse
{
    public List<dynamic>? ChangePasswordList { get; set; }
}