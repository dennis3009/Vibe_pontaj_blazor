# Attendance Module

## Overview
The Attendance module allows employees to track their daily work status with a comprehensive monthly calendar view.

## Features

### 1. Monthly Calendar View
- Full month calendar with weeks starting on Monday
- Color-coded attendance status badges
- Easy navigation between months
- Current day highlighting
- Weekend cells with different background

### 2. Attendance Status Types
- **WFO** (Work From Office) - Blue badge
- **WFH** (Work From Home) - Green badge
- **TB** (Travel/Business) - Purple badge
- **SC** (Sick) - Orange badge
- **PL** (Paid Leave) - Red badge
- **UL** (Unpaid Leave) - Red badge
- **AL** (Absent/Leave) - Red badge

### 3. Edit Functionality
- Click on any past or current day to edit attendance
- Modal dialog for quick status updates
- Optional notes field for additional details
- Delete existing attendance records
- Future dates are disabled (cannot be edited)

### 4. Monthly Summary
- Visual summary cards showing count for each status type
- Quick overview of attendance patterns

### 5. Excel Export
- Export attendance records to Excel format
- Includes all attendance data for the selected month
- File naming format: `Attendance_YYYY_MM.xlsx`

### 6. User Experience Features
- Loading spinner during data fetch
- Success and error message alerts
- Responsive design for all screen sizes
- Intuitive modal-based editing
- Hover effects on editable cells

## Usage

### Viewing Attendance
1. Navigate to `/attendance` in the application
2. Current month's attendance is displayed by default
3. Use navigation buttons to move between months

### Adding/Editing Attendance
1. Click on any cell for the current or past dates
2. Select the appropriate status from the dropdown
3. Optionally add notes
4. Click "Save" to confirm

### Deleting Attendance
1. Click on the day with existing attendance
2. In the modal, click "Delete" button
3. Confirm the deletion when prompted

### Exporting to Excel
1. Click the "Export to Excel" button
2. File will be automatically downloaded to your browser

## Security
- Page requires authentication (`@attribute [Authorize]`)
- Users can only view and edit their own attendance
- Employee is identified through authenticated user's email

## Technical Details

### Dependencies
- `IAttendanceRepository` - Data access for attendance records
- `IEmployeeRepository` - Employee information lookup
- `IExcelExportService` - Excel file generation
- `AuthenticationStateProvider` - User authentication
- `IJSRuntime` - JavaScript interop for file downloads

### Data Flow
1. User authentication via `AuthenticationStateProvider`
2. Employee lookup by email
3. Attendance data loaded for selected month
4. Changes persisted through repository pattern

## Future Enhancements
- Bulk edit capability for multiple days
- Attendance approval workflow
- Calendar integration for leave requests
- Mobile-optimized view
- Attendance reports and analytics
