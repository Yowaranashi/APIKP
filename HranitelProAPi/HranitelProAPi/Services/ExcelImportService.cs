using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using ClosedXML.Excel;
using HranitelPro.API.Data;
using HranitelPRO.API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;

namespace HranitelPRO.API.Services
{
    public class ExcelImportService : IExcelImportService
    {
        private static readonly string[] DepartmentHeaders = { "department", "departmentname", "building", "facility", "отдел", "здание", "подразделение" };
        private static readonly string[] DepartmentDescriptionHeaders = { "description", "departmentdescription", "описание", "comment" };
        private static readonly string[] VisitorFullNameHeaders = { "fullname", "fio", "visitor", "фио" };
        private static readonly string[] VisitorPhoneHeaders = { "phone", "телефон", "номертелефона", "contact" };
        private static readonly string[] VisitorBirthDateHeaders = { "birthdate", "датарождения", "дата рождения" };
        private static readonly string[] VisitorPassportHeaders = { "passport", "паспорт", "данныепаспорта", "паспортныеданные" };
        private static readonly string[] VisitorGroupHeaders = { "group", "groupname", "группа" };
        private static readonly string[] VisitorGroupDescriptionHeaders = { "назначение", "purpose", "описаниегруппы" };
        private static readonly string[] EmployeeFullNameHeaders = { "fullname", "fio", "фио" };
        private static readonly string[] EmployeeDepartmentHeaders = { "подразделение", "department", "division" };
        private static readonly string[] EmployeeSubDepartmentHeaders = { "отдел", "departmentname", "unit" };
        private static readonly string[] EmployeeCodeHeaders = { "кодсотрудника", "tabnumber", "employeecode", "табельныйномер" };
        private static readonly string[] RequestTypeHeaders = { "requesttype", "type", "тип" };
        private static readonly string[] StartDateHeaders = { "startdate", "start", "начало", "датаначала" };
        private static readonly string[] EndDateHeaders = { "enddate", "end", "окончание", "датaокончания", "датаокончания" };
        private static readonly string[] PurposeHeaders = { "purpose", "описание", "цель", "comment" };
        private static readonly string[] ApplicantLastNameHeaders = { "lastname", "applicantlastname", "фамилия" };
        private static readonly string[] ApplicantFirstNameHeaders = { "firstname", "applicantfirstname", "имя" };
        private static readonly string[] ApplicantMiddleNameHeaders = { "middlename", "applicantmiddlename", "отчество" };
        private static readonly string[] PhoneHeaders = { "phone", "телефон", "contact" };
        private static readonly string[] EmailHeaders = { "email", "почта" };
        private static readonly string[] StatusHeaders = { "status", "статус" };
        private static readonly string[] SessionCodeHeaders = { "sessioncode", "externalid", "code", "кодовнешний", "номерсессии" };
        private static readonly string[] PhotoHeaders = { "photo", "photofile", "photofilename", "фото" };
        private static readonly string[] PdfHeaders = { "pdf", "pdffile", "document", "attachment", "паспорт", "документ" };
        private static readonly string[] RoleNameHeaders = { "role", "rolename", "name", "роль" };
        private static readonly string[] RoleDescriptionHeaders = { "description", "details", "описание" };
        private static readonly string[] StatusNameHeaders = { "status", "statusname", "name", "статус" };
        private static readonly string[] GroupNameHeaders = { "group", "groupname", "name", "группа" };
        private static readonly string[] GroupDescriptionHeaders = { "description", "details", "назначение", "описание" };

        private readonly HranitelContext _context;
        private readonly IHostEnvironment _environment;
        private readonly string _uploadsRoot;
        private readonly Dictionary<string, FileType> _fileTypeCache = new(StringComparer.OrdinalIgnoreCase);

        public ExcelImportService(HranitelContext context, IHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
            _uploadsRoot = Path.Combine(_environment.ContentRootPath, "Uploads");
            Directory.CreateDirectory(_uploadsRoot);
        }

        public async Task<int> ImportVisitorsAsync(IFormFile file)
        {
            if (file == null)
            {
                throw new ArgumentNullException(nameof(file));
            }

            using var stream = new MemoryStream();
            await file.CopyToAsync(stream);
            stream.Position = 0;
            using var workbook = new XLWorkbook(stream);
            var worksheet = workbook.Worksheets.FirstOrDefault();
            if (worksheet == null)
            {
                return 0;
            }

            var range = worksheet.RangeUsed();
            if (range == null)
            {
                return 0;
            }

            var rows = range.RowsUsed().ToList();
            if (rows.Count <= 1)
            {
                return 0;
            }

            var headerMap = BuildHeaderMap(rows.First());
            var dataRows = rows.Skip(1).ToList();

            var records = headerMap.Count > 0
                ? BuildVisitorRecordsFromHeaders(dataRows, headerMap)
                : new List<VisitorImportModel>();

            if (records.Count == 0)
            {
                records = BuildVisitorRecordsFromLegacy(dataRows);
            }

            if (records.Count == 0)
            {
                return 0;
            }

            return await UpsertVisitorsAsync(records);
        }

        public async Task<int> ImportEmployeesAsync(IFormFile file)
        {
            if (file == null)
            {
                throw new ArgumentNullException(nameof(file));
            }

            using var stream = new MemoryStream();
            await file.CopyToAsync(stream);
            stream.Position = 0;
            using var workbook = new XLWorkbook(stream);
            var worksheet = workbook.Worksheets.FirstOrDefault();
            if (worksheet == null)
            {
                return 0;
            }

            var range = worksheet.RangeUsed();
            if (range == null)
            {
                return 0;
            }

            var rows = range.RowsUsed().ToList();
            if (rows.Count <= 1)
            {
                return 0;
            }

            var headerMap = BuildHeaderMap(rows.First());
            var dataRows = rows.Skip(1).ToList();

            var records = headerMap.Count > 0
                ? BuildEmployeeRecordsFromHeaders(dataRows, headerMap)
                : new List<EmployeeImportModel>();

            if (records.Count == 0)
            {
                records = BuildEmployeeRecordsFromLegacy(dataRows);
            }

            if (records.Count == 0)
            {
                return 0;
            }

            return await UpsertEmployeesAsync(records);
        }

        public async Task<int> ImportDepartmentsAsync(IFormFile file)
        {
            if (file == null)
            {
                throw new ArgumentNullException(nameof(file));
            }

            using var stream = new MemoryStream();
            await file.CopyToAsync(stream);
            stream.Position = 0;
            using var workbook = new XLWorkbook(stream);
            var worksheet = workbook.Worksheets.FirstOrDefault();
            if (worksheet == null)
            {
                return 0;
            }

            var range = worksheet.RangeUsed();
            if (range == null)
            {
                return 0;
            }

            var rows = range.RowsUsed().ToList();
            if (rows.Count <= 1)
            {
                return 0;
            }

            var headerMap = BuildHeaderMap(rows.First());
            var records = rows
                .Skip(1)
                .Select(row => new
                {
                    Name = GetString(row, headerMap, DepartmentHeaders),
                    Description = GetString(row, headerMap, DepartmentDescriptionHeaders)
                })
                .Where(r => !string.IsNullOrWhiteSpace(r.Name))
                .Select(r => new
                {
                    Name = r.Name!.Trim(),
                    Description = string.IsNullOrWhiteSpace(r.Description) ? null : r.Description!.Trim()
                })
                .ToList();

            if (records.Count == 0)
            {
                return 0;
            }

            var distinct = records
                .GroupBy(r => r.Name, StringComparer.OrdinalIgnoreCase)
                .Select(group => new
                {
                    Name = group.Key,
                    Description = group.Select(r => r.Description).FirstOrDefault(d => !string.IsNullOrWhiteSpace(d))
                })
                .ToList();

            var names = distinct.Select(d => d.Name).ToList();
            var existing = await _context.Departments
                .Where(d => names.Contains(d.Name))
                .ToDictionaryAsync(d => d.Name, StringComparer.OrdinalIgnoreCase);

            var imported = 0;
            foreach (var entry in distinct)
            {
                if (existing.TryGetValue(entry.Name, out var department))
                {
                    if (!string.IsNullOrWhiteSpace(entry.Description) && entry.Description != department.Description)
                    {
                        department.Description = entry.Description;
                    }
                }
                else
                {
                    department = new Department
                    {
                        Name = entry.Name,
                        Description = entry.Description
                    };

                    _context.Departments.Add(department);
                    existing[entry.Name] = department;
                    imported++;
                }
            }

            if (_context.ChangeTracker.HasChanges())
            {
                await _context.SaveChangesAsync();
            }

            return imported;
        }

        public async Task<int> ImportRolesAsync(IFormFile file)
        {
            if (file == null)
            {
                throw new ArgumentNullException(nameof(file));
            }

            using var stream = new MemoryStream();
            await file.CopyToAsync(stream);
            stream.Position = 0;
            using var workbook = new XLWorkbook(stream);
            var worksheet = workbook.Worksheets.FirstOrDefault();
            if (worksheet == null)
            {
                return 0;
            }

            var range = worksheet.RangeUsed();
            if (range == null)
            {
                return 0;
            }

            var rows = range.RowsUsed().ToList();
            if (rows.Count <= 1)
            {
                return 0;
            }

            var headerMap = BuildHeaderMap(rows.First());
            var records = rows
                .Skip(1)
                .Select(row => new
                {
                    Name = GetString(row, headerMap, RoleNameHeaders),
                    Description = GetString(row, headerMap, RoleDescriptionHeaders)
                })
                .Where(r => !string.IsNullOrWhiteSpace(r.Name))
                .Select(r => new
                {
                    Name = r.Name!.Trim(),
                    Description = string.IsNullOrWhiteSpace(r.Description) ? null : r.Description!.Trim()
                })
                .ToList();

            if (records.Count == 0)
            {
                return 0;
            }

            var distinct = records
                .GroupBy(r => r.Name, StringComparer.OrdinalIgnoreCase)
                .Select(group => new
                {
                    Name = group.Key,
                    Description = group.Select(r => r.Description).FirstOrDefault(d => !string.IsNullOrWhiteSpace(d))
                })
                .ToList();

            var names = distinct.Select(d => d.Name).ToList();
            var existing = await _context.Roles
                .Where(r => names.Contains(r.Name))
                .ToDictionaryAsync(r => r.Name, StringComparer.OrdinalIgnoreCase);

            var imported = 0;
            foreach (var entry in distinct)
            {
                if (existing.TryGetValue(entry.Name, out var role))
                {
                    if (!string.IsNullOrWhiteSpace(entry.Description) && entry.Description != role.Description)
                    {
                        role.Description = entry.Description;
                    }
                }
                else
                {
                    role = new Role
                    {
                        Name = entry.Name,
                        Description = entry.Description
                    };

                    _context.Roles.Add(role);
                    existing[entry.Name] = role;
                    imported++;
                }
            }

            if (_context.ChangeTracker.HasChanges())
            {
                await _context.SaveChangesAsync();
            }

            return imported;
        }

        public async Task<int> ImportStatusesAsync(IFormFile file)
        {
            if (file == null)
            {
                throw new ArgumentNullException(nameof(file));
            }

            using var stream = new MemoryStream();
            await file.CopyToAsync(stream);
            stream.Position = 0;
            using var workbook = new XLWorkbook(stream);
            var worksheet = workbook.Worksheets.FirstOrDefault();
            if (worksheet == null)
            {
                return 0;
            }

            var range = worksheet.RangeUsed();
            if (range == null)
            {
                return 0;
            }

            var rows = range.RowsUsed().ToList();
            if (rows.Count <= 1)
            {
                return 0;
            }

            var headerMap = BuildHeaderMap(rows.First());
            var names = rows
                .Skip(1)
                .Select(row => GetString(row, headerMap, StatusNameHeaders))
                .Where(name => !string.IsNullOrWhiteSpace(name))
                .Select(name => name!.Trim())
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToList();

            if (names.Count == 0)
            {
                return 0;
            }

            var existing = await _context.ApplicationStatuses
                .Where(s => names.Contains(s.StatusName))
                .ToDictionaryAsync(s => s.StatusName, StringComparer.OrdinalIgnoreCase);

            var imported = 0;
            foreach (var name in names)
            {
                if (existing.ContainsKey(name))
                {
                    continue;
                }

                var status = new ApplicationStatus
                {
                    StatusName = name
                };

                _context.ApplicationStatuses.Add(status);
                existing[name] = status;
                imported++;
            }

            if (_context.ChangeTracker.HasChanges())
            {
                await _context.SaveChangesAsync();
            }

            return imported;
        }

        public async Task<int> ImportGroupsAsync(IFormFile file)
        {
            if (file == null)
            {
                throw new ArgumentNullException(nameof(file));
            }

            using var stream = new MemoryStream();
            await file.CopyToAsync(stream);
            stream.Position = 0;
            using var workbook = new XLWorkbook(stream);
            var worksheet = workbook.Worksheets.FirstOrDefault();
            if (worksheet == null)
            {
                return 0;
            }

            var range = worksheet.RangeUsed();
            if (range == null)
            {
                return 0;
            }

            var rows = range.RowsUsed().ToList();
            if (rows.Count <= 1)
            {
                return 0;
            }

            var headerMap = BuildHeaderMap(rows.First());
            var records = rows
                .Skip(1)
                .Select(row => new
                {
                    Name = GetString(row, headerMap, GroupNameHeaders),
                    Description = GetString(row, headerMap, GroupDescriptionHeaders)
                })
                .Where(r => !string.IsNullOrWhiteSpace(r.Name))
                .GroupBy(r => r.Name!.Trim(), StringComparer.OrdinalIgnoreCase)
                .Select(group => new
                {
                    Name = group.Key,
                    Description = group.Select(g => g.Description)
                        .Select(value => string.IsNullOrWhiteSpace(value) ? null : value!.Trim())
                        .FirstOrDefault(value => !string.IsNullOrWhiteSpace(value))
                })
                .ToList();

            if (records.Count == 0)
            {
                return 0;
            }

            var names = records.Select(r => r.Name).ToList();
            var existing = await _context.Groups
                .Where(g => g.GroupName != null && names.Contains(g.GroupName))
                .ToDictionaryAsync(g => g.GroupName!, StringComparer.OrdinalIgnoreCase);

            var imported = 0;
            foreach (var record in records)
            {
                if (existing.TryGetValue(record.Name, out var group))
                {
                    if (!string.IsNullOrWhiteSpace(record.Description) && string.IsNullOrWhiteSpace(group.Description))
                    {
                        group.Description = record.Description;
                    }
                    continue;
                }

                group = new Group
                {
                    GroupName = record.Name,
                    Description = record.Description
                };

                _context.Groups.Add(group);
                existing[record.Name] = group;
                imported++;
            }

            if (_context.ChangeTracker.HasChanges())
            {
                await _context.SaveChangesAsync();
            }

            return imported;
        }


        public async Task<SessionImportResult> ImportSessionsAsync(SessionImportOptions options)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            if (options.ExcelFile == null)
            {
                throw new ArgumentNullException(nameof(options.ExcelFile));
            }

            var result = new SessionImportResult();
            var attachments = options.Attachments?.ToList() ?? new List<IFormFile>();
            var attachmentCatalog = new AttachmentCatalog(attachments);

            using var stream = new MemoryStream();
            await options.ExcelFile.CopyToAsync(stream);
            stream.Position = 0;

            using var workbook = new XLWorkbook(stream);
            var worksheet = workbook.Worksheets.FirstOrDefault();
            if (worksheet == null)
            {
                result.Warnings.Add("Файл Excel не содержит листов.");
                return result;
            }

            var range = worksheet.RangeUsed();
            if (range == null)
            {
                result.Warnings.Add("Файл Excel не содержит данных.");
                return result;
            }

            var rows = range.RowsUsed().ToList();
            if (rows.Count <= 1)
            {
                result.Warnings.Add("Файл Excel не содержит строк с данными.");
                return result;
            }

            var headerMap = BuildHeaderMap(rows.First());
            if (headerMap.Count == 0)
            {
                result.Warnings.Add("Не удалось определить заголовки столбцов в файле Excel.");
                return result;
            }

            var dataRows = rows.Skip(1).ToList();

            var departmentNames = dataRows
                .Select(row => GetString(row, headerMap, DepartmentHeaders))
                .Where(name => !string.IsNullOrWhiteSpace(name))
                .Select(name => name!.Trim())
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToList();

            var departmentMap = departmentNames.Count > 0
                ? await _context.Departments
                    .Where(d => departmentNames.Contains(d.Name))
                    .ToDictionaryAsync(d => d.Name, StringComparer.OrdinalIgnoreCase)
                : new Dictionary<string, Department>(StringComparer.OrdinalIgnoreCase);

            foreach (var name in departmentNames)
            {
                if (departmentMap.ContainsKey(name))
                {
                    continue;
                }

                var department = new Department { Name = name };
                _context.Departments.Add(department);
                departmentMap[name] = department;
                result.CreatedDepartments++;
            }

            if (_context.ChangeTracker.Entries<Department>().Any(e => e.State == EntityState.Added))
            {
                await _context.SaveChangesAsync();
            }

            var statusNames = dataRows
                .Select(row => GetString(row, headerMap, StatusHeaders) ?? "Pending")
                .Select(status => string.IsNullOrWhiteSpace(status) ? "Pending" : status!.Trim())
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToList();

            var statusMap = statusNames.Count > 0
                ? await _context.ApplicationStatuses
                    .Where(s => statusNames.Contains(s.StatusName))
                    .ToDictionaryAsync(s => s.StatusName, StringComparer.OrdinalIgnoreCase)
                : new Dictionary<string, ApplicationStatus>(StringComparer.OrdinalIgnoreCase);

            foreach (var statusName in statusNames)
            {
                if (statusMap.ContainsKey(statusName))
                {
                    continue;
                }

                var status = new ApplicationStatus { StatusName = statusName };
                _context.ApplicationStatuses.Add(status);
                statusMap[statusName] = status;
            }

            if (_context.ChangeTracker.Entries<ApplicationStatus>().Any(e => e.State == EntityState.Added))
            {
                await _context.SaveChangesAsync();
            }

            var sessionCodes = dataRows
                .Select(row => GetString(row, headerMap, SessionCodeHeaders))
                .Where(code => !string.IsNullOrWhiteSpace(code))
                .Select(code => code!.Trim())
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToList();

            var existingSessions = sessionCodes.Count > 0
                ? await _context.PassRequests
                    .Where(r => r.Note != null && sessionCodes.Contains(r.Note))
                    .Include(r => r.Department)
                    .ToDictionaryAsync(r => r.Note!, StringComparer.OrdinalIgnoreCase)
                : new Dictionary<string, PassRequest>(StringComparer.OrdinalIgnoreCase);

            var existingSessionCodes = new HashSet<string>(existingSessions.Keys, StringComparer.OrdinalIgnoreCase);
            var processedCodes = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            var departmentIds = departmentMap.Values.Select(d => d.Id).Where(id => id != 0).Distinct().ToList();
            var existingAttachments = departmentIds.Count > 0
                ? await _context.FileAttachments
                    .Where(f => f.DepartmentId != null && departmentIds.Contains(f.DepartmentId.Value))
                    .ToListAsync()
                : new List<FileAttachment>();

            var attachmentIndex = existingAttachments
                .GroupBy(f => f.DepartmentId!.Value)
                .ToDictionary(
                    g => g.Key,
                    g => g.ToDictionary(a => SanitizeFileName(a.FileName), a => a, StringComparer.OrdinalIgnoreCase));

            var pendingAttachments = new List<PendingAttachment>();

            foreach (var row in dataRows)
            {
                var rowNumber = row.RowNumber();
                var departmentName = GetString(row, headerMap, DepartmentHeaders);
                if (string.IsNullOrWhiteSpace(departmentName))
                {
                    result.Warnings.Add($"Строка {rowNumber}: не указано здание или подразделение.");
                    continue;
                }

                departmentName = departmentName.Trim();
                if (!departmentMap.TryGetValue(departmentName, out var department))
                {
                    department = new Department { Name = departmentName };
                    _context.Departments.Add(department);
                    departmentMap[departmentName] = department;
                    result.CreatedDepartments++;
                }

                var statusName = GetString(row, headerMap, StatusHeaders);
                statusName = string.IsNullOrWhiteSpace(statusName) ? "Pending" : statusName!.Trim();
                if (!statusMap.TryGetValue(statusName, out var status))
                {
                    status = new ApplicationStatus { StatusName = statusName };
                    _context.ApplicationStatuses.Add(status);
                    statusMap[statusName] = status;
                }

                var requestType = GetString(row, headerMap, RequestTypeHeaders);
                if (string.IsNullOrWhiteSpace(requestType))
                {
                    requestType = "Personal";
                }

                var startDate = GetDate(row, headerMap, StartDateHeaders) ?? DateTime.UtcNow.Date;
                var endDate = GetDate(row, headerMap, EndDateHeaders) ?? startDate;
                if (endDate < startDate)
                {
                    endDate = startDate;
                }

                var purpose = GetString(row, headerMap, PurposeHeaders);
                if (string.IsNullOrWhiteSpace(purpose))
                {
                    purpose = $"Посещение {department.Name}";
                }

                var sessionCode = GetString(row, headerMap, SessionCodeHeaders)?.Trim();
                var existedBeforeImport = false;
                PassRequest request;

                if (!string.IsNullOrWhiteSpace(sessionCode) && existingSessions.TryGetValue(sessionCode, out var existingRequest))
                {
                    request = existingRequest;
                    existedBeforeImport = existingSessionCodes.Contains(sessionCode);
                }
                else
                {
                    request = new PassRequest();
                    _context.PassRequests.Add(request);
                    if (!string.IsNullOrWhiteSpace(sessionCode))
                    {
                        existingSessions[sessionCode] = request;
                    }
                }

                request.Department = department;
                request.DepartmentId = department.Id;
                request.RequestType = requestType;
                request.StartDate = startDate;
                request.EndDate = endDate;
                request.Purpose = purpose;
                request.ApplicantLastName = GetString(row, headerMap, ApplicantLastNameHeaders);
                request.ApplicantFirstName = GetString(row, headerMap, ApplicantFirstNameHeaders);
                request.ApplicantMiddleName = GetString(row, headerMap, ApplicantMiddleNameHeaders);
                request.Phone = GetString(row, headerMap, PhoneHeaders);
                request.ApplicantEmail = GetString(row, headerMap, EmailHeaders);
                request.Status = statusName;
                request.StatusRef = status;
                request.StatusID = status.StatusID;
                request.Note = sessionCode;

                if (!string.IsNullOrWhiteSpace(sessionCode))
                {
                    var firstOccurrence = processedCodes.Add(sessionCode);
                    if (existedBeforeImport)
                    {
                        if (firstOccurrence)
                        {
                            result.UpdatedSessions++;
                        }
                    }
                    else
                    {
                        if (firstOccurrence)
                        {
                            result.CreatedSessions++;
                        }
                    }
                }
                else
                {
                    if (!existedBeforeImport)
                    {
                        result.CreatedSessions++;
                    }
                }

                var photoName = GetString(row, headerMap, PhotoHeaders);
                if (!string.IsNullOrWhiteSpace(photoName))
                {
                    pendingAttachments.Add(new PendingAttachment(department, photoName.Trim(), "Image", rowNumber));
                }

                var pdfName = GetString(row, headerMap, PdfHeaders);
                if (!string.IsNullOrWhiteSpace(pdfName))
                {
                    pendingAttachments.Add(new PendingAttachment(department, pdfName.Trim(), "Document", rowNumber));
                }
            }

            if (_context.ChangeTracker.HasChanges())
            {
                await _context.SaveChangesAsync();
            }

            foreach (var pending in pendingAttachments)
            {
                if (pending.Department.Id == 0)
                {
                    await _context.Entry(pending.Department).ReloadAsync();
                }

                if (pending.Department.Id == 0)
                {
                    result.Warnings.Add($"Строка {pending.RowNumber}: невозможно сохранить файл \"{pending.FileName}\" без идентификатора здания.");
                    continue;
                }

                if (!attachmentCatalog.TryTake(pending.FileName, out var uploadFile, out var resolvedName))
                {
                    result.Warnings.Add($"Строка {pending.RowNumber}: файл \"{pending.FileName}\" не найден среди вложений.");
                    continue;
                }

                var sanitizedFileName = SanitizeFileName(resolvedName);
                if (attachmentIndex.TryGetValue(pending.Department.Id, out var departmentFiles) &&
                    departmentFiles.ContainsKey(sanitizedFileName))
                {
                    result.Warnings.Add($"Строка {pending.RowNumber}: файл \"{sanitizedFileName}\" уже прикреплен к зданию \"{pending.Department.Name}\".");
                    continue;
                }

                var attachment = await SaveAttachmentAsync(pending.Department, uploadFile, pending.TypeName, sanitizedFileName);
                if (!attachmentIndex.TryGetValue(pending.Department.Id, out departmentFiles))
                {
                    departmentFiles = new Dictionary<string, FileAttachment>(StringComparer.OrdinalIgnoreCase);
                    attachmentIndex[pending.Department.Id] = departmentFiles;
                }

                var storedKey = SanitizeFileName(attachment.FileName);
                departmentFiles[storedKey] = attachment;
                result.SavedAttachments++;
            }

            if (_context.ChangeTracker.Entries<FileAttachment>().Any(e => e.State == EntityState.Added))
            {
                await _context.SaveChangesAsync();
            }

            return result;
        }

        private List<VisitorImportModel> BuildVisitorRecordsFromHeaders(
            IReadOnlyCollection<IXLRangeRow> rows,
            IReadOnlyDictionary<string, int> headerMap)
        {
            var records = new List<VisitorImportModel>();
            foreach (var row in rows)
            {
                var fullName = GetString(row, headerMap, VisitorFullNameHeaders);
                if (string.IsNullOrWhiteSpace(fullName))
                {
                    continue;
                }

                var (series, number) = SplitPassport(GetString(row, headerMap, VisitorPassportHeaders));
                if (string.IsNullOrWhiteSpace(series) || string.IsNullOrWhiteSpace(number))
                {
                    continue;
                }

                var (lastName, firstName, middleName) = SplitFullName(fullName);
                var birthDate = ParseFlexibleDate(
                    GetDate(row, headerMap, VisitorBirthDateHeaders),
                    GetString(row, headerMap, VisitorBirthDateHeaders));

                records.Add(new VisitorImportModel
                {
                    LastName = (lastName ?? fullName).Trim(),
                    FirstName = (firstName ?? fullName).Trim(),
                    MiddleName = string.IsNullOrWhiteSpace(middleName) ? null : middleName,
                    Phone = NormalizePhone(GetString(row, headerMap, VisitorPhoneHeaders)),
                    Email = GetString(row, headerMap, EmailHeaders)?.Trim(),
                    BirthDate = birthDate,
                    PassportSeries = series!,
                    PassportNumber = number!,
                    GroupName = NormalizeGroupName(GetString(row, headerMap, VisitorGroupHeaders)),
                    GroupDescription = NormalizeGroupDescription(GetString(row, headerMap, VisitorGroupDescriptionHeaders))
                });
            }

            return records;
        }

        private static List<VisitorImportModel> BuildVisitorRecordsFromLegacy(IReadOnlyCollection<IXLRangeRow> rows)
        {
            var records = new List<VisitorImportModel>();
            foreach (var row in rows)
            {
                var lastName = row.Cell(1).GetString();
                var firstName = row.Cell(2).GetString();
                var middleName = row.Cell(3).GetString();
                var passportSeries = row.Cell(6).GetString();
                var passportNumber = row.Cell(7).GetString();

                if (string.IsNullOrWhiteSpace(lastName) || string.IsNullOrWhiteSpace(firstName) ||
                    string.IsNullOrWhiteSpace(passportSeries) || string.IsNullOrWhiteSpace(passportNumber))
                {
                    continue;
                }

                records.Add(new VisitorImportModel
                {
                    LastName = lastName.Trim(),
                    FirstName = firstName.Trim(),
                    MiddleName = string.IsNullOrWhiteSpace(middleName) ? null : middleName.Trim(),
                    Phone = NormalizePhone(row.Cell(4).GetString()),
                    Email = string.IsNullOrWhiteSpace(row.Cell(5).GetString()) ? null : row.Cell(5).GetString().Trim(),
                    PassportSeries = passportSeries.Trim(),
                    PassportNumber = passportNumber.Trim()
                });
            }

            return records;
        }

        private async Task<int> UpsertVisitorsAsync(List<VisitorImportModel> records)
        {
            if (records.Count == 0)
            {
                return 0;
            }

            records = records
                .GroupBy(r => r.PassportKey, StringComparer.OrdinalIgnoreCase)
                .Select(MergeVisitorRecords)
                .ToList();

            var groupNames = records
                .Select(r => r.GroupName)
                .Where(name => !string.IsNullOrWhiteSpace(name))
                .Select(name => name!)
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToList();

            var groupMap = groupNames.Count > 0
                ? await _context.Groups
                    .Where(g => g.GroupName != null && groupNames.Contains(g.GroupName))
                    .ToDictionaryAsync(g => g.GroupName!, StringComparer.OrdinalIgnoreCase)
                : new Dictionary<string, Group>(StringComparer.OrdinalIgnoreCase);

            foreach (var name in groupNames)
            {
                if (groupMap.ContainsKey(name))
                {
                    continue;
                }

                var description = records
                    .FirstOrDefault(r => string.Equals(r.GroupName, name, StringComparison.OrdinalIgnoreCase))?
                    .GroupDescription;

                var group = new Group
                {
                    GroupName = name,
                    Description = string.IsNullOrWhiteSpace(description) ? null : description
                };

                _context.Groups.Add(group);
                groupMap[name] = group;
            }

            foreach (var record in records)
            {
                if (!string.IsNullOrWhiteSpace(record.GroupName) &&
                    groupMap.TryGetValue(record.GroupName!, out var groupRef) &&
                    string.IsNullOrWhiteSpace(groupRef.Description) &&
                    !string.IsNullOrWhiteSpace(record.GroupDescription))
                {
                    groupRef.Description = record.GroupDescription;
                }
            }

            var passportKeys = records
                .Select(r => r.PassportKey)
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToList();

            var existing = passportKeys.Count > 0
                ? await _context.PassVisitors
                    .Include(v => v.Group)
                    .Where(v => passportKeys.Contains(v.PassportSeries + v.PassportNumber))
                    .ToDictionaryAsync(v => v.PassportSeries + v.PassportNumber, StringComparer.OrdinalIgnoreCase)
                : new Dictionary<string, PassVisitor>(StringComparer.OrdinalIgnoreCase);

            var imported = 0;
            foreach (var record in records)
            {
                var key = record.PassportKey;
                existing.TryGetValue(key, out var visitor);

                Group? group = null;
                if (!string.IsNullOrWhiteSpace(record.GroupName) &&
                    groupMap.TryGetValue(record.GroupName!, out var resolvedGroup))
                {
                    group = resolvedGroup;
                }

                if (visitor == null)
                {
                    visitor = new PassVisitor
                    {
                        LastName = record.LastName,
                        FirstName = record.FirstName,
                        MiddleName = record.MiddleName,
                        Phone = record.Phone,
                        Email = record.Email,
                        BirthDate = record.BirthDate,
                        PassportSeries = record.PassportSeries,
                        PassportNumber = record.PassportNumber,
                        Group = group
                    };

                    _context.PassVisitors.Add(visitor);
                    existing[key] = visitor;
                    imported++;
                }
                else
                {
                    visitor.LastName = record.LastName;
                    visitor.FirstName = record.FirstName;
                    visitor.MiddleName = record.MiddleName;
                    visitor.Phone = record.Phone;
                    visitor.Email = record.Email;
                    visitor.BirthDate = record.BirthDate;
                    if (group != null)
                    {
                        visitor.Group = group;
                    }
                }

                if (group != null)
                {
                    visitor.Group = group;
                }
            }

            if (_context.ChangeTracker.HasChanges())
            {
                await _context.SaveChangesAsync();
            }

            return imported;
        }

        private static VisitorImportModel MergeVisitorRecords(IGrouping<string, VisitorImportModel> group)
        {
            var result = new VisitorImportModel
            {
                PassportSeries = group.First().PassportSeries,
                PassportNumber = group.First().PassportNumber
            };

            foreach (var record in group)
            {
                if (!string.IsNullOrWhiteSpace(record.LastName))
                {
                    result.LastName = record.LastName;
                }

                if (!string.IsNullOrWhiteSpace(record.FirstName))
                {
                    result.FirstName = record.FirstName;
                }

                if (!string.IsNullOrWhiteSpace(record.MiddleName))
                {
                    result.MiddleName = record.MiddleName;
                }

                if (!string.IsNullOrWhiteSpace(record.Phone))
                {
                    result.Phone = record.Phone;
                }

                if (!string.IsNullOrWhiteSpace(record.Email))
                {
                    result.Email = record.Email;
                }

                if (record.BirthDate.HasValue)
                {
                    result.BirthDate = record.BirthDate;
                }

                if (!string.IsNullOrWhiteSpace(record.GroupName))
                {
                    result.GroupName = record.GroupName;
                }

                if (!string.IsNullOrWhiteSpace(record.GroupDescription))
                {
                    result.GroupDescription = record.GroupDescription;
                }
            }

            if (string.IsNullOrWhiteSpace(result.LastName))
            {
                result.LastName = group.First().LastName;
            }

            if (string.IsNullOrWhiteSpace(result.FirstName))
            {
                result.FirstName = group.First().FirstName;
            }

            return result;
        }

        private List<EmployeeImportModel> BuildEmployeeRecordsFromHeaders(
            IReadOnlyCollection<IXLRangeRow> rows,
            IReadOnlyDictionary<string, int> headerMap)
        {
            var records = new List<EmployeeImportModel>();
            foreach (var row in rows)
            {
                var fullName = GetString(row, headerMap, EmployeeFullNameHeaders);
                if (string.IsNullOrWhiteSpace(fullName))
                {
                    continue;
                }

                var primaryDepartment = NormalizeDepartmentValue(GetString(row, headerMap, EmployeeDepartmentHeaders));
                var secondaryDepartment = NormalizeDepartmentValue(GetString(row, headerMap, EmployeeSubDepartmentHeaders));
                var departmentName = primaryDepartment ?? secondaryDepartment;

                string? departmentDescription = null;
                if (!string.IsNullOrWhiteSpace(primaryDepartment) &&
                    !string.IsNullOrWhiteSpace(secondaryDepartment) &&
                    !string.Equals(primaryDepartment, secondaryDepartment, StringComparison.OrdinalIgnoreCase))
                {
                    departmentDescription = secondaryDepartment;
                }
                else if (string.IsNullOrWhiteSpace(primaryDepartment) && !string.IsNullOrWhiteSpace(secondaryDepartment))
                {
                    departmentDescription = secondaryDepartment;
                }

                records.Add(new EmployeeImportModel
                {
                    FullName = fullName.Trim(),
                    EmployeeCode = NormalizeEmployeeCode(GetString(row, headerMap, EmployeeCodeHeaders)),
                    DepartmentName = departmentName,
                    DepartmentDescription = departmentDescription
                });
            }

            return records;
        }

        private static List<EmployeeImportModel> BuildEmployeeRecordsFromLegacy(IReadOnlyCollection<IXLRangeRow> rows)
        {
            var records = new List<EmployeeImportModel>();
            foreach (var row in rows)
            {
                var fullName = row.Cell(1).GetString();
                if (string.IsNullOrWhiteSpace(fullName))
                {
                    continue;
                }

                var employeeCode = row.Cell(2).GetString();
                var departmentId = row.Cell(3).GetValue<int?>();

                records.Add(new EmployeeImportModel
                {
                    FullName = fullName.Trim(),
                    EmployeeCode = string.IsNullOrWhiteSpace(employeeCode) ? null : employeeCode.Trim(),
                    DepartmentId = departmentId
                });
            }

            return records;
        }

        private async Task<int> UpsertEmployeesAsync(List<EmployeeImportModel> records)
        {
            if (records.Count == 0)
            {
                return 0;
            }

            records = records
                .GroupBy(r => string.IsNullOrWhiteSpace(r.EmployeeCode) ? r.FullName : r.EmployeeCode!, StringComparer.OrdinalIgnoreCase)
                .Select(MergeEmployeeRecords)
                .ToList();

            var departmentNames = records
                .Select(r => r.DepartmentName)
                .Where(name => !string.IsNullOrWhiteSpace(name))
                .Select(name => name!)
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToList();

            var departmentMap = departmentNames.Count > 0
                ? await _context.Departments
                    .Where(d => departmentNames.Contains(d.Name))
                    .ToDictionaryAsync(d => d.Name, StringComparer.OrdinalIgnoreCase)
                : new Dictionary<string, Department>(StringComparer.OrdinalIgnoreCase);

            foreach (var name in departmentNames)
            {
                if (departmentMap.ContainsKey(name))
                {
                    continue;
                }

                var description = records
                    .FirstOrDefault(r => string.Equals(r.DepartmentName, name, StringComparison.OrdinalIgnoreCase))?
                    .DepartmentDescription;

                var department = new Department
                {
                    Name = name,
                    Description = string.IsNullOrWhiteSpace(description) ? null : description
                };

                _context.Departments.Add(department);
                departmentMap[name] = department;
            }

            foreach (var record in records)
            {
                if (!string.IsNullOrWhiteSpace(record.DepartmentName) &&
                    departmentMap.TryGetValue(record.DepartmentName, out var department) &&
                    string.IsNullOrWhiteSpace(department.Description) &&
                    !string.IsNullOrWhiteSpace(record.DepartmentDescription))
                {
                    department.Description = record.DepartmentDescription;
                }
            }

            var codes = records
                .Where(r => !string.IsNullOrWhiteSpace(r.EmployeeCode))
                .Select(r => r.EmployeeCode!)
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToList();

            var existingByCode = codes.Count > 0
                ? await _context.Employees
                    .Where(e => e.EmployeeCode != null && codes.Contains(e.EmployeeCode))
                    .ToDictionaryAsync(e => e.EmployeeCode!, StringComparer.OrdinalIgnoreCase)
                : new Dictionary<string, Employee>(StringComparer.OrdinalIgnoreCase);

            var names = records
                .Select(r => r.FullName)
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToList();

            var existingByName = names.Count > 0
                ? await _context.Employees
                    .Where(e => names.Contains(e.FullName))
                    .ToDictionaryAsync(e => e.FullName, StringComparer.OrdinalIgnoreCase)
                : new Dictionary<string, Employee>(StringComparer.OrdinalIgnoreCase);

            var imported = 0;
            foreach (var record in records)
            {
                Employee? employee = null;
                if (!string.IsNullOrWhiteSpace(record.EmployeeCode) &&
                    existingByCode.TryGetValue(record.EmployeeCode, out var byCode))
                {
                    employee = byCode;
                }
                else if (existingByName.TryGetValue(record.FullName, out var byName))
                {
                    employee = byName;
                }

                Department? department = null;
                if (!string.IsNullOrWhiteSpace(record.DepartmentName) &&
                    departmentMap.TryGetValue(record.DepartmentName, out var mappedDepartment))
                {
                    department = mappedDepartment;
                }

                var departmentId = record.DepartmentId ?? department?.Id;

                if (employee == null)
                {
                    employee = new Employee
                    {
                        FullName = record.FullName,
                        EmployeeCode = record.EmployeeCode,
                        DepartmentId = departmentId
                    };

                    if (department != null)
                    {
                        employee.Department = department;
                    }

                    _context.Employees.Add(employee);
                    imported++;

                    if (!string.IsNullOrWhiteSpace(record.EmployeeCode))
                    {
                        existingByCode[record.EmployeeCode] = employee;
                    }

                    existingByName[record.FullName] = employee;
                }
                else
                {
                    employee.FullName = record.FullName;
                    if (!string.IsNullOrWhiteSpace(record.EmployeeCode))
                    {
                        employee.EmployeeCode = record.EmployeeCode;
                    }
                }

                if (department != null)
                {
                    employee.Department = department;
                }
                else if (departmentId.HasValue)
                {
                    employee.DepartmentId = departmentId;
                }
            }

            if (_context.ChangeTracker.HasChanges())
            {
                await _context.SaveChangesAsync();
            }

            return imported;
        }

        private static EmployeeImportModel MergeEmployeeRecords(IGrouping<string, EmployeeImportModel> group)
        {
            var result = new EmployeeImportModel
            {
                FullName = group.First().FullName,
                EmployeeCode = group.First().EmployeeCode,
                DepartmentId = group.First().DepartmentId,
                DepartmentName = group.First().DepartmentName,
                DepartmentDescription = group.First().DepartmentDescription
            };

            foreach (var record in group)
            {
                if (!string.IsNullOrWhiteSpace(record.FullName))
                {
                    result.FullName = record.FullName;
                }

                if (!string.IsNullOrWhiteSpace(record.EmployeeCode))
                {
                    result.EmployeeCode = record.EmployeeCode;
                }

                if (record.DepartmentId.HasValue)
                {
                    result.DepartmentId = record.DepartmentId;
                }

                if (!string.IsNullOrWhiteSpace(record.DepartmentName))
                {
                    result.DepartmentName = record.DepartmentName;
                }

                if (!string.IsNullOrWhiteSpace(record.DepartmentDescription))
                {
                    result.DepartmentDescription = record.DepartmentDescription;
                }
            }

            return result;
        }

        private static string? NormalizeDepartmentValue(string? value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return null;
            }

            var trimmed = value.Trim();
            if (trimmed == "-" || trimmed == "—" || trimmed.Equals("нет", StringComparison.OrdinalIgnoreCase))
            {
                return null;
            }

            return trimmed;
        }

        private static string? NormalizeEmployeeCode(string? value) => string.IsNullOrWhiteSpace(value) ? null : value.Trim();

        private static string? NormalizePhone(string? value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return null;
            }

            var trimmed = value.Trim();
            var digits = Regex.Replace(trimmed, "\\D", string.Empty);

            if (digits.Length == 11 && digits.StartsWith("8"))
            {
                digits = "7" + digits.Substring(1);
            }

            if (digits.Length == 11 && digits.StartsWith("7"))
            {
                return "+" + digits;
            }

            if (digits.Length == 10)
            {
                return "+7" + digits;
            }

            return trimmed;
        }

        private static string? NormalizeGroupName(string? value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return null;
            }

            var trimmed = value.Trim();
            return string.IsNullOrWhiteSpace(trimmed) ? null : trimmed;
        }

        private static string? NormalizeGroupDescription(string? value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return null;
            }

            return value.Trim();
        }

        private static DateTime? ParseFlexibleDate(DateTime? dateValue, string? textValue)
        {
            if (dateValue.HasValue)
            {
                return dateValue;
            }

            if (string.IsNullOrWhiteSpace(textValue))
            {
                return null;
            }

            var normalized = textValue.Replace("года", string.Empty, StringComparison.OrdinalIgnoreCase).Trim();
            if (DateTime.TryParse(normalized, CultureInfo.InvariantCulture, DateTimeStyles.AssumeLocal, out var parsed) ||
                DateTime.TryParse(normalized, new CultureInfo("ru-RU"), DateTimeStyles.AssumeLocal, out parsed))
            {
                return parsed;
            }

            return null;
        }

        private static (string? LastName, string? FirstName, string? MiddleName) SplitFullName(string? fullName)
        {
            if (string.IsNullOrWhiteSpace(fullName))
            {
                return (null, null, null);
            }

            var parts = fullName.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            return parts.Length switch
            {
                0 => (null, null, null),
                1 => (parts[0], parts[0], null),
                2 => (parts[0], parts[1], null),
                _ => (parts[0], parts[1], string.Join(" ", parts.Skip(2)))
            };
        }

        private static (string? Series, string? Number) SplitPassport(string? passport)
        {
            if (string.IsNullOrWhiteSpace(passport))
            {
                return (null, null);
            }

            var digits = Regex.Replace(passport, "\\D", string.Empty);
            if (digits.Length < 10)
            {
                return (null, null);
            }

            var series = digits.Substring(0, 4);
            var number = digits.Substring(4);
            if (number.Length < 6)
            {
                return (null, null);
            }

            return (series, number.Substring(0, 6));
        }

        private sealed class VisitorImportModel
        {
            public string LastName { get; set; } = string.Empty;
            public string FirstName { get; set; } = string.Empty;
            public string? MiddleName { get; set; }
            public string? Phone { get; set; }
            public string? Email { get; set; }
            public DateTime? BirthDate { get; set; }
            public string PassportSeries { get; set; } = string.Empty;
            public string PassportNumber { get; set; } = string.Empty;
            public string? GroupName { get; set; }
            public string? GroupDescription { get; set; }
            public string PassportKey => PassportSeries + PassportNumber;
        }

        private sealed class EmployeeImportModel
        {
            public string FullName { get; set; } = string.Empty;
            public string? EmployeeCode { get; set; }
            public int? DepartmentId { get; set; }
            public string? DepartmentName { get; set; }
            public string? DepartmentDescription { get; set; }
        }

        private static Dictionary<string, int> BuildHeaderMap(IXLRangeRow headerRow)
        {
            var map = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
            foreach (var cell in headerRow.CellsUsed())
            {
                var header = NormalizeHeader(cell.GetString());
                if (string.IsNullOrWhiteSpace(header))
                {
                    continue;
                }

                if (!map.ContainsKey(header))
                {
                    map[header] = cell.Address.ColumnNumber;
                }
            }

            return map;
        }

        private static string NormalizeHeader(string? value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return string.Empty;
            }

            var normalized = value.Trim().ToLowerInvariant();
            normalized = normalized.Replace(" ", string.Empty)
                                   .Replace("-", string.Empty)
                                   .Replace("_", string.Empty);
            return normalized;
        }

        private static string? GetString(IXLRangeRow row, IReadOnlyDictionary<string, int> headerMap, params string[] keys)
        {
            foreach (var key in keys)
            {
                var normalized = NormalizeHeader(key);
                if (!headerMap.TryGetValue(normalized, out var column))
                {
                    continue;
                }

                var value = row.Cell(column).GetString();
                if (!string.IsNullOrWhiteSpace(value))
                {
                    return value.Trim();
                }
            }

            return null;
        }

        private static DateTime? GetDate(IXLRangeRow row, IReadOnlyDictionary<string, int> headerMap, params string[] keys)
        {
            foreach (var key in keys)
            {
                var normalized = NormalizeHeader(key);
                if (!headerMap.TryGetValue(normalized, out var column))
                {
                    continue;
                }

                var cell = row.Cell(column);
                if (cell.DataType == XLDataType.DateTime || cell.DataType == XLDataType.Number)
                {
                    try
                    {
                        return cell.GetDateTime();
                    }
                    catch
                    {
                        // ignore parse errors
                    }
                }

                var raw = cell.GetString();
                if (DateTime.TryParse(raw, CultureInfo.InvariantCulture, DateTimeStyles.AssumeLocal, out var parsed) ||
                    DateTime.TryParse(raw, new CultureInfo("ru-RU"), DateTimeStyles.AssumeLocal, out parsed))
                {
                    return parsed;
                }
            }

            return null;
        }

        private async Task<FileAttachment> SaveAttachmentAsync(Department department, IFormFile file, string typeName, string preferredFileName)
        {
            var directory = Path.Combine(_uploadsRoot, "departments", department.Id.ToString());
            Directory.CreateDirectory(directory);

            var sanitizedFileName = EnsureUniqueFileName(directory, preferredFileName);
            var physicalPath = Path.Combine(directory, sanitizedFileName);

            await using (var output = new FileStream(physicalPath, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                await file.CopyToAsync(output);
            }

            var relativePath = Path.Combine("departments", department.Id.ToString(), sanitizedFileName)
                .Replace("\\", "/");

            var fileType = await EnsureFileTypeAsync(typeName);

            var attachment = new FileAttachment
            {
                DepartmentId = department.Id,
                Department = department,
                FileName = sanitizedFileName,
                FilePath = relativePath,
                ContentType = file.ContentType ?? GetContentTypeFromExtension(sanitizedFileName),
                FileSize = file.Length,
                FileType = fileType,
                FileTypeID = fileType.FileTypeID
            };

            _context.FileAttachments.Add(attachment);
            return attachment;
        }

        private async Task<FileType> EnsureFileTypeAsync(string typeName)
        {
            if (_fileTypeCache.TryGetValue(typeName, out var cached))
            {
                return cached;
            }

            var local = _context.FileTypes.Local.FirstOrDefault(f => f.TypeName.Equals(typeName, StringComparison.OrdinalIgnoreCase));
            if (local != null)
            {
                _fileTypeCache[typeName] = local;
                return local;
            }

            var existing = await _context.FileTypes.FirstOrDefaultAsync(f => f.TypeName == typeName);
            if (existing != null)
            {
                _fileTypeCache[typeName] = existing;
                return existing;
            }

            var fileType = new FileType { TypeName = typeName };
            _context.FileTypes.Add(fileType);
            _fileTypeCache[typeName] = fileType;
            return fileType;
        }

        private static string EnsureUniqueFileName(string directory, string fileName)
        {
            var sanitized = SanitizeFileName(fileName);
            var name = Path.GetFileNameWithoutExtension(sanitized);
            var extension = Path.GetExtension(sanitized);
            var candidate = sanitized;
            var index = 1;

            while (File.Exists(Path.Combine(directory, candidate)))
            {
                candidate = $"{name}_{index}{extension}";
                index++;
            }

            return candidate;
        }

        private static string SanitizeFileName(string fileName)
        {
            var invalidChars = Path.GetInvalidFileNameChars();
            var sanitizedChars = fileName.Select(ch => invalidChars.Contains(ch) ? '_' : ch).ToArray();
            var sanitized = new string(sanitizedChars).Trim();
            return string.IsNullOrWhiteSpace(sanitized) ? $"file_{Guid.NewGuid():N}" : sanitized;
        }

        private static string GetContentTypeFromExtension(string fileName)
        {
            var extension = Path.GetExtension(fileName).ToLowerInvariant();
            return extension switch
            {
                ".jpg" or ".jpeg" => "image/jpeg",
                ".png" => "image/png",
                ".gif" => "image/gif",
                ".bmp" => "image/bmp",
                ".pdf" => "application/pdf",
                _ => "application/octet-stream"
            };
        }

        private sealed record PendingAttachment(Department Department, string FileName, string TypeName, int RowNumber);

        private sealed class AttachmentCatalog
        {
            private sealed class AttachmentEntry
            {
                public AttachmentEntry(IFormFile file)
                {
                    File = file;
                }

                public IFormFile File { get; }
                public bool Used { get; set; }
            }

            private readonly Dictionary<string, List<AttachmentEntry>> _lookup = new(StringComparer.OrdinalIgnoreCase);

            public AttachmentCatalog(IEnumerable<IFormFile> files)
            {
                foreach (var file in files)
                {
                    var entry = new AttachmentEntry(file);
                    foreach (var key in EnumerateKeys(file.FileName))
                    {
                        if (!_lookup.TryGetValue(key, out var list))
                        {
                            list = new List<AttachmentEntry>();
                            _lookup[key] = list;
                        }

                        list.Add(entry);
                    }
                }
            }

            public bool TryTake(string fileName, out IFormFile file, out string resolvedName)
            {
                foreach (var key in EnumerateKeys(fileName))
                {
                    if (_lookup.TryGetValue(key, out var list))
                    {
                        var entry = list.FirstOrDefault(e => !e.Used);
                        if (entry != null)
                        {
                            entry.Used = true;
                            file = entry.File;
                            resolvedName = key;
                            return true;
                        }
                    }
                }

                file = null!;
                resolvedName = fileName;
                return false;
            }

            private static IEnumerable<string> EnumerateKeys(string fileName)
            {
                yield return fileName;
                var sanitized = SanitizeFileName(fileName);
                if (!string.Equals(fileName, sanitized, StringComparison.OrdinalIgnoreCase))
                {
                    yield return sanitized;
                }
            }
        }
    }
}
