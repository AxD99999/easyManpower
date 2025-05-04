using Dapper;
using System.Data;
using Microsoft.AspNetCore.Mvc;
public class PostSetService : IPostSet
{
    private readonly DapperContext _dapperContext;

    public PostSetService(DapperContext dapperContext)
    {
        _dapperContext = dapperContext;
    }

    public async Task<PostSetRes> PostSetAsync(PostSet postSet)
    {
        using var connection = _dapperContext.CreateConnection();
        var sql = "spPostSetting"; // Stored procedure name
        var res = new PostSetRes();
        var Parameters = new DynamicParameters();

        Parameters.Add("@UserID", postSet.UserID);
        Parameters.Add("@AuthCode", postSet.AuthCode);
        Parameters.Add("@Device", postSet.Device);
        Parameters.Add("@ID", postSet.ID);
        Parameters.Add("@Post", postSet.Post);
        Parameters.Add("@Status", postSet.Status);
        Parameters.Add("@Flag", postSet.Flag);


        var data = DbHelper.RunProc<dynamic>(sql, Parameters);


        if (data.Any() && data.FirstOrDefault().Message == null)
        {
            res.StatusCode = 200;
            res.Message = "Success";
            res.PostSetList = data.ToList();
        }
        else if (data.Count() == 1 && data.FirstOrDefault().Message != null)
        {
            res.StatusCode = data.FirstOrDefault().StatusCode;
            res.Message = data.FirstOrDefault().Message;
            res.PostSetList = null;
        }
        else
        {
            res.StatusCode = 400;
            res.Message = "No Data";
            res.PostSetList = null;
        }

        return res;
    }
}