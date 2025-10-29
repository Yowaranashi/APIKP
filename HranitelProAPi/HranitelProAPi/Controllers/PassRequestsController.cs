using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HranitelPro.API.Data;
using HranitelPRO.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HranitelPRO.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PassRequestsController : ControllerBase
    {
        private const string BlacklistAutoReason =
            "Заявка на посещение объекта КИИ отклонена в связи с нарушением Федерального закона от 26.07.2017 № 187-ФЗ «О безопасно"
            + "сти критической информационной инфраструктуры Российской Федерации».";

        private readonly HranitelContext _context;

        public PassRequestsController(HranitelContext context)
        {
            _context = context;
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

            var requests = await baseQuery
                .Include(p => p.Department)
                .Include(p => p.ResponsibleEmployee)
                .Include(p => p.StatusRef)
                .Include(p => p.Visitors)
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
                .FirstOrDefaultAsync(p => p.Id == id);

            if (request == null)
            {
                return NotFound();
            }

            return Ok(MapToDetails(request));
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult<PassRequestDetailsDto>> Create([FromBody] PassRequestCreateDto dto)
        {
            var validationErrors = ValidateCreateDto(dto);
            if (validationErrors.Count > 0)
            {
                return BadRequest(new { errors = validationErrors });
            }

            await EnsureApplicationStatusExistsAsync("Pending");

            var request = new PassRequest
            {
                RequestType = dto.RequestType,
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
                Status = "Pending",
                StatusID = await ResolveStatusIdAsync("Pending"),
                CreatedAt = DateTime.UtcNow
            };

            if (dto.Visitors != null && dto.Visitors.Count > 0)
            {
                request.Visitors = dto.Visitors.Select(v => new PassVisitor
                {
                    LastName = v.LastName,
                    FirstName = v.FirstName,
                    MiddleName = v.MiddleName,
                    Phone = v.Phone,
                    Email = v.Email,
                    PassportSeries = v.PassportSeries,
                    PassportNumber = v.PassportNumber
                }).ToList();
            }

            _context.PassRequests.Add(request);
            await _context.SaveChangesAsync();

            var created = await _context.PassRequests
                .AsNoTracking()
                .Include(p => p.Department)
                .Include(p => p.ResponsibleEmployee)
                .Include(p => p.StatusRef)
                .Include(p => p.Visitors)
                .FirstAsync(p => p.Id == request.Id);

            return CreatedAtAction(nameof(GetById), new { id = request.Id }, MapToDetails(created));
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

            if (dto.Visitors == null || dto.Visitors.Count == 0)
            {
                errors.Add("Необходимо указать посетителей заявки.");
            }
            else
            {
                foreach (var visitor in dto.Visitors)
                {
                    if (string.IsNullOrWhiteSpace(visitor.LastName) || string.IsNullOrWhiteSpace(visitor.FirstName))
                    {
                        errors.Add("Для посетителя необходимо указать фамилию и имя.");
                        break;
                    }

                    if (visitor.PassportSeries?.Length != 4)
                    {
                        errors.Add("Серия паспорта должна содержать 4 символа.");
                        break;
                    }

                    if (visitor.PassportNumber?.Length != 6)
                    {
                        errors.Add("Номер паспорта должен содержать 6 символов.");
                        break;
                    }
                }
            }

            if (dto.RequestType.Equals("Group", StringComparison.OrdinalIgnoreCase) && dto.Visitors?.Count < 5)
            {
                errors.Add("Для групповой заявки необходимо указать минимум 5 посетителей.");
            }

            if (!string.IsNullOrWhiteSpace(dto.ApplicantEmail) && !dto.ApplicantEmail.Contains('@'))
            {
                errors.Add("Укажите корректный адрес электронной почты заявителя.");
            }

            if (dto.BirthDate.HasValue && dto.BirthDate.Value.AddYears(16) > DateTime.UtcNow.Date)
            {
                errors.Add("Посетитель должен быть не моложе 16 лет.");
            }

            return errors;
        }

        private PassRequestSummaryDto MapToSummary(PassRequest request) => new()
        {
            Id = request.Id,
            RequestType = request.RequestType,
            Department = request.Department?.Name,
            StartDate = request.StartDate,
            EndDate = request.EndDate,
            Status = request.Status,
            Purpose = request.Purpose,
            VisitorCount = request.Visitors?.Count ?? 0
        };

        private PassRequestDetailsDto MapToDetails(PassRequest request) => new()
        {
            Id = request.Id,
            RequestType = request.RequestType,
            DepartmentId = request.DepartmentId,
            DepartmentName = request.Department?.Name,
            StartDate = request.StartDate,
            EndDate = request.EndDate,
            Purpose = request.Purpose,
            Status = request.Status,
            StatusId = request.StatusID,
            RejectionReason = request.RejectionReason,
            Visitors = request.Visitors?.Select(v => new PassVisitorDto
            {
                Id = v.Id,
                LastName = v.LastName,
                FirstName = v.FirstName,
                MiddleName = v.MiddleName,
                Email = v.Email,
                Phone = v.Phone,
                PassportNumber = v.PassportNumber,
                PassportSeries = v.PassportSeries
            }).ToList() ?? new List<PassVisitorDto>()
        };

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
    }

    public class PassRequestSummaryDto
    {
        public int Id { get; set; }
        public string? RequestType { get; set; }
        public string? Department { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Status { get; set; } = null!;
        public string Purpose { get; set; } = null!;
        public int VisitorCount { get; set; }
    }

    public class PassRequestDetailsDto
    {
        public int Id { get; set; }
        public string RequestType { get; set; } = null!;
        public int DepartmentId { get; set; }
        public string? DepartmentName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Purpose { get; set; } = null!;
        public string Status { get; set; } = null!;
        public int StatusId { get; set; }
        public string? RejectionReason { get; set; }
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
        public List<PassVisitorCreateDto> Visitors { get; set; } = new();
    }

    public class PassVisitorCreateDto
    {
        public string LastName { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string? MiddleName { get; set; }
        public string PassportSeries { get; set; } = null!;
        public string PassportNumber { get; set; } = null!;
        public string? Email { get; set; }
        public string? Phone { get; set; }
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
