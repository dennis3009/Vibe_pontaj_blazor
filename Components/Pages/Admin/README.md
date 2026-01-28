# Admin Module - VibePontaj

This directory contains comprehensive admin pages for managing core entities in the VibePontaj Blazor application.

## Pages Overview

### 1. Employees Management (`/admin/employees`)
**File:** `Employees.razor`

Manage employee accounts with full CRUD operations.

#### Features:
- **List View**: Display all employees with ID, name, email, role, hire date, and status
- **Add Employee**: Create new employee accounts with:
  - First Name, Last Name, Email (required)
  - Role selection (Employee, Manager, Admin)
  - Hire Date
  - Password (hashed using BCrypt via AuthService)
  - Auto-set to Active status
- **Edit Employee**: Update employee details including:
  - Personal information
  - Role changes
  - Password reset (leave blank to keep current)
  - Active/Inactive status toggle
- **Soft Delete**: Deactivate employees (sets IsActive = false)
- **Excel Export**: Export all employees to Excel with timestamp
- **Role Badges**: Color-coded badges (Admin=danger, Manager=warning, Employee=info)

#### Authorization:
- Restricted to Admin role only
- Uses `@attribute [Authorize(Roles = "Admin")]`

---

### 2. Projects Management (`/admin/projects`)
**File:** `Projects.razor`

Manage project information and lifecycle.

#### Features:
- **List View**: Display all projects with:
  - Project Code (badge)
  - Name and Description
  - Start and End dates
  - Active/Inactive status
- **Add Project**: Create new projects with:
  - Project Code (e.g., PROJ-001)
  - Project Name (required)
  - Description (optional)
  - Start Date (required)
  - End Date (optional for ongoing projects)
- **Edit Project**: Update project details and status
- **Soft Delete**: Deactivate projects (sets IsActive = false)
- **Excel Export**: Export all projects to Excel with timestamp

#### Business Rules:
- Projects without end dates are displayed as "Ongoing"
- Project code is displayed in a badge for easy identification

#### Authorization:
- Restricted to Admin role only

---

### 3. Contracts Management (`/admin/contracts`)
**File:** `Contracts.razor`

Manage employee contracts and compensation details.

#### Features:
- **List View**: Display all contracts with:
  - Employee name (from navigation property)
  - Contract Type (Full-time, Part-time, Contract)
  - Start and End dates
  - Hourly Rate ($)
  - Working Hours per Day
  - Active/Inactive status
- **Add Contract**: Create new contracts with:
  - Employee selection (dropdown, required)
  - Contract Type selection (required)
  - Start Date (required)
  - End Date (optional)
  - Hourly Rate (required, decimal)
  - Working Hours per Day (required, 1-24)
- **Edit Contract**: Update contract details
  - Employee selection is disabled (cannot change after creation)
  - Can update dates, rate, hours, and status
- **Soft Delete**: Deactivate contracts (sets IsActive = false)
- **Excel Export**: Export all contracts with employee names to Excel

#### Business Rules:
- Employee cannot be changed after contract creation
- Hourly rate must be greater than 0
- Working hours must be between 1-24
- Contracts without end dates are "Ongoing"

#### Authorization:
- Restricted to Admin role only

---

### 4. Project Allocations Management (`/admin/allocations`)
**File:** `Allocations.razor`

Manage employee assignments to projects.

#### Features:
- **List View**: Display all allocations with:
  - Employee name
  - Project name
  - Project Code (badge)
  - Start and End dates
  - Active/Inactive status
- **Add Allocation**: Create new allocations with:
  - Employee selection (dropdown, required)
  - Project selection (dropdown with code, required)
  - Start Date (required)
  - End Date (optional for ongoing allocations)
- **Edit Allocation**: Update allocation details
  - Employee and Project are disabled (cannot change after creation)
  - Can update dates and status
- **Soft Delete**: Deactivate allocations (sets IsActive = false)
- **Excel Export**: Export all allocations with employee and project details

#### Business Rules:
- Employee and Project cannot be changed after allocation creation
- Allocations without end dates are "Ongoing"
- Each allocation represents active assignment of employee to project

#### Authorization:
- Restricted to Admin role only

---

## Common Features Across All Pages

### UI/UX Patterns:
- **Bootstrap 5 Styling**: Consistent responsive design
- **Modal Forms**: Add/Edit operations in modal dialogs
- **Loading Spinners**: Displayed during data fetching
- **Alert Messages**: 
  - Success alerts (green, dismissible)
  - Error alerts (red, dismissible)
- **Confirmation Dialogs**: JavaScript confirm before delete operations
- **Required Fields**: Marked with red asterisk (*)
- **Responsive Tables**: Horizontal scroll on small screens

### Data Operations:
- **Soft Deletes**: All entities use IsActive flag instead of hard deletes
- **Validation**: Client-side validation for required fields and data types
- **Error Handling**: Try-catch blocks with user-friendly error messages
- **Async Operations**: All repository calls are asynchronous

### Excel Export:
- Uses `ExcelExportService` with ClosedXML library
- Generates XLSX files with formatted headers
- Includes all visible columns
- Filename format: `EntityName_YYYYMMDD_HHMMSS.xlsx`
- Downloads via JavaScript `downloadFile` function

---

## Technical Implementation

### Dependencies:
```csharp
@inject IEmployeeRepository EmployeeRepository
@inject IProjectRepository ProjectRepository
@inject IContractRepository ContractRepository
@inject IProjectAllocationRepository AllocationRepository
@inject IAuthService AuthService
@inject IExcelExportService ExcelExportService
@inject IJSRuntime JSRuntime
```

### Repository Pattern:
All pages use repository pattern for data access:
- `GetAllAsync()` - Fetch all active entities
- `GetByIdAsync(id)` - Fetch single entity
- `CreateAsync(entity)` - Create new entity
- `UpdateAsync(entity)` - Update existing entity
- `DeleteAsync(id)` - Soft delete (set IsActive = false)

### Navigation Properties:
- Contracts: Includes Employee navigation property
- Allocations: Includes both Employee and Project navigation properties
- Proper Dapper multi-mapping configured in repositories

---

## Security Considerations

### Authorization:
- All pages require Admin role
- Uses `@attribute [Authorize(Roles = "Admin")]`
- Enforced at both page and API level

### Password Security:
- Passwords hashed using BCrypt (via AuthService)
- Password hashing only in Employees page
- Never display or transmit plain text passwords

### Data Validation:
- Required field validation
- Type validation (email, numbers, dates)
- Range validation (hourly rate > 0, hours 1-24)
- Confirmation before destructive operations

---

## Future Enhancements

Potential improvements for the Admin module:

1. **Search and Filtering**:
   - Search by name, email, code
   - Filter by status, role, date ranges
   - Advanced filtering UI

2. **Pagination**:
   - Server-side pagination for large datasets
   - Page size configuration
   - Jump to page functionality

3. **Sorting**:
   - Client-side column sorting
   - Multi-column sort
   - Save sort preferences

4. **Bulk Operations**:
   - Multi-select checkboxes
   - Bulk activate/deactivate
   - Bulk export selected items

5. **Audit Trail**:
   - Track who created/updated records
   - Display CreatedAt/UpdatedAt timestamps
   - View change history

6. **Data Import**:
   - Excel import functionality
   - CSV import support
   - Data validation during import

7. **Advanced Reporting**:
   - Employee allocation reports
   - Contract expiration notifications
   - Project capacity planning

---

## Usage Examples

### Adding a New Employee:
1. Navigate to `/admin/employees`
2. Click "Add New Employee" button
3. Fill in required fields:
   - First Name: "John"
   - Last Name: "Doe"
   - Email: "john.doe@example.com"
   - Role: "Employee"
   - Hire Date: Today
   - Password: "SecurePass123!"
4. Click "Save"
5. Success message appears, employee added to table

### Creating a Project Allocation:
1. Navigate to `/admin/allocations`
2. Click "Add New Allocation" button
3. Select Employee from dropdown
4. Select Project from dropdown (shows code)
5. Set Start Date
6. Optionally set End Date
7. Click "Save"
8. Allocation appears in table

### Exporting Contracts to Excel:
1. Navigate to `/admin/contracts`
2. Click "Export to Excel" button
3. File downloads automatically: `Contracts_20250128_134500.xlsx`
4. Success message confirms export

---

## Troubleshooting

### Common Issues:

**Issue**: Modal doesn't close after save
- **Solution**: Check for JavaScript errors in browser console

**Issue**: Excel export downloads empty file
- **Solution**: Ensure ExcelExportService is registered in Program.cs

**Issue**: Employee dropdown empty in Contracts page
- **Solution**: Verify employees exist with IsActive = true

**Issue**: Unauthorized access to admin pages
- **Solution**: Ensure logged-in user has "Admin" role

**Issue**: Password not hashing on employee creation
- **Solution**: Verify AuthService.HashPassword is called before CreateAsync

---

## Related Documentation

- [Services Documentation](../../Services/README.md) - Details on AuthService and ExcelExportService
- [Repository Pattern](../../Data/Repositories/README.md) - Repository implementations
- [Model Definitions](../../Models/README.md) - Entity model details

---

## Version History

- **v1.0** (2025-01-28): Initial release with all four admin pages
  - Employees management
  - Projects management
  - Contracts management
  - Project allocations management
