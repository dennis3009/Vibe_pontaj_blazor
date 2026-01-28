using VibePontaj.Models;

namespace VibePontaj.Data.Repositories;

public interface ILeaveRepository
{
    Task<IEnumerable<Leave>> GetAllAsync();
    Task<IEnumerable<Leave>> GetByEmployeeIdAsync(int employeeId);
    Task<IEnumerable<Leave>> GetPendingAsync();
    Task<Leave?> GetByIdAsync(int id);
    Task<int> CreateAsync(Leave leave);
    Task UpdateAsync(Leave leave);
    Task DeleteAsync(int id);
}
