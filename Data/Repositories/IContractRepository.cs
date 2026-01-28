using VibePontaj.Models;

namespace VibePontaj.Data.Repositories;

public interface IContractRepository
{
    Task<IEnumerable<Contract>> GetAllAsync();
    Task<IEnumerable<Contract>> GetByEmployeeIdAsync(int employeeId);
    Task<Contract?> GetByIdAsync(int id);
    Task<int> CreateAsync(Contract contract);
    Task UpdateAsync(Contract contract);
    Task DeleteAsync(int id);
}
