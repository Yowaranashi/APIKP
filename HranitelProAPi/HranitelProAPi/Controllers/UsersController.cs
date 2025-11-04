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
    [Route("api/users")]
    public class UsersController : ControllerBase
    {
        private readonly HranitelContext _context;

        public UsersController(HranitelContext context)
        {
            _context = context;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<EmployeeDirectoryItemDto>>> Get([FromQuery] int? departmentId)
        {
            var query = _context.Employees
                .AsNoTracking()
                .OrderBy(e => e.FullName)
                .AsQueryable();

            if (departmentId.HasValue)
            {
                query = query.Where(e => e.DepartmentId == departmentId.Value);
            }

            var employees = await query
                .Select(e => new EmployeeDirectoryItemDto
                {
                    Id = e.Id,
                    FullName = e.FullName,
                    DepartmentId = e.DepartmentId
                })
                .ToListAsync();

            return Ok(employees);
        }
    }

    public class EmployeeDirectoryItemDto
    {
        public int Id { get; set; }
        public string FullName { get; set; } = null!;
        public int? DepartmentId { get; set; }
    }
}
