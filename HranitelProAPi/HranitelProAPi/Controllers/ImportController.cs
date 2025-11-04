using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HranitelPRO.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HranitelPRO.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ImportController : ControllerBase
    {
        private readonly IExcelImportService _importService;

        public ImportController(IExcelImportService importService)
        {
            _importService = importService;
        }

        [HttpPost("visitors")]
        [Authorize]
        [RequestSizeLimit(10 * 1024 * 1024)]
        public async Task<ActionResult> ImportVisitors([FromForm] FileUploadDto request)
        {
            if (request.File == null || request.File.Length == 0)
            {
                return BadRequest(new { message = "Файл не найден" });
            }

            var imported = await _importService.ImportVisitorsAsync(request.File);
            return Ok(new { imported });
        }


        [HttpPost("departments")]
        [Authorize]
        [RequestSizeLimit(5 * 1024 * 1024)]
        public async Task<ActionResult> ImportDepartments([FromForm] FileUploadDto request)
        {
            if (request.File == null || request.File.Length == 0)
            {
                return BadRequest(new { message = "Файл не найден" });
            }

            var imported = await _importService.ImportDepartmentsAsync(request.File);
            return Ok(new { imported });
        }

        [HttpPost("roles")]
        [Authorize]
        [RequestSizeLimit(5 * 1024 * 1024)]
        public async Task<ActionResult> ImportRoles([FromForm] FileUploadDto request)
        {
            if (request.File == null || request.File.Length == 0)
            {
                return BadRequest(new { message = "Файл не найден" });
            }

            var imported = await _importService.ImportRolesAsync(request.File);
            return Ok(new { imported });
        }

        [HttpPost("statuses")]
        [Authorize]
        [RequestSizeLimit(5 * 1024 * 1024)]
        public async Task<ActionResult> ImportStatuses([FromForm] FileUploadDto request)
        {
            if (request.File == null || request.File.Length == 0)
            {
                return BadRequest(new { message = "Файл не найден" });
            }

            var imported = await _importService.ImportStatusesAsync(request.File);
            return Ok(new { imported });
        }

        [HttpPost("groups")]
        [Authorize]
        [RequestSizeLimit(5 * 1024 * 1024)]
        public async Task<ActionResult> ImportGroups([FromForm] FileUploadDto request)
        {
            if (request.File == null || request.File.Length == 0)
            {
                return BadRequest(new { message = "Файл не найден" });
            }

            var imported = await _importService.ImportGroupsAsync(request.File);
            return Ok(new { imported });
        }

        [HttpPost("employees")]
        [Authorize]
        [RequestSizeLimit(10 * 1024 * 1024)]
        public async Task<ActionResult> ImportEmployees([FromForm] FileUploadDto request)
        {
            if (request.File == null || request.File.Length == 0)
            {
                return BadRequest(new { message = "Файл не найден" });
            }

            var imported = await _importService.ImportEmployeesAsync(request.File);
            return Ok(new { imported });
        }

        [HttpPost("sessions")]
        [Authorize]
        [RequestSizeLimit(50 * 1024 * 1024)]
        public async Task<ActionResult> ImportSessions([FromForm] SessionImportDto request)
        {
            if (request.Excel == null || request.Excel.Length == 0)
            {
                return BadRequest(new { message = "Excel-файл не найден" });
            }

            var result = await _importService.ImportSessionsAsync(new SessionImportOptions
            {
                ExcelFile = request.Excel,
                Attachments = request.Attachments?.ToArray() ?? Array.Empty<IFormFile>()

            });

            return Ok(result);
        }
    }

    public class FileUploadDto
    {
        public IFormFile? File { get; set; }
    }

    public class SessionImportDto
    {
        public IFormFile? Excel { get; set; }
        public List<IFormFile>? Attachments { get; set; }
    }
}
