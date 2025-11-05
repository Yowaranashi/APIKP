using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using HranitelPro.API.Data;
using HranitelPRO.API.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace HranitelPRO.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Route("api/passrequests")]
    [Route("api/applications")]
    public class PassRequestsController : ControllerBase
    {
        private const string BlacklistAutoReason =
            "Заявка на посещение объекта КИИ отклонена в связи с нарушением Федерального закона от 26.07.2017 № 187-ФЗ «О безопасно"
            + "сти критической информационной инфраструктуры Российской Федерации».";

        private readonly HranitelContext _context;
        private readonly IWebHostEnvironment _environment;
        private readonly ILogger<PassRequestsController> _logger;

        public PassRequestsController(HranitelContext context, IWebHostEnvironment environment, ILogger<PassRequestsController> logger)
        {
            _context = context;
            _environment = environment;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PassRequestSummaryDto>>> Get([FromQuery] PassRequestQuery query)
        {
            var baseQuery = _context.PassRequests
                .AsNoTracking()
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(query.Status))
            {
                var statusValue = query.Status.Trim();
                baseQuery = baseQuery.Where(p => p.Status == statusValue || p.StatusRef.StatusName == statusValue);
            }

            if (query.DepartmentId.HasValue)
            {
                baseQuery = baseQuery.Where(p => p.DepartmentId == query.DepartmentId.Value);
            }

            if (!string.IsNullOrWhiteSpace(query.RequestType))
            {
                var type = query.RequestType.Trim();
                baseQuery = baseQuery.Where(p => p.RequestType == type);
            }

            if (query.DateFrom.HasValue)
            {
                var from = query.DateFrom.Value.Date;
                baseQuery = baseQuery.Where(p => p.StartDate.Date >= from);
            }

            if (query.DateTo.HasValue)
            {
                var to = query.DateTo.Value.Date;
                baseQuery = baseQuery.Where(p => p.EndDate.Date <= to);
            }

            if (query.ApplicantUserId.HasValue)
            {
                baseQuery = baseQuery.Where(p => p.ApplicantUserId == query.ApplicantUserId.Value);
            }

            var requests = await baseQuery
                .Include(p => p.Department)
                .Include(p => p.ResponsibleEmployee)
                .Include(p => p.StatusRef)
                .Include(p => p.Visitors)
                    .ThenInclude(v => v.Group)
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();

            var summaries = requests.Select(MapToSummary).ToList();
            return Ok(summaries);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<PassRequestDetailsDto>> GetById(int id)
        {
            var request = await _context.PassRequests
                .AsNoTracking()
                .Include(p => p.Department)
                .Include(p => p.ResponsibleEmployee)
                .Include(p => p.StatusRef)
                .Include(p => p.Visitors)
                    .ThenInclude(v => v.Group)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (request == null)
            {
                return NotFound();
            }

            return Ok(MapToDetails(request));
        }

        [HttpPost]
        [AllowAnonymous]
        [Consumes("application/json")]
        public Task<ActionResult<PassRequestDetailsDto>> Create([FromBody] PassRequestCreateDto dto)
        {
            return CreateRequestAsync(dto, null);
        }

        [HttpPost]
        [AllowAnonymous]
        [Consumes("multipart/form-data")]
        public Task<ActionResult<PassRequestDetailsDto>> Create([FromForm] PassRequestFormDto formDto)
        {
            var (mappedDto, mappingErrors) = MapFormToCreateDto(formDto);
            if (mappingErrors.Count > 0)
            {
                return Task.FromResult<ActionResult<PassRequestDetailsDto>>(BadRequest(new { errors = mappingErrors }));
            }

            return CreateRequestAsync(mappedDto, request => SaveFormFilesAsync(request, formDto));
        }

        [HttpPost("{id:int}/review")]
        public async Task<ActionResult<PassRequestDetailsDto>> Review(int id, [FromBody] PassRequestReviewDto dto)
        {
            var request = await _context.PassRequests
                .Include(p => p.Visitors)
                .Include(p => p.StatusRef)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (request == null)
            {
                return NotFound();
            }

            if (dto.CheckedByUserId.HasValue)
            {
                request.CheckedByUserId = dto.CheckedByUserId.Value;
            }

            // Проверка по черному списку
            var visitorDocuments = request.Visitors?
                .Where(v => !string.IsNullOrEmpty(v.PassportSeries) && !string.IsNullOrEmpty(v.PassportNumber))
                .Select(v => v.PassportSeries + v.PassportNumber)
                .ToList() ?? new List<string>();

            var blacklistedVisitors = visitorDocuments.Count == 0
                ? new List<BlacklistEntry>()
                : await _context.BlacklistEntries
                    .Where(b => visitorDocuments.Contains((b.PassportSeries ?? string.Empty) + (b.PassportNumber ?? string.Empty)))
                    .ToListAsync();

            if (blacklistedVisitors.Any())
            {
                request.Status = "Rejected";
                request.StatusID = await ResolveStatusIdAsync("Rejected");
                request.RejectionReason = BlacklistAutoReason;
                await CreateNotificationsAsync(request, BlacklistAutoReason);
                await _context.SaveChangesAsync();
                return Ok(MapToDetails(request));
            }

            if (dto.Decision.Equals("Approved", StringComparison.OrdinalIgnoreCase))
            {
                await EnsureApplicationStatusExistsAsync("Approved");
                request.Status = "Approved";
                request.StatusID = await ResolveStatusIdAsync("Approved");

                if (dto.VisitStart.HasValue)
                {
                    request.StartDate = dto.VisitStart.Value;
                }
                if (dto.VisitEnd.HasValue)
                {
                    request.EndDate = dto.VisitEnd.Value;
                }

                request.RejectionReason = null;
                await CreateNotificationsAsync(request,
                    $"Заявка на посещение объекта КИИ одобрена, дата посещения: {request.StartDate:dd.MM.yyyy}, время посещения: {request.StartDate:HH:mm}");
            }
            else if (dto.Decision.Equals("Rejected", StringComparison.OrdinalIgnoreCase))
            {
                if (string.IsNullOrWhiteSpace(dto.RejectionReason))
                {
                    return BadRequest(new { message = "Необходимо указать причину отклонения" });
                }

                await EnsureApplicationStatusExistsAsync("Rejected");
                request.Status = "Rejected";
                request.StatusID = await ResolveStatusIdAsync("Rejected");
                request.RejectionReason = dto.RejectionReason;
                await CreateNotificationsAsync(request, dto.RejectionReason);

                if (dto.RejectionReason.Contains("недостоверных", StringComparison.OrdinalIgnoreCase))
                {
                    await TryAddVisitorsToBlacklistAsync(request, dto.CheckedByEmployeeId, dto.RejectionReason);
                }
            }
            else
            {
                return BadRequest(new { message = "Неизвестное решение. Используйте Approved или Rejected." });
            }

            await _context.SaveChangesAsync();
            return Ok(MapToDetails(request));
        }

        [HttpPut("{id:int}/status")]
        public async Task<ActionResult<PassRequestDetailsDto>> UpdateStatus(int id, [FromBody] PassRequestStatusDto dto)
        {
            var request = await _context.PassRequests
                .Include(p => p.StatusRef)
                .Include(p => p.Visitors)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (request == null)
            {
                return NotFound();
            }

            await EnsureApplicationStatusExistsAsync(dto.Status);
            request.Status = dto.Status;
            request.StatusID = await ResolveStatusIdAsync(dto.Status);
            request.RejectionReason = dto.RejectionReason;

            await _context.SaveChangesAsync();
            return Ok(MapToDetails(request));
        }

        private async Task<ActionResult<PassRequestDetailsDto>> CreateRequestAsync(PassRequestCreateDto dto, Func<PassRequest, Task>? afterSave)
        {
            var validationErrors = ValidateCreateDto(dto);
            if (validationErrors.Count > 0)
            {
                return BadRequest(new { errors = validationErrors });
            }

            var normalizedType = NormalizeRequestType(dto.RequestType);
            await EnsureApplicationStatusExistsAsync("Pending");

            var request = new PassRequest
            {
                RequestType = normalizedType,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate,
                Purpose = dto.Purpose,
                DepartmentId = dto.DepartmentId,
                ResponsibleEmployeeId = dto.ResponsibleEmployeeId,
                ApplicantUserId = dto.ApplicantUserId,
                ApplicantFirstName = dto.ApplicantFirstName,
                ApplicantLastName = dto.ApplicantLastName,
                ApplicantMiddleName = dto.ApplicantMiddleName,
                ApplicantEmail = dto.ApplicantEmail,
                Organization = dto.Organization,
                Note = dto.Note,
                BirthDate = dto.BirthDate,
                Phone = dto.Phone,
                PassportSeries = dto.PassportSeries,
                PassportNumber = dto.PassportNumber,
                GroupSize = dto.GroupSize ?? dto.ExpectedVisitorCount,
                Status = "Pending",
                StatusID = await ResolveStatusIdAsync("Pending"),
                CreatedAt = DateTime.UtcNow
            };

            if (!request.GroupSize.HasValue && dto.Visitors?.Count > 0)
            {
                request.GroupSize = dto.Visitors.Count;
            }

            if (dto.Visitors != null && dto.Visitors.Count > 0)
            {
                request.Visitors = dto.Visitors.Select(v => new PassVisitor
                {
                    LastName = v.LastName,
                    FirstName = v.FirstName,
                    MiddleName = v.MiddleName,
                    Phone = v.Phone,
                    Email = v.Email,
                    BirthDate = v.BirthDate,
                    PassportSeries = v.PassportSeries,
                    PassportNumber = v.PassportNumber
                }).ToList();
            }

            _context.PassRequests.Add(request);
            await _context.SaveChangesAsync();

            if (afterSave != null)
            {
                try
                {
                    await afterSave(request);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Failed to handle files for pass request {RequestId}", request.Id);
                    return StatusCode(500, new { message = "Не удалось сохранить вложения заявки" });
                }
            }

            var created = await LoadRequestAsync(request.Id);
            return CreatedAtAction(nameof(GetById), new { id = request.Id }, MapToDetails(created));
        }

        private Task<PassRequest> LoadRequestAsync(int id)
        {
            return _context.PassRequests
                .AsNoTracking()
                .Include(p => p.Department)
                .Include(p => p.ResponsibleEmployee)
                .Include(p => p.StatusRef)
                .Include(p => p.Visitors)
                    .ThenInclude(v => v.Group)
                .FirstAsync(p => p.Id == id);
        }

        private List<string> ValidateCreateDto(PassRequestCreateDto dto)
        {
            var errors = new List<string>();

            if (dto.StartDate.Date < DateTime.UtcNow.Date.AddDays(1))
            {
                errors.Add("Дата начала должна быть не ранее следующего дня от текущей даты.");
            }

            if (dto.StartDate.Date > DateTime.UtcNow.Date.AddDays(15))
            {
                errors.Add("Дата начала должна быть не позднее чем через 15 дней от текущей даты.");
            }

            if (dto.EndDate.Date < dto.StartDate.Date)
            {
                errors.Add("Дата окончания не может быть раньше даты начала.");
            }

            if (dto.EndDate.Date > dto.StartDate.Date.AddDays(15))
            {
                errors.Add("Длительность заявки не может превышать 15 дней.");
            }

            if (string.IsNullOrWhiteSpace(dto.Purpose))
            {
                errors.Add("Не указана цель посещения.");
            }

            if (dto.DepartmentId <= 0)
            {
                errors.Add("Не указано подразделение для посещения.");
            }

            if (!string.IsNullOrWhiteSpace(dto.PassportSeries) && dto.PassportSeries.Length != 4)
            {
                errors.Add("Серия паспорта должна содержать 4 цифры.");
            }

            if (!string.IsNullOrWhiteSpace(dto.PassportNumber) && dto.PassportNumber.Length != 6)
            {
                errors.Add("Номер паспорта должен содержать 6 цифр.");
            }

            if (!string.IsNullOrWhiteSpace(dto.ApplicantEmail) && !dto.ApplicantEmail.Contains('@'))
            {
                errors.Add("Укажите корректный адрес электронной почты заявителя.");
            }

            if (dto.BirthDate.HasValue && dto.BirthDate.Value.AddYears(16) > DateTime.UtcNow.Date)
            {
                errors.Add("Посетитель должен быть не моложе 16 лет.");
            }

            var visitors = dto.Visitors ?? new List<PassVisitorCreateDto>();
            if (visitors.Count > 0)
            {
                foreach (var visitor in visitors)
                {
                    if (string.IsNullOrWhiteSpace(visitor.LastName) || string.IsNullOrWhiteSpace(visitor.FirstName))
                    {
                        errors.Add("Для посетителя необходимо указать фамилию и имя.");
                        break;
                    }

                    if (visitor.PassportSeries?.Length != 4)
                    {
                        errors.Add("Серия паспорта посетителя должна содержать 4 символа.");
                        break;
                    }

                    if (visitor.PassportNumber?.Length != 6)
                    {
                        errors.Add("Номер паспорта посетителя должен содержать 6 символов.");
                        break;
                    }
                }
            }

            if (string.Equals(dto.RequestType, "Group", StringComparison.OrdinalIgnoreCase))
            {
                var expected = dto.ExpectedVisitorCount ?? dto.GroupSize ?? visitors.Count;
                if (expected < 5)
                {
                    errors.Add("Для групповой заявки необходимо указать минимум 5 участников или ожидаемое количество не менее 5.");
                }
            }

            return errors;
        }

        private PassRequestSummaryDto MapToSummary(PassRequest request) => new()
        {
            Id = request.Id,
            RequestType = request.RequestType,
            Department = request.Department?.Name,
            DepartmentId = request.DepartmentId,
            ResponsibleEmployeeId = request.ResponsibleEmployeeId,
            StartDate = request.StartDate,
            EndDate = request.EndDate,
            Status = request.Status,
            Purpose = request.Purpose,
            VisitorCount = request.Visitors?.Count ?? request.GroupSize ?? 0,
            GroupSize = request.GroupSize ?? request.Visitors?.Count,
            CreatedAt = request.CreatedAt,
            ApplicantFullName = FormatFullName(request.ApplicantLastName, request.ApplicantFirstName, request.ApplicantMiddleName),
            ApplicantPhone = request.Phone,
            ApplicantBirthDate = request.BirthDate,
            ApplicantPassport = FormatPassport(request.PassportSeries, request.PassportNumber),
            ApplicantEmail = request.ApplicantEmail,
            ApplicantOrganization = request.Organization
        };

        private PassRequestDetailsDto MapToDetails(PassRequest request) => new()
        {
            Id = request.Id,
            RequestType = request.RequestType,
            DepartmentId = request.DepartmentId,
            DepartmentName = request.Department?.Name,
            ResponsibleEmployeeId = request.ResponsibleEmployeeId,
            StartDate = request.StartDate,
            EndDate = request.EndDate,
            Purpose = request.Purpose,
            Status = request.Status,
            StatusId = request.StatusID,
            RejectionReason = request.RejectionReason,
            GroupSize = request.GroupSize ?? request.Visitors?.Count,
            CreatedAt = request.CreatedAt,
            ApplicantFullName = FormatFullName(request.ApplicantLastName, request.ApplicantFirstName, request.ApplicantMiddleName),
            ApplicantPhone = request.Phone,
            ApplicantBirthDate = request.BirthDate,
            ApplicantPassport = FormatPassport(request.PassportSeries, request.PassportNumber),
            ApplicantEmail = request.ApplicantEmail,
            ApplicantOrganization = request.Organization,
            Visitors = request.Visitors?.Select(v => new PassVisitorDto
            {
                Id = v.Id,
                LastName = v.LastName,
                FirstName = v.FirstName,
                MiddleName = v.MiddleName,
                Email = v.Email,
                Phone = v.Phone,
                BirthDate = v.BirthDate,
                PassportNumber = v.PassportNumber,
                PassportSeries = v.PassportSeries,
                GroupId = v.GroupId,
                GroupName = v.Group?.GroupName
            }).ToList() ?? new List<PassVisitorDto>()
        };

        private (PassRequestCreateDto dto, List<string> errors) MapFormToCreateDto(PassRequestFormDto formDto)
        {
            var errors = new List<string>();

            if (!DateTime.TryParse(formDto.StartDate, CultureInfo.InvariantCulture, DateTimeStyles.AssumeLocal, out var startDate))
            {
                errors.Add("Укажите корректную дату начала.");
            }

            if (!DateTime.TryParse(formDto.EndDate, CultureInfo.InvariantCulture, DateTimeStyles.AssumeLocal, out var endDate))
            {
                errors.Add("Укажите корректную дату окончания.");
            }

            DateTime? birthDate = null;
            if (!string.IsNullOrWhiteSpace(formDto.ApplicantBirthDate))
            {
                if (DateTime.TryParse(formDto.ApplicantBirthDate, CultureInfo.InvariantCulture, DateTimeStyles.AssumeLocal, out var parsedBirth))
                {
                    birthDate = parsedBirth;
                }
                else
                {
                    errors.Add("Укажите корректную дату рождения заявителя.");
                }
            }

            if (!int.TryParse(formDto.DepartmentId, NumberStyles.Integer, CultureInfo.InvariantCulture, out var departmentId))
            {
                errors.Add("Некорректный идентификатор подразделения.");
            }

            int? responsibleEmployeeId = null;
            if (!string.IsNullOrWhiteSpace(formDto.EmployeeId))
            {
                if (int.TryParse(formDto.EmployeeId, NumberStyles.Integer, CultureInfo.InvariantCulture, out var parsedEmployeeId))
                {
                    responsibleEmployeeId = parsedEmployeeId;
                }
                else
                {
                    errors.Add("Некорректный идентификатор ответственного сотрудника.");
                }
            }

            int? applicantUserId = null;
            if (!string.IsNullOrWhiteSpace(formDto.ApplicantUserId) &&
                int.TryParse(formDto.ApplicantUserId, NumberStyles.Integer, CultureInfo.InvariantCulture, out var parsedUserId))
            {
                applicantUserId = parsedUserId;
            }

            var (lastName, firstName, middleName) = SplitFullName(formDto.ApplicantName);
            var (passportSeries, passportNumber) = SplitPassport(formDto.ApplicantPassport);
            if (string.IsNullOrWhiteSpace(passportSeries) || string.IsNullOrWhiteSpace(passportNumber))
            {
                errors.Add("Укажите паспортные данные в формате 1234 567890.");
            }

            var visitors = ParseParticipants(formDto.ParticipantsJson, errors);
            var normalizedType = NormalizeRequestType(formDto.Type);

            if (!string.IsNullOrWhiteSpace(passportSeries) && !string.IsNullOrWhiteSpace(passportNumber))
            {
                if (!visitors.Any(v => v.PassportSeries == passportSeries && v.PassportNumber == passportNumber))
                {
                    visitors.Insert(0, new PassVisitorCreateDto
                    {
                        LastName = lastName ?? formDto.ApplicantName,
                        FirstName = firstName ?? formDto.ApplicantName,
                        MiddleName = middleName,
                        BirthDate = birthDate,
                        PassportSeries = passportSeries,
                        PassportNumber = passportNumber,
                        Phone = formDto.ApplicantPhone,
                        Email = formDto.ApplicantEmail
                    });
                }
            }

            var dto = new PassRequestCreateDto
            {
                RequestType = normalizedType,
                StartDate = startDate,
                EndDate = endDate,
                Purpose = formDto.Purpose ?? string.Empty,
                DepartmentId = departmentId,
                ResponsibleEmployeeId = responsibleEmployeeId,
                ApplicantUserId = applicantUserId,
                ApplicantLastName = lastName,
                ApplicantFirstName = firstName,
                ApplicantMiddleName = middleName,
                ApplicantEmail = formDto.ApplicantEmail,
                Organization = formDto.ApplicantCompany,
                Note = formDto.Note,
                BirthDate = birthDate,
                Phone = formDto.ApplicantPhone,
                PassportSeries = passportSeries,
                PassportNumber = passportNumber,
                GroupSize = formDto.GroupSize,
                ExpectedVisitorCount = formDto.GroupSize,
                Visitors = visitors
            };

            return (dto, errors);
        }

        private List<PassVisitorCreateDto> ParseParticipants(string? participantsJson, List<string> errors)
        {
            var visitors = new List<PassVisitorCreateDto>();
            if (string.IsNullOrWhiteSpace(participantsJson))
            {
                return visitors;
            }

            try
            {
                var parsed = JsonSerializer.Deserialize<List<ApplicationParticipantFormDto>>(participantsJson,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new List<ApplicationParticipantFormDto>();

                foreach (var participant in parsed)
                {
                    if (participant == null || string.IsNullOrWhiteSpace(participant.FullName))
                    {
                        continue;
                    }

                    var (lastName, firstName, middleName) = SplitFullName(participant.FullName);
                    var (series, number) = SplitPassport(participant.Passport);
                    DateTime? birthDate = null;
                    if (!string.IsNullOrWhiteSpace(participant.BirthDate))
                    {
                        if (DateTime.TryParse(participant.BirthDate, CultureInfo.InvariantCulture, DateTimeStyles.AssumeLocal, out var parsedBirth)
                            || DateTime.TryParse(participant.BirthDate, new CultureInfo("ru-RU"), DateTimeStyles.AssumeLocal, out parsedBirth))
                        {
                            birthDate = parsedBirth;
                        }
                        else
                        {
                            errors.Add("В списке участников найдены записи с некорректной датой рождения.");
                            continue;
                        }
                    }

                    if (string.IsNullOrWhiteSpace(lastName) || string.IsNullOrWhiteSpace(firstName) ||
                        string.IsNullOrWhiteSpace(series) || string.IsNullOrWhiteSpace(number))
                    {
                        errors.Add("В списке участников найдены записи с некорректными данными.");
                        continue;
                    }

                    visitors.Add(new PassVisitorCreateDto
                    {
                        LastName = lastName,
                        FirstName = firstName,
                        MiddleName = middleName,
                        BirthDate = birthDate,
                        PassportSeries = series,
                        PassportNumber = number,
                        Phone = participant.Phone,
                        Email = participant.Email
                    });
                }
            }
            catch (JsonException)
            {
                errors.Add("Не удалось прочитать список участников. Проверьте файл и повторите попытку.");
            }

            return visitors;
        }

        private async Task SaveFormFilesAsync(PassRequest request, PassRequestFormDto formDto)
        {
            var attachments = new List<FileAttachment>();

            if (formDto.Photo != null && formDto.Photo.Length > 0)
            {
                var typeId = await EnsureFileTypeIdAsync("Photo");
                attachments.Add(await SaveAttachmentAsync(request, formDto.Photo, typeId, "photo"));
            }

            if (formDto.Passport != null && formDto.Passport.Length > 0)
            {
                var typeId = await EnsureFileTypeIdAsync("PassportScan");
                attachments.Add(await SaveAttachmentAsync(request, formDto.Passport, typeId, "passport"));
            }

            if (formDto.Participants != null && formDto.Participants.Length > 0)
            {
                var typeId = await EnsureFileTypeIdAsync("ParticipantsList");
                attachments.Add(await SaveAttachmentAsync(request, formDto.Participants, typeId, "participants"));
            }

            if (attachments.Count == 0)
            {
                return;
            }

            _context.FileAttachments.AddRange(attachments);
            await _context.SaveChangesAsync();
        }

        private async Task<FileAttachment> SaveAttachmentAsync(PassRequest request, IFormFile file, int fileTypeId, string prefix)
        {
            var directory = GetRequestUploadDirectory(request);
            Directory.CreateDirectory(directory);

            var sanitizedName = SanitizeFileName($"{prefix}_{file.FileName}");
            var fullPath = Path.Combine(directory, sanitizedName);

            await using (var stream = System.IO.File.Create(fullPath))
            {
                await file.CopyToAsync(stream);
            }

            var relativePath = Path.GetRelativePath(_environment.ContentRootPath, fullPath)
                .Replace(Path.DirectorySeparatorChar, '/');

            return new FileAttachment
            {
                PassRequestId = request.Id,
                DepartmentId = request.DepartmentId,
                FileTypeID = fileTypeId,
                FilePath = relativePath,
                FileName = sanitizedName,
                ContentType = string.IsNullOrWhiteSpace(file.ContentType) ? "application/octet-stream" : file.ContentType,
                FileSize = file.Length,
                UploadedAt = DateTime.UtcNow
            };
        }

        private async Task<int> EnsureFileTypeIdAsync(string typeName)
        {
            var existing = await _context.FileTypes.FirstOrDefaultAsync(f => f.TypeName == typeName);
            if (existing != null)
            {
                return existing.FileTypeID;
            }

            var fileType = new FileType { TypeName = typeName };
            _context.FileTypes.Add(fileType);
            await _context.SaveChangesAsync();
            return fileType.FileTypeID;
        }

        private string GetRequestUploadDirectory(PassRequest request)
        {
            return Path.Combine(_environment.ContentRootPath, "Uploads", "passrequests", request.Id.ToString());
        }

        private static string SanitizeFileName(string fileName)
        {
            var cleaned = Regex.Replace(fileName, @"[^\w\d\-.]+", "_", RegexOptions.Compiled);
            return string.IsNullOrWhiteSpace(cleaned) ? Guid.NewGuid().ToString("N") : cleaned;
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

            var digits = Regex.Replace(passport, @"\D", string.Empty);
            if (digits.Length < 10)
            {
                return (null, null);
            }

            return (digits[..4], digits.Substring(4, Math.Min(6, digits.Length - 4)));
        }

        private static string NormalizeRequestType(string? type)
        {
            return type?.Equals("group", StringComparison.OrdinalIgnoreCase) == true ? "Group" : "Personal";
        }

        private static string? FormatPassport(string? series, string? number)
        {
            if (string.IsNullOrWhiteSpace(series) || string.IsNullOrWhiteSpace(number))
            {
                return null;
            }

            return $"{series} {number}";
        }

        private static string? FormatFullName(string? lastName, string? firstName, string? middleName)
        {
            var parts = new List<string>();
            if (!string.IsNullOrWhiteSpace(lastName))
            {
                parts.Add(lastName);
            }

            if (!string.IsNullOrWhiteSpace(firstName))
            {
                parts.Add(firstName);
            }

            if (!string.IsNullOrWhiteSpace(middleName))
            {
                parts.Add(middleName);
            }

            return parts.Count == 0 ? null : string.Join(' ', parts);
        }

        private async Task EnsureApplicationStatusExistsAsync(string status)
        {
            if (await _context.ApplicationStatuses.AnyAsync(s => s.StatusName == status))
            {
                return;
            }

            _context.ApplicationStatuses.Add(new ApplicationStatus { StatusName = status });
            await _context.SaveChangesAsync();
        }

        private Task<int> ResolveStatusIdAsync(string status)
        {
            return _context.ApplicationStatuses
                .Where(s => s.StatusName == status)
                .Select(s => s.StatusID)
                .FirstAsync();
        }

        private Task CreateNotificationsAsync(PassRequest request, string message)
        {
            if (request.ApplicantUserId == null)
            {
                return Task.CompletedTask;
            }

            var notification = new Notification
            {
                UserId = request.ApplicantUserId,
                Title = "Статус заявки",
                Body = message,
                CreatedAt = DateTime.UtcNow
            };

            _context.Notifications.Add(notification);
            return Task.CompletedTask;
        }

        private async Task TryAddVisitorsToBlacklistAsync(PassRequest request, int? employeeId, string reason)
        {
            if (request.Visitors == null)
            {
                return;
            }

            foreach (var visitor in request.Visitors)
            {
                var rejectedCount = await _context.PassVisitors
                    .Include(v => v.PassRequest)
                    .Where(v => v.PassportSeries == visitor.PassportSeries && v.PassportNumber == visitor.PassportNumber)
                    .CountAsync(v => v.PassRequest.RejectionReason != null &&
                                     v.PassRequest.RejectionReason.Contains("недостоверных", StringComparison.OrdinalIgnoreCase));

                if (rejectedCount < 2)
                {
                    continue;
                }

                var alreadyBlacklisted = await _context.BlacklistEntries.AnyAsync(b =>
                    b.PassportSeries == visitor.PassportSeries && b.PassportNumber == visitor.PassportNumber);

                if (!alreadyBlacklisted)
                {
                    _context.BlacklistEntries.Add(new BlacklistEntry
                    {
                        LastName = visitor.LastName,
                        FirstName = visitor.FirstName,
                        MiddleName = visitor.MiddleName,
                        PassportSeries = visitor.PassportSeries,
                        PassportNumber = visitor.PassportNumber,
                        Reason = reason,
                        AddedByEmployeeId = employeeId,
                        AddedAt = DateTime.UtcNow
                    });
                }
            }
        }
    }

    public class PassRequestQuery
    {
        public string? Status { get; set; }
        public int? DepartmentId { get; set; }
        public string? RequestType { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }

        [FromQuery(Name = "userId")]
        public int? ApplicantUserId { get; set; }
    }

    public class PassRequestSummaryDto
    {
        public int Id { get; set; }
        public string? RequestType { get; set; }
        public string? Department { get; set; }
        public int DepartmentId { get; set; }
        public int? ResponsibleEmployeeId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Status { get; set; } = null!;
        public string Purpose { get; set; } = null!;
        public int VisitorCount { get; set; }
        public int? GroupSize { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? ApplicantFullName { get; set; }
        public string? ApplicantPhone { get; set; }
        public DateTime? ApplicantBirthDate { get; set; }
        public string? ApplicantPassport { get; set; }
        public string? ApplicantEmail { get; set; }
        public string? ApplicantOrganization { get; set; }
    }

    public class PassRequestDetailsDto
    {
        public int Id { get; set; }
        public string RequestType { get; set; } = null!;
        public int DepartmentId { get; set; }
        public string? DepartmentName { get; set; }
        public int? ResponsibleEmployeeId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Purpose { get; set; } = null!;
        public string Status { get; set; } = null!;
        public int StatusId { get; set; }
        public string? RejectionReason { get; set; }
        public int? GroupSize { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? ApplicantFullName { get; set; }
        public string? ApplicantPhone { get; set; }
        public DateTime? ApplicantBirthDate { get; set; }
        public string? ApplicantPassport { get; set; }
        public string? ApplicantEmail { get; set; }
        public string? ApplicantOrganization { get; set; }
        public List<PassVisitorDto> Visitors { get; set; } = new();
    }

    public class PassVisitorDto
    {
        public int Id { get; set; }
        public string LastName { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string? MiddleName { get; set; }
        public string PassportSeries { get; set; } = null!;
        public string PassportNumber { get; set; } = null!;
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public DateTime? BirthDate { get; set; }
        public int? GroupId { get; set; }
        public string? GroupName { get; set; }
    }

    public class PassRequestCreateDto
    {
        public string RequestType { get; set; } = "Personal";
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Purpose { get; set; } = null!;
        public int DepartmentId { get; set; }
        public int? ResponsibleEmployeeId { get; set; }
        public int? ApplicantUserId { get; set; }
        public string? ApplicantLastName { get; set; }
        public string? ApplicantFirstName { get; set; }
        public string? ApplicantMiddleName { get; set; }
        public string? ApplicantEmail { get; set; }
        public string? Organization { get; set; }
        public string? Note { get; set; }
        public DateTime? BirthDate { get; set; }
        public string? Phone { get; set; }
        public string? PassportSeries { get; set; }
        public string? PassportNumber { get; set; }
        public int? GroupSize { get; set; }
        public int? ExpectedVisitorCount { get; set; }
        public List<PassVisitorCreateDto> Visitors { get; set; } = new();
    }

    public class PassVisitorCreateDto
    {
        public string LastName { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string? MiddleName { get; set; }
        public DateTime? BirthDate { get; set; }
        public string PassportSeries { get; set; } = null!;
        public string PassportNumber { get; set; } = null!;
        public string? Email { get; set; }
        public string? Phone { get; set; }
    }

    public class PassRequestFormDto
    {
        public string Type { get; set; } = "personal";
        public string StartDate { get; set; } = null!;
        public string EndDate { get; set; } = null!;
        public string Purpose { get; set; } = null!;
        public string DepartmentId { get; set; } = null!;
        public string? EmployeeId { get; set; }
        public string ApplicantName { get; set; } = null!;
        public string ApplicantPhone { get; set; } = null!;
        public string ApplicantBirthDate { get; set; } = null!;
        public string ApplicantPassport { get; set; } = null!;
        public string? ApplicantEmail { get; set; }
        public string? ApplicantCompany { get; set; }
        public string? Note { get; set; }
        public string? ParticipantsJson { get; set; }
        public int? GroupSize { get; set; }
        public string? ApplicantUserId { get; set; }
        public IFormFile? Photo { get; set; }
        public IFormFile? Passport { get; set; }
        public IFormFile? Participants { get; set; }
    }

    public class ApplicationParticipantFormDto
    {
        public string? FullName { get; set; }
        public string? BirthDate { get; set; }
        public string? Passport { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
    }

    public class PassRequestReviewDto
    {
        public string Decision { get; set; } = null!;
        public DateTime? VisitStart { get; set; }
        public DateTime? VisitEnd { get; set; }
        public string? RejectionReason { get; set; }
        public int? CheckedByUserId { get; set; }
        public int? CheckedByEmployeeId { get; set; }
    }

    public class PassRequestStatusDto
    {
        public string Status { get; set; } = null!;
        public string? RejectionReason { get; set; }
    }
}
