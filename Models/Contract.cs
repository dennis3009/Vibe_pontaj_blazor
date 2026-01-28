namespace VibePontaj.Models;

public class Contract
{
    public int Id { get; set; }
    public int EmployeeId { get; set; }
    public string ContractType { get; set; } = string.Empty; // Full-time, Part-time, Contract
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public decimal HourlyRate { get; set; }
    public int WorkingHoursPerDay { get; set; } = 8;
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    
    // Navigation property
    public Employee? Employee { get; set; }
}
