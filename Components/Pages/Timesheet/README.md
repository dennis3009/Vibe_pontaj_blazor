# Timesheet Module Documentation

## Overview
The Timesheet module allows employees to track their daily work hours across different projects on a weekly basis.

## Features

### Weekly View
- Displays Monday through Sunday for the selected week
- Shows week number and date range
- Navigate between weeks using Previous/Next/This Week buttons

### Time Entry Management
- **Add Entry**: Click "Add Entry" button for any day to create a new timesheet entry
- **Edit Entry**: Click the pencil icon to modify an existing entry
- **Delete Entry**: Click the trash icon to remove an entry (with confirmation)

### Entry Details
Each timesheet entry includes:
- **Date**: The day of the week (automatically set based on which day you're adding to)
- **Project**: Dropdown showing only projects you're allocated to
- **Hours**: Decimal input (0-24 hours, supports 0.5 increments)
- **Description**: Text area to describe the work performed

### Daily and Weekly Totals
- Each day shows the total hours logged
- The footer displays the total hours for the entire week

### Week Submission
1. Fill in your timesheet entries for the week
2. Click "Submit for Approval" when ready
3. Once submitted, the week becomes locked and cannot be edited
4. Your manager will review and approve/reject the submission

### Status Indicators
- **Draft** (Blue): Week is still being edited
- **Submitted** (Yellow): Week is pending manager approval
- **Approved** (Green): Week has been approved
- **Rejected** (Red): Week was rejected, may need resubmission

### Excel Export
Click "Export to Excel" to download your timesheet data for the current week in Excel format.

## Access
- **Route**: `/timesheet`
- **Authorization**: Requires authenticated user
- **Role**: Available to all authenticated employees

## User Interface Elements

### Navigation Header
```
[← Previous Week]  [Week 47, 2024: Nov 18 - Nov 24]  [Next Week →] [This Week]
```

### Daily Entry Row
```
Date | Day | Project | Hours | Description | Actions
Nov 18 | Mon | ProjectA | 8.0 | Development work | [Edit] [Delete]
                                                   | [Add Entry]
```

### Week Status Alert
Shows current status with appropriate color coding and any relevant messages.

## Validation Rules
1. **Hours**: Must be between 0 and 24
2. **Project**: Must select a project from the dropdown
3. **Submit**: Can only submit weeks with at least one entry
4. **Editing**: Cannot edit locked or submitted weeks

## Project Allocation
Only projects that you are actively allocated to will appear in the project dropdown. If you don't see any projects:
- Contact your manager to ensure you're allocated to active projects
- Check that the projects are marked as active

## Technical Details

### Services Used
- `ITimesheetRepository`: CRUD operations for timesheet entries
- `IProjectRepository`: Fetches project information
- `IProjectAllocationRepository`: Gets user's allocated projects
- `IEmployeeRepository`: Validates current user
- `IExcelExportService`: Generates Excel files

### Week Calculation
Uses ISO 8601 standard with Monday as the first day of the week.

### State Management
- Week status determined by entry statuses
- Week locked if any entry is marked as locked
- Prevents editing of submitted/approved weeks

## Tips
1. **Regular Updates**: Update your timesheet daily for accuracy
2. **Descriptions**: Provide clear descriptions of work performed
3. **Review Before Submit**: Double-check all entries before submitting
4. **Weekend Work**: Weekend days are highlighted differently
5. **Multiple Projects**: You can log hours to multiple projects per day

## Troubleshooting

### "No Projects Allocated" Warning
**Solution**: Contact your manager to have projects allocated to you

### Cannot Submit Week
**Possible Reasons**:
- No entries in the week
- Week is already submitted/locked
- Validation errors in entries

### Export Not Working
**Check**:
- Ensure JavaScript is enabled
- Check browser's download settings
- Try a different browser

## Future Enhancements
- Timesheet copying from previous weeks
- Bulk entry for multiple days
- Comments and notes on entries
- Notification when week is approved/rejected
- Historical timesheet viewing
