using VibePontaj.Models;

namespace VibePontaj.Data.Repositories;

public interface IAttendanceRepository
{
    Task<IEnumerable<Attendance>> GetByEmployeeIdAsync(int employeeId);
    Task<IEnumerable<Attendance>> GetByEmployeeIdAndMonthAsync(int employeeId, int year, int month);
    Task<Attendance?> GetByIdAsync(int id);
    Task<Attendance?> GetByEmployeeIdAndDateAsync(int employeeId, DateTime date);
    Task<int> CreateAsync(Attendance attendance);
    Task UpdateAsync(Attendance attendance);
    Task DeleteAsync(int id);
}
