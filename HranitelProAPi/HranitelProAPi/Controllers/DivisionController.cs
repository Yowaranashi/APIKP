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
    [Authorize]
    public class DivisionController : ControllerBase
    {
        private readonly HranitelContext _context;

        public DivisionController(HranitelContext context)
        {
            _context = context;
        }

        [HttpGet("requests")]
        public async Task<ActionResult<IEnumerable<DivisionRequestDto>>> GetRequests([FromQuery] DivisionRequestQuery query)
        {
            if (!query.DepartmentId.HasValue)
            {
                return BadRequest(new { message = "Не указан идентификатор подразделения" });
            }

            var requests = await _context.PassRequests
                .AsNoTracking()
                .Include(r => r.Visitors)
                .Include(r => r.VisitRecords)
                .Where(r => r.DepartmentId == query.DepartmentId.Value &&
                            (r.Status == "Approved" || r.Status == "InProgress"))
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();

            var result = requests.Select(r => new DivisionRequestDto
            {
                Id = r.Id,
                Status = r.Status,
                StartDate = r.StartDate,
                EndDate = r.EndDate,
                Purpose = r.Purpose,
                Visitors = r.Visitors?.Select(v => new DivisionVisitorDto
                {
                    Id = v.Id,
                    FullName = string.Join(" ", new[] { v.LastName, v.FirstName, v.MiddleName }.Where(s => !string.IsNullOrEmpty(s))),
                    PassportSeries = v.PassportSeries,
                    PassportNumber = v.PassportNumber,
                    EntryTime = r.VisitRecords?.FirstOrDefault(rec => rec.VisitorId == v.Id)?.EntryTime,
                    ExitTime = r.VisitRecords?.FirstOrDefault(rec => rec.VisitorId == v.Id)?.ExitTime
                }).ToList() ?? new List<DivisionVisitorDto>()
            });

            return Ok(result);
        }

        [HttpPost("requests/{id:int}/visit")]
        public async Task<ActionResult> UpdateVisit(int id, [FromBody] DivisionVisitUpdateDto dto)
        {
            var request = await _context.PassRequests
                .Include(r => r.VisitRecords)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (request == null)
            {
                return NotFound(new { message = "Заявка не найдена" });
            }

            foreach (var update in dto.Visitors)
            {
                var record = request.VisitRecords?.FirstOrDefault(r => r.VisitorId == update.VisitorId);
                if (record == null)
                {
                    record = new VisitRecord
                    {
                        PassRequestId = id,
                        VisitorId = update.VisitorId,
                        DepartmentId = request.DepartmentId
                    };
                    _context.VisitRecords.Add(record);
                }

                if (update.EntryTime.HasValue)
                {
                    record.EntryTime = update.EntryTime;
                }

                if (update.ExitTime.HasValue)
                {
                    record.ExitTime = update.ExitTime;
                }
            }

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpPost("visitors/{visitorId:int}/blacklist")]
        public async Task<ActionResult> AddToBlacklist(int visitorId, [FromBody] DivisionBlacklistDto dto)
        {
            var visitor = await _context.PassVisitors.FirstOrDefaultAsync(v => v.Id == visitorId);
            if (visitor == null)
            {
                return NotFound(new { message = "Посетитель не найден" });
            }

            if (string.IsNullOrWhiteSpace(dto.Reason))
            {
                return BadRequest(new { message = "Необходимо указать причину" });
            }

            var entry = new BlacklistEntry
            {
                LastName = visitor.LastName,
                FirstName = visitor.FirstName,
                MiddleName = visitor.MiddleName,
                PassportSeries = visitor.PassportSeries,
                PassportNumber = visitor.PassportNumber,
                Reason = dto.Reason,
                AddedByEmployeeId = dto.EmployeeId,
                AddedAt = DateTime.UtcNow
            };

            _context.BlacklistEntries.Add(entry);
            await _context.SaveChangesAsync();

            return Ok(new { entry.Id });
        }
    }

    public class DivisionRequestQuery
    {
        public int? DepartmentId { get; set; }
    }

    public class DivisionRequestDto
    {
        public int Id { get; set; }
        public string Status { get; set; } = null!;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Purpose { get; set; } = null!;
        public List<DivisionVisitorDto> Visitors { get; set; } = new();
    }

    public class DivisionVisitorDto
    {
        public int Id { get; set; }
        public string FullName { get; set; } = null!;
        public string PassportSeries { get; set; } = null!;
        public string PassportNumber { get; set; } = null!;
        public DateTime? EntryTime { get; set; }
        public DateTime? ExitTime { get; set; }
    }

    public class DivisionVisitUpdateDto
    {
        public List<DivisionVisitorUpdateDto> Visitors { get; set; } = new();
    }

    public class DivisionVisitorUpdateDto
    {
        public int VisitorId { get; set; }
        public DateTime? EntryTime { get; set; }
        public DateTime? ExitTime { get; set; }
    }

    public class DivisionBlacklistDto
    {
        public int? EmployeeId { get; set; }
        public string Reason { get; set; } = null!;
    }
}
