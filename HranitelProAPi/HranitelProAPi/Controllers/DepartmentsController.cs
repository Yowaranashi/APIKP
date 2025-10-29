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
    public class DepartmentsController : ControllerBase
    {
        private readonly HranitelContext _context;

        public DepartmentsController(HranitelContext context)
        {
            _context = context;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<DepartmentDto>>> GetAll()
        {
            var departments = await _context.Departments
                .AsNoTracking()
                .Include(d => d.Employees)
                .OrderBy(d => d.Name)
                .ToListAsync();

            var result = departments.Select(d => new DepartmentDto
            {
                Id = d.Id,
                Name = d.Name,
                Description = d.Description,
                Employees = d.Employees?.Select(e => new EmployeeDto
                {
                    Id = e.Id,
                    FullName = e.FullName,
                    EmployeeCode = e.EmployeeCode
                }).ToList() ?? new List<EmployeeDto>()
            });

            return Ok(result);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<DepartmentDto>> GetById(int id)
        {
            var department = await _context.Departments
                .AsNoTracking()
                .Include(d => d.Employees)
                .FirstOrDefaultAsync(d => d.Id == id);

            if (department == null)
            {
                return NotFound();
            }

            return Ok(new DepartmentDto
            {
                Id = department.Id,
                Name = department.Name,
                Description = department.Description,
                Employees = department.Employees?.Select(e => new EmployeeDto
                {
                    Id = e.Id,
                    FullName = e.FullName,
                    EmployeeCode = e.EmployeeCode
                }).ToList() ?? new List<EmployeeDto>()
            });
        }

        [HttpPost]
        public async Task<ActionResult<DepartmentDto>> Create([FromBody] DepartmentCreateDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Name))
            {
                return BadRequest(new { message = "Название подразделения обязательно" });
            }

            var department = new Department
            {
                Name = dto.Name,
                Description = dto.Description
            };

            _context.Departments.Add(department);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = department.Id }, new DepartmentDto
            {
                Id = department.Id,
                Name = department.Name,
                Description = department.Description,
                Employees = new List<EmployeeDto>()
            });
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] DepartmentCreateDto dto)
        {
            var department = await _context.Departments.FindAsync(id);
            if (department == null)
            {
                return NotFound();
            }

            if (string.IsNullOrWhiteSpace(dto.Name))
            {
                return BadRequest(new { message = "Название подразделения обязательно" });
            }

            department.Name = dto.Name;
            department.Description = dto.Description;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var department = await _context.Departments.FindAsync(id);
            if (department == null)
            {
                return NotFound();
            }

            var hasRequests = await _context.PassRequests.AnyAsync(p => p.DepartmentId == id);
            if (hasRequests)
            {
                return Conflict(new { message = "Нельзя удалить подразделение, для которого существуют заявки" });
            }

            _context.Departments.Remove(department);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }

    public class DepartmentDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public List<EmployeeDto> Employees { get; set; } = new();
    }

    public class EmployeeDto
    {
        public int Id { get; set; }
        public string FullName { get; set; } = null!;
        public string? EmployeeCode { get; set; }
    }

    public class DepartmentCreateDto
    {
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
    }
}
