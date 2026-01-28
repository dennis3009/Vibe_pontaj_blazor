# Approvals Dashboard

## Overview
The Approvals Dashboard (`/approvals`) provides a unified interface for managers and administrators to approve or reject timesheet submissions from employees. This page consolidates approval workflows and provides easy navigation to leave approval functionality.

## Route
- **URL**: `/approvals`
- **Authorization**: Manager, Admin roles only
- **Component**: `Components/Pages/Approvals.razor`

## Features

### 1. Timesheet Approvals
- **Grouped View**: Timesheets are grouped by employee and week for easy review
- **Summary Information**:
  - Employee name
  - Week number and year
  - Date range (week start - week end)
  - Total hours for the week
  - Current status
- **Expandable Details**: Click "View Details" to see all individual timesheet entries
- **Entry Details Display**:
  - Date and day of week
  - Project name
  - Hours logged
  - Description/notes
  - Weekend highlighting (Saturday/Sunday)

### 2. Approval Actions

#### Approve Timesheet
- **Effect**:
  - Updates all entries in the week to "Approved" status
  - Sets the approver (current manager/admin)
  - Records approval timestamp
  - Locks the week (employee cannot edit)
- **Workflow**:
  1. Expand the timesheet to review entries (optional)
  2. Click "Approve Timesheet" button
  3. Confirm the action in the warning dialog
  4. System processes approval and locks the week

#### Reject Timesheet
- **Effect**:
  - Updates all entries in the week to "Rejected" status
  - Records rejection timestamp and approver
  - Unlocks the week (employee can edit and resubmit)
- **Workflow**:
  1. Expand the timesheet to review entries (optional)
  2. Click "Reject Timesheet" button
  3. Enter a mandatory rejection reason
  4. Confirm the rejection
  5. System processes rejection and unlocks the week

### 3. Leave Approvals Integration
- **Tab Navigation**: Switch between Timesheet and Leave approvals
- **Direct Link**: Clicking "Leave Approvals" tab navigates to `/leave/approve`
- **Existing Functionality**: Uses the established Leave approval workflow

## UI Elements

### Color Coding
- **Submitted**: Yellow/Warning (pending approval)
- **Approved**: Green/Success (completed)
- **Rejected**: Red/Danger (needs revision)

### Status Indicators
- **Icons**: Bootstrap icons for visual status representation
  - Submitted: Hourglass icon
  - Approved: Check circle icon
  - Rejected: X circle icon

### Responsive Design
- Bootstrap grid system for responsive layout
- Card-based design for timesheet groups
- Collapsible details to reduce clutter
- Mobile-friendly interface

## Data Flow

### Loading Process
1. **Authentication Check**: Verify user is logged in and has Manager/Admin role
2. **Load Employee Names**: Fetch all employees for name resolution
3. **Load Pending Timesheets**: Query all submitted timesheets
4. **Group Timesheets**: Group by employee, year, and week number
5. **Calculate Totals**: Sum hours for each week

### Approval Process
1. **Select Week**: Manager reviews the timesheet group
2. **Expand Details** (optional): View individual entries
3. **Decision**: Choose Approve or Reject
4. **Confirmation**: Confirm the action
5. **Database Update**:
   - Update status for all entries in the week
   - Set approver and timestamp
   - Lock/unlock the week as appropriate
6. **Refresh**: Reload pending timesheets list
7. **Feedback**: Show success message

## Technical Implementation

### Dependencies
- **ITimesheetRepository**: Data access for timesheet operations
  - `GetPendingApprovalsAsync()`: Fetch submitted timesheets
  - `UpdateAsync()`: Update timesheet entry status
  - `LockWeekAsync()`: Lock a week for an employee
  - `UnlockWeekAsync()`: Unlock a week for editing
- **IEmployeeRepository**: Employee information lookup
  - `GetByEmailAsync()`: Get current manager/admin
  - `GetAllAsync()`: Load all employees for name display
- **AuthenticationStateProvider**: User authentication and role checking
- **IJSRuntime**: JavaScript interop for confirmation dialogs
- **NavigationManager**: Navigation to Leave approvals

### Internal Classes

#### TimesheetGroup
Groups timesheets by employee and week for display:
```csharp
private class TimesheetGroup
{
    public int EmployeeId { get; set; }
    public int Year { get; set; }
    public int WeekNumber { get; set; }
    public DateTime WeekStart { get; set; }
    public DateTime WeekEnd { get; set; }
    public List<VibePontaj.Models.Timesheet> Entries { get; set; }
    public decimal TotalHours { get; set; }
    public string Status { get; set; }
}
```

### Key Methods

#### Week Calculation
- `GetWeekStartDate(year, weekNumber)`: Calculate the Monday of a given week
- `GetWeekEndDate(year, weekNumber)`: Calculate the Sunday of a given week
- Uses ISO 8601 week numbering (week starts on Monday)

#### Status Helpers
- `GetStatusBadgeClass()`: Returns Bootstrap class for status badges
- `GetStatusIcon()`: Returns Bootstrap icon class for status
- `GetCardBorderClass()`: Returns card border color based on status
- `GetCardHeaderClass()`: Returns card header color based on status

## Security Considerations

### Authorization
- **Role-Based Access**: Only Manager and Admin roles can access
- **Attribute**: `[Authorize(Roles = "Manager,Admin")]`
- **Navigation**: Menu item only visible to authorized roles

### Validation
- **Rejection Reason**: Mandatory field when rejecting
- **User Identity**: Verifies authenticated user before operations
- **Manager ID**: Records who performed the approval/rejection

### Data Integrity
- **Atomic Updates**: All entries in a week updated together
- **Lock/Unlock**: Ensures data consistency
- **Timestamps**: Records when actions were performed

## User Experience

### Loading States
- **Initial Load**: Spinner while fetching data
- **Processing Actions**: Disabled buttons with spinner during approval/rejection

### Error Handling
- **Error Messages**: Red alert banner for errors
- **Success Messages**: Green alert banner for successful operations
- **Dismissible Alerts**: Users can close messages

### Confirmation Dialogs
- **Approve**: Warns that the week will be locked
- **Reject**: Confirms rejection action
- **JavaScript Confirm**: Native browser confirmation dialogs

## Navigation
- **Main Menu**: "Approvals" link visible to Manager/Admin roles
- **Tab Navigation**: Switch between Timesheet and Leave approvals
- **Back Navigation**: Browser back button returns to previous page

## Future Enhancements

### Potential Improvements
1. **Rejection Reason Field**: Add RejectionReason field to Timesheet model
2. **Bulk Actions**: Approve/reject multiple weeks at once
3. **Filters**: Filter by employee, date range, status
4. **Search**: Search for specific timesheets
5. **Comments**: Add manager comments/notes
6. **History**: View approval history and audit trail
7. **Notifications**: Email notifications on approval/rejection
8. **Export**: Export pending approvals to Excel/PDF
9. **Statistics**: Dashboard showing approval metrics
10. **Approval Workflow**: Multi-level approval process

## Related Pages
- **Timesheet Index** (`/timesheet`): Employee timesheet entry
- **Leave Approve** (`/leave/approve`): Leave request approvals
- **Admin Dashboard**: Administrative functions

## Best Practices for Managers

### Reviewing Timesheets
1. **Check Totals**: Verify total hours are reasonable (typically 40 hrs/week)
2. **Review Projects**: Ensure employees are logging to correct projects
3. **Weekend Work**: Check if weekend hours are expected/authorized
4. **Descriptions**: Verify work descriptions are meaningful
5. **Patterns**: Look for unusual patterns (all 8hrs days, no variation)

### Rejection Guidelines
- **Be Specific**: Provide clear reason for rejection
- **Be Constructive**: Explain what needs to be corrected
- **Be Timely**: Review and respond promptly
- **Follow Up**: Ensure employees understand the issue

### Approval Best Practices
- **Regular Review**: Don't let pending approvals accumulate
- **Fair Treatment**: Apply same standards to all employees
- **Documentation**: Rejection reasons serve as documentation
- **Communication**: Discuss issues with employee if needed
