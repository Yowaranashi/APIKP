using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace HranitelPRO.API.Services
{
    public interface IExcelImportService
    {
        Task<int> ImportVisitorsAsync(IFormFile file);
        Task<int> ImportEmployeesAsync(IFormFile file);
        Task<int> ImportDepartmentsAsync(IFormFile file);
        Task<int> ImportRolesAsync(IFormFile file);
        Task<int> ImportStatusesAsync(IFormFile file);
        Task<int> ImportGroupsAsync(IFormFile file);
        Task<SessionImportResult> ImportSessionsAsync(SessionImportOptions options);
    }

    public class SessionImportOptions
    {
        public IFormFile ExcelFile { get; set; } = null!;
        public IEnumerable<IFormFile> Attachments { get; set; } = new List<IFormFile>();
    }

    public class SessionImportResult
    {
        public int CreatedSessions { get; set; }
        public int UpdatedSessions { get; set; }
        public int CreatedDepartments { get; set; }
        public int SavedAttachments { get; set; }
        public List<string> Warnings { get; set; } = new();
    }
}
