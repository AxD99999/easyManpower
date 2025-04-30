using Dapper;
using System.Data;
using Microsoft.AspNetCore.Mvc;
public class LoginService : ILogin
{
    private readonly DapperContext _dapperContext;

    public LoginService (DapperContext dapperContext)
    {
        _dapperContext = dapperContext;
    }

    public async Task<LoginRes> LoginAsync(Login login)
    {
        using var connection = _dapperContext.CreateConnection();
        var sql = "spLogin"; // Stored procedure name
        var res = new LoginRes();
        var Parameters = new DynamicParameters();

        Parameters.Add("@ComID", login.ComID);
        Parameters.Add("@UserName", login.UserName);
        Parameters.Add("@@Password", login.Password);
        Parameters.Add("@Device", login.Device);

        var data = DbHelper.RunProc<dynamic>(sql, Parameters);


        if (data.Any() && data.FirstOrDefault().Message == null)
        {
            res.StatusCode = 200;
            res.Message = "Success";
            res.LoginList = data.ToList();
        }
        else if (data.Count() == 1 && data.FirstOrDefault().Message != null)
        {
            res.StatusCode = data.FirstOrDefault().StatusCode;
            res.Message = data.FirstOrDefault().Message;
            res.LoginList = null;
        }
        else
        {
            res.StatusCode = 400;
            res.Message = "No Data";
            res.LoginList = null;
        }

        return res;
    }
}