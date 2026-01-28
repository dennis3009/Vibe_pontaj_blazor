# VibePontaj - Employee Management System

A comprehensive .NET 10 C# Blazor web server application for internal company management with SQL Server database integration.

## Features

### 🔐 Authentication & Authorization
- Role-based access control (Employee, Manager, Admin)
- Secure password hashing with BCrypt
- Session-based authentication
- Custom authentication state provider

### ⏰ Timesheet Module
- Daily project hour logging
- Weekly view (Monday-Sunday)
- Submit for manager approval
- Week locking after approval
- Excel export functionality
- Real-time hour totals

### 📅 Attendance Module
- Monthly calendar view
- Multiple status types: WFO, WFH, TB, SC, PL, UL, AL
- Color-coded status indicators
- Edit attendance with notes
- Monthly statistics
- Excel export

### 🏖️ Leave Management
- Leave request form (Paid, Sick, Unpaid)
- Working days calculation
- Manager approval workflow
- Automatic attendance updates
- Timesheet blocking for approved leaves
- Excel export

### 👥 Admin Module
- Employee management with CRUD operations
- Project management
- Contract management
- Resource allocation (Employee-Project assignments)
- Soft delete functionality
- Excel export for all entities

### ✅ Approvals Dashboard
- Unified view for managers and admins
- Timesheet approval workflow
- Leave approval workflow
- Week locking capabilities
- Audit trail with approver information

## Technology Stack

- **Framework**: .NET 10 (Blazor Server)
- **Database**: SQL Server
- **ORM**: Dapper
- **Authentication**: Custom Authentication State Provider with BCrypt
- **UI Framework**: Bootstrap 5
- **Icons**: Bootstrap Icons
- **Excel Export**: ClosedXML

## Prerequisites

- .NET 10 SDK
- SQL Server (LocalDB, Express, or Full Edition)
- Visual Studio 2022/2026 or Visual Studio Code
- SQL Server Management Studio (optional, for database management)

## Installation & Setup

### 1. Clone the Repository
```bash
git clone https://github.com/dennis3009/Vibe_pontaj_blazor.git
cd Vibe_pontaj_blazor
```

### 2. Configure Database Connection

Edit `appsettings.json` and update the connection string:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=VibePontaj;Integrated Security=true;TrustServerCertificate=true;"
  }
}
```

**Note**: Adjust the connection string based on your SQL Server configuration:
- For SQL Server Express: `Server=localhost\\SQLEXPRESS;...`
- For SQL Server with authentication: `Server=localhost;Database=VibePontaj;User Id=youruser;Password=yourpassword;TrustServerCertificate=true;`

### 3. Initialize the Database

Run the SQL initialization script to create the database and tables:

```bash
# Using sqlcmd (Windows)
sqlcmd -S localhost -i Data/InitDatabase.sql

# Or using SQL Server Management Studio (SSMS)
# Open SSMS, connect to your server, and execute the script from Data/InitDatabase.sql
```

The script will:
- Create the `VibePontaj` database if it doesn't exist
- Create all necessary tables (Employees, Projects, Contracts, etc.)
- Insert a default admin user
- Insert sample projects

### 4. Build and Run

#### Using Visual Studio 2022/2026
1. Open `VibePontaj.sln` in Visual Studio
2. Press F5 or click the "Run" button to build and start the application
3. Visual Studio will automatically restore NuGet packages and build the solution

#### Using Command Line
```bash
# Restore dependencies
dotnet restore VibePontaj.sln

# Build the project
dotnet build VibePontaj.sln

# Run the application
dotnet run --project VibePontaj.csproj
```

The application will start and be available at:
- HTTPS: `https://localhost:5001`
- HTTP: `http://localhost:5000`

## Default Credentials

After running the initialization script, you can log in with:

- **Email**: `admin@vibe.com`
- **Password**: `Admin@123`

**⚠️ IMPORTANT**: Change the default password immediately after first login!

## Project Structure

```
VibePontaj/
├── Components/
│   ├── Layout/
│   │   ├── MainLayout.razor      # Main application layout
│   │   └── NavMenu.razor         # Navigation menu with role-based access
│   ├── Pages/
│   │   ├── Admin/                # Admin module pages
│   │   │   ├── Employees.razor
│   │   │   ├── Projects.razor
│   │   │   ├── Contracts.razor
│   │   │   └── Allocations.razor
│   │   ├── Attendance/
│   │   │   └── Index.razor       # Monthly calendar view
│   │   ├── Auth/
│   │   │   ├── Login.razor       # Login page
│   │   │   └── Logout.razor      # Logout handler
│   │   ├── Leave/
│   │   │   ├── Index.razor       # Employee leave requests
│   │   │   └── Approve.razor     # Manager approval page
│   │   ├── Timesheet/
│   │   │   └── Index.razor       # Weekly timesheet view
│   │   ├── Approvals.razor       # Unified approvals dashboard
│   │   └── Home.razor            # Dashboard/home page
│   ├── Shared/
│   │   └── RedirectToLogin.razor
│   └── _Imports.razor
├── Data/
│   ├── DbContext.cs              # Database context
│   ├── InitDatabase.sql          # Database initialization script
│   └── Repositories/             # Data access layer
│       ├── EmployeeRepository.cs
│       ├── ProjectRepository.cs
│       ├── ContractRepository.cs
│       ├── TimesheetRepository.cs
│       ├── AttendanceRepository.cs
│       ├── LeaveRepository.cs
│       └── ProjectAllocationRepository.cs
├── Models/                       # Domain models
│   ├── Employee.cs
│   ├── Project.cs
│   ├── Contract.cs
│   ├── Timesheet.cs
│   ├── Attendance.cs
│   ├── Leave.cs
│   └── ProjectAllocation.cs
├── Services/                     # Business logic layer
│   ├── AuthService.cs
│   ├── CustomAuthenticationStateProvider.cs
│   ├── ExcelExportService.cs
│   └── LeaveService.cs
├── wwwroot/                      # Static files
├── appsettings.json              # Configuration
├── Program.cs                    # Application startup
└── VibePontaj.csproj            # Project file
```

## User Roles & Permissions

### Employee
- ✅ View and log timesheet entries
- ✅ View attendance calendar
- ✅ Submit leave requests
- ✅ View own leave history
- ✅ Export own data to Excel

### Manager
- ✅ All Employee permissions
- ✅ Approve/reject timesheet submissions
- ✅ Approve/reject leave requests
- ✅ Lock/unlock weekly timesheets
- ✅ View team timesheet and leave data

### Admin
- ✅ All Manager permissions
- ✅ Manage employees (CRUD)
- ✅ Manage projects (CRUD)
- ✅ Manage contracts (CRUD)
- ✅ Manage project allocations (CRUD)
- ✅ Access all system data

## Module Integrations

### Leave → Attendance
When a leave request is approved:
1. Attendance records are automatically created/updated for the leave period
2. Status codes are set based on leave type (PL, SC, UL)
3. Weekends are automatically excluded

### Leave → Timesheet
When a leave request is approved:
1. Timesheet entries for those dates should be blocked (handled via business logic)
2. Employees cannot log hours for approved leave days

### Timesheet → Approval
When a timesheet week is submitted:
1. All entries for that week are marked as "Submitted"
2. Managers can review and approve/reject
3. Upon approval, the week is locked
4. Approver information is recorded

## Excel Export Features

All modules support Excel export with the following features:
- Timestamped file names
- Formatted headers with bold text and background color
- Auto-sized columns
- Filtered data based on current view
- Proper data types (dates, numbers, text)

## Security Features

- ✅ Password hashing with BCrypt (cost factor 11)
- ✅ Role-based authorization on all pages
- ✅ Parameterized SQL queries (via Dapper) to prevent SQL injection
- ✅ Anti-forgery tokens for forms (built-in Blazor protection)
- ✅ HTML encoding to prevent XSS attacks (built-in Blazor protection)
- ✅ Secure session management
- ✅ Soft deletes to prevent data loss
- ✅ Audit trail for approvals

## Development

### Adding a New Employee
1. Log in as Admin
2. Navigate to Admin → Employees
3. Click "Add New Employee"
4. Fill in the form (password will be hashed automatically)
5. Save

### Creating Project Allocations
1. Log in as Admin
2. Navigate to Admin → Projects (create projects first)
3. Navigate to Admin → Allocations
4. Click "Add New Allocation"
5. Select Employee and Project
6. Set dates
7. Save

### Logging Timesheet Hours
1. Log in as Employee
2. Navigate to Timesheet
3. Select the week using navigation buttons
4. Click on a day to add an entry
5. Select project (only allocated projects will show)
6. Enter hours and description
7. Save
8. Submit the week when ready

### Approving Leave Requests
1. Log in as Manager or Admin
2. Navigate to Leave → Approve
3. Review pending requests
4. Click Approve or Reject
5. For rejection, provide a reason

## Troubleshooting

### Database Connection Issues
- Verify SQL Server is running
- Check the connection string in `appsettings.json`
- Ensure the database exists (run InitDatabase.sql)
- Check Windows Firewall settings

### Login Issues
- Ensure the database is initialized with the default admin user
- Try resetting the password by updating the database directly
- Check browser console for JavaScript errors

### Build Errors
- Run `dotnet restore` to restore NuGet packages
- Run `dotnet clean` followed by `dotnet build`
- Check that .NET 10 SDK is installed: `dotnet --version`

### Excel Export Not Working
- Check browser console for JavaScript errors
- Ensure ClosedXML package is installed
- Check that the download function is available in site.js

## Future Enhancements

- [ ] Email notifications for approvals
- [ ] Dashboard widgets with statistics
- [ ] Reporting module with charts
- [ ] Mobile app (Blazor MAUI)
- [ ] Dark mode theme
- [ ] Multi-language support
- [ ] API for external integrations
- [ ] Advanced filtering and search
- [ ] Bulk operations
- [ ] Import from Excel

## Support

For issues, questions, or contributions:
- Create an issue on GitHub
- Contact the development team
- Check the module-specific README files in each folder

## License

This project is proprietary software for internal use only.

## Contributors

- Development Team
- GitHub Copilot (AI Assistant)

---

**Last Updated**: January 2026
**Version**: 1.0.0
