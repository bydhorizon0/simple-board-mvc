using System.Data;
using SimpleFreeBoard.Contexts;

namespace SimpleFreeBoard.Repositories;

public abstract class BaseRepository<L>
{
    protected readonly ILogger<L> _logger;
    protected readonly DapperContext _context;

    public BaseRepository(ILogger<L> logger, DapperContext context)
    {
        _logger = logger;
        _context = context;
    }

    protected async Task<T> ExecuteWithConnectionAsync<T>(Func<IDbConnection, Task<T>> action)
    {
        var conn = _context.GetConnection();
        return await action(conn);
    }
    
    protected async Task ExecuteWithConnectionAsync(Func<IDbConnection, Task> action)
    {
        var conn = _context.GetConnection();
        await action(conn);
    }
}