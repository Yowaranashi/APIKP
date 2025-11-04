using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HranitelPRO.API.Models
{
    #region Entities

    public class User
    {
        [Key]
        public int Id { get; set; }
        [Required, MaxLength(200)]
        public string FullName { get; set; } = null!;
        [Required, MaxLength(200)]
        public string Email { get; set; } = null!;
        [Required, MaxLength(128)]
        public string PasswordHash { get; set; } = null!; 
        public bool EmailConfirmed { get; set; } = false;
        public int? EmployeeId { get; set; }
        public Employee? Employee { get; set; }
        public int RoleId { get; set; }
        public Role Role { get; set; } = null!;
        public ICollection<PassRequest>? PassRequests { get; set; }
        public ICollection<Notification>? Notifications { get; set; }
        public ICollection<AuditLog>? AuditLogs { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }

    public class Role
    {
        [Key]
        public int Id { get; set; }
        [Required, MaxLength(100)]
        public string Name { get; set; } = null!; // Guest, Security, CommonDept, Division, Admin
        public string? Description { get; set; }
        public ICollection<User>? Users { get; set; }
    }

    public class Department
    {
        [Key]
        public int Id { get; set; }
        [Required, MaxLength(200)]
        public string Name { get; set; } = null!;
        [MaxLength(1000)]
        public string? Description { get; set; }
        public ICollection<Employee>? Employees { get; set; }
        public ICollection<PassRequest>? PassRequests { get; set; }
        public ICollection<FileAttachment>? Attachments { get; set; }
    }

    public class PassRequest
    {
        [Key]
        public int Id { get; set; }
        // тип заявки: личная или групповая
        [Required, MaxLength(50)]
        public string RequestType { get; set; } = "Personal"; // "Personal" or "Group"
        [Required]
        public DateTime StartDate { get; set; } // начало действия заявки (дата+время)
        [Required]
        public DateTime EndDate { get; set; } // конец действия заявки
        [Required, MaxLength(1000)]
        public string Purpose { get; set; } = null!;
        public int DepartmentId { get; set; }
        public Department Department { get; set; } = null!;
        public int? ResponsibleEmployeeId { get; set; } 
        public Employee? ResponsibleEmployee { get; set; }
        public int? ApplicantUserId { get; set; }
        public User? ApplicantUser { get; set; }
        [MaxLength(100)]
        public string? ApplicantLastName { get; set; }
        [MaxLength(100)]
        public string? ApplicantFirstName { get; set; }
        [MaxLength(100)]
        public string? ApplicantMiddleName { get; set; }
        [MaxLength(20)]
        public string? Phone { get; set; }
        [MaxLength(200)]
        public string? ApplicantEmail { get; set; }
        [MaxLength(200)]
        public string? Organization { get; set; }
        [MaxLength(4000)]
        public string? Note { get; set; }
        public DateTime? BirthDate { get; set; }
        [MaxLength(10)]
        public string? PassportSeries { get; set; } // 4 chars
        [MaxLength(10)]
        public string? PassportNumber { get; set; } // 6 chars
        [Required, MaxLength(50)]
        public string Status { get; set; } = "Pending"; // Pending, Approved, Rejected, Checked
        [MaxLength(2000)]
        public string? RejectionReason { get; set; }
        public int? GroupSize { get; set; }
        public ICollection<PassVisitor>? Visitors { get; set; }
        public ICollection<FileAttachment>? Attachments { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public int? CheckedByUserId { get; set; }
        public User? CheckedByUser { get; set; }
        public ICollection<VisitRecord>? VisitRecords { get; set; }
        [Required]
        public int StatusID { get; set; }
        public ApplicationStatus StatusRef { get; set; } = null!;
    }

    public class PassVisitor
    {
        [Key]
        public int Id { get; set; }
        public int PassRequestId { get; set; }
        public PassRequest PassRequest { get; set; } = null!;
        [Required, MaxLength(100)]
        public string LastName { get; set; } = null!;
        [Required, MaxLength(100)]
        public string FirstName { get; set; } = null!;
        [MaxLength(100)]
        public string? MiddleName { get; set; }
        [MaxLength(20)]
        public string? Phone { get; set; }
        [MaxLength(200)]
        public string? Email { get; set; }
        public DateTime? BirthDate { get; set; }
        [MaxLength(10)]
        public string PassportSeries { get; set; } = null!;
        [MaxLength(10)]
        public string PassportNumber { get; set; } = null!;
        [MaxLength(500)]
        public string? PhotoPath { get; set; }
        public ICollection<FileAttachment>? Attachments { get; set; }
        public bool IsBlacklisted { get; set; } = false;
        public int? GroupId { get; set; }
        public Group? Group { get; set; }
    }

    public class BlacklistEntry
    {
        [Key]
        public int Id { get; set; }
        [Required, MaxLength(100)]
        public string LastName { get; set; } = null!;
        [Required, MaxLength(100)]
        public string FirstName { get; set; } = null!;
        [MaxLength(100)]
        public string? MiddleName { get; set; }
        [MaxLength(10)]
        public string? PassportSeries { get; set; }
        [MaxLength(10)]
        public string? PassportNumber { get; set; }
        [MaxLength(5000)]
        public string Reason { get; set; } = null!;
        public DateTime AddedAt { get; set; } = DateTime.UtcNow;
        public int? AddedByEmployeeId { get; set; }
        public Employee? AddedByEmployee { get; set; }
        public int? ReasonID { get; set; } 
        public ViolationReason? ViolationReason { get; set; }
    }

    public class Employee
    {
        [Key]
        public int Id { get; set; }
        [Required, MaxLength(200)]
        public string FullName { get; set; } = null!;
        [MaxLength(50)]
        public string? EmployeeCode { get; set; } // код входа
        public int? DepartmentId { get; set; }
        public Department? Department { get; set; }
        public ICollection<User>? Users { get; set; }
    }

    public class SecurityLog
    {
        [Key]
        public int Id { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
        [Required, MaxLength(100)]
        public string EventType { get; set; } = null!; // Enter, Exit, AllowPass, TurnstileOpen
        public int? EmployeeId { get; set; } // сотрудник охраны
        public Employee? Employee { get; set; }
        public int? PassRequestId { get; set; }
        public PassRequest? PassRequest { get; set; }
        [MaxLength(2000)]
        public string? Details { get; set; }
    }

    public class VisitRecord
    {
        [Key]
        public int Id { get; set; }
        public int PassRequestId { get; set; }
        public PassRequest PassRequest { get; set; } = null!;
        public int? VisitorId { get; set; } 
        public PassVisitor? Visitor { get; set; }
        public DateTime? EntryTime { get; set; }
        public DateTime? ExitTime { get; set; }
        public int? DepartmentId { get; set; } // куда направлялся
        public Department? Department { get; set; }
        public bool IsOnPremisesNow => EntryTime != null && (ExitTime == null || ExitTime > EntryTime);
    }

    public class Report
    {
        [Key]
        public int Id { get; set; }
        [Required, MaxLength(200)]
        public string ReportType { get; set; } = null!; 
        public DateTime PeriodStart { get; set; }
        public DateTime PeriodEnd { get; set; }
        [MaxLength(500)]
        public string? PdfFilePath { get; set; }
        public DateTime GeneratedAt { get; set; } = DateTime.UtcNow;
    }

    public class Notification
    {
        [Key]
        public int Id { get; set; }
        public int? UserId { get; set; }
        public User? User { get; set; }
        [Required, MaxLength(200)]
        public string Title { get; set; } = null!;
        [Required]
        public string Body { get; set; } = null!;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public bool IsRead { get; set; } = false;
    }

    public class FileAttachment
    {
        [Key]
        public int Id { get; set; }
        public int? PassRequestId { get; set; }
        public PassRequest? PassRequest { get; set; }
        public int? PassVisitorId { get; set; }
        public PassVisitor? PassVisitor { get; set; }
        public int? DepartmentId { get; set; }
        public Department? Department { get; set; }
        public int FileTypeID { get; set; }
        public FileType FileType { get; set; } = null!;
        [Required, MaxLength(500)]
        public string FilePath { get; set; } = null!;
        [MaxLength(100)]
        public string FileName { get; set; } = null!;
        [MaxLength(50)]
        public string ContentType { get; set; } = null!;
        public long FileSize { get; set; }
        public DateTime UploadedAt { get; set; } = DateTime.UtcNow;
    }

    public class PolicyRule
    {
        [Key]
        public int Id { get; set; }
        [Required, MaxLength(200)]
        public string Name { get; set; } = null!;
        [MaxLength(4000)]
        public string? Description { get; set; }
    }

    public class SecurityIncident
    {
        [Key]
        public int Id { get; set; }
        public DateTime OccurredAt { get; set; } = DateTime.UtcNow;
        [Required, MaxLength(500)]
        public string Title { get; set; } = null!;
        [MaxLength(4000)]
        public string? Details { get; set; }
        public int? DetectedByEmployeeId { get; set; }
        public Employee? DetectedByEmployee { get; set; }
        public int? RelatedVisitRecordId { get; set; }
        public VisitRecord? RelatedVisitRecord { get; set; }
    }

    public class WorkingTimeRule
    {
        [Key]
        public int Id { get; set; }
        public int? EmployeeId { get; set; } // если правило для сотрудника конкретного
        public Employee? Employee { get; set; }
        public int? DepartmentId { get; set; }
        public Department? Department { get; set; }
        [Required]
        public TimeSpan WorkStart { get; set; }
        [Required]
        public TimeSpan WorkEnd { get; set; }
        [MaxLength(200)]
        public string? Note { get; set; }
    }

    public class AccessEvent
    {
        [Key]
        public int Id { get; set; }
        public DateTime EventTime { get; set; } = DateTime.UtcNow;
        [MaxLength(100)]
        public string EventType { get; set; } = "Turnstile"; // Turnstile, Gate, CardSwipe
        public int? EmployeeId { get; set; } // сотрудник, зафиксировавший событие
        public Employee? Employee { get; set; }
        public int? PassRequestId { get; set; }
        public PassRequest? PassRequest { get; set; }
        [MaxLength(500)]
        public string? Metadata { get; set; } // json or details (QR code, device id)
    }

    public class AuditLog
    {
        [Key]
        public int Id { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
        public int? UserId { get; set; } // кто выполнил действие
        public User? User { get; set; }
        [Required, MaxLength(200)]
        public string Action { get; set; } = null!;
        [MaxLength(4000)]
        public string? Details { get; set; }
    }

    public class ImportTemplate
    {
        [Key]
        public int Id { get; set; }
        [Required, MaxLength(200)]
        public string TemplateName { get; set; } = null!;
        [Required]
        public string Description { get; set; } = null!; // структура / колонки
        [MaxLength(500)]
        public string? ExampleFilePath { get; set; } // optional sample xlsx path
    }

    public class SystemSetting
    {
        [Key]
        public int Id { get; set; }
        [Required, MaxLength(200)]
        public string Key { get; set; } = null!;
        [Required]
        public string Value { get; set; } = null!;
        [MaxLength(1000)]
        public string? Description { get; set; }
    }

    public class ApplicationStatus
    {
        [Key]
        public int StatusID { get; set; }
        [Required, MaxLength(50)]
        public string StatusName { get; set; } = null!;
    }

    public class FileType
    {
        [Key]
        public int FileTypeID { get; set; }
        [Required, MaxLength(100)]
        public string TypeName { get; set; } = null!;
    }

    public class ViolationReason
    {
        [Key]
        public int ReasonID { get; set; }
        [Required]
        public string ReasonText { get; set; } = null!;
    }

    public class Group
    {
        [Key]
        public int GroupID { get; set; }
        [MaxLength(200)]
        public string? GroupName { get; set; }
        [MaxLength(500)]
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public ICollection<PassVisitor>? Visitors { get; set; }
    }

    public class AccessToken
    {
        [Key]
        public int TokenID { get; set; }
        [Required, MaxLength(255)]
        public string TokenValue { get; set; } = null!;
        public int? UserId { get; set; }
        public User? User { get; set; }
        public int? EmployeeId { get; set; }
        public Employee? Employee { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? ExpiresAt { get; set; }
    }

    public class WorkSchedule
    {
        [Key]
        public int ScheduleID { get; set; }
        public int EmployeeID { get; set; }
        public Employee Employee { get; set; } = null!;
        public DateTime WorkDate { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
    }

    public class FreeTimeSlot
    {
        [Key]
        public int SlotID { get; set; }
        public int EmployeeID { get; set; }
        public Employee Employee { get; set; } = null!;
        [MaxLength(50)]
        public string SlotRange { get; set; } = null!;
        public DateTime CalculatedAt { get; set; } = DateTime.UtcNow;
    }

    #endregion
}
