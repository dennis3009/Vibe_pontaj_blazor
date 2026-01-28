using Dapper;
using VibePontaj.Models;

namespace VibePontaj.Data.Repositories;

public class EmployeeRepository : IEmployeeRepository
{
    private readonly IDbContext _context;

    public EmployeeRepository(IDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Employee>> GetAllAsync()
    {
        using var connection = _context.CreateConnection();
        return await connection.QueryAsync<Employee>(
            "SELECT * FROM Employees WHERE IsActive = 1 ORDER BY FirstName, LastName");
    }

    public async Task<Employee?> GetByIdAsync(int id)
    {
        using var connection = _context.CreateConnection();
        return await connection.QueryFirstOrDefaultAsync<Employee>(
            "SELECT * FROM Employees WHERE Id = @Id", new { Id = id });
    }

    public async Task<Employee?> GetByEmailAsync(string email)
    {
        using var connection = _context.CreateConnection();
        return await connection.QueryFirstOrDefaultAsync<Employee>(
            "SELECT * FROM Employees WHERE Email = @Email", new { Email = email });
    }

    public async Task<int> CreateAsync(Employee employee)
    {
        using var connection = _context.CreateConnection();
        var sql = @"INSERT INTO Employees (FirstName, LastName, Email, PasswordHash, Role, HireDate, IsActive, CreatedAt)
                    VALUES (@FirstName, @LastName, @Email, @PasswordHash, @Role, @HireDate, @IsActive, @CreatedAt);
                    SELECT CAST(SCOPE_IDENTITY() as int)";
        return await connection.ExecuteScalarAsync<int>(sql, employee);
    }

    public async Task UpdateAsync(Employee employee)
    {
        using var connection = _context.CreateConnection();
        var sql = @"UPDATE Employees 
                    SET FirstName = @FirstName, LastName = @LastName, Email = @Email, 
                        Role = @Role, HireDate = @HireDate, IsActive = @IsActive, UpdatedAt = @UpdatedAt
                    WHERE Id = @Id";
        await connection.ExecuteAsync(sql, employee);
    }

    public async Task DeleteAsync(int id)
    {
        using var connection = _context.CreateConnection();
        await connection.ExecuteAsync(
            "UPDATE Employees SET IsActive = 0, UpdatedAt = @UpdatedAt WHERE Id = @Id",
            new { Id = id, UpdatedAt = DateTime.UtcNow });
    }
}
