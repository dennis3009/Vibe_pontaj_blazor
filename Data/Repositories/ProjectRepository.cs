using Dapper;
using VibePontaj.Models;

namespace VibePontaj.Data.Repositories;

public class ProjectRepository : IProjectRepository
{
    private readonly IDbContext _context;

    public ProjectRepository(IDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Project>> GetAllAsync()
    {
        using var connection = _context.CreateConnection();
        return await connection.QueryAsync<Project>(
            "SELECT * FROM Projects WHERE IsActive = 1 ORDER BY Name");
    }

    public async Task<Project?> GetByIdAsync(int id)
    {
        using var connection = _context.CreateConnection();
        return await connection.QueryFirstOrDefaultAsync<Project>(
            "SELECT * FROM Projects WHERE Id = @Id", new { Id = id });
    }

    public async Task<int> CreateAsync(Project project)
    {
        using var connection = _context.CreateConnection();
        var sql = @"INSERT INTO Projects (Name, Description, ProjectCode, StartDate, EndDate, IsActive, CreatedAt)
                    VALUES (@Name, @Description, @ProjectCode, @StartDate, @EndDate, @IsActive, @CreatedAt);
                    SELECT CAST(SCOPE_IDENTITY() as int)";
        return await connection.ExecuteScalarAsync<int>(sql, project);
    }

    public async Task UpdateAsync(Project project)
    {
        using var connection = _context.CreateConnection();
        var sql = @"UPDATE Projects 
                    SET Name = @Name, Description = @Description, ProjectCode = @ProjectCode,
                        StartDate = @StartDate, EndDate = @EndDate, IsActive = @IsActive, UpdatedAt = @UpdatedAt
                    WHERE Id = @Id";
        await connection.ExecuteAsync(sql, project);
    }

    public async Task DeleteAsync(int id)
    {
        using var connection = _context.CreateConnection();
        await connection.ExecuteAsync(
            "UPDATE Projects SET IsActive = 0, UpdatedAt = @UpdatedAt WHERE Id = @Id",
            new { Id = id, UpdatedAt = DateTime.UtcNow });
    }
}
