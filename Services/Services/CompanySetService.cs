using Dapper;
using System.Data;
using Microsoft.AspNetCore.Mvc;
public class CompanySetService : ICompanySet
{
    private readonly DapperContext _dapperContext;

    public CompanySetService(DapperContext dapperContext)
    {
        _dapperContext = dapperContext;
    }

    public async Task<CompanySetRes> CompanySetAsync(CompanySet companySet)
    {
        using var connection = _dapperContext.CreateConnection();
        var sql = "spCompanySet"; // Stored procedure name
        var res = new CompanySetRes();
        var Parameters = new DynamicParameters();

        Parameters.Add("@UserID", companySet.UserID);
        Parameters.Add("@AuthCode", companySet.AuthCode);
        Parameters.Add("@Device", companySet.Device);
        Parameters.Add("@ID", companySet.ID);
        Parameters.Add("@Company", companySet.Company);
        Parameters.Add("@Cell", companySet.Cell);
        Parameters.Add("@Mail", companySet.Mail);
        Parameters.Add("@City", companySet.City);
        Parameters.Add("@Contact", companySet.Contact);
        Parameters.Add("@Owner", companySet.Owner);
        Parameters.Add("@Address", companySet.Address);
        Parameters.Add("@CR", companySet.CR);
        Parameters.Add("@Location", companySet.Location);
        Parameters.Add("@Status", companySet.Status);
        Parameters.Add("@Flag", companySet.Flag);


        var data = DbHelper.RunProc<dynamic>(sql, Parameters);


        if (data.Any() && data.FirstOrDefault().Message == null)
        {
            res.StatusCode = 200;
            res.Message = "Success";
            res.CompanySetList = data.ToList();
        }
        else if (data.Count() == 1 && data.FirstOrDefault().Message != null)
        {
            res.StatusCode = data.FirstOrDefault().StatusCode;
            res.Message = data.FirstOrDefault().Message;
            res.CompanySetList = null;
        }
        else
        {
            res.StatusCode = 400;
            res.Message = "No Data";
            res.CompanySetList = null;
        }

        return res;
    }
}