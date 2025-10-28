using ClosedXML.Excel;
using HranitelPro.API.Data;
using HranitelPRO.API.Models;
using Microsoft.AspNetCore.Http;

namespace HranitelPRO.API.Services
{
    public class ExcelImportService : IExcelImportService
    {
        private readonly HranitelContext _context;

        public ExcelImportService(HranitelContext context)
        {
            _context = context;
        }

        public async Task<int> ImportVisitorsAsync(IFormFile file)
        {
            int imported = 0;
            using var stream = new MemoryStream();
            await file.CopyToAsync(stream);
            using var workbook = new XLWorkbook(stream);
            var worksheet = workbook.Worksheets.First();
            var rows = worksheet.RangeUsed().RowsUsed().Skip(1);
            foreach (var row in rows)
            {
                var visitor = new PassVisitor
                {
                    LastName = row.Cell(1).GetString(),
                    FirstName = row.Cell(2).GetString(),
                    MiddleName = row.Cell(3).GetString(),
                    Phone = row.Cell(4).GetString(),
                    Email = row.Cell(5).GetString(),
                    PassportSeries = row.Cell(6).GetString(),
                    PassportNumber = row.Cell(7).GetString()
                };
                _context.PassVisitors.Add(visitor);
                imported++;
            }
            await _context.SaveChangesAsync();
            return imported;
        }

        public async Task<int> ImportEmployeesAsync(IFormFile file)
        {
            int imported = 0;
            using var stream = new MemoryStream();
            await file.CopyToAsync(stream);
            using var workbook = new XLWorkbook(stream);
            var worksheet = workbook.Worksheets.First();
            var rows = worksheet.RangeUsed().RowsUsed().Skip(1);
            foreach (var row in rows)
            {
                var employee = new Employee
                {
                    FullName = row.Cell(1).GetString(),
                    EmployeeCode = row.Cell(2).GetString(),
                    DepartmentId = row.Cell(3).GetValue<int?>()
                };
                _context.Employees.Add(employee);
                imported++;
            }
            await _context.SaveChangesAsync();
            return imported;
        }
    }
}
