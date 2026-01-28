using ClosedXML.Excel;
using VibePontaj.Models;

namespace VibePontaj.Services;

public interface IExcelExportService
{
    byte[] ExportEmployees(IEnumerable<Employee> employees);
    byte[] ExportProjects(IEnumerable<Project> projects);
    byte[] ExportTimesheets(IEnumerable<Timesheet> timesheets);
    byte[] ExportAttendance(IEnumerable<Attendance> attendance);
    byte[] ExportLeaves(IEnumerable<Leave> leaves);
    byte[] ExportContracts(IEnumerable<Contract> contracts);
    byte[] ExportProjectAllocations(IEnumerable<ProjectAllocation> allocations);
}

public class ExcelExportService : IExcelExportService
{
    public byte[] ExportEmployees(IEnumerable<Employee> employees)
    {
        using var workbook = new XLWorkbook();
        var worksheet = workbook.Worksheets.Add("Employees");
        
        // Headers
        worksheet.Cell(1, 1).Value = "ID";
        worksheet.Cell(1, 2).Value = "First Name";
        worksheet.Cell(1, 3).Value = "Last Name";
        worksheet.Cell(1, 4).Value = "Email";
        worksheet.Cell(1, 5).Value = "Role";
        worksheet.Cell(1, 6).Value = "Hire Date";
        worksheet.Cell(1, 7).Value = "Active";
        
        // Style headers
        var headerRange = worksheet.Range(1, 1, 1, 7);
        headerRange.Style.Font.Bold = true;
        headerRange.Style.Fill.BackgroundColor = XLColor.LightGray;
        
        // Data
        int row = 2;
        foreach (var emp in employees)
        {
            worksheet.Cell(row, 1).Value = emp.Id;
            worksheet.Cell(row, 2).Value = emp.FirstName;
            worksheet.Cell(row, 3).Value = emp.LastName;
            worksheet.Cell(row, 4).Value = emp.Email;
            worksheet.Cell(row, 5).Value = emp.Role;
            worksheet.Cell(row, 6).Value = emp.HireDate;
            worksheet.Cell(row, 7).Value = emp.IsActive ? "Yes" : "No";
            row++;
        }
        
        worksheet.Columns().AdjustToContents();
        
        using var stream = new MemoryStream();
        workbook.SaveAs(stream);
        return stream.ToArray();
    }

    public byte[] ExportProjects(IEnumerable<Project> projects)
    {
        using var workbook = new XLWorkbook();
        var worksheet = workbook.Worksheets.Add("Projects");
        
        worksheet.Cell(1, 1).Value = "ID";
        worksheet.Cell(1, 2).Value = "Name";
        worksheet.Cell(1, 3).Value = "Code";
        worksheet.Cell(1, 4).Value = "Description";
        worksheet.Cell(1, 5).Value = "Start Date";
        worksheet.Cell(1, 6).Value = "End Date";
        worksheet.Cell(1, 7).Value = "Active";
        
        var headerRange = worksheet.Range(1, 1, 1, 7);
        headerRange.Style.Font.Bold = true;
        headerRange.Style.Fill.BackgroundColor = XLColor.LightGray;
        
        int row = 2;
        foreach (var proj in projects)
        {
            worksheet.Cell(row, 1).Value = proj.Id;
            worksheet.Cell(row, 2).Value = proj.Name;
            worksheet.Cell(row, 3).Value = proj.ProjectCode;
            worksheet.Cell(row, 4).Value = proj.Description;
            worksheet.Cell(row, 5).Value = proj.StartDate;
            worksheet.Cell(row, 6).Value = proj.EndDate?.ToString("yyyy-MM-dd") ?? "";
            worksheet.Cell(row, 7).Value = proj.IsActive ? "Yes" : "No";
            row++;
        }
        
        worksheet.Columns().AdjustToContents();
        
        using var stream = new MemoryStream();
        workbook.SaveAs(stream);
        return stream.ToArray();
    }

    public byte[] ExportTimesheets(IEnumerable<Timesheet> timesheets)
    {
        using var workbook = new XLWorkbook();
        var worksheet = workbook.Worksheets.Add("Timesheets");
        
        worksheet.Cell(1, 1).Value = "ID";
        worksheet.Cell(1, 2).Value = "Employee";
        worksheet.Cell(1, 3).Value = "Project";
        worksheet.Cell(1, 4).Value = "Date";
        worksheet.Cell(1, 5).Value = "Hours";
        worksheet.Cell(1, 6).Value = "Description";
        worksheet.Cell(1, 7).Value = "Status";
        worksheet.Cell(1, 8).Value = "Locked";
        
        var headerRange = worksheet.Range(1, 1, 1, 8);
        headerRange.Style.Font.Bold = true;
        headerRange.Style.Fill.BackgroundColor = XLColor.LightGray;
        
        int row = 2;
        foreach (var ts in timesheets)
        {
            worksheet.Cell(row, 1).Value = ts.Id;
            worksheet.Cell(row, 2).Value = ts.Employee?.FullName ?? "";
            worksheet.Cell(row, 3).Value = ts.Project?.Name ?? "";
            worksheet.Cell(row, 4).Value = ts.Date;
            worksheet.Cell(row, 5).Value = ts.Hours;
            worksheet.Cell(row, 6).Value = ts.Description;
            worksheet.Cell(row, 7).Value = ts.Status;
            worksheet.Cell(row, 8).Value = ts.IsLocked ? "Yes" : "No";
            row++;
        }
        
        worksheet.Columns().AdjustToContents();
        
        using var stream = new MemoryStream();
        workbook.SaveAs(stream);
        return stream.ToArray();
    }

    public byte[] ExportAttendance(IEnumerable<Attendance> attendance)
    {
        using var workbook = new XLWorkbook();
        var worksheet = workbook.Worksheets.Add("Attendance");
        
        worksheet.Cell(1, 1).Value = "ID";
        worksheet.Cell(1, 2).Value = "Employee";
        worksheet.Cell(1, 3).Value = "Date";
        worksheet.Cell(1, 4).Value = "Status";
        worksheet.Cell(1, 5).Value = "Notes";
        
        var headerRange = worksheet.Range(1, 1, 1, 5);
        headerRange.Style.Font.Bold = true;
        headerRange.Style.Fill.BackgroundColor = XLColor.LightGray;
        
        int row = 2;
        foreach (var att in attendance)
        {
            worksheet.Cell(row, 1).Value = att.Id;
            worksheet.Cell(row, 2).Value = att.Employee?.FullName ?? "";
            worksheet.Cell(row, 3).Value = att.Date;
            worksheet.Cell(row, 4).Value = att.Status;
            worksheet.Cell(row, 5).Value = att.Notes ?? "";
            row++;
        }
        
        worksheet.Columns().AdjustToContents();
        
        using var stream = new MemoryStream();
        workbook.SaveAs(stream);
        return stream.ToArray();
    }

    public byte[] ExportLeaves(IEnumerable<Leave> leaves)
    {
        using var workbook = new XLWorkbook();
        var worksheet = workbook.Worksheets.Add("Leaves");
        
        worksheet.Cell(1, 1).Value = "ID";
        worksheet.Cell(1, 2).Value = "Employee";
        worksheet.Cell(1, 3).Value = "Leave Type";
        worksheet.Cell(1, 4).Value = "Start Date";
        worksheet.Cell(1, 5).Value = "End Date";
        worksheet.Cell(1, 6).Value = "Total Days";
        worksheet.Cell(1, 7).Value = "Reason";
        worksheet.Cell(1, 8).Value = "Status";
        
        var headerRange = worksheet.Range(1, 1, 1, 8);
        headerRange.Style.Font.Bold = true;
        headerRange.Style.Fill.BackgroundColor = XLColor.LightGray;
        
        int row = 2;
        foreach (var leave in leaves)
        {
            worksheet.Cell(row, 1).Value = leave.Id;
            worksheet.Cell(row, 2).Value = leave.Employee?.FullName ?? "";
            worksheet.Cell(row, 3).Value = leave.LeaveType;
            worksheet.Cell(row, 4).Value = leave.StartDate;
            worksheet.Cell(row, 5).Value = leave.EndDate;
            worksheet.Cell(row, 6).Value = leave.TotalDays;
            worksheet.Cell(row, 7).Value = leave.Reason;
            worksheet.Cell(row, 8).Value = leave.Status;
            row++;
        }
        
        worksheet.Columns().AdjustToContents();
        
        using var stream = new MemoryStream();
        workbook.SaveAs(stream);
        return stream.ToArray();
    }

    public byte[] ExportContracts(IEnumerable<Contract> contracts)
    {
        using var workbook = new XLWorkbook();
        var worksheet = workbook.Worksheets.Add("Contracts");
        
        worksheet.Cell(1, 1).Value = "ID";
        worksheet.Cell(1, 2).Value = "Employee";
        worksheet.Cell(1, 3).Value = "Contract Type";
        worksheet.Cell(1, 4).Value = "Start Date";
        worksheet.Cell(1, 5).Value = "End Date";
        worksheet.Cell(1, 6).Value = "Hourly Rate";
        worksheet.Cell(1, 7).Value = "Working Hours/Day";
        worksheet.Cell(1, 8).Value = "Active";
        
        var headerRange = worksheet.Range(1, 1, 1, 8);
        headerRange.Style.Font.Bold = true;
        headerRange.Style.Fill.BackgroundColor = XLColor.LightGray;
        
        int row = 2;
        foreach (var contract in contracts)
        {
            worksheet.Cell(row, 1).Value = contract.Id;
            worksheet.Cell(row, 2).Value = contract.Employee?.FullName ?? "";
            worksheet.Cell(row, 3).Value = contract.ContractType;
            worksheet.Cell(row, 4).Value = contract.StartDate;
            worksheet.Cell(row, 5).Value = contract.EndDate?.ToString("yyyy-MM-dd") ?? "";
            worksheet.Cell(row, 6).Value = contract.HourlyRate;
            worksheet.Cell(row, 7).Value = contract.WorkingHoursPerDay;
            worksheet.Cell(row, 8).Value = contract.IsActive ? "Yes" : "No";
            row++;
        }
        
        worksheet.Columns().AdjustToContents();
        
        using var stream = new MemoryStream();
        workbook.SaveAs(stream);
        return stream.ToArray();
    }

    public byte[] ExportProjectAllocations(IEnumerable<ProjectAllocation> allocations)
    {
        using var workbook = new XLWorkbook();
        var worksheet = workbook.Worksheets.Add("Project Allocations");
        
        worksheet.Cell(1, 1).Value = "ID";
        worksheet.Cell(1, 2).Value = "Employee";
        worksheet.Cell(1, 3).Value = "Project";
        worksheet.Cell(1, 4).Value = "Project Code";
        worksheet.Cell(1, 5).Value = "Start Date";
        worksheet.Cell(1, 6).Value = "End Date";
        worksheet.Cell(1, 7).Value = "Active";
        
        var headerRange = worksheet.Range(1, 1, 1, 7);
        headerRange.Style.Font.Bold = true;
        headerRange.Style.Fill.BackgroundColor = XLColor.LightGray;
        
        int row = 2;
        foreach (var allocation in allocations)
        {
            worksheet.Cell(row, 1).Value = allocation.Id;
            worksheet.Cell(row, 2).Value = allocation.Employee?.FullName ?? "";
            worksheet.Cell(row, 3).Value = allocation.Project?.Name ?? "";
            worksheet.Cell(row, 4).Value = allocation.Project?.ProjectCode ?? "";
            worksheet.Cell(row, 5).Value = allocation.StartDate;
            worksheet.Cell(row, 6).Value = allocation.EndDate?.ToString("yyyy-MM-dd") ?? "";
            worksheet.Cell(row, 7).Value = allocation.IsActive ? "Yes" : "No";
            row++;
        }
        
        worksheet.Columns().AdjustToContents();
        
        using var stream = new MemoryStream();
        workbook.SaveAs(stream);
        return stream.ToArray();
    }
}
