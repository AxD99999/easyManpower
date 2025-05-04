using Dapper;
using System.Data;
using Microsoft.AspNetCore.Mvc;
public class CountrySetService : ICountrySet
{
    private readonly DapperContext _dapperContext;

    public CountrySetService (DapperContext dapperContext)
    {
        _dapperContext = dapperContext;
    }

    public async Task<CountrySetRes> CountrySetAsync(CountrySet countrySet)
    {
        using var connection = _dapperContext.CreateConnection();
        var sql = "spCountrySetting"; // Stored procedure name
        var res = new CountrySetRes();
        var Parameters = new DynamicParameters();

        Parameters.Add("@UserID", countrySet.UserID);
        Parameters.Add("@AuthCode", countrySet.AuthCode);
        Parameters.Add("@Device", countrySet.Device);
        Parameters.Add("@ID", countrySet.ID);
        Parameters.Add("@Country", countrySet.Country);
        Parameters.Add("@Status", countrySet.Status);
        Parameters.Add("@Flag", countrySet.Flag);


        var data = DbHelper.RunProc<dynamic>(sql, Parameters);


        if (data.Any() && data.FirstOrDefault().Message == null)
        {
            res.StatusCode = 200;
            res.Message = "Success";
            res.CountrySetList = data.ToList();
        }
        else if (data.Count() == 1 && data.FirstOrDefault().Message != null)
        {
            res.StatusCode = data.FirstOrDefault().StatusCode;
            res.Message = data.FirstOrDefault().Message;
            res.CountrySetList = null;
        }
        else
        {
            res.StatusCode = 400;
            res.Message = "No Data";
            res.CountrySetList = null;
        }

        return res;
    }
}