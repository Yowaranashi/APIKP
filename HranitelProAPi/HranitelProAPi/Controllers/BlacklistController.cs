using HranitelPro.API.Data;
using HranitelPRO.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HranitelPRO.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] // требует JWT токен
    public class BlacklistController : ControllerBase
    {
        private readonly HranitelContext _context;

        public BlacklistController(HranitelContext context)
        {
            _context = context;
        }

        // GET: api/blacklist
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BlacklistEntry>>> GetAll()
        {
            var entries = await _context.BlacklistEntries
                .Include(b => b.AddedByEmployee)
                .ToListAsync();

            return Ok(entries);
        }

        // GET: api/blacklist/5
        [HttpGet("{id}")]
        public async Task<ActionResult<BlacklistEntry>> Get(int id)
        {
            var entry = await _context.BlacklistEntries
                .Include(b => b.AddedByEmployee)
                .FirstOrDefaultAsync(b => b.Id == id);

            if (entry == null)
                return NotFound(new { message = "Запись не найдена" });

            return Ok(entry);
        }

        // POST: api/blacklist
        [HttpPost]
        public async Task<ActionResult> Add([FromBody] BlacklistCreateDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.LastName) || string.IsNullOrWhiteSpace(dto.FirstName))
                return BadRequest(new { message = "Имя и фамилия обязательны" });

            var entry = new BlacklistEntry
            {
                LastName = dto.LastName,
                FirstName = dto.FirstName,
                MiddleName = dto.MiddleName,
                PassportSeries = dto.PassportSeries,
                PassportNumber = dto.PassportNumber,
                Reason = dto.Reason,
                AddedByEmployeeId = dto.AddedByEmployeeId,
                AddedAt = DateTime.UtcNow
            };

            _context.BlacklistEntries.Add(entry);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(Get), new { id = entry.Id }, entry);
        }

        // PUT: api/blacklist/5
        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, [FromBody] BlacklistUpdateDto dto)
        {
            var entry = await _context.BlacklistEntries.FindAsync(id);
            if (entry == null)
                return NotFound(new { message = "Запись не найдена" });

            entry.Reason = dto.Reason ?? entry.Reason;
            entry.PassportSeries = dto.PassportSeries ?? entry.PassportSeries;
            entry.PassportNumber = dto.PassportNumber ?? entry.PassportNumber;
            entry.AddedByEmployeeId = dto.AddedByEmployeeId ?? entry.AddedByEmployeeId;

            await _context.SaveChangesAsync();
            return Ok(entry);
        }

        // DELETE: api/blacklist/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var entry = await _context.BlacklistEntries.FindAsync(id);
            if (entry == null)
                return NotFound(new { message = "Запись не найдена" });

            _context.BlacklistEntries.Remove(entry);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }

    // DTOs
    public class BlacklistCreateDto
    {
        public string LastName { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string? MiddleName { get; set; }
        public string? PassportSeries { get; set; }
        public string? PassportNumber { get; set; }
        public string Reason { get; set; } = null!;
        public int? AddedByEmployeeId { get; set; }
    }

    public class BlacklistUpdateDto
    {
        public string? PassportSeries { get; set; }
        public string? PassportNumber { get; set; }
        public string? Reason { get; set; }
        public int? AddedByEmployeeId { get; set; }
    }
}
