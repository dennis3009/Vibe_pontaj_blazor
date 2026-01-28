using Dapper;
using VibePontaj.Models;

namespace VibePontaj.Data.Repositories;

public class ProjectAllocationRepository : IProjectAllocationRepository
{
    private readonly IDbContext _context;

    public ProjectAllocationRepository(IDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<ProjectAllocation>> GetAllAsync()
    {
        using var connection = _context.CreateConnection();
        var sql = @"SELECT pa.*, e.Id, e.FirstName, e.LastName, e.Email, e.Role, 
                           p.Id, p.Name, p.ProjectCode, p.Description
                    FROM ProjectAllocations pa
                    INNER JOIN Employees e ON pa.EmployeeId = e.Id
                    INNER JOIN Projects p ON pa.ProjectId = p.Id
                    WHERE pa.IsActive = 1
                    ORDER BY pa.StartDate DESC";
        return await connection.QueryAsync<ProjectAllocation, Employee, Project, ProjectAllocation>(
            sql,
            (allocation, employee, project) =>
            {
                allocation.Employee = employee;
                allocation.Project = project;
                return allocation;
            },
            splitOn: "Id,Id");
    }

    public async Task<IEnumerable<ProjectAllocation>> GetByEmployeeIdAsync(int employeeId)
    {
        using var connection = _context.CreateConnection();
        var sql = @"SELECT pa.*, p.Name as ProjectName, p.ProjectCode
                    FROM ProjectAllocations pa
                    INNER JOIN Projects p ON pa.ProjectId = p.Id
                    WHERE pa.EmployeeId = @EmployeeId AND pa.IsActive = 1
                    ORDER BY pa.StartDate DESC";
        return await connection.QueryAsync<ProjectAllocation>(sql, new { EmployeeId = employeeId });
    }

    public async Task<IEnumerable<ProjectAllocation>> GetByProjectIdAsync(int projectId)
    {
        using var connection = _context.CreateConnection();
        var sql = @"SELECT pa.*, e.FirstName, e.LastName, e.Email
                    FROM ProjectAllocations pa
                    INNER JOIN Employees e ON pa.EmployeeId = e.Id
                    WHERE pa.ProjectId = @ProjectId AND pa.IsActive = 1
                    ORDER BY pa.StartDate DESC";
        return await connection.QueryAsync<ProjectAllocation>(sql, new { ProjectId = projectId });
    }

    public async Task<ProjectAllocation?> GetByIdAsync(int id)
    {
        using var connection = _context.CreateConnection();
        return await connection.QueryFirstOrDefaultAsync<ProjectAllocation>(
            "SELECT * FROM ProjectAllocations WHERE Id = @Id", new { Id = id });
    }

    public async Task<int> CreateAsync(ProjectAllocation allocation)
    {
        using var connection = _context.CreateConnection();
        var sql = @"INSERT INTO ProjectAllocations (EmployeeId, ProjectId, StartDate, EndDate, IsActive, CreatedAt)
                    VALUES (@EmployeeId, @ProjectId, @StartDate, @EndDate, @IsActive, @CreatedAt);
                    SELECT CAST(SCOPE_IDENTITY() as int)";
        return await connection.ExecuteScalarAsync<int>(sql, allocation);
    }

    public async Task UpdateAsync(ProjectAllocation allocation)
    {
        using var connection = _context.CreateConnection();
        var sql = @"UPDATE ProjectAllocations 
                    SET EmployeeId = @EmployeeId, ProjectId = @ProjectId, 
                        StartDate = @StartDate, EndDate = @EndDate, IsActive = @IsActive
                    WHERE Id = @Id";
        await connection.ExecuteAsync(sql, allocation);
    }

    public async Task DeleteAsync(int id)
    {
        using var connection = _context.CreateConnection();
        await connection.ExecuteAsync(
            "UPDATE ProjectAllocations SET IsActive = 0 WHERE Id = @Id",
            new { Id = id });
    }
}
