using Dapper;

public interface ISession
{
    Task<SessionRes> SessionAsync(Session session);
}