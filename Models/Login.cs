public class Login
{
    public string? ComID { get; set; }
    public string? UserName { get; set; }
    public string? Password { get; set; }
    public string? Device { get; set; }
}

public class LoginRes : CommonResponse
{
    public List<dynamic>? LoginList { get; set; }
}