IF DB_ID(N'HranitelPRO') IS NULL
BEGIN
    CREATE DATABASE HranitelPRO;
END
GO
USE HranitelPRO;
GO

------------------------------------------------------------
-- 1. Reference tables
------------------------------------------------------------

IF OBJECT_ID(N'dbo.Roles', N'U') IS NULL
BEGIN
    CREATE TABLE Roles (
        Id INT IDENTITY PRIMARY KEY,
        Name NVARCHAR(100) NOT NULL,
        Description NVARCHAR(200) NULL
    );
END
GO

IF OBJECT_ID(N'dbo.Departments', N'U') IS NULL
BEGIN
    CREATE TABLE Departments (
        Id INT IDENTITY PRIMARY KEY,
        Name NVARCHAR(200) NOT NULL,
        Description NVARCHAR(1000) NULL
    );
END
GO

IF OBJECT_ID(N'dbo.ApplicationStatuses', N'U') IS NULL
BEGIN
    CREATE TABLE ApplicationStatuses (
        StatusID INT IDENTITY PRIMARY KEY,
        StatusName NVARCHAR(50) NOT NULL
    );
END
GO

IF OBJECT_ID(N'dbo.FileTypes', N'U') IS NULL
BEGIN
    CREATE TABLE FileTypes (
        FileTypeID INT IDENTITY PRIMARY KEY,
        TypeName NVARCHAR(100) NOT NULL
    );
END
GO

IF OBJECT_ID(N'dbo.ViolationReasons', N'U') IS NULL
BEGIN
    CREATE TABLE ViolationReasons (
        ReasonID INT IDENTITY PRIMARY KEY,
        ReasonText NVARCHAR(MAX) NOT NULL
    );
END
GO

------------------------------------------------------------
-- 2. Users and employees
------------------------------------------------------------

IF OBJECT_ID(N'dbo.Employees', N'U') IS NULL
BEGIN
    CREATE TABLE Employees (
        Id INT IDENTITY PRIMARY KEY,
        FullName NVARCHAR(200) NOT NULL,
        EmployeeCode NVARCHAR(50) NULL UNIQUE,
        DepartmentId INT NULL,
        FOREIGN KEY (DepartmentId) REFERENCES Departments(Id)
    );
END
GO

IF OBJECT_ID(N'dbo.Users', N'U') IS NULL
BEGIN
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
END
GO

------------------------------------------------------------
-- 3. Pass requests and visitors
------------------------------------------------------------

IF OBJECT_ID(N'dbo.PassRequests', N'U') IS NULL
BEGIN
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
END
GO

IF OBJECT_ID(N'dbo.Groups', N'U') IS NULL
BEGIN
    CREATE TABLE Groups (
        GroupID INT IDENTITY PRIMARY KEY,
        GroupName NVARCHAR(200) NULL,
        Description NVARCHAR(500) NULL,
        CreatedAt DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME()
    );
END
GO

IF OBJECT_ID(N'dbo.PassVisitors', N'U') IS NULL
BEGIN
    CREATE TABLE PassVisitors (
        Id INT IDENTITY PRIMARY KEY,
        PassRequestId INT NULL,
        LastName NVARCHAR(100) NOT NULL,
        FirstName NVARCHAR(100) NOT NULL,
        MiddleName NVARCHAR(100) NULL,
        Phone NVARCHAR(20) NULL,
        Email NVARCHAR(200) NULL,
        BirthDate DATETIME2 NULL,
        PassportSeries NVARCHAR(10) NOT NULL,
        PassportNumber NVARCHAR(10) NOT NULL,
        PhotoPath NVARCHAR(500) NULL,
        IsBlacklisted BIT NOT NULL DEFAULT 0,
        GroupID INT NULL,
        FOREIGN KEY (PassRequestId) REFERENCES PassRequests(Id),
        FOREIGN KEY (GroupID) REFERENCES Groups(GroupID)
    );
END
GO

------------------------------------------------------------
-- 4. Files and attachments
------------------------------------------------------------

IF OBJECT_ID(N'dbo.FileAttachments', N'U') IS NULL
BEGIN
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
END
GO

------------------------------------------------------------
-- 5. Security and monitoring
------------------------------------------------------------

IF OBJECT_ID(N'dbo.BlacklistEntries', N'U') IS NULL
BEGIN
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
END
GO

IF OBJECT_ID(N'dbo.SecurityLogs', N'U') IS NULL
BEGIN
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
END
GO

IF OBJECT_ID(N'dbo.VisitRecords', N'U') IS NULL
BEGIN
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
END
GO

IF OBJECT_ID(N'dbo.SecurityIncidents', N'U') IS NULL
BEGIN
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
END
GO

IF OBJECT_ID(N'dbo.AccessEvents', N'U') IS NULL
BEGIN
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
END
GO

------------------------------------------------------------
-- 6. Communications and auditing
------------------------------------------------------------

IF OBJECT_ID(N'dbo.Notifications', N'U') IS NULL
BEGIN
    CREATE TABLE Notifications (
        Id INT IDENTITY PRIMARY KEY,
        UserId INT NULL,
        Title NVARCHAR(200) NOT NULL,
        Body NVARCHAR(MAX) NOT NULL,
        CreatedAt DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
        IsRead BIT NOT NULL DEFAULT 0,
        FOREIGN KEY (UserId) REFERENCES Users(Id)
    );
END
GO

IF OBJECT_ID(N'dbo.AuditLogs', N'U') IS NULL
BEGIN
    CREATE TABLE AuditLogs (
        Id INT IDENTITY PRIMARY KEY,
        Timestamp DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
        UserId INT NULL,
        Action NVARCHAR(200) NOT NULL,
        Details NVARCHAR(4000) NULL,
        FOREIGN KEY (UserId) REFERENCES Users(Id)
    );
END
GO


------------------------------------------------------------
-- 7. Configuration and templates
------------------------------------------------------------

IF OBJECT_ID(N'dbo.PolicyRules', N'U') IS NULL
BEGIN
    CREATE TABLE PolicyRules (
        Id INT IDENTITY PRIMARY KEY,
        Name NVARCHAR(200) NOT NULL,
        Description NVARCHAR(4000) NULL
    );
END
GO

IF OBJECT_ID(N'dbo.WorkingTimeRules', N'U') IS NULL
BEGIN
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
END
GO

IF OBJECT_ID(N'dbo.ImportTemplates', N'U') IS NULL
BEGIN
    CREATE TABLE ImportTemplates (
        Id INT IDENTITY PRIMARY KEY,
        TemplateName NVARCHAR(200) NOT NULL,
        Description NVARCHAR(MAX) NOT NULL,
        ExampleFilePath NVARCHAR(500) NULL
    );
END
GO

IF OBJECT_ID(N'dbo.SystemSettings', N'U') IS NULL
BEGIN
    CREATE TABLE SystemSettings (
        Id INT IDENTITY PRIMARY KEY,
        [Key] NVARCHAR(200) NOT NULL,
        Value NVARCHAR(MAX) NOT NULL,
        Description NVARCHAR(1000) NULL
    );
END
GO

------------------------------------------------------------
-- 8. Reports and documents
------------------------------------------------------------

IF OBJECT_ID(N'dbo.Reports', N'U') IS NULL
BEGIN
    CREATE TABLE Reports (
        Id INT IDENTITY PRIMARY KEY,
        ReportType NVARCHAR(200) NOT NULL,
        PeriodStart DATETIME2 NOT NULL,
        PeriodEnd DATETIME2 NOT NULL,
        PdfFilePath NVARCHAR(500) NULL,
        GeneratedAt DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME()
    );
END
GO

------------------------------------------------------------
-- 9. Access tokens and schedules
------------------------------------------------------------

IF OBJECT_ID(N'dbo.AccessTokens', N'U') IS NULL
BEGIN
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
END
GO

IF OBJECT_ID(N'dbo.WorkSchedules', N'U') IS NULL
BEGIN
    CREATE TABLE WorkSchedules (
        ScheduleID INT IDENTITY PRIMARY KEY,
        EmployeeID INT NOT NULL,
        WorkDate DATE NOT NULL,
        StartTime TIME NOT NULL,
        EndTime TIME NOT NULL,
        FOREIGN KEY (EmployeeID) REFERENCES Employees(Id)
    );
END
GO

IF OBJECT_ID(N'dbo.FreeTimeSlots', N'U') IS NULL
BEGIN
    CREATE TABLE FreeTimeSlots (
        SlotID INT IDENTITY PRIMARY KEY,
        EmployeeID INT NOT NULL,
        SlotRange NVARCHAR(50) NOT NULL,
        CalculatedAt DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
        FOREIGN KEY (EmployeeID) REFERENCES Employees(Id)
    );
END
GO

------------------------------------------------------------
-- Completion
------------------------------------------------------------
PRINT 'Database HranitelPRO schema created.';
GO

------------------------------------------------------------
-- 10. Seed data generated from provided visitor tables
------------------------------------------------------------
-- Normalize legacy SHA256 hashes to include algorithm prefix
UPDATE Users
SET Email = LTRIM(RTRIM(Email))
WHERE Email <> LTRIM(RTRIM(Email));

UPDATE Users
SET PasswordHash = SUBSTRING(PasswordHash, 3, LEN(PasswordHash) - 2)
WHERE PasswordHash LIKE N'0x%' AND LEN(PasswordHash) = 66;

UPDATE Users
SET PasswordHash = N'SHA256::' + PasswordHash
WHERE PasswordHash NOT LIKE N'SHA256::%' AND LEN(PasswordHash) = 64;
GO

-- Roles
MERGE Roles AS target
USING (VALUES
    (N'Администратор системы', N'Полный доступ к настройкам и управлению платформой.'),
    (N'Оператор пропусков', N'Обработка заявок на посещение и управление посетителями.'),
    (N'Сотрудник безопасности', N'Контроль доступа и журналирование инцидентов.'),
    (N'Сотрудник подразделения', N'Работа с заявками и посещениями своего подразделения.'),
    (N'Посетитель', N'Доступ к личному кабинету посетителя для подачи заявок и отслеживания статусов.')
) AS source (Name, Description)
ON target.Name = source.Name
WHEN MATCHED THEN UPDATE SET Description = source.Description
WHEN NOT MATCHED THEN INSERT (Name, Description) VALUES (source.Name, source.Description);
GO

-- Departments
MERGE Departments AS target
USING (VALUES
    (N'Производство'),
    (N'Сбыт'),
    (N'Администрация'),
    (N'Служба безопасности'),
    (N'Планирование'),
    (N'Общий отдел'),
    (N'Охрана')
) AS source (Name)
ON target.Name = source.Name
WHEN NOT MATCHED THEN INSERT (Name) VALUES (source.Name);
GO

-- Application statuses
MERGE ApplicationStatuses AS target
USING (VALUES (N'Pending')) AS source (StatusName)
ON target.StatusName = source.StatusName
WHEN NOT MATCHED THEN INSERT (StatusName) VALUES (source.StatusName);
GO

-- Employees
;WITH EmployeeSeed AS (
    SELECT N'Фомичева Авдотья Трофимовна' AS FullName, N'9367788' AS EmployeeCode, N'Производство' AS DepartmentName
    UNION ALL SELECT N'Гаврилова Римма Ефимовна', N'9788737', N'Сбыт'
    UNION ALL SELECT N'Носкова Наталия Прохоровна', N'9736379', N'Администрация'
    UNION ALL SELECT N'Архипов Тимофей Васильевич', N'9362832', N'Служба безопасности'
    UNION ALL SELECT N'Орехова Вероника Артемовна', N'9737848', N'Планирование'
    UNION ALL SELECT N'Савельев Павел Степанович', N'9768239', N'Общий отдел'
    UNION ALL SELECT N'Чернов Всеволод Наумович', N'9404040', N'Охрана'
    UNION ALL SELECT N'Марков Юрий Романович', NULL, NULL
)
MERGE Employees AS target
USING (
    SELECT
        s.FullName,
        s.EmployeeCode,
        d.Id AS DepartmentId
    FROM EmployeeSeed s
    LEFT JOIN Departments d ON d.Name = s.DepartmentName
) AS source
ON ISNULL(target.EmployeeCode, target.FullName) = ISNULL(source.EmployeeCode, source.FullName)
WHEN MATCHED THEN UPDATE SET
    FullName = source.FullName,
    DepartmentId = source.DepartmentId
WHEN NOT MATCHED THEN INSERT (FullName, EmployeeCode, DepartmentId)
    VALUES (source.FullName, source.EmployeeCode, source.DepartmentId);
GO

-- Users
;WITH UserSeed AS (
    SELECT N'Администратор системы' AS FullName, N'admin@hranitelpro.local' AS Email, N'Admin@Hranitel2023' AS PlainPassword, CAST(NULL AS NVARCHAR(50)) AS EmployeeCode, N'Администратор системы' AS RoleName
    UNION ALL SELECT N'Фомичева Авдотья Трофимовна', N'afomicheva@hranitelpro.local', N'Fomicheva#Pass1', N'9367788', N'Оператор пропусков'
    UNION ALL SELECT N'Архипов Тимофей Васильевич', N'tarchipov@hranitelpro.local', N'Security!Archipov', N'9362832', N'Сотрудник безопасности'
    UNION ALL SELECT N'Гаврилова Римма Ефимовна', N'rgavrilova@hranitelpro.local', N'SalesRimma2023', N'9788737', N'Сотрудник подразделения'
    UNION ALL SELECT N'Носкова Наталия Прохоровна', N'nnoskova@hranitelpro.local', N'AdminDept#2023', N'9736379', N'Сотрудник подразделения'
)
MERGE Users AS target
USING (
    SELECT
        s.FullName,
        s.Email,
        N'SHA256::' + CONVERT(NVARCHAR(128), HASHBYTES('SHA2_256', s.PlainPassword), 2) AS PasswordHash,
        CAST(1 AS BIT) AS EmailConfirmed,
        e.Id AS EmployeeId,
        r.Id AS RoleId
    FROM UserSeed s
    LEFT JOIN Employees e ON e.EmployeeCode = s.EmployeeCode
    JOIN Roles r ON r.Name = s.RoleName
) AS source
ON target.Email = source.Email
WHEN MATCHED THEN UPDATE SET
    FullName = source.FullName,
    PasswordHash = source.PasswordHash,
    EmailConfirmed = source.EmailConfirmed,
    EmployeeId = source.EmployeeId,
    RoleId = source.RoleId
WHEN NOT MATCHED THEN INSERT (FullName, Email, PasswordHash, EmailConfirmed, EmployeeId, RoleId)
    VALUES (source.FullName, source.Email, source.PasswordHash, source.EmailConfirmed, source.EmployeeId, source.RoleId);
GO

-- Visitor accounts
;WITH VisitorSeed AS (
    SELECT v.FullName, v.Email, v.PlainPassword
    FROM (VALUES
        (N'Степанова Радинка Власовна', N'Radinka100@yandex.ru', N'b3uWS6#Thuvq'),
        (N'Шилов Прохор Герасимович', N'Prohor156@list.ru', N'zDdom}SIhWs?'),
        (N'Кононов Юрин Романович', N'YUrin155@gmail.com', N'u@m*~ACBCqNQ'),
        (N'Елисеева Альбина Николаевна', N'Aljbina33@lenta.ru', N'Bu?BHCtwDFin'),
        (N'Шарова Клавдия Макаровна', N'Klavdiya113@live.com', N'FjC#hNIJori}'),
        (N'Сидорова Тамара Григорьевна', N'Tamara179@live.com', N'TJxVqMXrbesI'),
        (N'Петухов Тарас Фадеевич', N'Taras24@rambler.ru', N'07m5yspn3K~K'),
        (N'Родионов Аркадий Власович', N'Arkadij123@inbox.ru', N'vk2N7lxX}ck%'),
        (N'Горшкова Глафира Валентиновна', N'Glafira73@outlook.com', N'Zz8POQlP}M4~'),
        (N'Кириллова Гавриила Яковна', N'Gavriila68@msn.com', N'x4K5WthEe8ua'),
        (N'Овчинников Кузьма Ефимович', N'Kuzjma124@yandex.ru', N'OsByQJ}vYznW'),
        (N'Беляков Роман Викторович', N'Roman89@gmail.com', N'Xd?xP$2yICcG'),
        (N'Лыткин Алексей Максимович', N'Aleksej43@gmail.com', N'~c%PlTY0?qgl'),
        (N'Шубина Надежда Викторовна', N'Nadezhda137@outlook.com', N'QQ~0q~rXHb?p'),
        (N'Зиновьева Бронислава Викторовна', N'Bronislava56@yahoo.com', N'LO}xyC~1S4l6'),
        (N'Самойлова Таисия Гермоновна', N'Taisiya177@lenta.ru', N'R94YGT3XFjgD'),
        (N'Ситникова Аделаида Гермоновна', N'Adelaida20@hotmail.com', N'LCY*{L*fEGYB'),
        (N'Исаев Лев Юлианович', N'Lev131@rambler.ru', N'~?oj2Lh@S7*T'),
        (N'Никифоров Даниил Степанович', N'Daniil198@bk.ru', N'L2W#uo7z{EsO'),
        (N'Титова Людмила Якововна', N'Lyudmila123@hotmail.com', N'@8mk9M?NBAGj'),
        (N'Абрамова Таисия Дмитриевна', N'Taisiya176@hotmail.com', N'~rIWfsnXA~7C'),
        (N'Кузьмина Вера Максимовна', N'Vera195@list.ru', N'B|5v$2r$7luL'),
        (N'Мартынов Яков Ростиславович', N'YAkov196@rambler.ru', N'$6s5bggKP7aw'),
        (N'Евсеева Нина Павловна', N'Nina145@msn.com', N'Uxy6RtBXIcpT'),
        (N'Голубев Леонтий Вячеславович', N'Leontij161@mail.ru', N'KkMY8yKw@oCa'),
        (N'Карпова Серафима Михаиловна', N'Serafima169@yahoo.com', N'gNe3I9}8J3Z@'),
        (N'Орехов Сергей Емельянович', N'Sergej35@inbox.ru', N'$39vc%ljqN%r'),
        (N'Исаев Георгий Павлович', N'Georgij121@inbox.ru', N'bbx5H}f*BC?w'),
        (N'Богданов Елизар Артемович', N'Elizar30@yandex.ru', N'wJs1~r3RS~dr'),
        (N'Тихонова Лана Семеновна', N'Lana117@outlook.com', N'mFoG$jcS3c4~')
    ) AS v(FullName, Email, PlainPassword)
)
MERGE Users AS target
USING (
    SELECT
        v.FullName,
        v.Email,
        N'SHA256::' + CONVERT(NVARCHAR(128), HASHBYTES('SHA2_256', v.PlainPassword), 2) AS PasswordHash,
        CAST(1 AS BIT) AS EmailConfirmed,
        CAST(NULL AS INT) AS EmployeeId,
        r.Id AS RoleId
    FROM VisitorSeed v
    JOIN Roles r ON r.Name = N'Посетитель'
) AS source
ON target.Email = source.Email
WHEN MATCHED THEN UPDATE SET
    FullName = source.FullName,
    PasswordHash = source.PasswordHash,
    EmailConfirmed = source.EmailConfirmed,
    EmployeeId = source.EmployeeId,
    RoleId = source.RoleId
WHEN NOT MATCHED THEN INSERT (FullName, Email, PasswordHash, EmailConfirmed, EmployeeId, RoleId)
    VALUES (source.FullName, source.Email, source.PasswordHash, source.EmailConfirmed, source.EmployeeId, source.RoleId);
GO

-- Pass requests generated from visitor assignments
;WITH PassRequestSeed AS (
    SELECT v.Purpose, v.StartDate, v.EndDate, v.Phone, v.ApplicantEmail, v.GroupSize, v.Note, v.EmployeeCode
    FROM (VALUES
        (N'Назначение 24/04/2023_9367788', '2023-04-24T09:00:00', '2023-04-24T18:00:00', N'+7 (613) 272-60-62', N'Radinka100@yandex.ru', 16, N'Создано на основе предоставленных данных', N'9367788'),
        (N'Назначение 24/04/2023_9736379', '2023-04-24T09:00:00', '2023-04-24T18:00:00', N'+7 (784) 673-51-91', N'YUrin155@gmail.com', 1, N'Создано на основе предоставленных данных', N'9736379'),
        (N'Назначение 24/04/2023_9788737', '2023-04-24T09:00:00', '2023-04-24T18:00:00', N'+7 (615) 594-77-66', N'Prohor156@list.ru', 1, N'Создано на основе предоставленных данных', N'9788737'),
        (N'Назначение 25/04/2023_9367788', '2023-04-25T09:00:00', '2023-04-25T18:00:00', N'+7 (654) 864-77-46', N'Aljbina33@lenta.ru', 1, N'Создано на основе предоставленных данных', N'9367788'),
        (N'Назначение 25/04/2023_9736379', '2023-04-25T09:00:00', '2023-04-25T18:00:00', N'+7 (334) 692-79-77', N'Tamara179@live.com', 1, N'Создано на основе предоставленных данных', N'9736379'),
        (N'Назначение 25/04/2023_9788737', '2023-04-25T09:00:00', '2023-04-25T18:00:00', N'+7 (822) 525-82-40', N'Klavdiya113@live.com', 1, N'Создано на основе предоставленных данных', N'9788737'),
        (N'Назначение 26/04/2023_9367788', '2023-04-26T09:00:00', '2023-04-26T18:00:00', N'+7 (376) 220-62-51', N'Taras24@rambler.ru', 1, N'Создано на основе предоставленных данных', N'9367788'),
        (N'Назначение 26/04/2023_9736379', '2023-04-26T09:00:00', '2023-04-26T18:00:00', N'+7 (553) 343-38-82', N'Glafira73@outlook.com', 1, N'Создано на основе предоставленных данных', N'9736379'),
        (N'Назначение 26/04/2023_9788737', '2023-04-26T09:00:00', '2023-04-26T18:00:00', N'+7 (491) 696-17-11', N'Arkadij123@inbox.ru', 1, N'Создано на основе предоставленных данных', N'9788737'),
        (N'Назначение 27/04/2023_9367788', '2023-04-27T09:00:00', '2023-04-27T18:00:00', N'+7 (648) 700-43-34', N'Gavriila68@msn.com', 1, N'Создано на основе предоставленных данных', N'9367788'),
        (N'Назначение 27/04/2023_9736379', '2023-04-27T09:00:00', '2023-04-27T18:00:00', N'+7 (595) 196-56-28', N'Roman89@gmail.com', 1, N'Создано на основе предоставленных данных', N'9736379'),
        (N'Назначение 27/04/2023_9788737', '2023-04-27T09:00:00', '2023-04-27T18:00:00', N'+7 (562) 866-15-27', N'Kuzjma124@yandex.ru', 1, N'Создано на основе предоставленных данных', N'9788737'),
        (N'Назначение 28/04/2023_9367788', '2023-04-28T09:00:00', '2023-04-28T18:00:00', N'+7 (994) 353-29-52', N'Aleksej43@gmail.com', 1, N'Создано на основе предоставленных данных', N'9367788'),
        (N'Назначение 28/04/2023_9736379', '2023-04-28T09:00:00', '2023-04-28T18:00:00', N'+7 (778) 565-12-18', N'Bronislava56@yahoo.com', 1, N'Создано на основе предоставленных данных', N'9736379'),
        (N'Назначение 28/04/2023_9788737', '2023-04-28T09:00:00', '2023-04-28T18:00:00', N'+7 (736) 488-66-95', N'Nadezhda137@outlook.com', 1, N'Создано на основе предоставленных данных', N'9788737')
    ) AS v(Purpose, StartDate, EndDate, Phone, ApplicantEmail, GroupSize, Note, EmployeeCode)
),
SourceData AS (
    SELECT
        'Personal' AS RequestType,
        s.Purpose,
        s.StartDate,
        s.EndDate,
        e.DepartmentId,
        e.Id AS ResponsibleEmployeeId,
        s.Phone,
        s.ApplicantEmail,
        status.StatusID,
        s.GroupSize,
        s.Note
    FROM PassRequestSeed s
    JOIN Employees e ON e.EmployeeCode = s.EmployeeCode
    JOIN ApplicationStatuses status ON status.StatusName = N'Pending'
)
UPDATE pr
SET
    RequestType = source.RequestType,
    StartDate = source.StartDate,
    EndDate = source.EndDate,
    DepartmentId = source.DepartmentId,
    ResponsibleEmployeeId = source.ResponsibleEmployeeId,
    Phone = source.Phone,
    ApplicantEmail = source.ApplicantEmail,
    StatusID = source.StatusID,
    GroupSize = source.GroupSize,
    Note = source.Note
FROM PassRequests pr
JOIN SourceData source ON pr.Purpose = source.Purpose;

INSERT INTO PassRequests (RequestType, StartDate, EndDate, Purpose, DepartmentId, ResponsibleEmployeeId, Phone, ApplicantEmail, StatusID, GroupSize, Note)
SELECT
    source.RequestType,
    source.StartDate,
    source.EndDate,
    source.Purpose,
    source.DepartmentId,
    source.ResponsibleEmployeeId,
    source.Phone,
    source.ApplicantEmail,
    source.StatusID,
    source.GroupSize,
    source.Note
FROM SourceData source
WHERE NOT EXISTS (SELECT 1 FROM PassRequests pr WHERE pr.Purpose = source.Purpose);
GO
-- Groups derived from group visitor data
MERGE Groups AS target
USING (VALUES
    (N'ГР1', N'24/04/2023_Производство_Фомичева_9367788_ГР1'),
    (N'ГР2', N'24/04/2023_Производство_Фомичева_9367788_ГР2')
) AS source (GroupName, Description)
ON target.Description = source.Description
WHEN MATCHED THEN UPDATE SET GroupName = source.GroupName
WHEN NOT MATCHED THEN INSERT (GroupName, Description) VALUES (source.GroupName, source.Description);
GO

-- Pass visitors (individual)
;WITH RequestLookup AS (
    SELECT Purpose, MIN(Id) AS PassRequestId
    FROM PassRequests
    GROUP BY Purpose
),
IndividualVisitors AS (
    SELECT v.Purpose, v.LastName, v.FirstName, NULLIF(v.MiddleName, N'') AS MiddleName, v.Phone, v.Email, CAST(v.BirthDate AS DATETIME2) AS BirthDate, v.PassportSeries, v.PassportNumber
    FROM (VALUES
        (N'Назначение 24/04/2023_9367788', N'Степанова', N'Радинка', N'Власовна', N'+7 (613) 272-60-62', N'Radinka100@yandex.ru', '1986-10-18', N'0208', N'530509'),
        (N'Назначение 24/04/2023_9788737', N'Шилов', N'Прохор', N'Герасимович', N'+7 (615) 594-77-66', N'Prohor156@list.ru', '1977-10-09', N'3036', N'796488'),
        (N'Назначение 24/04/2023_9736379', N'Кононов', N'Юрин', N'Романович', N'+7 (784) 673-51-91', N'YUrin155@gmail.com', '1971-10-08', N'2747', N'790512'),
        (N'Назначение 25/04/2023_9367788', N'Елисеева', N'Альбина', N'Николаевна', N'+7 (654) 864-77-46', N'Aljbina33@lenta.ru', '1983-02-15', N'5241', N'213304'),
        (N'Назначение 25/04/2023_9788737', N'Шарова', N'Клавдия', N'Макаровна', N'+7 (822) 525-82-40', N'Klavdiya113@live.com', '1980-07-22', N'8143', N'593309'),
        (N'Назначение 25/04/2023_9736379', N'Сидорова', N'Тамара', N'Григорьевна', N'+7 (334) 692-79-77', N'Tamara179@live.com', '1995-11-22', N'8143', N'905520'),
        (N'Назначение 26/04/2023_9367788', N'Петухов', N'Тарас', N'Фадеевич', N'+7 (376) 220-62-51', N'Taras24@rambler.ru', '1991-01-05', N'1609', N'171096'),
        (N'Назначение 26/04/2023_9788737', N'Родионов', N'Аркадий', N'Власович', N'+7 (491) 696-17-11', N'Arkadij123@inbox.ru', '1993-08-11', N'3841', N'642594'),
        (N'Назначение 26/04/2023_9736379', N'Горшкова', N'Глафира', N'Валентиновна', N'+7 (553) 343-38-82', N'Glafira73@outlook.com', '1978-05-25', N'9170', N'402601'),
        (N'Назначение 27/04/2023_9367788', N'Кириллова', N'Гавриила', N'Яковна', N'+7 (648) 700-43-34', N'Gavriila68@msn.com', '1992-04-26', N'9438', N'379667'),
        (N'Назначение 27/04/2023_9788737', N'Овчинников', N'Кузьма', N'Ефимович', N'+7 (562) 866-15-27', N'Kuzjma124@yandex.ru', '1993-08-02', N'0766', N'647226'),
        (N'Назначение 27/04/2023_9736379', N'Беляков', N'Роман', N'Викторович', N'+7 (595) 196-56-28', N'Roman89@gmail.com', '1991-06-07', N'2411', N'478305'),
        (N'Назначение 28/04/2023_9367788', N'Лыткин', N'Алексей', N'Максимович', N'+7 (994) 353-29-52', N'Aleksej43@gmail.com', '1996-03-07', N'2383', N'259825'),
        (N'Назначение 28/04/2023_9788737', N'Шубина', N'Надежда', N'Викторовна', N'+7 (736) 488-66-95', N'Nadezhda137@outlook.com', '1981-09-24', N'8844', N'708476'),
        (N'Назначение 28/04/2023_9736379', N'Зиновьева', N'Бронислава', N'Викторовна', N'+7 (778) 565-12-18', N'Bronislava56@yahoo.com', '1981-03-19', N'6736', N'319423')
    ) AS v(Purpose, LastName, FirstName, MiddleName, Phone, Email, BirthDate, PassportSeries, PassportNumber)
)
MERGE PassVisitors AS target
USING (
    SELECT
        rl.PassRequestId,
        iv.LastName,
        iv.FirstName,
        iv.MiddleName,
        iv.Phone,
        iv.Email,
        iv.BirthDate,
        iv.PassportSeries,
        iv.PassportNumber
    FROM IndividualVisitors iv
    JOIN RequestLookup rl ON rl.Purpose = iv.Purpose
) AS source
ON target.PassportSeries = source.PassportSeries AND target.PassportNumber = source.PassportNumber
WHEN MATCHED THEN UPDATE SET
    PassRequestId = source.PassRequestId,
    LastName = source.LastName,
    FirstName = source.FirstName,
    MiddleName = source.MiddleName,
    Phone = source.Phone,
    Email = source.Email,
    BirthDate = source.BirthDate
WHEN NOT MATCHED THEN INSERT (PassRequestId, LastName, FirstName, MiddleName, Phone, Email, BirthDate, PassportSeries, PassportNumber)
    VALUES (source.PassRequestId, source.LastName, source.FirstName, source.MiddleName, source.Phone, source.Email, source.BirthDate, source.PassportSeries, source.PassportNumber);
GO

-- Pass visitors (group members)
;WITH RequestLookup AS (
    SELECT Purpose, MIN(Id) AS PassRequestId
    FROM PassRequests
    GROUP BY Purpose
),
GroupLookup AS (
    SELECT Description, MIN(GroupID) AS GroupID
    FROM Groups
    GROUP BY Description
),
GroupVisitors AS (
    SELECT v.Purpose, v.LastName, v.FirstName, NULLIF(v.MiddleName, N'') AS MiddleName, v.Phone, v.Email, CAST(v.BirthDate AS DATETIME2) AS BirthDate, v.PassportSeries, v.PassportNumber, v.GroupDescription
    FROM (VALUES
        (N'Назначение 24/04/2023_9367788', N'Самойлова', N'Таисия', N'Гермоновна', N'+7 (891) 555-81-44', N'Taisiya177@lenta.ru', '1979-11-14', N'5193', N'897719', N'24/04/2023_Производство_Фомичева_9367788_ГР1'),
        (N'Назначение 24/04/2023_9367788', N'Ситникова', N'Аделаида', N'Гермоновна', N'+7 (793) 736-70-31', N'Adelaida20@hotmail.com', '1979-01-21', N'7561', N'148016', N'24/04/2023_Производство_Фомичева_9367788_ГР1'),
        (N'Назначение 24/04/2023_9367788', N'Исаев', N'Лев', N'Юлианович', N'+7 (675) 593-89-30', N'Lev131@rambler.ru', '1994-08-05', N'1860', N'680004', N'24/04/2023_Производство_Фомичева_9367788_ГР1'),
        (N'Назначение 24/04/2023_9367788', N'Никифоров', N'Даниил', N'Степанович', N'+7 (384) 358-77-82', N'Daniil198@bk.ru', '1970-12-13', N'4557', N'999958', N'24/04/2023_Производство_Фомичева_9367788_ГР1'),
        (N'Назначение 24/04/2023_9367788', N'Титова', N'Людмила', N'Якововна', N'+7 (221) 729-16-84', N'Lyudmila123@hotmail.com', '1976-08-21', N'7715', N'639425', N'24/04/2023_Производство_Фомичева_9367788_ГР1'),
        (N'Назначение 24/04/2023_9367788', N'Абрамова', N'Таисия', N'Дмитриевна', N'+7 (528) 312-18-20', N'Taisiya176@hotmail.com', '1982-11-20', N'7310', N'893510', N'24/04/2023_Производство_Фомичева_9367788_ГР1'),
        (N'Назначение 24/04/2023_9367788', N'Кузьмина', N'Вера', N'Максимовна', N'+7 (598) 583-53-45', N'Vera195@list.ru', '1989-12-10', N'3537', N'982933', N'24/04/2023_Производство_Фомичева_9367788_ГР1'),
        (N'Назначение 24/04/2023_9367788', N'Мартынов', N'Яков', N'Ростиславович', N'+7 (546) 159-67-33', N'YAkov196@rambler.ru', '1976-12-05', N'1793', N'986063', N'24/04/2023_Производство_Фомичева_9367788_ГР2'),
        (N'Назначение 24/04/2023_9367788', N'Евсеева', N'Нина', N'Павловна', N'+7 (833) 521-31-50', N'Nina145@msn.com', '1994-09-26', N'9323', N'745717', N'24/04/2023_Производство_Фомичева_9367788_ГР2'),
        (N'Назначение 24/04/2023_9367788', N'Голубев', N'Леонтий', N'Вячеславович', N'+7 (160) 527-57-41', N'Leontij161@mail.ru', '1990-10-03', N'1059', N'822077', N'24/04/2023_Производство_Фомичева_9367788_ГР2'),
        (N'Назначение 24/04/2023_9367788', N'Карпова', N'Серафима', N'Михаиловна', N'+7 (459) 930-91-70', N'Serafima169@yahoo.com', '1989-11-19', N'7034', N'858987', N'24/04/2023_Производство_Фомичева_9367788_ГР2'),
        (N'Назначение 24/04/2023_9367788', N'Орехов', N'Сергей', N'Емельянович', N'+7 (669) 603-29-87', N'Sergej35@inbox.ru', '1972-02-11', N'3844', N'223682', N'24/04/2023_Производство_Фомичева_9367788_ГР2'),
        (N'Назначение 24/04/2023_9367788', N'Исаев', N'Георгий', N'Павлович', N'+7 (678) 516-36-86', N'Georgij121@inbox.ru', '1987-08-11', N'4076', N'629809', N'24/04/2023_Производство_Фомичева_9367788_ГР2'),
        (N'Назначение 24/04/2023_9367788', N'Богданов', N'Елизар', N'Артемович', N'+7 (165) 768-30-97', N'Elizar30@yandex.ru', '1978-02-02', N'0573', N'198559', N'24/04/2023_Производство_Фомичева_9367788_ГР2'),
        (N'Назначение 24/04/2023_9367788', N'Тихонова', N'Лана', N'Семеновна', N'+7 (478) 467-75-15', N'Lana117@outlook.com', '1990-07-24', N'8761', N'609740', N'24/04/2023_Производство_Фомичева_9367788_ГР2')
    ) AS v(Purpose, LastName, FirstName, MiddleName, Phone, Email, BirthDate, PassportSeries, PassportNumber, GroupDescription)
)
MERGE PassVisitors AS target
USING (
    SELECT
        rl.PassRequestId,
        gl.GroupID,
        gv.LastName,
        gv.FirstName,
        gv.MiddleName,
        gv.Phone,
        gv.Email,
        gv.BirthDate,
        gv.PassportSeries,
        gv.PassportNumber
    FROM GroupVisitors gv
    JOIN RequestLookup rl ON rl.Purpose = gv.Purpose
    JOIN GroupLookup gl ON gl.Description = gv.GroupDescription
) AS source
ON target.PassportSeries = source.PassportSeries AND target.PassportNumber = source.PassportNumber
WHEN MATCHED THEN UPDATE SET
    PassRequestId = source.PassRequestId,
    GroupID = source.GroupID,
    LastName = source.LastName,
    FirstName = source.FirstName,
    MiddleName = source.MiddleName,
    Phone = source.Phone,
    Email = source.Email,
    BirthDate = source.BirthDate
WHEN NOT MATCHED THEN INSERT (PassRequestId, GroupID, LastName, FirstName, MiddleName, Phone, Email, BirthDate, PassportSeries, PassportNumber)
    VALUES (source.PassRequestId, source.GroupID, source.LastName, source.FirstName, source.MiddleName, source.Phone, source.Email, source.BirthDate, source.PassportSeries, source.PassportNumber);
GO
