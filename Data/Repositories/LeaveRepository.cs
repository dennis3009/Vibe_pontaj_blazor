using Dapper;
using VibePontaj.Models;

namespace VibePontaj.Data.Repositories;

public class LeaveRepository : ILeaveRepository
{
    private readonly IDbContext _context;

    public LeaveRepository(IDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Leave>> GetAllAsync()
    {
        using var connection = _context.CreateConnection();
        var sql = @"SELECT l.*, e.FirstName, e.LastName, e.Email
                    FROM Leaves l
                    INNER JOIN Employees e ON l.EmployeeId = e.Id
                    ORDER BY l.CreatedAt DESC";
        return await connection.QueryAsync<Leave>(sql);
    }

    public async Task<IEnumerable<Leave>> GetByEmployeeIdAsync(int employeeId)
    {
        using var connection = _context.CreateConnection();
        return await connection.QueryAsync<Leave>(
            "SELECT * FROM Leaves WHERE EmployeeId = @EmployeeId ORDER BY StartDate DESC",
            new { EmployeeId = employeeId });
    }

    public async Task<IEnumerable<Leave>> GetPendingAsync()
    {
        using var connection = _context.CreateConnection();
        var sql = @"SELECT l.*, e.FirstName, e.LastName, e.Email
                    FROM Leaves l
                    INNER JOIN Employees e ON l.EmployeeId = e.Id
                    WHERE l.Status = 'Pending'
                    ORDER BY l.StartDate";
        return await connection.QueryAsync<Leave>(sql);
    }

    public async Task<Leave?> GetByIdAsync(int id)
    {
        using var connection = _context.CreateConnection();
        return await connection.QueryFirstOrDefaultAsync<Leave>(
            "SELECT * FROM Leaves WHERE Id = @Id", new { Id = id });
    }

    public async Task<int> CreateAsync(Leave leave)
    {
        using var connection = _context.CreateConnection();
        var sql = @"INSERT INTO Leaves (EmployeeId, LeaveType, StartDate, EndDate, Reason, Status, CreatedAt)
                    VALUES (@EmployeeId, @LeaveType, @StartDate, @EndDate, @Reason, @Status, @CreatedAt);
                    SELECT CAST(SCOPE_IDENTITY() as int)";
        return await connection.ExecuteScalarAsync<int>(sql, leave);
    }

    public async Task UpdateAsync(Leave leave)
    {
        using var connection = _context.CreateConnection();
        var sql = @"UPDATE Leaves 
                    SET LeaveType = @LeaveType, StartDate = @StartDate, EndDate = @EndDate, 
                        Reason = @Reason, Status = @Status, ApprovedBy = @ApprovedBy, 
                        ApprovedAt = @ApprovedAt, RejectionReason = @RejectionReason, UpdatedAt = @UpdatedAt
                    WHERE Id = @Id";
        await connection.ExecuteAsync(sql, leave);
    }

    public async Task DeleteAsync(int id)
    {
        using var connection = _context.CreateConnection();
        await connection.ExecuteAsync("DELETE FROM Leaves WHERE Id = @Id", new { Id = id });
    }
}
