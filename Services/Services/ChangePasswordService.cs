using Dapper;
using System.Data;
using Microsoft.AspNetCore.Mvc;
public class ChangePasswordService : IChangePassword
{
    private readonly DapperContext _dapperContext;

    public ChangePasswordService(DapperContext dapperContext)
    {
        _dapperContext = dapperContext;
    }

    public async Task<ChangePasswordRes> ChangePasswordAsync(ChangePassword changePassword)
    {
        using var connection = _dapperContext.CreateConnection();
        var sql = "spChangePassword"; // Stored procedure name
        var res = new ChangePasswordRes();
        var Parameters = new DynamicParameters();

        Parameters.Add("@ComID", changePassword.ComID);
        Parameters.Add("@UserID", changePassword.UserID);
        Parameters.Add("@Auth", changePassword.AuthCode);
        Parameters.Add("@OldPwd", changePassword.OldPass);
        Parameters.Add("@NewPwd", changePassword.NewPass);

        var data = DbHelper.RunProc<dynamic>(sql, Parameters);


        if (data.Any() && data.FirstOrDefault().Message == null)
        {
            res.StatusCode = 200;
            res.Message = "Success";
            res.ChangePasswordList = data.ToList();
        }
        else if (data.Count() == 1 && data.FirstOrDefault().Message != null)
        {
            res.StatusCode = data.FirstOrDefault().StatusCode;
            res.Message = data.FirstOrDefault().Message;
            res.ChangePasswordList = null;
        }
        else
        {
            res.StatusCode = 400;
            res.Message = "No Data";
            res.ChangePasswordList = null;
        }

        return res;
    }
}