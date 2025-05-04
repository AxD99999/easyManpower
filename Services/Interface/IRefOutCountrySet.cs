using Dapper;

public interface IRefOutCountrySet
{
    Task<RefOutCountrySetRes> RefOutCountrySetAsync(RefOutCountrySet refOutCountrySet);
}