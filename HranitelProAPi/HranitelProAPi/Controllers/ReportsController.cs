using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HranitelPro.API.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HranitelPRO.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ReportsController : ControllerBase
    {
        private readonly HranitelContext _context;

        public ReportsController(HranitelContext context)
        {
            _context = context;
        }

        [HttpGet("traffic")]
        public async Task<ActionResult<IEnumerable<VisitTrafficDto>>> GetTraffic([FromQuery] ReportTrafficQuery query)
        {
            var start = (query.Start ?? DateTime.UtcNow.Date).Date;
            var end = (query.End ?? start).Date;

            if (end < start)
            {
                return BadRequest(new { message = "Дата окончания не может быть раньше даты начала" });
            }

            var records = _context.VisitRecords
                .AsNoTracking()
                .Include(v => v.PassRequest)
                .Where(v => (v.EntryTime ?? v.PassRequest.StartDate) >= start &&
                            (v.EntryTime ?? v.PassRequest.StartDate) <= end.AddDays(1).AddTicks(-1));

            if (query.DepartmentId.HasValue)
            {
                records = records.Where(v => (v.DepartmentId ?? v.PassRequest.DepartmentId) == query.DepartmentId.Value);
            }

            var grouped = await records
                .Select(v => new
                {
                    Date = (v.EntryTime ?? v.PassRequest.StartDate).Date,
                    DepartmentId = v.DepartmentId ?? v.PassRequest.DepartmentId
                })
                .GroupBy(x => new { x.Date, x.DepartmentId })
                .Select(g => new VisitTrafficDto
                {
                    Date = g.Key.Date,
                    DepartmentId = g.Key.DepartmentId,
                    Count = g.Count()
                })
                .OrderBy(g => g.Date)
                .ThenBy(g => g.DepartmentId)
                .ToListAsync();

            return Ok(grouped);
        }

        [HttpGet("on-premises")]
        public async Task<ActionResult<IEnumerable<OnPremisesDto>>> GetOnPremises()
        {
            var now = DateTime.UtcNow;
            var visits = await _context.VisitRecords
                .AsNoTracking()
                .Include(v => v.PassRequest)
                .Include(v => v.Visitor)
                .Where(v => v.EntryTime != null && v.ExitTime == null)
                .ToListAsync();

            var result = visits
                .GroupBy(v => v.DepartmentId ?? v.PassRequest.DepartmentId)
                .Select(g => new OnPremisesDto
                {
                    DepartmentId = g.Key,
                    Count = g.Count(),
                    Visitors = g.Select(v => new VisitorShortDto
                    {
                        VisitorId = v.VisitorId,
                        FullName = v.Visitor == null
                            ? null
                            : string.Join(" ", new[] { v.Visitor.LastName, v.Visitor.FirstName, v.Visitor.MiddleName }
                                .Where(s => !string.IsNullOrEmpty(s))),
                        EntryTime = v.EntryTime ?? now
                    }).ToList()
                })
                .OrderBy(r => r.DepartmentId)
                .ToList();

            return Ok(result);
        }

        [HttpGet("summary")]
        public async Task<ActionResult<ReportSummaryDto>> GetSummary()
        {
            var today = DateTime.UtcNow.Date;
            var startOfMonth = new DateTime(today.Year, today.Month, 1);
            var startOfYear = new DateTime(today.Year, 1, 1);

            var daily = await _context.PassRequests.CountAsync(r => r.StartDate.Date == today);
            var monthly = await _context.PassRequests.CountAsync(r => r.StartDate.Date >= startOfMonth && r.StartDate.Date <= today);
            var yearly = await _context.PassRequests.CountAsync(r => r.StartDate.Date >= startOfYear && r.StartDate.Date <= today);

            var departmentBreakdown = await _context.PassRequests
                .AsNoTracking()
                .Where(r => r.StartDate.Date >= startOfMonth && r.StartDate.Date <= today)
                .GroupBy(r => r.DepartmentId)
                .Select(g => new DepartmentSummaryDto
                {
                    DepartmentId = g.Key,
                    Count = g.Count()
                })
                .OrderBy(g => g.DepartmentId)
                .ToListAsync();

            return Ok(new ReportSummaryDto
            {
                Daily = daily,
                Monthly = monthly,
                Yearly = yearly,
                Departments = departmentBreakdown
            });
        }
    }

    public class ReportTrafficQuery
    {
        public DateTime? Start { get; set; }
        public DateTime? End { get; set; }
        public int? DepartmentId { get; set; }
    }

    public class VisitTrafficDto
    {
        public DateTime Date { get; set; }
        public int? DepartmentId { get; set; }
        public int Count { get; set; }
    }

    public class OnPremisesDto
    {
        public int? DepartmentId { get; set; }
        public int Count { get; set; }
        public List<VisitorShortDto> Visitors { get; set; } = new();
    }

    public class VisitorShortDto
    {
        public int? VisitorId { get; set; }
        public string? FullName { get; set; }
        public DateTime EntryTime { get; set; }
    }

    public class ReportSummaryDto
    {
        public int Daily { get; set; }
        public int Monthly { get; set; }
        public int Yearly { get; set; }
        public List<DepartmentSummaryDto> Departments { get; set; } = new();
    }

    public class DepartmentSummaryDto
    {
        public int DepartmentId { get; set; }
        public int Count { get; set; }
    }
}
