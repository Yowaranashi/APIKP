using System;
using System.Collections.Generic;

namespace HranitelPRO.API.Contracts
{
    public class SecurityQuery
    {
        public DateTime? Date { get; set; }
        public int? DepartmentId { get; set; }
        public string? RequestType { get; set; }
        public string? Search { get; set; }
    }

    public class SecurityRequestDto
    {
        public int Id { get; set; }
        public string? DepartmentName { get; set; }
        public string RequestType { get; set; } = null!;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int VisitorCount { get; set; }
        public List<SecurityVisitorDto> Visitors { get; set; } = new();
    }

    public class SecurityVisitorDto
    {
        public int Id { get; set; }
        public string FullName { get; set; } = null!;
        public string Passport { get; set; } = null!;
    }

    public class SecurityAccessDto
    {
        public int? EmployeeId { get; set; }
        public DateTime? AccessTime { get; set; }
        public string? TurnstileId { get; set; }
    }

    public class SecurityCompleteDto
    {
        public int? EmployeeId { get; set; }
        public DateTime? ExitTime { get; set; }
        public List<int>? VisitorIds { get; set; }
    }
}
