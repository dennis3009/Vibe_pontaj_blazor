namespace VibePontaj.Models;

public class Leave
{
    public int Id { get; set; }
    public int EmployeeId { get; set; }
    public string LeaveType { get; set; } = string.Empty; // Paid, Sick, Unpaid
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string Reason { get; set; } = string.Empty;
    public string Status { get; set; } = "Pending"; // Pending, Approved, Rejected
    public int? ApprovedBy { get; set; }
    public DateTime? ApprovedAt { get; set; }
    public string? RejectionReason { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    
    // Navigation properties
    public Employee? Employee { get; set; }
    public Employee? Approver { get; set; }
    
    // Calculated property
    public int TotalDays => (int)(EndDate - StartDate).TotalDays + 1;
}
