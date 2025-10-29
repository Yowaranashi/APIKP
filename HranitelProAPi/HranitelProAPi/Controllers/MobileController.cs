using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using HranitelPro.API.Data;
using HranitelPRO.API.Contracts;
using HranitelPRO.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HranitelPRO.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class MobileController : ControllerBase
    {
        private readonly ISecurityWorkflowService _workflowService;
        private readonly HranitelContext _context;

        public MobileController(ISecurityWorkflowService workflowService, HranitelContext context)
        {
            _workflowService = workflowService;
            _context = context;
        }

        [HttpGet("profile/{employeeCode}")]
        public async Task<ActionResult<MobileProfileDto>> GetProfile(string employeeCode)
        {
            var employee = await _context.Employees
                .AsNoTracking()
                .Include(e => e.Department)
                .FirstOrDefaultAsync(e => e.EmployeeCode == employeeCode);

            if (employee == null)
            {
                return NotFound(new { message = "Сотрудник не найден" });
            }

            return Ok(new MobileProfileDto
            {
                EmployeeId = employee.Id,
                FullName = employee.FullName,
                DepartmentId = employee.DepartmentId,
                DepartmentName = employee.Department?.Name
            });
        }

        [HttpGet("requests")]
        public async Task<ActionResult<IEnumerable<SecurityRequestDto>>> GetRequests([FromQuery] SecurityQuery query)
        {
            var requests = await _workflowService.GetApprovedRequestsAsync(query);
            return Ok(requests);
        }

        [HttpPost("requests/{id:int}/allow")]
        public async Task<ActionResult> Allow(int id, [FromBody] SecurityAccessDto dto)
        {
            try
            {
                await _workflowService.AllowAccessAsync(id, dto);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("requests/{id:int}/checkout")]
        public async Task<ActionResult> Checkout(int id, [FromBody] SecurityCompleteDto dto)
        {
            try
            {
                var completed = await _workflowService.CompleteVisitAsync(id, dto);
                return Ok(new { completed });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("visitors/{visitorId:int}/photo")]
        [RequestSizeLimit(5 * 1024 * 1024)]
        public async Task<ActionResult> UploadVisitorPhoto(int visitorId, IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest(new { message = "Файл не найден" });
            }

            var visitor = await _context.PassVisitors.FirstOrDefaultAsync(v => v.Id == visitorId);
            if (visitor == null)
            {
                return NotFound(new { message = "Посетитель не найден" });
            }

            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "Uploads", "mobile");
            Directory.CreateDirectory(uploadsFolder);

            var fileName = $"visitor_{visitorId}_{Guid.NewGuid():N}{Path.GetExtension(file.FileName)}";
            var path = Path.Combine(uploadsFolder, fileName);

            await using (var stream = System.IO.File.Create(path))
            {
                await file.CopyToAsync(stream);
            }

            visitor.PhotoPath = Path.Combine("uploads", "mobile", fileName).Replace('\', '/');
            await _context.SaveChangesAsync();

            return Ok(new { photo = visitor.PhotoPath });
        }
    }

    public class MobileProfileDto
    {
        public int EmployeeId { get; set; }
        public string FullName { get; set; } = null!;
        public int? DepartmentId { get; set; }
        public string? DepartmentName { get; set; }
    }
}
