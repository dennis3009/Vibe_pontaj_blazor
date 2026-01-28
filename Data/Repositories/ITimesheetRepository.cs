using VibePontaj.Models;

namespace VibePontaj.Data.Repositories;

public interface ITimesheetRepository
{
    Task<IEnumerable<Timesheet>> GetByEmployeeIdAsync(int employeeId);
    Task<IEnumerable<Timesheet>> GetByEmployeeIdAndDateRangeAsync(int employeeId, DateTime startDate, DateTime endDate);
    Task<IEnumerable<Timesheet>> GetByWeekAsync(int employeeId, int year, int weekNumber);
    Task<Timesheet?> GetByIdAsync(int id);
    Task<int> CreateAsync(Timesheet timesheet);
    Task UpdateAsync(Timesheet timesheet);
    Task DeleteAsync(int id);
    Task LockWeekAsync(int employeeId, int year, int weekNumber);
    Task UnlockWeekAsync(int employeeId, int year, int weekNumber);
    Task<IEnumerable<Timesheet>> GetPendingApprovalsAsync();
}
