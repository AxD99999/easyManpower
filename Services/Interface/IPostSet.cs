using Dapper;

public interface IPostSet
{
    Task<PostSetRes> PostSetAsync(PostSet postSet);
}