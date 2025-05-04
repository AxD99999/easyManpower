using Dapper;

public interface ICountrySet
{
    Task<CountrySetRes> CountrySetAsync(CountrySet countrySet);
}