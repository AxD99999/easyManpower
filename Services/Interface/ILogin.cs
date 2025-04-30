using Dapper;

public interface ILogin
{
    Task<LoginRes> LoginAsync(Login login);
}