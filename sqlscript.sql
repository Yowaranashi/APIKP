CREATE DATABASE HranitelPRO;
GO
USE HranitelPRO;
GO

------------------------------------------------------------
-- 1. Reference tables
------------------------------------------------------------

CREATE TABLE Roles (
    Id INT IDENTITY PRIMARY KEY,
    Name NVARCHAR(100) NOT NULL,
    Description NVARCHAR(200) NULL
);

CREATE TABLE Departments (
    Id INT IDENTITY PRIMARY KEY,
    Name NVARCHAR(200) NOT NULL,
    Description NVARCHAR(1000) NULL
);

CREATE TABLE ApplicationStatuses (
    StatusID INT IDENTITY PRIMARY KEY,
    StatusName NVARCHAR(50) NOT NULL
);

CREATE TABLE FileTypes (
    FileTypeID INT IDENTITY PRIMARY KEY,
    TypeName NVARCHAR(100) NOT NULL
);

CREATE TABLE ViolationReasons (
    ReasonID INT IDENTITY PRIMARY KEY,
    ReasonText NVARCHAR(MAX) NOT NULL
);

------------------------------------------------------------
-- 2. Users and employees
------------------------------------------------------------

CREATE TABLE Employees (
    Id INT IDENTITY PRIMARY KEY,
    FullName NVARCHAR(200) NOT NULL,
    EmployeeCode NVARCHAR(50) NULL UNIQUE,
    DepartmentId INT NULL,
    FOREIGN KEY (DepartmentId) REFERENCES Departments(Id)
);

CREATE TABLE Users (
    Id INT IDENTITY PRIMARY KEY,
    FullName NVARCHAR(200) NOT NULL,
    Email NVARCHAR(200) NOT NULL UNIQUE,
    PasswordHash NVARCHAR(128) NOT NULL,
    EmailConfirmed BIT NOT NULL DEFAULT 0,
    EmployeeId INT NULL,
    RoleId INT NOT NULL,
    CreatedAt DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
    FOREIGN KEY (EmployeeId) REFERENCES Employees(Id),
    FOREIGN KEY (RoleId) REFERENCES Roles(Id)
);

------------------------------------------------------------
-- 3. Pass requests and visitors
------------------------------------------------------------

CREATE TABLE PassRequests (
    Id INT IDENTITY PRIMARY KEY,
    RequestType NVARCHAR(50) NOT NULL DEFAULT 'Personal',
    StartDate DATETIME2 NOT NULL,
    EndDate DATETIME2 NOT NULL,
    Purpose NVARCHAR(1000) NOT NULL,
    DepartmentId INT NOT NULL,
    ResponsibleEmployeeId INT NULL,
    ApplicantUserId INT NULL,
    ApplicantLastName NVARCHAR(100) NULL,
    ApplicantFirstName NVARCHAR(100) NULL,
    ApplicantMiddleName NVARCHAR(100) NULL,
    Phone NVARCHAR(20) NULL,
    ApplicantEmail NVARCHAR(200) NULL,
    Organization NVARCHAR(200) NULL,
    Note NVARCHAR(4000) NULL,
    BirthDate DATETIME2 NULL,
    PassportSeries NVARCHAR(10) NULL,
    PassportNumber NVARCHAR(10) NULL,
    Status NVARCHAR(50) NOT NULL DEFAULT 'Pending',
    RejectionReason NVARCHAR(2000) NULL,
    CreatedAt DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
    CheckedByUserId INT NULL,
    StatusID INT NOT NULL,
    GroupSize INT NULL,
    FOREIGN KEY (DepartmentId) REFERENCES Departments(Id),
    FOREIGN KEY (ResponsibleEmployeeId) REFERENCES Employees(Id),
    FOREIGN KEY (ApplicantUserId) REFERENCES Users(Id),
    FOREIGN KEY (CheckedByUserId) REFERENCES Users(Id),
    FOREIGN KEY (StatusID) REFERENCES ApplicationStatuses(StatusID)
);

CREATE TABLE Groups (
    GroupID INT IDENTITY PRIMARY KEY,
    GroupName NVARCHAR(200) NULL,
    CreatedAt DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME()
);

CREATE TABLE PassVisitors (
    Id INT IDENTITY PRIMARY KEY,
    PassRequestId INT NOT NULL,
    LastName NVARCHAR(100) NOT NULL,
    FirstName NVARCHAR(100) NOT NULL,
    MiddleName NVARCHAR(100) NULL,
    Phone NVARCHAR(20) NULL,
    Email NVARCHAR(200) NULL,
    PassportSeries NVARCHAR(10) NOT NULL,
    PassportNumber NVARCHAR(10) NOT NULL,
    PhotoPath NVARCHAR(500) NULL,
    IsBlacklisted BIT NOT NULL DEFAULT 0,
    FOREIGN KEY (PassRequestId) REFERENCES PassRequests(Id)
);

------------------------------------------------------------
-- 4. Files and attachments
------------------------------------------------------------

CREATE TABLE FileAttachments (
    Id INT IDENTITY PRIMARY KEY,
    PassRequestId INT NULL,
    PassVisitorId INT NULL,
    FileTypeID INT NOT NULL,
    DepartmentId INT NULL,
    FilePath NVARCHAR(500) NOT NULL,
    FileName NVARCHAR(100) NOT NULL,
    ContentType NVARCHAR(50) NOT NULL,
    FileSize BIGINT NOT NULL,
    UploadedAt DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
    FOREIGN KEY (PassRequestId) REFERENCES PassRequests(Id),
    FOREIGN KEY (PassVisitorId) REFERENCES PassVisitors(Id),
    FOREIGN KEY (FileTypeID) REFERENCES FileTypes(FileTypeID),
    FOREIGN KEY (DepartmentId) REFERENCES Departments(Id)
);

------------------------------------------------------------
-- 5. Security and monitoring
------------------------------------------------------------

CREATE TABLE BlacklistEntries (
    Id INT IDENTITY PRIMARY KEY,
    LastName NVARCHAR(100) NOT NULL,
    FirstName NVARCHAR(100) NOT NULL,
    MiddleName NVARCHAR(100) NULL,
    PassportSeries NVARCHAR(10) NULL,
    PassportNumber NVARCHAR(10) NULL,
    Reason NVARCHAR(5000) NOT NULL,
    AddedAt DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
    AddedByEmployeeId INT NULL,
    ReasonID INT NULL,
    FOREIGN KEY (AddedByEmployeeId) REFERENCES Employees(Id),
    FOREIGN KEY (ReasonID) REFERENCES ViolationReasons(ReasonID)
);

CREATE TABLE SecurityLogs (
    Id INT IDENTITY PRIMARY KEY,
    Timestamp DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
    EventType NVARCHAR(100) NOT NULL,
    EmployeeId INT NULL,
    PassRequestId INT NULL,
    Details NVARCHAR(2000) NULL,
    FOREIGN KEY (EmployeeId) REFERENCES Employees(Id),
    FOREIGN KEY (PassRequestId) REFERENCES PassRequests(Id)
);

CREATE TABLE VisitRecords (
    Id INT IDENTITY PRIMARY KEY,
    PassRequestId INT NOT NULL,
    VisitorId INT NULL,
    EntryTime DATETIME2 NULL,
    ExitTime DATETIME2 NULL,
    DepartmentId INT NULL,
    FOREIGN KEY (PassRequestId) REFERENCES PassRequests(Id),
    FOREIGN KEY (VisitorId) REFERENCES PassVisitors(Id),
    FOREIGN KEY (DepartmentId) REFERENCES Departments(Id)
);

CREATE TABLE SecurityIncidents (
    Id INT IDENTITY PRIMARY KEY,
    OccurredAt DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
    Title NVARCHAR(500) NOT NULL,
    Details NVARCHAR(4000) NULL,
    DetectedByEmployeeId INT NULL,
    RelatedVisitRecordId INT NULL,
    FOREIGN KEY (DetectedByEmployeeId) REFERENCES Employees(Id),
    FOREIGN KEY (RelatedVisitRecordId) REFERENCES VisitRecords(Id)
);

CREATE TABLE AccessEvents (
    Id INT IDENTITY PRIMARY KEY,
    EventTime DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
    EventType NVARCHAR(100) NOT NULL,
    EmployeeId INT NULL,
    PassRequestId INT NULL,
    Metadata NVARCHAR(500) NULL,
    FOREIGN KEY (EmployeeId) REFERENCES Employees(Id),
    FOREIGN KEY (PassRequestId) REFERENCES PassRequests(Id)
);

------------------------------------------------------------
-- 6. Communications and auditing
------------------------------------------------------------

CREATE TABLE Notifications (
    Id INT IDENTITY PRIMARY KEY,
    UserId INT NULL,
    Title NVARCHAR(200) NOT NULL,
    Body NVARCHAR(MAX) NOT NULL,
    CreatedAt DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
    IsRead BIT NOT NULL DEFAULT 0,
    FOREIGN KEY (UserId) REFERENCES Users(Id)
);

CREATE TABLE AuditLogs (
    Id INT IDENTITY PRIMARY KEY,
    Timestamp DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
    UserId INT NULL,
    Action NVARCHAR(200) NOT NULL,
    Details NVARCHAR(4000) NULL,
    FOREIGN KEY (UserId) REFERENCES Users(Id)
);


------------------------------------------------------------
-- 7. Configuration and templates
------------------------------------------------------------

CREATE TABLE PolicyRules (
    Id INT IDENTITY PRIMARY KEY,
    Name NVARCHAR(200) NOT NULL,
    Description NVARCHAR(4000) NULL
);

CREATE TABLE WorkingTimeRules (
    Id INT IDENTITY PRIMARY KEY,
    EmployeeId INT NULL,
    DepartmentId INT NULL,
    WorkStart TIME NOT NULL,
    WorkEnd TIME NOT NULL,
    Note NVARCHAR(200) NULL,
    FOREIGN KEY (EmployeeId) REFERENCES Employees(Id),
    FOREIGN KEY (DepartmentId) REFERENCES Departments(Id)
);

CREATE TABLE ImportTemplates (
    Id INT IDENTITY PRIMARY KEY,
    TemplateName NVARCHAR(200) NOT NULL,
    Description NVARCHAR(MAX) NOT NULL,
    ExampleFilePath NVARCHAR(500) NULL
);

CREATE TABLE SystemSettings (
    Id INT IDENTITY PRIMARY KEY,
    [Key] NVARCHAR(200) NOT NULL,
    Value NVARCHAR(MAX) NOT NULL,
    Description NVARCHAR(1000) NULL
);

------------------------------------------------------------
-- 8. Reports and documents
------------------------------------------------------------

CREATE TABLE Reports (
    Id INT IDENTITY PRIMARY KEY,
    ReportType NVARCHAR(200) NOT NULL,
    PeriodStart DATETIME2 NOT NULL,
    PeriodEnd DATETIME2 NOT NULL,
    PdfFilePath NVARCHAR(500) NULL,
    GeneratedAt DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME()
);

------------------------------------------------------------
-- 9. Access tokens and schedules
------------------------------------------------------------

CREATE TABLE AccessTokens (
    TokenID INT IDENTITY PRIMARY KEY,
    TokenValue NVARCHAR(255) NOT NULL,
    UserId INT NULL,
    EmployeeId INT NULL,
    CreatedAt DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
    ExpiresAt DATETIME2 NULL,
    FOREIGN KEY (UserId) REFERENCES Users(Id),
    FOREIGN KEY (EmployeeId) REFERENCES Employees(Id)
);

CREATE TABLE WorkSchedules (
    ScheduleID INT IDENTITY PRIMARY KEY,
    EmployeeID INT NOT NULL,
    WorkDate DATE NOT NULL,
    StartTime TIME NOT NULL,
    EndTime TIME NOT NULL,
    FOREIGN KEY (EmployeeID) REFERENCES Employees(Id)
);

CREATE TABLE FreeTimeSlots (
    SlotID INT IDENTITY PRIMARY KEY,
    EmployeeID INT NOT NULL,
    SlotRange NVARCHAR(50) NOT NULL,
    CalculatedAt DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
    FOREIGN KEY (EmployeeID) REFERENCES Employees(Id)
);

------------------------------------------------------------
-- Completion
------------------------------------------------------------
PRINT 'Database HranitelPRO schema created.';
GO
