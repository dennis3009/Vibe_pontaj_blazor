namespace VibePontaj.Models;

public class Attendance
{
    public int Id { get; set; }
    public int EmployeeId { get; set; }
    public DateTime Date { get; set; }
    public string Status { get; set; } = "WFO"; // WFO (Work From Office), WFH (Work From Home), TB (Travel/Business), SC (Sick), PL (Paid Leave), UL (Unpaid Leave), AL (Absent/Leave)
    public string? Notes { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    
    // Navigation property
    public Employee? Employee { get; set; }
}
