using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;

namespace VibePontaj.Data;

public interface IDbContext
{
    IDbConnection CreateConnection();
    Task InitializeDatabaseAsync();
}

public class DbContext : IDbContext
{
    private readonly string _connectionString;

    public DbContext(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection") 
            ?? throw new ArgumentNullException(nameof(configuration), "Connection string 'DefaultConnection' not found.");
    }

    public IDbConnection CreateConnection()
    {
        return new SqlConnection(_connectionString);
    }

    public async Task InitializeDatabaseAsync()
    {
        var masterConnectionString = _connectionString.Replace("Database=VibePontaj;", "Database=master;");
        
        using var connection = new SqlConnection(masterConnectionString);
        await connection.OpenAsync();
        
        // Check if database exists
        var checkDbSql = "SELECT COUNT(*) FROM sys.databases WHERE name = 'VibePontaj'";
        var dbExists = await connection.ExecuteScalarAsync<int>(checkDbSql);
        
        if (dbExists == 0)
        {
            await connection.ExecuteAsync("CREATE DATABASE VibePontaj");
        }
    }
}
