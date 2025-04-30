// method to initialise sql connection 
using Microsoft.Data.SqlClient;
using System.Data;
using Dapper;
public class DbConn
{
    public static string ConnectionString = "Server=WIN-PLO3PGFAQN8\\SQLEXPRESS;Database=manpowerDB;user id=sa;password=sa@123;MultipleActiveResultSets=true;";
    //public static string ConnectionString = "Server=Sedai;Database=manpowerDB;Trusted_Connection=True;TrustServerCertificate=true;";
}

public class DbHelper
{
    public static SqlConnection GetSql()
    {
        var conn = new SqlConnection(DbConn.ConnectionString);
        conn.Open();
        return conn;
    }
    public static IEnumerable<T> RunProc<T>(string sql, object? parameter = null)
    {
        using (SqlConnection conn = GetSql())
        {
            var data = conn.Query<T>(sql, parameter, commandType: CommandType.StoredProcedure);
            return data;
        }
    }
    public async static Task<IEnumerable<T>> RunQuery<T>(string sql, object? param = null)
    {
        using (var conn = GetSql())
        {
            var data = await conn.QueryAsync<T>(sql, param);
            return data;
        }
    }
}