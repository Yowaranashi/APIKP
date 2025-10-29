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
    }

    public class FileUploadDto
    {
        public IFormFile? File { get; set; }
    }
}
