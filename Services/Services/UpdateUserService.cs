using Dapper;
using System.Data;
using Microsoft.AspNetCore.Mvc;
using System.Drawing.Imaging;
public class UpdateUserService : IUpdateUser
{
    private readonly DapperContext _dapperContext;

    public UpdateUserService(DapperContext dapperContext)
    {
        _dapperContext = dapperContext;
    }

    public async Task<UpdateUserRes> UpdateUserAsync(UpdateUser updateUser)
    {
        using var connection = _dapperContext.CreateConnection();
        var sql = "spUpdateUserProfile"; // Stored procedure name
        var res = new UpdateUserRes();
        var Parameters = new DynamicParameters();

        Parameters.Add("@ComID", updateUser.ComID);
        Parameters.Add("@UserID", updateUser.UserID);
        Parameters.Add("@AuthCode", updateUser.AuthCode);
        Parameters.Add("@Device", updateUser.Device);
        Parameters.Add("@FullName", updateUser.FullName);
        Parameters.Add("@Address", updateUser.Address);
        Parameters.Add("@Phone", updateUser.Phone);
        Parameters.Add("@Mobile", updateUser.Mobile);

        var data = DbHelper.RunProc<dynamic>(sql, Parameters);


        if (data.Any() && data.FirstOrDefault().Message == null)
        {
            res.StatusCode = 200;
            res.Message = "Success";
            res.UpdateUserList = data.ToList();
        }
        else if (data.Count() == 1 && data.FirstOrDefault().Message != null)
        {
            res.StatusCode = data.FirstOrDefault().StatusCode;
            res.Message = data.FirstOrDefault().Message;
            res.UpdateUserList = null;
        }
        else
        {
            res.StatusCode = 400;
            res.Message = "No Data";
            res.UpdateUserList = null;
        }

        return res;
    }
}