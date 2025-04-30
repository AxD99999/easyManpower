using Dapper;
using System.Data;
using Microsoft.AspNetCore.Mvc;
public class SessionService : ISession
{
    private readonly DapperContext _dapperContext;

    public SessionService(DapperContext dapperContext)
    {
        _dapperContext = dapperContext;
    }

    public async Task<SessionRes> SessionAsync(Session session)
    {
        using var connection = _dapperContext.CreateConnection();
        var sql = "spSession"; // Stored procedure name
        var res = new SessionRes();
        var Parameters = new DynamicParameters();

        Parameters.Add("@ComID", session.ComID);
        Parameters.Add("@UserID", session.UserID);
        Parameters.Add("@AuthCode", session.AuthCode);
        Parameters.Add("@Device", session.Device);

        var data = DbHelper.RunProc<dynamic>(sql, Parameters);


        if (data.Any() && data.FirstOrDefault().Message == null)
        {
            res.StatusCode = 200;
            res.Message = "Success";
            res.SessionList = data.ToList();
        }
        else if (data.Count() == 1 && data.FirstOrDefault().Message != null)
        {
            res.StatusCode = data.FirstOrDefault().StatusCode;
            res.Message = data.FirstOrDefault().Message;
            res.SessionList = null;
        }
        else
        {
            res.StatusCode = 400;
            res.Message = "No Data";
            res.SessionList = null;
        }

        return res;
    }
}