using Dapper;
using VibePontaj.Models;

namespace VibePontaj.Data.Repositories;

public class TimesheetRepository : ITimesheetRepository
{
    private readonly IDbContext _context;

    public TimesheetRepository(IDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Timesheet>> GetByEmployeeIdAsync(int employeeId)
    {
        using var connection = _context.CreateConnection();
        var sql = @"SELECT t.*, e.FirstName, e.LastName, p.Name as ProjectName
                    FROM Timesheets t
                    INNER JOIN Employees e ON t.EmployeeId = e.Id
                    INNER JOIN Projects p ON t.ProjectId = p.Id
                    WHERE t.EmployeeId = @EmployeeId
                    ORDER BY t.Date DESC";
        return await connection.QueryAsync<Timesheet>(sql, new { EmployeeId = employeeId });
    }

    public async Task<IEnumerable<Timesheet>> GetByEmployeeIdAndDateRangeAsync(int employeeId, DateTime startDate, DateTime endDate)
    {
        using var connection = _context.CreateConnection();
        var sql = @"SELECT t.*, e.FirstName, e.LastName, e.Email, p.Name as ProjectName, p.ProjectCode
                    FROM Timesheets t
                    INNER JOIN Employees e ON t.EmployeeId = e.Id
                    INNER JOIN Projects p ON t.ProjectId = p.Id
                    WHERE t.EmployeeId = @EmployeeId AND t.Date >= @StartDate AND t.Date <= @EndDate
                    ORDER BY t.Date, p.Name";
        return await connection.QueryAsync<Timesheet>(sql, new { EmployeeId = employeeId, StartDate = startDate, EndDate = endDate });
    }

    public async Task<IEnumerable<Timesheet>> GetByWeekAsync(int employeeId, int year, int weekNumber)
    {
        using var connection = _context.CreateConnection();
        // Calculate date range for the week
        var firstDayOfYear = new DateTime(year, 1, 1);
        var daysToAdd = (weekNumber - 1) * 7;
        var startDate = firstDayOfYear.AddDays(daysToAdd);
        var endDate = startDate.AddDays(6);
        
        return await GetByEmployeeIdAndDateRangeAsync(employeeId, startDate, endDate);
    }

    public async Task<Timesheet?> GetByIdAsync(int id)
    {
        using var connection = _context.CreateConnection();
        return await connection.QueryFirstOrDefaultAsync<Timesheet>(
            "SELECT * FROM Timesheets WHERE Id = @Id", new { Id = id });
    }

    public async Task<int> CreateAsync(Timesheet timesheet)
    {
        using var connection = _context.CreateConnection();
        var sql = @"INSERT INTO Timesheets (EmployeeId, ProjectId, Date, Hours, Description, Status, IsLocked, CreatedAt)
                    VALUES (@EmployeeId, @ProjectId, @Date, @Hours, @Description, @Status, @IsLocked, @CreatedAt);
                    SELECT CAST(SCOPE_IDENTITY() as int)";
        return await connection.ExecuteScalarAsync<int>(sql, timesheet);
    }

    public async Task UpdateAsync(Timesheet timesheet)
    {
        using var connection = _context.CreateConnection();
        var sql = @"UPDATE Timesheets 
                    SET ProjectId = @ProjectId, Date = @Date, Hours = @Hours, Description = @Description,
                        Status = @Status, IsLocked = @IsLocked, ApprovedBy = @ApprovedBy, ApprovedAt = @ApprovedAt, UpdatedAt = @UpdatedAt
                    WHERE Id = @Id";
        await connection.ExecuteAsync(sql, timesheet);
    }

    public async Task DeleteAsync(int id)
    {
        using var connection = _context.CreateConnection();
        await connection.ExecuteAsync("DELETE FROM Timesheets WHERE Id = @Id", new { Id = id });
    }

    public async Task LockWeekAsync(int employeeId, int year, int weekNumber)
    {
        using var connection = _context.CreateConnection();
        var firstDayOfYear = new DateTime(year, 1, 1);
        var daysToAdd = (weekNumber - 1) * 7;
        var startDate = firstDayOfYear.AddDays(daysToAdd);
        var endDate = startDate.AddDays(6);
        
        var sql = @"UPDATE Timesheets 
                    SET IsLocked = 1, UpdatedAt = @UpdatedAt
                    WHERE EmployeeId = @EmployeeId AND Date >= @StartDate AND Date <= @EndDate";
        await connection.ExecuteAsync(sql, new { EmployeeId = employeeId, StartDate = startDate, EndDate = endDate, UpdatedAt = DateTime.UtcNow });
    }

    public async Task UnlockWeekAsync(int employeeId, int year, int weekNumber)
    {
        using var connection = _context.CreateConnection();
        var firstDayOfYear = new DateTime(year, 1, 1);
        var daysToAdd = (weekNumber - 1) * 7;
        var startDate = firstDayOfYear.AddDays(daysToAdd);
        var endDate = startDate.AddDays(6);
        
        var sql = @"UPDATE Timesheets 
                    SET IsLocked = 0, UpdatedAt = @UpdatedAt
                    WHERE EmployeeId = @EmployeeId AND Date >= @StartDate AND Date <= @EndDate";
        await connection.ExecuteAsync(sql, new { EmployeeId = employeeId, StartDate = startDate, EndDate = endDate, UpdatedAt = DateTime.UtcNow });
    }

    public async Task<IEnumerable<Timesheet>> GetPendingApprovalsAsync()
    {
        using var connection = _context.CreateConnection();
        var sql = @"SELECT t.*, e.FirstName, e.LastName, e.Email, p.Name as ProjectName, p.ProjectCode
                    FROM Timesheets t
                    INNER JOIN Employees e ON t.EmployeeId = e.Id
                    INNER JOIN Projects p ON t.ProjectId = p.Id
                    WHERE t.Status = 'Submitted'
                    ORDER BY t.Date DESC";
        return await connection.QueryAsync<Timesheet>(sql);
    }
}
