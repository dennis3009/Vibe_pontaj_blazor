namespace VibePontaj.Models;

public class Timesheet
{
    public int Id { get; set; }
    public int EmployeeId { get; set; }
    public int ProjectId { get; set; }
    public DateTime Date { get; set; }
    public decimal Hours { get; set; }
    public string Description { get; set; } = string.Empty;
    public string Status { get; set; } = "Draft"; // Draft, Submitted, Approved, Rejected
    public bool IsLocked { get; set; } = false;
    public int? ApprovedBy { get; set; }
    public DateTime? ApprovedAt { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    
    // Navigation properties
    public Employee? Employee { get; set; }
    public Project? Project { get; set; }
    public Employee? Approver { get; set; }
    
    // Calculated property for week number
    public int WeekNumber => GetWeekNumber(Date);
    public int Year => Date.Year;
    
    private static int GetWeekNumber(DateTime date)
    {
        var culture = System.Globalization.CultureInfo.CurrentCulture;
        return culture.Calendar.GetWeekOfYear(date, 
            System.Globalization.CalendarWeekRule.FirstDay, DayOfWeek.Monday);
    }
}
