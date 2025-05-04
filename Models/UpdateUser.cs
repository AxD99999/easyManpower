public class UpdateUser
{
    public string? ComID { get; set; }
    public int? UserID { get; set; }
    public string? AuthCode { get; set; }
    public string? Device { get; set; }
    public string? FullName { get; set; }
    public string? Address { get; set; }
    public string? Phone { get; set; }
    public string? Mobile { get; set; }
    public string? ImgPath { get; set; }
}

public class UpdateUserRes : CommonResponse
{
    public List<dynamic>? UpdateUserList { get; set; }
}