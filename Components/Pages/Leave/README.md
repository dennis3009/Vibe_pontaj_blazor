# Leave Module - VibePontaj

## Overview
The Leave module provides comprehensive leave request management for the VibePontaj application. It allows employees to submit leave requests and managers to approve or reject them, with automatic integration into attendance and timesheet systems.

## Features

### Employee Features (Index.razor - `/leave`)
- **Submit Leave Requests**: Create new leave requests with type, date range, and reason
- **View Leave History**: See all submitted leave requests with status
- **Working Days Calculation**: Automatically calculates working days excluding weekends
- **Cancel Pending Requests**: Cancel leave requests that haven't been approved yet
- **Excel Export**: Export leave history to Excel for personal records
- **Real-time Status**: View current status (Pending/Approved/Rejected) with color coding

### Manager Features (Approve.razor - `/leave/approve`)
- **Review Pending Requests**: View all pending leave requests in card format
- **Approve/Reject**: Quick approval or rejection with confirmation
- **Rejection Reason**: Mandatory rejection reason for transparency
- **All Requests View**: See complete leave history for all employees
- **Details Modal**: View complete information for any leave request
- **Automatic Attendance Update**: Approved leaves automatically update attendance records

## Leave Types
1. **Paid Leave**: Regular annual leave or vacation days
2. **Sick Leave**: Medical leave with or without documentation  
3. **Unpaid Leave**: Leave without pay for personal reasons

## Status Types
- **Pending** (Yellow badge): Awaiting manager approval
- **Approved** (Green badge): Approved by manager, attendance updated
- **Rejected** (Red badge): Rejected by manager with reason

## Pages

### 1. Index.razor (`/leave`)
**Route**: `/leave`  
**Authorization**: `[Authorize]` - All authenticated users  
**Purpose**: Employee self-service for leave requests

**Key Components**:
- New leave request form with validation
- Leave history table
- Status badges with color coding
- Excel export functionality
- Cancel pending requests option

**Validation Rules**:
- Leave type is required
- End date must be >= start date
- Reason is required (textarea)
- Warning for past date requests
- Automatic working days calculation (excludes weekends)

### 2. Approve.razor (`/leave/approve`)
**Route**: `/leave/approve`  
**Authorization**: `[Authorize(Roles = "Manager,Admin")]` - Managers and Admins only  
**Purpose**: Manager approval workflow for leave requests

**Key Components**:
- Two tabs: Pending Requests and All Requests
- Card-based layout for pending requests
- Table view for all requests history
- Inline approval/rejection actions
- Details modal for complete information

**Approval Workflow**:
1. Manager clicks "Approve" on a pending request
2. Confirmation dialog appears
3. On confirm: `LeaveService.ApproveLeaveAsync()` is called
4. Service updates leave status to "Approved"
5. Service automatically creates/updates attendance records for the leave period
6. Attendance status codes: PL (Paid), SC (Sick), UL (Unpaid)
7. Weekends are automatically skipped

**Rejection Workflow**:
1. Manager clicks "Reject" on a pending request
2. Rejection reason form appears (required)
3. Confirmation dialog appears
4. On confirm: `LeaveService.RejectLeaveAsync()` is called
5. Service updates leave status to "Rejected" with reason

## Integration with Other Modules

### Attendance Module
When a leave is **approved**:
- The `LeaveService` automatically creates or updates attendance records
- For each working day in the leave period:
  - Weekends (Saturday, Sunday) are skipped
  - Status is set to leave type code: PL, SC, or UL
  - Notes field includes leave type and reason
- Existing attendance records are updated if present
- New attendance records are created if missing

### Timesheet Module
- Approved leave dates should block timesheet entry
- This is handled via the `LeaveService` logic
- Employees cannot enter timesheet hours for dates marked as leave in attendance

## Services Used

### ILeaveRepository
- `GetByEmployeeIdAsync(int employeeId)` - Get employee's leave requests
- `GetAllAsync()` - Get all leave requests (managers)
- `GetPendingAsync()` - Get pending leave requests
- `GetByIdAsync(int id)` - Get single leave request
- `CreateAsync(Leave leave)` - Create new leave request
- `UpdateAsync(Leave leave)` - Update leave request
- `DeleteAsync(int id)` - Delete/cancel leave request

### ILeaveService
- `CreateLeaveRequestAsync(Leave leave)` - Create new leave (sets status to Pending)
- `ApproveLeaveAsync(int leaveId, int approvedBy)` - Approve leave and update attendance
- `RejectLeaveAsync(int leaveId, int rejectedBy, string reason)` - Reject leave with reason

### IEmployeeRepository
- `GetByEmailAsync(string email)` - Get current user's employee record
- `GetAllAsync()` - Get all employees (for manager view)

### IExcelExportService
- `ExportLeaves(IEnumerable<Leave> leaves)` - Export leave data to Excel

## UI/UX Features

### Color Coding
- **Pending**: Yellow/Warning - Awaiting action
- **Approved**: Green/Success - Completed and approved
- **Rejected**: Red/Danger - Denied with reason
- **Paid Leave**: Blue/Primary badge
- **Sick Leave**: Light Blue/Info badge
- **Unpaid Leave**: Grey/Secondary badge

### Loading States
- Spinner during initial page load
- Spinner during form submission ("Submitting...")
- Spinner during approval/rejection processing
- Disabled buttons during processing

### Alerts and Messages
- Success alerts (green) for successful operations
- Error alerts (red) for failures
- Info alerts (blue) for guidance
- Dismissible alerts with close button

### Responsive Design
- Card-based layout for mobile/tablet
- Table layout for desktop (pending tab on Approve page)
- Bootstrap grid system for responsiveness
- Proper button sizing for touch interfaces

## Form Validation

### New Leave Request Form
1. **Leave Type**: Must be selected (Paid/Sick/Unpaid)
2. **Start Date**: Cannot be empty, warning for past dates
3. **End Date**: Must be >= Start Date
4. **Reason**: Required, textarea with placeholder
5. **Total Days**: Auto-calculated, displays working days only

### Rejection Form
1. **Rejection Reason**: Required field
2. **Confirmation**: Requires explicit confirmation dialog

## Calculated Properties

### TotalDays (in Leave model)
```csharp
public int TotalDays => (int)(EndDate - StartDate).TotalDays + 1;
```
This calculation is done in the model, but the UI also has a working days calculator that excludes weekends for display purposes.

## Security
- Index page requires authentication (`[Authorize]`)
- Approve page requires Manager or Admin role (`[Authorize(Roles = "Manager,Admin")]`)
- Current user is retrieved from `AuthenticationStateProvider`
- Employee can only see their own leave requests
- Managers can see all leave requests

## Excel Export
The export includes:
- Employee name
- Leave type
- Start and end dates
- Total days
- Reason
- Status
- Date range and other metadata

File naming convention: `LeaveRequests_YYYYMMDD.xlsx`

## Database Fields (Leave Model)
- `Id` - Primary key
- `EmployeeId` - Foreign key to Employees
- `LeaveType` - Paid, Sick, Unpaid
- `StartDate` - Leave start date
- `EndDate` - Leave end date
- `Reason` - Employee's reason for leave
- `Status` - Pending, Approved, Rejected
- `ApprovedBy` - Manager/Admin who processed the request
- `ApprovedAt` - DateTime when processed
- `RejectionReason` - Reason if rejected (nullable)
- `CreatedAt` - When request was created
- `UpdatedAt` - Last update timestamp (nullable)

## Future Enhancements
- Leave balance tracking per employee
- Leave policy configuration (max days per type)
- Email notifications on approval/rejection
- Leave calendar view
- Conflict detection (overlapping leaves)
- Holiday calendar integration
- Manager delegation for approvals
- Bulk approval functionality
- Leave request editing (before approval)
- Attachment support (medical certificates, etc.)

## Usage Examples

### Submit a Leave Request (Employee)
1. Navigate to `/leave`
2. Click "New Leave Request"
3. Select leave type (Paid/Sick/Unpaid)
4. Choose start and end dates
5. Enter reason
6. Review calculated total days
7. Click "Submit Request"
8. Wait for manager approval

### Approve a Leave Request (Manager)
1. Navigate to `/leave/approve`
2. View pending requests on the "Pending" tab
3. Review employee details, dates, and reason
4. Click "Approve" button
5. Confirm in the dialog
6. System automatically updates attendance records

### Reject a Leave Request (Manager)
1. Navigate to `/leave/approve`
2. View pending requests
3. Click "Reject" button
4. Enter rejection reason
5. Click "Confirm Rejection"
6. Employee will see rejection reason in their leave history

## Dependencies
- Bootstrap 5 for UI components
- Bootstrap Icons for iconography
- ClosedXML for Excel export
- Dapper for database access
- Microsoft.AspNetCore.Authorization for security
