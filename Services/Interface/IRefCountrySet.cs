using Dapper;

public interface IRefCountrySet
{
    Task<RefCountrySetRes> RefCountrySetAsync(RefCountrySet refCountrySet);
}