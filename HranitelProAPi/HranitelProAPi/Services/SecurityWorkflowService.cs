using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HranitelPro.API.Data;
using HranitelPRO.API.Contracts;
using HranitelPRO.API.Models;
using Microsoft.EntityFrameworkCore;

namespace HranitelPRO.API.Services
{
    public interface ISecurityWorkflowService
    {
        Task<IReadOnlyCollection<SecurityRequestDto>> GetApprovedRequestsAsync(SecurityQuery query);
        Task AllowAccessAsync(int requestId, SecurityAccessDto dto);
        Task<bool> CompleteVisitAsync(int requestId, SecurityCompleteDto dto);
    }

    public class SecurityWorkflowService : ISecurityWorkflowService
    {
        private readonly HranitelContext _context;

        public SecurityWorkflowService(HranitelContext context)
        {
            _context = context;
        }

        public async Task<IReadOnlyCollection<SecurityRequestDto>> GetApprovedRequestsAsync(SecurityQuery query)
        {
            await EnsureStatusExistsAsync("Approved");

            var requestsQuery = _context.PassRequests
                .AsNoTracking()
                .Include(r => r.Department)
                .Include(r => r.Visitors)
                .Where(r => r.Status == "Approved");

            if (query.Date.HasValue)
            {
                var date = query.Date.Value.Date;
                requestsQuery = requestsQuery.Where(r => r.StartDate.Date <= date && r.EndDate.Date >= date);
            }

            if (query.DepartmentId.HasValue)
            {
                requestsQuery = requestsQuery.Where(r => r.DepartmentId == query.DepartmentId.Value);
            }

            if (!string.IsNullOrWhiteSpace(query.RequestType))
            {
                requestsQuery = requestsQuery.Where(r => r.RequestType == query.RequestType);
            }

            if (!string.IsNullOrWhiteSpace(query.Search))
            {
                var term = query.Search.Trim().ToLowerInvariant();
                requestsQuery = requestsQuery.Where(r =>
                    (r.ApplicantLastName + " " + r.ApplicantFirstName + " " + r.ApplicantMiddleName).ToLower().Contains(term) ||
                    r.Visitors.Any(v => (v.LastName + " " + v.FirstName + " " + v.MiddleName).ToLower().Contains(term) ||
                                        v.PassportNumber.Contains(term)));
            }

            var requests = await requestsQuery
                .OrderBy(r => r.StartDate)
                .ToListAsync();

            return requests.Select(r => new SecurityRequestDto
            {
                Id = r.Id,
                DepartmentName = r.Department?.Name,
                RequestType = r.RequestType,
                StartDate = r.StartDate,
                EndDate = r.EndDate,
                VisitorCount = r.Visitors?.Count ?? 0,
                Visitors = r.Visitors?.Select(v => new SecurityVisitorDto
                {
                    Id = v.Id,
                    FullName = string.Join(" ", new[] { v.LastName, v.FirstName, v.MiddleName }.Where(s => !string.IsNullOrEmpty(s))),
                    Passport = $"{v.PassportSeries} {v.PassportNumber}"
                }).ToList() ?? new List<SecurityVisitorDto>()
            }).ToList();
        }

        public async Task AllowAccessAsync(int requestId, SecurityAccessDto dto)
        {
            var request = await _context.PassRequests
                .Include(r => r.Visitors)
                .FirstOrDefaultAsync(r => r.Id == requestId);

            if (request == null)
            {
                throw new KeyNotFoundException("Заявка не найдена");
            }

            if (!request.Status.Equals("Approved", StringComparison.OrdinalIgnoreCase))
            {
                throw new InvalidOperationException("Проход возможен только для одобренных заявок");
            }

            await EnsureStatusExistsAsync("InProgress");
            var accessTime = dto.AccessTime ?? DateTime.UtcNow;

            foreach (var visitor in request.Visitors ?? Enumerable.Empty<PassVisitor>())
            {
                var record = await _context.VisitRecords
                    .FirstOrDefaultAsync(v => v.PassRequestId == requestId && v.VisitorId == visitor.Id);

                if (record == null)
                {
                    record = new VisitRecord
                    {
                        PassRequestId = request.Id,
                        VisitorId = visitor.Id,
                        DepartmentId = request.DepartmentId,
                        EntryTime = accessTime
                    };
                    _context.VisitRecords.Add(record);
                }
                else
                {
                    record.EntryTime ??= accessTime;
                }
            }

            request.Status = "InProgress";
            request.StatusID = await ResolveStatusIdAsync("InProgress");

            _context.SecurityLogs.Add(new SecurityLog
            {
                EventType = "AllowAccess",
                EmployeeId = dto.EmployeeId,
                PassRequestId = request.Id,
                Timestamp = accessTime,
                Details = dto.TurnstileId is null ? null : $"Turnstile: {dto.TurnstileId}"
            });

            _context.AccessEvents.Add(new AccessEvent
            {
                EventType = "Turnstile",
                EmployeeId = dto.EmployeeId,
                PassRequestId = request.Id,
                EventTime = accessTime,
                Metadata = dto.TurnstileId
            });

            await _context.SaveChangesAsync();
        }

        public async Task<bool> CompleteVisitAsync(int requestId, SecurityCompleteDto dto)
        {
            var request = await _context.PassRequests
                .Include(r => r.Visitors)
                .Include(r => r.VisitRecords)
                .FirstOrDefaultAsync(r => r.Id == requestId);

            if (request == null)
            {
                throw new KeyNotFoundException("Заявка не найдена");
            }

            if (!request.Status.Equals("InProgress", StringComparison.OrdinalIgnoreCase))
            {
                throw new InvalidOperationException("Завершить можно только активные посещения");
            }

            var exitTime = dto.ExitTime ?? DateTime.UtcNow;
            var visitorIds = dto.VisitorIds?.ToHashSet();

            foreach (var record in request.VisitRecords ?? Enumerable.Empty<VisitRecord>())
            {
                if (visitorIds != null && !visitorIds.Contains(record.VisitorId ?? 0))
                {
                    continue;
                }

                record.ExitTime = exitTime;
            }

            var allCompleted = (request.VisitRecords ?? new List<VisitRecord>()).All(v => v.ExitTime.HasValue);
            if (allCompleted)
            {
                await EnsureStatusExistsAsync("Completed");
                request.Status = "Completed";
                request.StatusID = await ResolveStatusIdAsync("Completed");
            }

            _context.SecurityLogs.Add(new SecurityLog
            {
                EventType = "CompleteVisit",
                EmployeeId = dto.EmployeeId,
                PassRequestId = request.Id,
                Timestamp = exitTime,
                Details = dto.VisitorIds == null ? null : $"Visitors: {string.Join(',', dto.VisitorIds)}"
            });

            await _context.SaveChangesAsync();
            return allCompleted;
        }

        private async Task EnsureStatusExistsAsync(string status)
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
    }
}
