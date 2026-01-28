using Dapper;
using VibePontaj.Models;

namespace VibePontaj.Data.Repositories;

public class AttendanceRepository : IAttendanceRepository
{
    private readonly IDbContext _context;

    public AttendanceRepository(IDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Attendance>> GetByEmployeeIdAsync(int employeeId)
    {
        using var connection = _context.CreateConnection();
        return await connection.QueryAsync<Attendance>(
            "SELECT * FROM Attendance WHERE EmployeeId = @EmployeeId ORDER BY Date DESC",
            new { EmployeeId = employeeId });
    }

    public async Task<IEnumerable<Attendance>> GetByEmployeeIdAndMonthAsync(int employeeId, int year, int month)
    {
        using var connection = _context.CreateConnection();
        var startDate = new DateTime(year, month, 1);
        var endDate = startDate.AddMonths(1).AddDays(-1);
        
        var sql = @"SELECT * FROM Attendance 
                    WHERE EmployeeId = @EmployeeId AND Date >= @StartDate AND Date <= @EndDate
                    ORDER BY Date";
        return await connection.QueryAsync<Attendance>(sql, 
            new { EmployeeId = employeeId, StartDate = startDate, EndDate = endDate });
    }

    public async Task<Attendance?> GetByIdAsync(int id)
    {
        using var connection = _context.CreateConnection();
        return await connection.QueryFirstOrDefaultAsync<Attendance>(
            "SELECT * FROM Attendance WHERE Id = @Id", new { Id = id });
    }

    public async Task<Attendance?> GetByEmployeeIdAndDateAsync(int employeeId, DateTime date)
    {
        using var connection = _context.CreateConnection();
        return await connection.QueryFirstOrDefaultAsync<Attendance>(
            "SELECT * FROM Attendance WHERE EmployeeId = @EmployeeId AND Date = @Date",
            new { EmployeeId = employeeId, Date = date.Date });
    }

    public async Task<int> CreateAsync(Attendance attendance)
    {
        using var connection = _context.CreateConnection();
        var sql = @"INSERT INTO Attendance (EmployeeId, Date, Status, Notes, CreatedAt)
                    VALUES (@EmployeeId, @Date, @Status, @Notes, @CreatedAt);
                    SELECT CAST(SCOPE_IDENTITY() as int)";
        return await connection.ExecuteScalarAsync<int>(sql, attendance);
    }

    public async Task UpdateAsync(Attendance attendance)
    {
        using var connection = _context.CreateConnection();
        var sql = @"UPDATE Attendance 
                    SET Status = @Status, Notes = @Notes, UpdatedAt = @UpdatedAt
                    WHERE Id = @Id";
        await connection.ExecuteAsync(sql, attendance);
    }

    public async Task DeleteAsync(int id)
    {
        using var connection = _context.CreateConnection();
        await connection.ExecuteAsync("DELETE FROM Attendance WHERE Id = @Id", new { Id = id });
    }
}
