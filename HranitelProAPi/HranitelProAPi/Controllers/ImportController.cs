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
0
        [RequestSizeLimit(10 * 1024 * 1024)]
        public async Task<ActionResult> ImportVisitors([FromForm] FileUploadDto request)
        {
            if (ValidateFile(request.File) is { } errorResult)
            {
                return errorResult;
            }

            var imported = await _importService.ImportVisitorsAsync(request.File!);
            return Ok(new { imported });
        }


        [HttpPost("departments")]
0
        [RequestSizeLimit(5 * 1024 * 1024)]
        public async Task<ActionResult> ImportDepartments([FromForm] FileUploadDto request)
        {
            if (ValidateFile(request.File) is { } errorResult)
            {
                return errorResult;
            }

            var imported = await _importService.ImportDepartmentsAsync(request.File!);
            return Ok(new { imported });
        }

        [HttpPost("roles")]
0
        [RequestSizeLimit(5 * 1024 * 1024)]
        public async Task<ActionResult> ImportRoles([FromForm] FileUploadDto request)
        {
            if (ValidateFile(request.File) is { } errorResult)
            {
                return errorResult;
            }

            var imported = await _importService.ImportRolesAsync(request.File!);
            return Ok(new { imported });
        }

        [HttpPost("statuses")]
0
        [RequestSizeLimit(5 * 1024 * 1024)]
        public async Task<ActionResult> ImportStatuses([FromForm] FileUploadDto request)
        {
            if (ValidateFile(request.File) is { } errorResult)
            {
                return errorResult;
            }

            var imported = await _importService.ImportStatusesAsync(request.File!);
            return Ok(new { imported });
        }

        [HttpPost("groups")]
0
        [RequestSizeLimit(5 * 1024 * 1024)]
        public async Task<ActionResult> ImportGroups([FromForm] FileUploadDto request)
        {
            if (ValidateFile(request.File) is { } errorResult)
            {
                return errorResult;
            }

            var imported = await _importService.ImportGroupsAsync(request.File!);
            return Ok(new { imported });
        }

        [HttpPost("employees")]
0
        [RequestSizeLimit(10 * 1024 * 1024)]
        public async Task<ActionResult> ImportEmployees([FromForm] FileUploadDto request)
        {
            if (ValidateFile(request.File) is { } errorResult)
            {
                return errorResult;
            }

            var imported = await _importService.ImportEmployeesAsync(request.File!);
            return Ok(new { imported });
        }

        [HttpPost("sessions")]
0
        [RequestSizeLimit(50 * 1024 * 1024)]
        public async Task<ActionResult> ImportSessions([FromForm] SessionImportDto request)
        {
            if (ValidateFile(request.Excel, "Excel-файл не найден") is { } errorResult)
            {
                return errorResult;
            }

            var result = await _importService.ImportSessionsAsync(new SessionImportOptions
            {
                ExcelFile = request.Excel!,
                Attachments = request.Attachments?.ToArray() ?? Array.Empty<IFormFile>()

            });

            return Ok(result);
        }

        private ActionResult? ValidateFile(IFormFile? file, string errorMessage = "Файл не найден")
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest(new { message = errorMessage });
            }

            return null;
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
