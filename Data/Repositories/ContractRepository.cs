using Dapper;
using VibePontaj.Models;

namespace VibePontaj.Data.Repositories;

public class ContractRepository : IContractRepository
{
    private readonly IDbContext _context;

    public ContractRepository(IDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Contract>> GetAllAsync()
    {
        using var connection = _context.CreateConnection();
        var sql = @"SELECT c.*, e.FirstName, e.LastName, e.Email
                    FROM Contracts c
                    INNER JOIN Employees e ON c.EmployeeId = e.Id
                    WHERE c.IsActive = 1
                    ORDER BY c.StartDate DESC";
        return await connection.QueryAsync<Contract>(sql);
    }

    public async Task<IEnumerable<Contract>> GetByEmployeeIdAsync(int employeeId)
    {
        using var connection = _context.CreateConnection();
        return await connection.QueryAsync<Contract>(
            "SELECT * FROM Contracts WHERE EmployeeId = @EmployeeId AND IsActive = 1 ORDER BY StartDate DESC",
            new { EmployeeId = employeeId });
    }

    public async Task<Contract?> GetByIdAsync(int id)
    {
        using var connection = _context.CreateConnection();
        return await connection.QueryFirstOrDefaultAsync<Contract>(
            "SELECT * FROM Contracts WHERE Id = @Id", new { Id = id });
    }

    public async Task<int> CreateAsync(Contract contract)
    {
        using var connection = _context.CreateConnection();
        var sql = @"INSERT INTO Contracts (EmployeeId, ContractType, StartDate, EndDate, HourlyRate, WorkingHoursPerDay, IsActive, CreatedAt)
                    VALUES (@EmployeeId, @ContractType, @StartDate, @EndDate, @HourlyRate, @WorkingHoursPerDay, @IsActive, @CreatedAt);
                    SELECT CAST(SCOPE_IDENTITY() as int)";
        return await connection.ExecuteScalarAsync<int>(sql, contract);
    }

    public async Task UpdateAsync(Contract contract)
    {
        using var connection = _context.CreateConnection();
        var sql = @"UPDATE Contracts 
                    SET ContractType = @ContractType, StartDate = @StartDate, EndDate = @EndDate,
                        HourlyRate = @HourlyRate, WorkingHoursPerDay = @WorkingHoursPerDay, 
                        IsActive = @IsActive, UpdatedAt = @UpdatedAt
                    WHERE Id = @Id";
        await connection.ExecuteAsync(sql, contract);
    }

    public async Task DeleteAsync(int id)
    {
        using var connection = _context.CreateConnection();
        await connection.ExecuteAsync(
            "UPDATE Contracts SET IsActive = 0, UpdatedAt = @UpdatedAt WHERE Id = @Id",
            new { Id = id, UpdatedAt = DateTime.UtcNow });
    }
}
