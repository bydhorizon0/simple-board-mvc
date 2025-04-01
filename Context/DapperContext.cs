using System.Data;
using Dapper;
using MySql.Data.MySqlClient;

namespace SimpleFreeBoard.Contexts;

public class DapperContext
{
    private readonly IConfiguration _configuration;
    private readonly string _connectionString;

    public DapperContext(IConfiguration configuration)
    {
        _configuration = configuration;
        _connectionString = configuration.GetConnectionString("DefaultConnection") ?? throw new ArgumentNullException($"DefaultConnection string not found");
        DefaultTypeMap.MatchNamesWithUnderscores = true;
    }
    
    public IDbConnection GetConnection() => new MySqlConnection(_connectionString);
}