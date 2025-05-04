using Dapper;
using System.Data;
using Microsoft.AspNetCore.Mvc;
public class RefCountrySetService : IRefCountrySet
{
    private readonly DapperContext _dapperContext;

    public RefCountrySetService(DapperContext dapperContext)
    {
        _dapperContext = dapperContext;
    }

    public async Task<RefCountrySetRes> RefCountrySetAsync(RefCountrySet refcountrySet)
    {
        using var connection = _dapperContext.CreateConnection();
        var sql = "spReferenceCountry"; // Stored procedure name
        var res = new RefCountrySetRes();
        var Parameters = new DynamicParameters();

        Parameters.Add("@UserID", refcountrySet.UserID);
        Parameters.Add("@AuthCode", refcountrySet.AuthCode);
        Parameters.Add("@Device", refcountrySet.Device);
        Parameters.Add("@ID", refcountrySet.ID);
        Parameters.Add("@Name", refcountrySet.Name);
        Parameters.Add("@Address", refcountrySet.Address);
        Parameters.Add("@Phone", refcountrySet.Phone);
        Parameters.Add("@Mobile", refcountrySet.Mobile);
        Parameters.Add("@Mail", refcountrySet.Mail);
        Parameters.Add("@Status", refcountrySet.Status);
        Parameters.Add("@Flag", refcountrySet.Flag);


        var data = DbHelper.RunProc<dynamic>(sql, Parameters);


        if (data.Any() && data.FirstOrDefault().Message == null)
        {
            res.StatusCode = 200;
            res.Message = "Success";
            res.RefCountrySetList = data.ToList();
        }
        else if (data.Count() == 1 && data.FirstOrDefault().Message != null)
        {
            res.StatusCode = data.FirstOrDefault().StatusCode;
            res.Message = data.FirstOrDefault().Message;
            res.RefCountrySetList = null;
        }
        else
        {
            res.StatusCode = 400;
            res.Message = "No Data";
            res.RefCountrySetList = null;
        }

        return res;
    }
}