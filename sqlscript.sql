CREATE DATABASE HranitelPRO;
GO
USE HranitelPRO;
GO

------------------------------------------------------------
-- 1. Справочники и базовые таблицы
------------------------------------------------------------

CREATE TABLE Role (
    RoleID INT IDENTITY PRIMARY KEY,
    RoleName NVARCHAR(100) NOT NULL
);

CREATE TABLE Department (
    DepartmentID INT IDENTITY PRIMARY KEY,
    DepartmentName NVARCHAR(200) NOT NULL,
    Description NVARCHAR(500)
);

CREATE TABLE ApplicationStatus (
    StatusID INT IDENTITY PRIMARY KEY,
    StatusName NVARCHAR(50) NOT NULL
);

CREATE TABLE FileType (
    FileTypeID INT IDENTITY PRIMARY KEY,
    TypeName NVARCHAR(100) NOT NULL
);

CREATE TABLE ViolationReason (
    ReasonID INT IDENTITY PRIMARY KEY,
    ReasonText NVARCHAR(MAX) NOT NULL
);

------------------------------------------------------------
-- 2. Пользователи и сотрудники
------------------------------------------------------------

CREATE TABLE [User] (
    UserID INT IDENTITY PRIMARY KEY,
    Email NVARCHAR(200) NOT NULL UNIQUE,
    PasswordHash NVARCHAR(255) NOT NULL,
    FullName NVARCHAR(200) NOT NULL,
    Phone NVARCHAR(20),
    RoleID INT NOT NULL,
    FOREIGN KEY (RoleID) REFERENCES Role(RoleID)
);

CREATE TABLE Employee (
    EmployeeID INT IDENTITY PRIMARY KEY,
    FullName NVARCHAR(200) NOT NULL,
    Code NVARCHAR(50) NOT NULL UNIQUE,
    DepartmentID INT NOT NULL,
    RoleID INT NOT NULL,
    FOREIGN KEY (DepartmentID) REFERENCES Department(DepartmentID),
    FOREIGN KEY (RoleID) REFERENCES Role(RoleID)
);

------------------------------------------------------------
-- 3. Заявки, группы и посетители
------------------------------------------------------------

CREATE TABLE [Group] (
    GroupID INT IDENTITY PRIMARY KEY,
    GroupName NVARCHAR(200),
    CreatedAt DATETIME2 DEFAULT GETDATE()
);

CREATE TABLE Application (
    ApplicationID INT IDENTITY PRIMARY KEY,
    ApplicationType NVARCHAR(20) CHECK (ApplicationType IN ('Individual','Group')),
    CreatedAt DATETIME2 DEFAULT GETDATE(),
    VisitStartDate DATE NOT NULL,
    VisitEndDate DATE NOT NULL,
    Purpose NVARCHAR(500) NOT NULL,
    Notes NVARCHAR(500),
    StatusID INT NOT NULL,
    DepartmentID INT NOT NULL,
    EmployeeReceiverID INT NOT NULL,
    UserID INT NOT NULL,
    GroupID INT NULL,
    FOREIGN KEY (StatusID) REFERENCES ApplicationStatus(StatusID),
    FOREIGN KEY (DepartmentID) REFERENCES Department(DepartmentID),
    FOREIGN KEY (EmployeeReceiverID) REFERENCES Employee(EmployeeID),
    FOREIGN KEY (UserID) REFERENCES [User](UserID),
    FOREIGN KEY (GroupID) REFERENCES [Group](GroupID)
);

CREATE TABLE Visitor (
    VisitorID INT IDENTITY PRIMARY KEY,
    LastName NVARCHAR(100) NOT NULL,
    FirstName NVARCHAR(100) NOT NULL,
    MiddleName NVARCHAR(100),
    BirthDate DATE NOT NULL,
    Phone NVARCHAR(20),
    Email NVARCHAR(200),
    Organization NVARCHAR(200),
    PassportSeries NVARCHAR(10) NOT NULL,
    PassportNumber NVARCHAR(10) NOT NULL,
    Comment NVARCHAR(500),
    ApplicationID INT NULL,
    GroupID INT NULL,
    FOREIGN KEY (ApplicationID) REFERENCES Application(ApplicationID),
    FOREIGN KEY (GroupID) REFERENCES [Group](GroupID)
);

------------------------------------------------------------
-- 4. Посещения и контроль доступа
------------------------------------------------------------

CREATE TABLE Visit (
    VisitID INT IDENTITY PRIMARY KEY,
    ApplicationID INT NOT NULL,
    VisitorID INT NOT NULL,
    SecurityEmployeeID INT NOT NULL,
    EntryTime DATETIME2,
    ExitTime DATETIME2,
    IsAccessGranted BIT DEFAULT 0,
    FOREIGN KEY (ApplicationID) REFERENCES Application(ApplicationID),
    FOREIGN KEY (VisitorID) REFERENCES Visitor(VisitorID),
    FOREIGN KEY (SecurityEmployeeID) REFERENCES Employee(EmployeeID)
);

CREATE TABLE Blacklist (
    BlacklistID INT IDENTITY PRIMARY KEY,
    VisitorID INT NOT NULL,
    AddedByEmployeeID INT NOT NULL,
    ReasonID INT NOT NULL,
    AddedAt DATETIME2 DEFAULT GETDATE(),
    FOREIGN KEY (VisitorID) REFERENCES Visitor(VisitorID),
    FOREIGN KEY (AddedByEmployeeID) REFERENCES Employee(EmployeeID),
    FOREIGN KEY (ReasonID) REFERENCES ViolationReason(ReasonID)
);

CREATE TABLE SecurityRule (
    RuleID INT IDENTITY PRIMARY KEY,
    DepartmentID INT NOT NULL,
    Description NVARCHAR(500),
    MaxTravelMinutes INT NOT NULL,
    FOREIGN KEY (DepartmentID) REFERENCES Department(DepartmentID)
);

------------------------------------------------------------
-- 5. Файлы и вложения
------------------------------------------------------------

CREATE TABLE AttachedFile (
    FileID INT IDENTITY PRIMARY KEY,
    FileName NVARCHAR(255) NOT NULL,
    FilePath NVARCHAR(500) NOT NULL,
    UploadedAt DATETIME2 DEFAULT GETDATE(),
    FileTypeID INT NOT NULL,
    VisitorID INT NULL,
    ApplicationID INT NULL,
    DepartmentID INT NULL,
    FOREIGN KEY (FileTypeID) REFERENCES FileType(FileTypeID),
    FOREIGN KEY (VisitorID) REFERENCES Visitor(VisitorID),
    FOREIGN KEY (ApplicationID) REFERENCES Application(ApplicationID),
    FOREIGN KEY (DepartmentID) REFERENCES Department(DepartmentID)
);

------------------------------------------------------------
-- 6. Отчеты и логи
------------------------------------------------------------

CREATE TABLE Report (
    ReportID INT IDENTITY PRIMARY KEY,
    EmployeeID INT NOT NULL,
    CreatedAt DATETIME2 DEFAULT GETDATE(),
    ReportType NVARCHAR(100),
    FilePath NVARCHAR(500),
    FOREIGN KEY (EmployeeID) REFERENCES Employee(EmployeeID)
);

CREATE TABLE EventLog (
    LogID INT IDENTITY PRIMARY KEY,
    EventTime DATETIME2 DEFAULT GETDATE(),
    EventType NVARCHAR(100),
    Description NVARCHAR(1000),
    UserID INT NULL,
    EmployeeID INT NULL,
    FOREIGN KEY (UserID) REFERENCES [User](UserID),
    FOREIGN KEY (EmployeeID) REFERENCES Employee(EmployeeID)
);

------------------------------------------------------------
-- 7. Авторизация и рабочие графики
------------------------------------------------------------

CREATE TABLE AccessToken (
    TokenID INT IDENTITY PRIMARY KEY,
    TokenValue NVARCHAR(255) NOT NULL,
    UserID INT NULL,
    EmployeeID INT NULL,
    CreatedAt DATETIME2 DEFAULT GETDATE(),
    ExpiresAt DATETIME2,
    FOREIGN KEY (UserID) REFERENCES [User](UserID),
    FOREIGN KEY (EmployeeID) REFERENCES Employee(EmployeeID)
);

CREATE TABLE WorkSchedule (
    ScheduleID INT IDENTITY PRIMARY KEY,
    EmployeeID INT NOT NULL,
    WorkDate DATE NOT NULL,
    StartTime TIME NOT NULL,
    EndTime TIME NOT NULL,
    FOREIGN KEY (EmployeeID) REFERENCES Employee(EmployeeID)
);

------------------------------------------------------------
-- 8. Просчитанные интервалы (временные)
------------------------------------------------------------

CREATE TABLE FreeTimeSlot (
    SlotID INT IDENTITY PRIMARY KEY,
    EmployeeID INT NOT NULL,
    SlotRange NVARCHAR(50),
    CalculatedAt DATETIME2 DEFAULT GETDATE(),
    FOREIGN KEY (EmployeeID) REFERENCES Employee(EmployeeID)
);

------------------------------------------------------------
-- Завершение
------------------------------------------------------------
PRINT 'База данных HranitelPRO успешно создана.';
GO
