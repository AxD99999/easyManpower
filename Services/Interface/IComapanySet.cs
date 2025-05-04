using Dapper;

public interface ICompanySet
{
    Task<CompanySetRes> CompanySetAsync(CompanySet companySet);
}