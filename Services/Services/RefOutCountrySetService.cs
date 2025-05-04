using Dapper;
using System.Data;
using Microsoft.AspNetCore.Mvc;
public class RefOutCountrySetService : IRefOutCountrySet
{
    private readonly DapperContext _dapperContext;

    public RefOutCountrySetService(DapperContext dapperContext)
    {
        _dapperContext = dapperContext;
    }

    public async Task<RefOutCountrySetRes> RefOutCountrySetAsync(RefOutCountrySet refOutcountrySet)
    {
        using var connection = _dapperContext.CreateConnection();
        var sql = "spReferenceOutCountry"; // Stored procedure name
        var res = new RefOutCountrySetRes();
        var Parameters = new DynamicParameters();

        Parameters.Add("@UserID", refOutcountrySet.UserID);
        Parameters.Add("@AuthCode", refOutcountrySet.AuthCode);
        Parameters.Add("@Device", refOutcountrySet.Device);
        Parameters.Add("@ID", refOutcountrySet.ID);
        Parameters.Add("@Name", refOutcountrySet.Name);
        Parameters.Add("@Address", refOutcountrySet.Address);
        Parameters.Add("@Phone", refOutcountrySet.Phone);
        Parameters.Add("@Mobile", refOutcountrySet.Mobile);
        Parameters.Add("@Mail", refOutcountrySet.Mail);
        Parameters.Add("@Status", refOutcountrySet.Status);
        Parameters.Add("@Flag", refOutcountrySet.Flag);


        var data = DbHelper.RunProc<dynamic>(sql, Parameters);


        if (data.Any() && data.FirstOrDefault().Message == null)
        {
            res.StatusCode = 200;
            res.Message = "Success";
            res.RefOutCountrySetList = data.ToList();
        }
        else if (data.Count() == 1 && data.FirstOrDefault().Message != null)
        {
            res.StatusCode = data.FirstOrDefault().StatusCode;
            res.Message = data.FirstOrDefault().Message;
            res.RefOutCountrySetList = null;
        }
        else
        {
            res.StatusCode = 400;
            res.Message = "No Data";
            res.RefOutCountrySetList = null;
        }

        return res;
    }
}