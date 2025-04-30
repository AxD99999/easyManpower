using Dapper;

public interface IChangePassword
{
    Task<ChangePasswordRes> ChangePasswordAsync(ChangePassword changePassword);
}