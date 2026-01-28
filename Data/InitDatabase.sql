-- Create Database
IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'VibePontaj')
BEGIN
    CREATE DATABASE VibePontaj;
END
GO

USE VibePontaj;
GO

-- Employees Table
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Employees' AND xtype='U')
BEGIN
    CREATE TABLE Employees (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        FirstName NVARCHAR(100) NOT NULL,
        LastName NVARCHAR(100) NOT NULL,
        Email NVARCHAR(255) NOT NULL UNIQUE,
        PasswordHash NVARCHAR(255) NOT NULL,
        Role NVARCHAR(50) NOT NULL DEFAULT 'Employee',
        HireDate DATE NOT NULL,
        IsActive BIT NOT NULL DEFAULT 1,
        CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
        UpdatedAt DATETIME2 NULL
    );
END
GO

-- Contracts Table
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Contracts' AND xtype='U')
BEGIN
    CREATE TABLE Contracts (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        EmployeeId INT NOT NULL FOREIGN KEY REFERENCES Employees(Id),
        ContractType NVARCHAR(50) NOT NULL,
        StartDate DATE NOT NULL,
        EndDate DATE NULL,
        HourlyRate DECIMAL(10,2) NOT NULL,
        WorkingHoursPerDay INT NOT NULL DEFAULT 8,
        IsActive BIT NOT NULL DEFAULT 1,
        CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
        UpdatedAt DATETIME2 NULL
    );
END
GO

-- Projects Table
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Projects' AND xtype='U')
BEGIN
    CREATE TABLE Projects (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        Name NVARCHAR(200) NOT NULL,
        Description NVARCHAR(MAX) NOT NULL,
        ProjectCode NVARCHAR(50) NOT NULL UNIQUE,
        StartDate DATE NOT NULL,
        EndDate DATE NULL,
        IsActive BIT NOT NULL DEFAULT 1,
        CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
        UpdatedAt DATETIME2 NULL
    );
END
GO

-- Project Allocations Table
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='ProjectAllocations' AND xtype='U')
BEGIN
    CREATE TABLE ProjectAllocations (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        EmployeeId INT NOT NULL FOREIGN KEY REFERENCES Employees(Id),
        ProjectId INT NOT NULL FOREIGN KEY REFERENCES Projects(Id),
        StartDate DATE NOT NULL,
        EndDate DATE NULL,
        IsActive BIT NOT NULL DEFAULT 1,
        CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE()
    );
END
GO

-- Timesheets Table
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Timesheets' AND xtype='U')
BEGIN
    CREATE TABLE Timesheets (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        EmployeeId INT NOT NULL FOREIGN KEY REFERENCES Employees(Id),
        ProjectId INT NOT NULL FOREIGN KEY REFERENCES Projects(Id),
        Date DATE NOT NULL,
        Hours DECIMAL(4,2) NOT NULL,
        Description NVARCHAR(500) NOT NULL,
        Status NVARCHAR(50) NOT NULL DEFAULT 'Draft',
        IsLocked BIT NOT NULL DEFAULT 0,
        ApprovedBy INT NULL FOREIGN KEY REFERENCES Employees(Id),
        ApprovedAt DATETIME2 NULL,
        CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
        UpdatedAt DATETIME2 NULL
    );
END
GO

-- Attendance Table
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Attendance' AND xtype='U')
BEGIN
    CREATE TABLE Attendance (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        EmployeeId INT NOT NULL FOREIGN KEY REFERENCES Employees(Id),
        Date DATE NOT NULL,
        Status NVARCHAR(50) NOT NULL DEFAULT 'WFO',
        Notes NVARCHAR(500) NULL,
        CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
        UpdatedAt DATETIME2 NULL,
        CONSTRAINT UQ_Attendance_EmployeeId_Date UNIQUE (EmployeeId, Date)
    );
END
GO

-- Leaves Table
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Leaves' AND xtype='U')
BEGIN
    CREATE TABLE Leaves (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        EmployeeId INT NOT NULL FOREIGN KEY REFERENCES Employees(Id),
        LeaveType NVARCHAR(50) NOT NULL,
        StartDate DATE NOT NULL,
        EndDate DATE NOT NULL,
        Reason NVARCHAR(500) NOT NULL,
        Status NVARCHAR(50) NOT NULL DEFAULT 'Pending',
        ApprovedBy INT NULL FOREIGN KEY REFERENCES Employees(Id),
        ApprovedAt DATETIME2 NULL,
        RejectionReason NVARCHAR(500) NULL,
        CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
        UpdatedAt DATETIME2 NULL
    );
END
GO

-- Insert default admin user (password: Admin@123)
IF NOT EXISTS (SELECT * FROM Employees WHERE Email = 'admin@vibe.com')
BEGIN
    INSERT INTO Employees (FirstName, LastName, Email, PasswordHash, Role, HireDate, IsActive)
    VALUES ('Admin', 'User', 'admin@vibe.com', '$2a$11$EjDYPFZ7YJ8xN8qJ8.5kZuFg0Y8z0cXWoU4H3DdG9.VZ.JXqJ3gCy', 'Admin', GETDATE(), 1);
END
GO

-- Insert some sample projects
IF NOT EXISTS (SELECT * FROM Projects WHERE ProjectCode = 'PROJ001')
BEGIN
    INSERT INTO Projects (Name, Description, ProjectCode, StartDate, IsActive)
    VALUES 
        ('Internal Development', 'Internal company projects and improvements', 'PROJ001', GETDATE(), 1),
        ('Client Project A', 'Development work for Client A', 'PROJ002', GETDATE(), 1),
        ('Client Project B', 'Development work for Client B', 'PROJ003', GETDATE(), 1);
END
GO
