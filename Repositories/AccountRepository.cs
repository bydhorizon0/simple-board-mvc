using Dapper;
using SimpleFreeBoard.Contexts;
using SimpleFreeBoard.Dto;
using SimpleFreeBoard.Models;
using SimpleFreeBoard.Resources;
using SimpleFreeBoard.Services.Security;

namespace SimpleFreeBoard.Repositories;

public class AccountRepository : BaseRepository<AccountRepository>
{
    private readonly PasswordHasher _passwordHasher;

    public AccountRepository(ILogger<AccountRepository> logger, DapperContext context, PasswordHasher passwordHasher) :
        base(logger, context)
    {
        _passwordHasher = passwordHasher;
    }

    public async Task InsertUserAsync(UserRegisterDto dto)
    {
        await ExecuteWithConnectionAsync(conn => conn.ExecuteAsync(AccountQuries.InsertUser, new
        {
            Email = dto.Email,
            Nickname = dto.Nickname,
            Password = _passwordHasher.HashPassword(dto.Password),
            Role = Role.USER.ToString(),
        }));
    }

    public async Task<bool> ExistsUserAsync(string email)
        => await ExecuteWithConnectionAsync(conn =>
            conn.ExecuteScalarAsync<bool>(AccountQuries.ExistsUser, new { Email = email }));

    public async Task<User?> FindUserByEmailAsync(string email)
        => await ExecuteWithConnectionAsync(conn =>
            conn.QueryFirstOrDefaultAsync<User>(AccountQuries.SelectUser, new { Email = email }));
}