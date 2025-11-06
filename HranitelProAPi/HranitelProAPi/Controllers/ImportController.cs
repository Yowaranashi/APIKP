using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using HranitelPRO.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace HranitelPRO.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [AllowAnonymous]
    public class ImportController : ControllerBase
    {
        private readonly IExcelImportService _importService;
        private readonly ILogger<ImportController> _logger;

        public ImportController(IExcelImportService importService, ILogger<ImportController> logger)
        {
            _importService = importService;
            _logger = logger;
        }

        [HttpPost("visitors")]
        [AllowAnonymous]
        [RequestSizeLimit(10 * 1024 * 1024)]
        public async Task<ActionResult> ImportVisitors([FromForm] FileUploadDto request)
        {
            if (ValidateFile(request.File) is { } errorResult)
            {
                return errorResult;
            }

            return await ExecuteImportAsync(async () =>
            {
                var imported = await _importService.ImportVisitorsAsync(request.File!);
                return new { imported };
            }, "посетителей");
        }


        [HttpPost("departments")]
        [AllowAnonymous]
        [RequestSizeLimit(5 * 1024 * 1024)]
        public async Task<ActionResult> ImportDepartments([FromForm] FileUploadDto request)
        {
            if (ValidateFile(request.File) is { } errorResult)
            {
                return errorResult;
            }

            return await ExecuteImportAsync(async () =>
            {
                var imported = await _importService.ImportDepartmentsAsync(request.File!);
                return new { imported };
            }, "подразделений");
        }

        [HttpPost("roles")]
        [AllowAnonymous]
        [RequestSizeLimit(5 * 1024 * 1024)]
        public async Task<ActionResult> ImportRoles([FromForm] FileUploadDto request)
        {
            if (ValidateFile(request.File) is { } errorResult)
            {
                return errorResult;
            }

            return await ExecuteImportAsync(async () =>
            {
                var imported = await _importService.ImportRolesAsync(request.File!);
                return new { imported };
            }, "ролей");
        }

        [HttpPost("statuses")]
        [AllowAnonymous]
        [RequestSizeLimit(5 * 1024 * 1024)]
        public async Task<ActionResult> ImportStatuses([FromForm] FileUploadDto request)
        {
            if (ValidateFile(request.File) is { } errorResult)
            {
                return errorResult;
            }

            return await ExecuteImportAsync(async () =>
            {
                var imported = await _importService.ImportStatusesAsync(request.File!);
                return new { imported };
            }, "статусов");
        }

        [HttpPost("groups")]
        [AllowAnonymous]
        [RequestSizeLimit(5 * 1024 * 1024)]
        public async Task<ActionResult> ImportGroups([FromForm] FileUploadDto request)
        {
            if (ValidateFile(request.File) is { } errorResult)
            {
                return errorResult;
            }

            return await ExecuteImportAsync(async () =>
            {
                var imported = await _importService.ImportGroupsAsync(request.File!);
                return new { imported };
            }, "групп");
        }

        [HttpPost("employees")]
        [AllowAnonymous]
        [RequestSizeLimit(10 * 1024 * 1024)]
        public async Task<ActionResult> ImportEmployees([FromForm] FileUploadDto request)
        {
            if (ValidateFile(request.File) is { } errorResult)
            {
                return errorResult;
            }

            return await ExecuteImportAsync(async () =>
            {
                var imported = await _importService.ImportEmployeesAsync(request.File!);
                return new { imported };
            }, "сотрудников");
        }

        [HttpPost("sessions")]
        [AllowAnonymous]
        [RequestSizeLimit(50 * 1024 * 1024)]
        public async Task<ActionResult> ImportSessions([FromForm] SessionImportDto request)
        {
            if (ValidateFile(request.Excel, "Excel-файл не найден") is { } errorResult)
            {
                return errorResult;
            }

            return await ExecuteImportAsync(async () =>
            {
                var result = await _importService.ImportSessionsAsync(new SessionImportOptions
                {
                    ExcelFile = request.Excel!,
                    Attachments = request.Attachments?.ToArray() ?? Array.Empty<IFormFile>()

                });

                return result;
            }, "сессий");
        }

        private ActionResult? ValidateFile(IFormFile? file, string errorMessage = "Файл не найден")
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest(new { message = errorMessage });
            }

            return null;
        }

        private async Task<ActionResult> ExecuteImportAsync<T>(Func<Task<T>> importOperation, string importTarget)
        {
            try
            {
                var result = await importOperation();
                return Ok(result);
            }
            catch (InvalidDataException ex)
            {
                _logger.LogWarning(ex, "Некорректный файл для импорта {ImportTarget}", importTarget);
                return BadRequest(new
                {
                    message = "Некорректный формат файла",
                    detail = ex.Message
                });
            }
            catch (FormatException ex)
            {
                _logger.LogWarning(ex, "Некорректные данные в файле для импорта {ImportTarget}", importTarget);
                return BadRequest(new
                {
                    message = "Некорректные данные в файле",
                    detail = ex.Message
                });
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Ошибка базы данных при импорте {ImportTarget}", importTarget);
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    message = "Ошибка сохранения данных",
                    detail = ex.GetBaseException().Message
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Непредвиденная ошибка при импорте {ImportTarget}", importTarget);
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    message = "Произошла внутренняя ошибка при импорте",
                    detail = ex.Message
                });
            }
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
