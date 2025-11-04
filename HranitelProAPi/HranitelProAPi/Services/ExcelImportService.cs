using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
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
        private static readonly string[] DepartmentHeaders = { "department", "departmentname", "building", "facility", "отдел", "здание" };
        private static readonly string[] DepartmentDescriptionHeaders = { "description", "departmentdescription", "описание", "comment" };
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

            var imported = 0;
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

            foreach (var row in range.RowsUsed().Skip(1))
            {
                var visitor = new PassVisitor
                {
                    LastName = row.Cell(1).GetString(),
                    FirstName = row.Cell(2).GetString(),
                    MiddleName = row.Cell(3).GetString(),
                    Phone = row.Cell(4).GetString(),
                    Email = row.Cell(5).GetString(),
                    PassportSeries = row.Cell(6).GetString(),
                    PassportNumber = row.Cell(7).GetString()
                };
                _context.PassVisitors.Add(visitor);
                imported++;
            }

            if (imported > 0)
            {
                await _context.SaveChangesAsync();
            }

            return imported;
        }

        public async Task<int> ImportEmployeesAsync(IFormFile file)
        {
            if (file == null)
            {
                throw new ArgumentNullException(nameof(file));
            }

            var imported = 0;
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

            foreach (var row in range.RowsUsed().Skip(1))
            {
                var employee = new Employee
                {
                    FullName = row.Cell(1).GetString(),
                    EmployeeCode = row.Cell(2).GetString(),
                    DepartmentId = row.Cell(3).GetValue<int?>()
                };
                _context.Employees.Add(employee);
                imported++;
            }

            if (imported > 0)
            {
                await _context.SaveChangesAsync();
            }

            return imported;
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
            var names = rows
                .Skip(1)
                .Select(row => GetString(row, headerMap, GroupNameHeaders))
                .Where(name => !string.IsNullOrWhiteSpace(name))
                .Select(name => name!.Trim())
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToList();

            if (names.Count == 0)
            {
                return 0;
            }

            var existing = await _context.Groups
                .Where(g => g.GroupName != null && names.Contains(g.GroupName))
                .ToDictionaryAsync(g => g.GroupName!, StringComparer.OrdinalIgnoreCase);

            var imported = 0;
            foreach (var name in names)
            {
                if (existing.ContainsKey(name))
                {
                    continue;
                }

                var group = new Group
                {
                    GroupName = name
                };

                _context.Groups.Add(group);
                existing[name] = group;
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
