using Microsoft.AspNetCore.Http;

namespace HranitelPRO.API.Services
{
    public interface IExcelImportService
    {
        Task<int> ImportVisitorsAsync(IFormFile file);
        Task<int> ImportEmployeesAsync(IFormFile file);
    }
}
