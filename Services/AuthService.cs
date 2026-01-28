using VibePontaj.Data.Repositories;
using VibePontaj.Models;
using BCrypt.Net;

namespace VibePontaj.Services;

public interface IAuthService
{
    Task<Employee?> AuthenticateAsync(string email, string password);
    Task<Employee?> GetCurrentUserAsync(string email);
    string HashPassword(string password);
    bool VerifyPassword(string password, string hash);
}

public class AuthService : IAuthService
{
    private readonly IEmployeeRepository _employeeRepository;

    public AuthService(IEmployeeRepository employeeRepository)
    {
        _employeeRepository = employeeRepository;
    }

    public async Task<Employee?> AuthenticateAsync(string email, string password)
    {
        var employee = await _employeeRepository.GetByEmailAsync(email);
        
        if (employee == null || !employee.IsActive)
            return null;
            
        if (!VerifyPassword(password, employee.PasswordHash))
            return null;
            
        return employee;
    }

    public async Task<Employee?> GetCurrentUserAsync(string email)
    {
        return await _employeeRepository.GetByEmailAsync(email);
    }

    public string HashPassword(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }

    public bool VerifyPassword(string password, string hash)
    {
        return BCrypt.Net.BCrypt.Verify(password, hash);
    }
}
