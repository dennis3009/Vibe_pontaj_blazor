using VibePontaj.Models;

namespace VibePontaj.Data.Repositories;

public interface IProjectAllocationRepository
{
    Task<IEnumerable<ProjectAllocation>> GetAllAsync();
    Task<IEnumerable<ProjectAllocation>> GetByEmployeeIdAsync(int employeeId);
    Task<IEnumerable<ProjectAllocation>> GetByProjectIdAsync(int projectId);
    Task<ProjectAllocation?> GetByIdAsync(int id);
    Task<int> CreateAsync(ProjectAllocation allocation);
    Task UpdateAsync(ProjectAllocation allocation);
    Task DeleteAsync(int id);
}
