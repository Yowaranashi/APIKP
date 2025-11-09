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
    Description NVARCHAR(500) NULL,
    CreatedAt DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME()
);

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
    Reason NVARCHAR(4000) NOT NULL,
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

------------------------------------------------------------
-- 10. Seed data generated from provided visitor tables
------------------------------------------------------------
-- Roles
INSERT INTO Roles (Name, Description) VALUES (N'Администратор системы', N'Полный доступ к настройкам и управлению платформой.');
INSERT INTO Roles (Name, Description) VALUES (N'Оператор пропусков', N'Обработка заявок на посещение и управление посетителями.');
INSERT INTO Roles (Name, Description) VALUES (N'Сотрудник безопасности', N'Контроль доступа и журналирование инцидентов.');
INSERT INTO Roles (Name, Description) VALUES (N'Сотрудник подразделения', N'Работа с заявками и посещениями своего подразделения.');
INSERT INTO Roles (Name, Description) VALUES (N'Посетитель', N'Доступ к личному кабинету посетителя для подачи заявок и отслеживания статусов.');
GO

-- Departments
INSERT INTO Departments (Name) VALUES (N'Производство');
INSERT INTO Departments (Name) VALUES (N'Сбыт');
INSERT INTO Departments (Name) VALUES (N'Администрация');
INSERT INTO Departments (Name) VALUES (N'Служба безопасности');
INSERT INTO Departments (Name) VALUES (N'Планирование');
INSERT INTO Departments (Name) VALUES (N'Общий отдел');
INSERT INTO Departments (Name) VALUES (N'Охрана');
GO

-- Application statuses
INSERT INTO ApplicationStatuses (StatusName) VALUES (N'Pending');
GO

-- Employees
INSERT INTO Employees (FullName, EmployeeCode, DepartmentId) VALUES (N'Фомичева Авдотья Трофимовна', N'9367788', (SELECT Id FROM Departments WHERE Name = N'Производство'));
INSERT INTO Employees (FullName, EmployeeCode, DepartmentId) VALUES (N'Гаврилова Римма Ефимовна', N'9788737', (SELECT Id FROM Departments WHERE Name = N'Сбыт'));
INSERT INTO Employees (FullName, EmployeeCode, DepartmentId) VALUES (N'Носкова Наталия Прохоровна', N'9736379', (SELECT Id FROM Departments WHERE Name = N'Администрация'));
INSERT INTO Employees (FullName, EmployeeCode, DepartmentId) VALUES (N'Архипов Тимофей Васильевич', N'9362832', (SELECT Id FROM Departments WHERE Name = N'Служба безопасности'));
INSERT INTO Employees (FullName, EmployeeCode, DepartmentId) VALUES (N'Орехова Вероника Артемовна', N'9737848', (SELECT Id FROM Departments WHERE Name = N'Планирование'));
INSERT INTO Employees (FullName, EmployeeCode, DepartmentId) VALUES (N'Савельев Павел Степанович', N'9768239', (SELECT Id FROM Departments WHERE Name = N'Общий отдел'));
INSERT INTO Employees (FullName, EmployeeCode, DepartmentId) VALUES (N'Чернов Всеволод Наумович', N'9404040', (SELECT Id FROM Departments WHERE Name = N'Охрана'));
INSERT INTO Employees (FullName, EmployeeCode, DepartmentId) VALUES (N'Марков Юрий Романович', NULL, NULL);
GO

-- Users
INSERT INTO Users (FullName, Email, PasswordHash, EmailConfirmed, EmployeeId, RoleId)
VALUES (
    N'Администратор системы',
    N'admin@hranitelpro.local',
    CONVERT(NVARCHAR(128), HASHBYTES('SHA2_256', 'Admin@Hranitel2023'), 2),
    1,
    NULL,
    (SELECT Id FROM Roles WHERE Name = N'Администратор системы')
);

INSERT INTO Users (FullName, Email, PasswordHash, EmailConfirmed, EmployeeId, RoleId)
SELECT
    e.FullName,
    N'afomicheva@hranitelpro.local',
    CONVERT(NVARCHAR(128), HASHBYTES('SHA2_256', 'Fomicheva#Pass1'), 2),
    1,
    e.Id,
    r.Id
FROM Employees e
JOIN Roles r ON r.Name = N'Оператор пропусков'
WHERE e.EmployeeCode = N'9367788';

INSERT INTO Users (FullName, Email, PasswordHash, EmailConfirmed, EmployeeId, RoleId)
SELECT
    e.FullName,
    N'tarchipov@hranitelpro.local',
    CONVERT(NVARCHAR(128), HASHBYTES('SHA2_256', 'Security!Archipov'), 2),
    1,
    e.Id,
    r.Id
FROM Employees e
JOIN Roles r ON r.Name = N'Сотрудник безопасности'
WHERE e.EmployeeCode = N'9362832';

INSERT INTO Users (FullName, Email, PasswordHash, EmailConfirmed, EmployeeId, RoleId)
SELECT
    e.FullName,
    N'rgavrilova@hranitelpro.local',
    CONVERT(NVARCHAR(128), HASHBYTES('SHA2_256', 'SalesRimma2023'), 2),
    1,
    e.Id,
    r.Id
FROM Employees e
JOIN Roles r ON r.Name = N'Сотрудник подразделения'
WHERE e.EmployeeCode = N'9788737';

INSERT INTO Users (FullName, Email, PasswordHash, EmailConfirmed, EmployeeId, RoleId)
SELECT
    e.FullName,
    N'nnoskova@hranitelpro.local',
    CONVERT(NVARCHAR(128), HASHBYTES('SHA2_256', 'AdminDept#2023'), 2),
    1,
    e.Id,
    r.Id
FROM Employees e
JOIN Roles r ON r.Name = N'Сотрудник подразделения'
WHERE e.EmployeeCode = N'9736379';

-- Visitor accounts
INSERT INTO Users (FullName, Email, PasswordHash, EmailConfirmed, EmployeeId, RoleId)
VALUES
    (N'Степанова Радинка Власовна', N'Radinka100@yandex.ru', CONVERT(NVARCHAR(128), HASHBYTES('SHA2_256', N'b3uWS6#Thuvq'), 2), 1, NULL, (SELECT Id FROM Roles WHERE Name = N'Посетитель')),
    (N'Шилов Прохор Герасимович', N'Prohor156@list.ru', CONVERT(NVARCHAR(128), HASHBYTES('SHA2_256', N'zDdom}SIhWs?'), 2), 1, NULL, (SELECT Id FROM Roles WHERE Name = N'Посетитель')),
    (N'Кононов Юрин Романович', N'YUrin155@gmail.com', CONVERT(NVARCHAR(128), HASHBYTES('SHA2_256', N'u@m*~ACBCqNQ'), 2), 1, NULL, (SELECT Id FROM Roles WHERE Name = N'Посетитель')),
    (N'Елисеева Альбина Николаевна', N'Aljbina33@lenta.ru', CONVERT(NVARCHAR(128), HASHBYTES('SHA2_256', N'Bu?BHCtwDFin'), 2), 1, NULL, (SELECT Id FROM Roles WHERE Name = N'Посетитель')),
    (N'Шарова Клавдия Макаровна', N'Klavdiya113@live.com', CONVERT(NVARCHAR(128), HASHBYTES('SHA2_256', N'FjC#hNIJori}'), 2), 1, NULL, (SELECT Id FROM Roles WHERE Name = N'Посетитель')),
    (N'Сидорова Тамара Григорьевна', N'Tamara179@live.com', CONVERT(NVARCHAR(128), HASHBYTES('SHA2_256', N'TJxVqMXrbesI'), 2), 1, NULL, (SELECT Id FROM Roles WHERE Name = N'Посетитель')),
    (N'Петухов Тарас Фадеевич', N'Taras24@rambler.ru', CONVERT(NVARCHAR(128), HASHBYTES('SHA2_256', N'07m5yspn3K~K'), 2), 1, NULL, (SELECT Id FROM Roles WHERE Name = N'Посетитель')),
    (N'Родионов Аркадий Власович', N'Arkadij123@inbox.ru', CONVERT(NVARCHAR(128), HASHBYTES('SHA2_256', N'vk2N7lxX}ck%'), 2), 1, NULL, (SELECT Id FROM Roles WHERE Name = N'Посетитель')),
    (N'Горшкова Глафира Валентиновна', N'Glafira73@outlook.com', CONVERT(NVARCHAR(128), HASHBYTES('SHA2_256', N'Zz8POQlP}M4~'), 2), 1, NULL, (SELECT Id FROM Roles WHERE Name = N'Посетитель')),
    (N'Кириллова Гавриила Яковна', N'Gavriila68@msn.com', CONVERT(NVARCHAR(128), HASHBYTES('SHA2_256', N'x4K5WthEe8ua'), 2), 1, NULL, (SELECT Id FROM Roles WHERE Name = N'Посетитель')),
    (N'Овчинников Кузьма Ефимович', N'Kuzjma124@yandex.ru', CONVERT(NVARCHAR(128), HASHBYTES('SHA2_256', N'OsByQJ}vYznW'), 2), 1, NULL, (SELECT Id FROM Roles WHERE Name = N'Посетитель')),
    (N'Беляков Роман Викторович', N'Roman89@gmail.com', CONVERT(NVARCHAR(128), HASHBYTES('SHA2_256', N'Xd?xP$2yICcG'), 2), 1, NULL, (SELECT Id FROM Roles WHERE Name = N'Посетитель')),
    (N'Лыткин Алексей Максимович', N'Aleksej43@gmail.com', CONVERT(NVARCHAR(128), HASHBYTES('SHA2_256', N'~c%PlTY0?qgl'), 2), 1, NULL, (SELECT Id FROM Roles WHERE Name = N'Посетитель')),
    (N'Шубина Надежда Викторовна', N'Nadezhda137@outlook.com', CONVERT(NVARCHAR(128), HASHBYTES('SHA2_256', N'QQ~0q~rXHb?p'), 2), 1, NULL, (SELECT Id FROM Roles WHERE Name = N'Посетитель')),
    (N'Зиновьева Бронислава Викторовна', N'Bronislava56@yahoo.com', CONVERT(NVARCHAR(128), HASHBYTES('SHA2_256', N'LO}xyC~1S4l6'), 2), 1, NULL, (SELECT Id FROM Roles WHERE Name = N'Посетитель')),
    (N'Самойлова Таисия Гермоновна', N'Taisiya177@lenta.ru', CONVERT(NVARCHAR(128), HASHBYTES('SHA2_256', N'R94YGT3XFjgD'), 2), 1, NULL, (SELECT Id FROM Roles WHERE Name = N'Посетитель')),
    (N'Ситникова Аделаида Гермоновна', N'Adelaida20@hotmail.com', CONVERT(NVARCHAR(128), HASHBYTES('SHA2_256', N'LCY*{L*fEGYB'), 2), 1, NULL, (SELECT Id FROM Roles WHERE Name = N'Посетитель')),
    (N'Исаев Лев Юлианович', N'Lev131@rambler.ru', CONVERT(NVARCHAR(128), HASHBYTES('SHA2_256', N'~?oj2Lh@S7*T'), 2), 1, NULL, (SELECT Id FROM Roles WHERE Name = N'Посетитель')),
    (N'Никифоров Даниил Степанович', N'Daniil198@bk.ru', CONVERT(NVARCHAR(128), HASHBYTES('SHA2_256', N'L2W#uo7z{EsO'), 2), 1, NULL, (SELECT Id FROM Roles WHERE Name = N'Посетитель')),
    (N'Титова Людмила Якововна', N'Lyudmila123@hotmail.com', CONVERT(NVARCHAR(128), HASHBYTES('SHA2_256', N'@8mk9M?NBAGj'), 2), 1, NULL, (SELECT Id FROM Roles WHERE Name = N'Посетитель')),
    (N'Абрамова Таисия Дмитриевна', N'Taisiya176@hotmail.com', CONVERT(NVARCHAR(128), HASHBYTES('SHA2_256', N'~rIWfsnXA~7C'), 2), 1, NULL, (SELECT Id FROM Roles WHERE Name = N'Посетитель')),
    (N'Кузьмина Вера Максимовна', N'Vera195@list.ru', CONVERT(NVARCHAR(128), HASHBYTES('SHA2_256', N'B|5v$2r$7luL'), 2), 1, NULL, (SELECT Id FROM Roles WHERE Name = N'Посетитель')),
    (N'Мартынов Яков Ростиславович', N'YAkov196@rambler.ru', CONVERT(NVARCHAR(128), HASHBYTES('SHA2_256', N'$6s5bggKP7aw'), 2), 1, NULL, (SELECT Id FROM Roles WHERE Name = N'Посетитель')),
    (N'Евсеева Нина Павловна', N'Nina145@msn.com', CONVERT(NVARCHAR(128), HASHBYTES('SHA2_256', N'Uxy6RtBXIcpT'), 2), 1, NULL, (SELECT Id FROM Roles WHERE Name = N'Посетитель')),
    (N'Голубев Леонтий Вячеславович', N'Leontij161@mail.ru', CONVERT(NVARCHAR(128), HASHBYTES('SHA2_256', N'KkMY8yKw@oCa'), 2), 1, NULL, (SELECT Id FROM Roles WHERE Name = N'Посетитель')),
    (N'Карпова Серафима Михаиловна', N'Serafima169@yahoo.com', CONVERT(NVARCHAR(128), HASHBYTES('SHA2_256', N'gNe3I9}8J3Z@'), 2), 1, NULL, (SELECT Id FROM Roles WHERE Name = N'Посетитель')),
    (N'Орехов Сергей Емельянович', N'Sergej35@inbox.ru', CONVERT(NVARCHAR(128), HASHBYTES('SHA2_256', N'$39vc%ljqN%r'), 2), 1, NULL, (SELECT Id FROM Roles WHERE Name = N'Посетитель')),
    (N'Исаев Георгий Павлович', N'Georgij121@inbox.ru', CONVERT(NVARCHAR(128), HASHBYTES('SHA2_256', N'bbx5H}f*BC?w'), 2), 1, NULL, (SELECT Id FROM Roles WHERE Name = N'Посетитель')),
    (N'Богданов Елизар Артемович', N'Elizar30@yandex.ru', CONVERT(NVARCHAR(128), HASHBYTES('SHA2_256', N'wJs1~r3RS~dr'), 2), 1, NULL, (SELECT Id FROM Roles WHERE Name = N'Посетитель')),
    (N'Тихонова Лана Семеновна', N'Lana117@outlook.com', CONVERT(NVARCHAR(128), HASHBYTES('SHA2_256', N'mFoG$jcS3c4~'), 2), 1, NULL, (SELECT Id FROM Roles WHERE Name = N'Посетитель'));

-- Pass requests generated from visitor assignments
INSERT INTO PassRequests (RequestType, StartDate, EndDate, Purpose, DepartmentId, ResponsibleEmployeeId, Phone, ApplicantEmail, StatusID, GroupSize, Note) SELECT 'Personal', '2023-04-24T09:00:00', '2023-04-24T18:00:00', N'Назначение 24/04/2023_9367788', e.DepartmentId, e.Id, N'+7 (613) 272-60-62', N'Radinka100@yandex.ru', s.StatusID, 16, N'Создано на основе предоставленных данных' FROM Employees e CROSS JOIN ApplicationStatuses s WHERE e.EmployeeCode = N'9367788' AND s.StatusName = N'Pending';
INSERT INTO PassRequests (RequestType, StartDate, EndDate, Purpose, DepartmentId, ResponsibleEmployeeId, Phone, ApplicantEmail, StatusID, GroupSize, Note) SELECT 'Personal', '2023-04-24T09:00:00', '2023-04-24T18:00:00', N'Назначение 24/04/2023_9736379', e.DepartmentId, e.Id, N'+7 (784) 673-51-91', N'YUrin155@gmail.com', s.StatusID, 1, N'Создано на основе предоставленных данных' FROM Employees e CROSS JOIN ApplicationStatuses s WHERE e.EmployeeCode = N'9736379' AND s.StatusName = N'Pending';
INSERT INTO PassRequests (RequestType, StartDate, EndDate, Purpose, DepartmentId, ResponsibleEmployeeId, Phone, ApplicantEmail, StatusID, GroupSize, Note) SELECT 'Personal', '2023-04-24T09:00:00', '2023-04-24T18:00:00', N'Назначение 24/04/2023_9788737', e.DepartmentId, e.Id, N'+7 (615) 594-77-66', N'Prohor156@list.ru', s.StatusID, 1, N'Создано на основе предоставленных данных' FROM Employees e CROSS JOIN ApplicationStatuses s WHERE e.EmployeeCode = N'9788737' AND s.StatusName = N'Pending';
INSERT INTO PassRequests (RequestType, StartDate, EndDate, Purpose, DepartmentId, ResponsibleEmployeeId, Phone, ApplicantEmail, StatusID, GroupSize, Note) SELECT 'Personal', '2023-04-25T09:00:00', '2023-04-25T18:00:00', N'Назначение 25/04/2023_9367788', e.DepartmentId, e.Id, N'+7 (654) 864-77-46', N'Aljbina33@lenta.ru', s.StatusID, 1, N'Создано на основе предоставленных данных' FROM Employees e CROSS JOIN ApplicationStatuses s WHERE e.EmployeeCode = N'9367788' AND s.StatusName = N'Pending';
INSERT INTO PassRequests (RequestType, StartDate, EndDate, Purpose, DepartmentId, ResponsibleEmployeeId, Phone, ApplicantEmail, StatusID, GroupSize, Note) SELECT 'Personal', '2023-04-25T09:00:00', '2023-04-25T18:00:00', N'Назначение 25/04/2023_9736379', e.DepartmentId, e.Id, N'+7 (334) 692-79-77', N'Tamara179@live.com', s.StatusID, 1, N'Создано на основе предоставленных данных' FROM Employees e CROSS JOIN ApplicationStatuses s WHERE e.EmployeeCode = N'9736379' AND s.StatusName = N'Pending';
INSERT INTO PassRequests (RequestType, StartDate, EndDate, Purpose, DepartmentId, ResponsibleEmployeeId, Phone, ApplicantEmail, StatusID, GroupSize, Note) SELECT 'Personal', '2023-04-25T09:00:00', '2023-04-25T18:00:00', N'Назначение 25/04/2023_9788737', e.DepartmentId, e.Id, N'+7 (822) 525-82-40', N'Klavdiya113@live.com', s.StatusID, 1, N'Создано на основе предоставленных данных' FROM Employees e CROSS JOIN ApplicationStatuses s WHERE e.EmployeeCode = N'9788737' AND s.StatusName = N'Pending';
INSERT INTO PassRequests (RequestType, StartDate, EndDate, Purpose, DepartmentId, ResponsibleEmployeeId, Phone, ApplicantEmail, StatusID, GroupSize, Note) SELECT 'Personal', '2023-04-26T09:00:00', '2023-04-26T18:00:00', N'Назначение 26/04/2023_9367788', e.DepartmentId, e.Id, N'+7 (376) 220-62-51', N'Taras24@rambler.ru', s.StatusID, 1, N'Создано на основе предоставленных данных' FROM Employees e CROSS JOIN ApplicationStatuses s WHERE e.EmployeeCode = N'9367788' AND s.StatusName = N'Pending';
INSERT INTO PassRequests (RequestType, StartDate, EndDate, Purpose, DepartmentId, ResponsibleEmployeeId, Phone, ApplicantEmail, StatusID, GroupSize, Note) SELECT 'Personal', '2023-04-26T09:00:00', '2023-04-26T18:00:00', N'Назначение 26/04/2023_9736379', e.DepartmentId, e.Id, N'+7 (553) 343-38-82', N'Glafira73@outlook.com', s.StatusID, 1, N'Создано на основе предоставленных данных' FROM Employees e CROSS JOIN ApplicationStatuses s WHERE e.EmployeeCode = N'9736379' AND s.StatusName = N'Pending';
INSERT INTO PassRequests (RequestType, StartDate, EndDate, Purpose, DepartmentId, ResponsibleEmployeeId, Phone, ApplicantEmail, StatusID, GroupSize, Note) SELECT 'Personal', '2023-04-26T09:00:00', '2023-04-26T18:00:00', N'Назначение 26/04/2023_9788737', e.DepartmentId, e.Id, N'+7 (491) 696-17-11', N'Arkadij123@inbox.ru', s.StatusID, 1, N'Создано на основе предоставленных данных' FROM Employees e CROSS JOIN ApplicationStatuses s WHERE e.EmployeeCode = N'9788737' AND s.StatusName = N'Pending';
INSERT INTO PassRequests (RequestType, StartDate, EndDate, Purpose, DepartmentId, ResponsibleEmployeeId, Phone, ApplicantEmail, StatusID, GroupSize, Note) SELECT 'Personal', '2023-04-27T09:00:00', '2023-04-27T18:00:00', N'Назначение 27/04/2023_9367788', e.DepartmentId, e.Id, N'+7 (648) 700-43-34', N'Gavriila68@msn.com', s.StatusID, 1, N'Создано на основе предоставленных данных' FROM Employees e CROSS JOIN ApplicationStatuses s WHERE e.EmployeeCode = N'9367788' AND s.StatusName = N'Pending';
INSERT INTO PassRequests (RequestType, StartDate, EndDate, Purpose, DepartmentId, ResponsibleEmployeeId, Phone, ApplicantEmail, StatusID, GroupSize, Note) SELECT 'Personal', '2023-04-27T09:00:00', '2023-04-27T18:00:00', N'Назначение 27/04/2023_9736379', e.DepartmentId, e.Id, N'+7 (595) 196-56-28', N'Roman89@gmail.com', s.StatusID, 1, N'Создано на основе предоставленных данных' FROM Employees e CROSS JOIN ApplicationStatuses s WHERE e.EmployeeCode = N'9736379' AND s.StatusName = N'Pending';
INSERT INTO PassRequests (RequestType, StartDate, EndDate, Purpose, DepartmentId, ResponsibleEmployeeId, Phone, ApplicantEmail, StatusID, GroupSize, Note) SELECT 'Personal', '2023-04-27T09:00:00', '2023-04-27T18:00:00', N'Назначение 27/04/2023_9788737', e.DepartmentId, e.Id, N'+7 (562) 866-15-27', N'Kuzjma124@yandex.ru', s.StatusID, 1, N'Создано на основе предоставленных данных' FROM Employees e CROSS JOIN ApplicationStatuses s WHERE e.EmployeeCode = N'9788737' AND s.StatusName = N'Pending';
INSERT INTO PassRequests (RequestType, StartDate, EndDate, Purpose, DepartmentId, ResponsibleEmployeeId, Phone, ApplicantEmail, StatusID, GroupSize, Note) SELECT 'Personal', '2023-04-28T09:00:00', '2023-04-28T18:00:00', N'Назначение 28/04/2023_9367788', e.DepartmentId, e.Id, N'+7 (994) 353-29-52', N'Aleksej43@gmail.com', s.StatusID, 1, N'Создано на основе предоставленных данных' FROM Employees e CROSS JOIN ApplicationStatuses s WHERE e.EmployeeCode = N'9367788' AND s.StatusName = N'Pending';
INSERT INTO PassRequests (RequestType, StartDate, EndDate, Purpose, DepartmentId, ResponsibleEmployeeId, Phone, ApplicantEmail, StatusID, GroupSize, Note) SELECT 'Personal', '2023-04-28T09:00:00', '2023-04-28T18:00:00', N'Назначение 28/04/2023_9736379', e.DepartmentId, e.Id, N'+7 (778) 565-12-18', N'Bronislava56@yahoo.com', s.StatusID, 1, N'Создано на основе предоставленных данных' FROM Employees e CROSS JOIN ApplicationStatuses s WHERE e.EmployeeCode = N'9736379' AND s.StatusName = N'Pending';
INSERT INTO PassRequests (RequestType, StartDate, EndDate, Purpose, DepartmentId, ResponsibleEmployeeId, Phone, ApplicantEmail, StatusID, GroupSize, Note) SELECT 'Personal', '2023-04-28T09:00:00', '2023-04-28T18:00:00', N'Назначение 28/04/2023_9788737', e.DepartmentId, e.Id, N'+7 (736) 488-66-95', N'Nadezhda137@outlook.com', s.StatusID, 1, N'Создано на основе предоставленных данных' FROM Employees e CROSS JOIN ApplicationStatuses s WHERE e.EmployeeCode = N'9788737' AND s.StatusName = N'Pending';
GO

-- Groups derived from group visitor data
INSERT INTO Groups (GroupName, Description) VALUES (N'ГР1', N'24/04/2023_Производство_Фомичева_9367788_ГР1');
INSERT INTO Groups (GroupName, Description) VALUES (N'ГР2', N'24/04/2023_Производство_Фомичева_9367788_ГР2');
GO

-- Pass visitors (individual)
INSERT INTO PassVisitors (PassRequestId, LastName, FirstName, MiddleName, Phone, Email, BirthDate, PassportSeries, PassportNumber) VALUES ((SELECT Id FROM PassRequests WHERE Purpose = N'Назначение 24/04/2023_9367788'), N'Степанова', N'Радинка', N'Власовна', N'+7 (613) 272-60-62', N'Radinka100@yandex.ru', '1986-10-18', N'0208', N'530509');
INSERT INTO PassVisitors (PassRequestId, LastName, FirstName, MiddleName, Phone, Email, BirthDate, PassportSeries, PassportNumber) VALUES ((SELECT Id FROM PassRequests WHERE Purpose = N'Назначение 24/04/2023_9788737'), N'Шилов', N'Прохор', N'Герасимович', N'+7 (615) 594-77-66', N'Prohor156@list.ru', '1977-10-09', N'3036', N'796488');
INSERT INTO PassVisitors (PassRequestId, LastName, FirstName, MiddleName, Phone, Email, BirthDate, PassportSeries, PassportNumber) VALUES ((SELECT Id FROM PassRequests WHERE Purpose = N'Назначение 24/04/2023_9736379'), N'Кононов', N'Юрин', N'Романович', N'+7 (784) 673-51-91', N'YUrin155@gmail.com', '1971-10-08', N'2747', N'790512');
INSERT INTO PassVisitors (PassRequestId, LastName, FirstName, MiddleName, Phone, Email, BirthDate, PassportSeries, PassportNumber) VALUES ((SELECT Id FROM PassRequests WHERE Purpose = N'Назначение 25/04/2023_9367788'), N'Елисеева', N'Альбина', N'Николаевна', N'+7 (654) 864-77-46', N'Aljbina33@lenta.ru', '1983-02-15', N'5241', N'213304');
INSERT INTO PassVisitors (PassRequestId, LastName, FirstName, MiddleName, Phone, Email, BirthDate, PassportSeries, PassportNumber) VALUES ((SELECT Id FROM PassRequests WHERE Purpose = N'Назначение 25/04/2023_9788737'), N'Шарова', N'Клавдия', N'Макаровна', N'+7 (822) 525-82-40', N'Klavdiya113@live.com', '1980-07-22', N'8143', N'593309');
INSERT INTO PassVisitors (PassRequestId, LastName, FirstName, MiddleName, Phone, Email, BirthDate, PassportSeries, PassportNumber) VALUES ((SELECT Id FROM PassRequests WHERE Purpose = N'Назначение 25/04/2023_9736379'), N'Сидорова', N'Тамара', N'Григорьевна', N'+7 (334) 692-79-77', N'Tamara179@live.com', '1995-11-22', N'8143', N'905520');
INSERT INTO PassVisitors (PassRequestId, LastName, FirstName, MiddleName, Phone, Email, BirthDate, PassportSeries, PassportNumber) VALUES ((SELECT Id FROM PassRequests WHERE Purpose = N'Назначение 26/04/2023_9367788'), N'Петухов', N'Тарас', N'Фадеевич', N'+7 (376) 220-62-51', N'Taras24@rambler.ru', '1991-01-05', N'1609', N'171096');
INSERT INTO PassVisitors (PassRequestId, LastName, FirstName, MiddleName, Phone, Email, BirthDate, PassportSeries, PassportNumber) VALUES ((SELECT Id FROM PassRequests WHERE Purpose = N'Назначение 26/04/2023_9788737'), N'Родионов', N'Аркадий', N'Власович', N'+7 (491) 696-17-11', N'Arkadij123@inbox.ru', '1993-08-11', N'3841', N'642594');
INSERT INTO PassVisitors (PassRequestId, LastName, FirstName, MiddleName, Phone, Email, BirthDate, PassportSeries, PassportNumber) VALUES ((SELECT Id FROM PassRequests WHERE Purpose = N'Назначение 26/04/2023_9736379'), N'Горшкова', N'Глафира', N'Валентиновна', N'+7 (553) 343-38-82', N'Glafira73@outlook.com', '1978-05-25', N'9170', N'402601');
INSERT INTO PassVisitors (PassRequestId, LastName, FirstName, MiddleName, Phone, Email, BirthDate, PassportSeries, PassportNumber) VALUES ((SELECT Id FROM PassRequests WHERE Purpose = N'Назначение 27/04/2023_9367788'), N'Кириллова', N'Гавриила', N'Яковна', N'+7 (648) 700-43-34', N'Gavriila68@msn.com', '1992-04-26', N'9438', N'379667');
INSERT INTO PassVisitors (PassRequestId, LastName, FirstName, MiddleName, Phone, Email, BirthDate, PassportSeries, PassportNumber) VALUES ((SELECT Id FROM PassRequests WHERE Purpose = N'Назначение 27/04/2023_9788737'), N'Овчинников', N'Кузьма', N'Ефимович', N'+7 (562) 866-15-27', N'Kuzjma124@yandex.ru', '1993-08-02', N'0766', N'647226');
INSERT INTO PassVisitors (PassRequestId, LastName, FirstName, MiddleName, Phone, Email, BirthDate, PassportSeries, PassportNumber) VALUES ((SELECT Id FROM PassRequests WHERE Purpose = N'Назначение 27/04/2023_9736379'), N'Беляков', N'Роман', N'Викторович', N'+7 (595) 196-56-28', N'Roman89@gmail.com', '1991-06-07', N'2411', N'478305');
INSERT INTO PassVisitors (PassRequestId, LastName, FirstName, MiddleName, Phone, Email, BirthDate, PassportSeries, PassportNumber) VALUES ((SELECT Id FROM PassRequests WHERE Purpose = N'Назначение 28/04/2023_9367788'), N'Лыткин', N'Алексей', N'Максимович', N'+7 (994) 353-29-52', N'Aleksej43@gmail.com', '1996-03-07', N'2383', N'259825');
INSERT INTO PassVisitors (PassRequestId, LastName, FirstName, MiddleName, Phone, Email, BirthDate, PassportSeries, PassportNumber) VALUES ((SELECT Id FROM PassRequests WHERE Purpose = N'Назначение 28/04/2023_9788737'), N'Шубина', N'Надежда', N'Викторовна', N'+7 (736) 488-66-95', N'Nadezhda137@outlook.com', '1981-09-24', N'8844', N'708476');
INSERT INTO PassVisitors (PassRequestId, LastName, FirstName, MiddleName, Phone, Email, BirthDate, PassportSeries, PassportNumber) VALUES ((SELECT Id FROM PassRequests WHERE Purpose = N'Назначение 28/04/2023_9736379'), N'Зиновьева', N'Бронислава', N'Викторовна', N'+7 (778) 565-12-18', N'Bronislava56@yahoo.com', '1981-03-19', N'6736', N'319423');
GO

-- Pass visitors (group members)
INSERT INTO PassVisitors (PassRequestId, LastName, FirstName, MiddleName, Phone, Email, BirthDate, PassportSeries, PassportNumber, GroupID) VALUES ((SELECT Id FROM PassRequests WHERE Purpose = N'Назначение 24/04/2023_9367788'), N'Самойлова', N'Таисия', N'Гермоновна', N'+7 (891) 555-81-44', N'Taisiya177@lenta.ru', '1979-11-14', N'5193', N'897719', (SELECT GroupID FROM Groups WHERE Description = N'24/04/2023_Производство_Фомичева_9367788_ГР1'));
INSERT INTO PassVisitors (PassRequestId, LastName, FirstName, MiddleName, Phone, Email, BirthDate, PassportSeries, PassportNumber, GroupID) VALUES ((SELECT Id FROM PassRequests WHERE Purpose = N'Назначение 24/04/2023_9367788'), N'Ситникова', N'Аделаида', N'Гермоновна', N'+7 (793) 736-70-31', N'Adelaida20@hotmail.com', '1979-01-21', N'7561', N'148016', (SELECT GroupID FROM Groups WHERE Description = N'24/04/2023_Производство_Фомичева_9367788_ГР1'));
INSERT INTO PassVisitors (PassRequestId, LastName, FirstName, MiddleName, Phone, Email, BirthDate, PassportSeries, PassportNumber, GroupID) VALUES ((SELECT Id FROM PassRequests WHERE Purpose = N'Назначение 24/04/2023_9367788'), N'Исаев', N'Лев', N'Юлианович', N'+7 (675) 593-89-30', N'Lev131@rambler.ru', '1994-08-05', N'1860', N'680004', (SELECT GroupID FROM Groups WHERE Description = N'24/04/2023_Производство_Фомичева_9367788_ГР1'));
INSERT INTO PassVisitors (PassRequestId, LastName, FirstName, MiddleName, Phone, Email, BirthDate, PassportSeries, PassportNumber, GroupID) VALUES ((SELECT Id FROM PassRequests WHERE Purpose = N'Назначение 24/04/2023_9367788'), N'Никифоров', N'Даниил', N'Степанович', N'+7 (384) 358-77-82', N'Daniil198@bk.ru', '1970-12-13', N'4557', N'999958', (SELECT GroupID FROM Groups WHERE Description = N'24/04/2023_Производство_Фомичева_9367788_ГР1'));
INSERT INTO PassVisitors (PassRequestId, LastName, FirstName, MiddleName, Phone, Email, BirthDate, PassportSeries, PassportNumber, GroupID) VALUES ((SELECT Id FROM PassRequests WHERE Purpose = N'Назначение 24/04/2023_9367788'), N'Титова', N'Людмила', N'Якововна', N'+7 (221) 729-16-84', N'Lyudmila123@hotmail.com', '1976-08-21', N'7715', N'639425', (SELECT GroupID FROM Groups WHERE Description = N'24/04/2023_Производство_Фомичева_9367788_ГР1'));
INSERT INTO PassVisitors (PassRequestId, LastName, FirstName, MiddleName, Phone, Email, BirthDate, PassportSeries, PassportNumber, GroupID) VALUES ((SELECT Id FROM PassRequests WHERE Purpose = N'Назначение 24/04/2023_9367788'), N'Абрамова', N'Таисия', N'Дмитриевна', N'+7 (528) 312-18-20', N'Taisiya176@hotmail.com', '1982-11-20', N'7310', N'893510', (SELECT GroupID FROM Groups WHERE Description = N'24/04/2023_Производство_Фомичева_9367788_ГР1'));
INSERT INTO PassVisitors (PassRequestId, LastName, FirstName, MiddleName, Phone, Email, BirthDate, PassportSeries, PassportNumber, GroupID) VALUES ((SELECT Id FROM PassRequests WHERE Purpose = N'Назначение 24/04/2023_9367788'), N'Кузьмина', N'Вера', N'Максимовна', N'+7 (598) 583-53-45', N'Vera195@list.ru', '1989-12-10', N'3537', N'982933', (SELECT GroupID FROM Groups WHERE Description = N'24/04/2023_Производство_Фомичева_9367788_ГР1'));
INSERT INTO PassVisitors (PassRequestId, LastName, FirstName, MiddleName, Phone, Email, BirthDate, PassportSeries, PassportNumber, GroupID) VALUES ((SELECT Id FROM PassRequests WHERE Purpose = N'Назначение 24/04/2023_9367788'), N'Мартынов', N'Яков', N'Ростиславович', N'+7 (546) 159-67-33', N'YAkov196@rambler.ru', '1976-12-05', N'1793', N'986063', (SELECT GroupID FROM Groups WHERE Description = N'24/04/2023_Производство_Фомичева_9367788_ГР2'));
INSERT INTO PassVisitors (PassRequestId, LastName, FirstName, MiddleName, Phone, Email, BirthDate, PassportSeries, PassportNumber, GroupID) VALUES ((SELECT Id FROM PassRequests WHERE Purpose = N'Назначение 24/04/2023_9367788'), N'Евсеева', N'Нина', N'Павловна', N'+7 (833) 521-31-50', N'Nina145@msn.com', '1994-09-26', N'9323', N'745717', (SELECT GroupID FROM Groups WHERE Description = N'24/04/2023_Производство_Фомичева_9367788_ГР2'));
INSERT INTO PassVisitors (PassRequestId, LastName, FirstName, MiddleName, Phone, Email, BirthDate, PassportSeries, PassportNumber, GroupID) VALUES ((SELECT Id FROM PassRequests WHERE Purpose = N'Назначение 24/04/2023_9367788'), N'Голубев', N'Леонтий', N'Вячеславович', N'+7 (160) 527-57-41', N'Leontij161@mail.ru', '1990-10-03', N'1059', N'822077', (SELECT GroupID FROM Groups WHERE Description = N'24/04/2023_Производство_Фомичева_9367788_ГР2'));
INSERT INTO PassVisitors (PassRequestId, LastName, FirstName, MiddleName, Phone, Email, BirthDate, PassportSeries, PassportNumber, GroupID) VALUES ((SELECT Id FROM PassRequests WHERE Purpose = N'Назначение 24/04/2023_9367788'), N'Карпова', N'Серафима', N'Михаиловна', N'+7 (459) 930-91-70', N'Serafima169@yahoo.com', '1989-11-19', N'7034', N'858987', (SELECT GroupID FROM Groups WHERE Description = N'24/04/2023_Производство_Фомичева_9367788_ГР2'));
INSERT INTO PassVisitors (PassRequestId, LastName, FirstName, MiddleName, Phone, Email, BirthDate, PassportSeries, PassportNumber, GroupID) VALUES ((SELECT Id FROM PassRequests WHERE Purpose = N'Назначение 24/04/2023_9367788'), N'Орехов', N'Сергей', N'Емельянович', N'+7 (669) 603-29-87', N'Sergej35@inbox.ru', '1972-02-11', N'3844', N'223682', (SELECT GroupID FROM Groups WHERE Description = N'24/04/2023_Производство_Фомичева_9367788_ГР2'));
INSERT INTO PassVisitors (PassRequestId, LastName, FirstName, MiddleName, Phone, Email, BirthDate, PassportSeries, PassportNumber, GroupID) VALUES ((SELECT Id FROM PassRequests WHERE Purpose = N'Назначение 24/04/2023_9367788'), N'Исаев', N'Георгий', N'Павлович', N'+7 (678) 516-36-86', N'Georgij121@inbox.ru', '1987-08-11', N'4076', N'629809', (SELECT GroupID FROM Groups WHERE Description = N'24/04/2023_Производство_Фомичева_9367788_ГР2'));
INSERT INTO PassVisitors (PassRequestId, LastName, FirstName, MiddleName, Phone, Email, BirthDate, PassportSeries, PassportNumber, GroupID) VALUES ((SELECT Id FROM PassRequests WHERE Purpose = N'Назначение 24/04/2023_9367788'), N'Богданов', N'Елизар', N'Артемович', N'+7 (165) 768-30-97', N'Elizar30@yandex.ru', '1978-02-02', N'0573', N'198559', (SELECT GroupID FROM Groups WHERE Description = N'24/04/2023_Производство_Фомичева_9367788_ГР2'));
INSERT INTO PassVisitors (PassRequestId, LastName, FirstName, MiddleName, Phone, Email, BirthDate, PassportSeries, PassportNumber, GroupID) VALUES ((SELECT Id FROM PassRequests WHERE Purpose = N'Назначение 24/04/2023_9367788'), N'Тихонова', N'Лана', N'Семеновна', N'+7 (478) 467-75-15', N'Lana117@outlook.com', '1990-07-24', N'8761', N'609740', (SELECT GroupID FROM Groups WHERE Description = N'24/04/2023_Производство_Фомичева_9367788_ГР2'));
GO
