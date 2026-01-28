using VibePontaj.Data.Repositories;
using VibePontaj.Models;

namespace VibePontaj.Services;

public interface ILeaveService
{
    Task<int> CreateLeaveRequestAsync(Leave leave);
    Task ApproveLeaveAsync(int leaveId, int approvedBy);
    Task RejectLeaveAsync(int leaveId, int rejectedBy, string reason);
}

public class LeaveService : ILeaveService
{
    private readonly ILeaveRepository _leaveRepository;
    private readonly IAttendanceRepository _attendanceRepository;

    public LeaveService(ILeaveRepository leaveRepository, IAttendanceRepository attendanceRepository)
    {
        _leaveRepository = leaveRepository;
        _attendanceRepository = attendanceRepository;
    }

    public async Task<int> CreateLeaveRequestAsync(Leave leave)
    {
        leave.Status = "Pending";
        leave.CreatedAt = DateTime.UtcNow;
        return await _leaveRepository.CreateAsync(leave);
    }

    public async Task ApproveLeaveAsync(int leaveId, int approvedBy)
    {
        var leave = await _leaveRepository.GetByIdAsync(leaveId);
        if (leave == null)
            throw new Exception("Leave request not found");

        // Update leave status
        leave.Status = "Approved";
        leave.ApprovedBy = approvedBy;
        leave.ApprovedAt = DateTime.UtcNow;
        leave.UpdatedAt = DateTime.UtcNow;
        await _leaveRepository.UpdateAsync(leave);

        // Update attendance for the leave period
        var leaveTypeCode = leave.LeaveType switch
        {
            "Paid" => "PL",
            "Sick" => "SC",
            "Unpaid" => "UL",
            _ => "AL"
        };

        for (var date = leave.StartDate; date <= leave.EndDate; date = date.AddDays(1))
        {
            // Skip weekends
            if (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday)
                continue;

            var existingAttendance = await _attendanceRepository.GetByEmployeeIdAndDateAsync(leave.EmployeeId, date);
            
            if (existingAttendance != null)
            {
                // Update existing attendance
                existingAttendance.Status = leaveTypeCode;
                existingAttendance.Notes = $"Leave: {leave.LeaveType} - {leave.Reason}";
                existingAttendance.UpdatedAt = DateTime.UtcNow;
                await _attendanceRepository.UpdateAsync(existingAttendance);
            }
            else
            {
                // Create new attendance record
                var attendance = new Attendance
                {
                    EmployeeId = leave.EmployeeId,
                    Date = date,
                    Status = leaveTypeCode,
                    Notes = $"Leave: {leave.LeaveType} - {leave.Reason}",
                    CreatedAt = DateTime.UtcNow
                };
                await _attendanceRepository.CreateAsync(attendance);
            }
        }
    }

    public async Task RejectLeaveAsync(int leaveId, int rejectedBy, string reason)
    {
        var leave = await _leaveRepository.GetByIdAsync(leaveId);
        if (leave == null)
            throw new Exception("Leave request not found");

        leave.Status = "Rejected";
        leave.ApprovedBy = rejectedBy;
        leave.ApprovedAt = DateTime.UtcNow;
        leave.RejectionReason = reason;
        leave.UpdatedAt = DateTime.UtcNow;
        await _leaveRepository.UpdateAsync(leave);
    }
}
