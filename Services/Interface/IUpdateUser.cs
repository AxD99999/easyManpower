using Dapper;

public interface IUpdateUser
{
    Task<UpdateUserRes> UpdateUserAsync(UpdateUser updateUser);
}