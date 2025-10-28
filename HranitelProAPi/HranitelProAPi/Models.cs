using DocumentFormat.OpenXml.InkML;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection.Emit;

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

        [MaxLength(10)]
        public string PassportSeries { get; set; } = null!;
        [MaxLength(10)]
        public string PassportNumber { get; set; } = null!;

        // optional path to photo file (jpg)
        [MaxLength(500)]
        public string? PhotoPath { get; set; }

        // attachments specific to visitor (scan passport)
        public ICollection<FileAttachment>? Attachments { get; set; }

        public bool IsBlacklisted { get; set; } = false;
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

        // by whom added (employee)
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

        public int? VisitorId { get; set; } // optional link to PassVisitor
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
        public string ReportType { get; set; } = null!; // e.g. "DailyVisits", "OccupancySnapshot"

        public DateTime PeriodStart { get; set; }
        public DateTime PeriodEnd { get; set; }

        // путь до файла PDF
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

        public int FileTypeID { get; set; }         // <== добавить
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

        public int? DepartmentId { get; set; } // или для подразделения
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

    #region DbContext

    public class HranitelContext : DbContext
    {
        public HranitelContext(DbContextOptions<HranitelContext> options) : base(options) { }

        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Role> Roles { get; set; } = null!;
        public DbSet<Department> Departments { get; set; } = null!;
        public DbSet<PassRequest> PassRequests { get; set; } = null!;
        public DbSet<PassVisitor> PassVisitors { get; set; } = null!;
        public DbSet<BlacklistEntry> BlacklistEntries { get; set; } = null!;
        public DbSet<Employee> Employees { get; set; } = null!;
        public DbSet<SecurityLog> SecurityLogs { get; set; } = null!;
        public DbSet<VisitRecord> VisitRecords { get; set; } = null!;
        public DbSet<Report> Reports { get; set; } = null!;
        public DbSet<Notification> Notifications { get; set; } = null!;
        public DbSet<FileAttachment> FileAttachments { get; set; } = null!;
        public DbSet<PolicyRule> PolicyRules { get; set; } = null!;
        public DbSet<SecurityIncident> SecurityIncidents { get; set; } = null!;
        public DbSet<WorkingTimeRule> WorkingTimeRules { get; set; } = null!;
        public DbSet<AccessEvent> AccessEvents { get; set; } = null!;
        public DbSet<AuditLog> AuditLogs { get; set; } = null!;
        public DbSet<ImportTemplate> ImportTemplates { get; set; } = null!;
        public DbSet<SystemSetting> SystemSettings { get; set; } = null!;
        public DbSet<ApplicationStatus> ApplicationStatuses { get; set; } = null!;
        public DbSet<FileType> FileTypes { get; set; } = null!;
        public DbSet<ViolationReason> ViolationReasons { get; set; } = null!;
        public DbSet<Group> Groups { get; set; } = null!;
        public DbSet<AccessToken> AccessTokens { get; set; } = null!;
        public DbSet<WorkSchedule> WorkSchedules { get; set; } = null!;
        public DbSet<FreeTimeSlot> FreeTimeSlots { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(
                    "Server=localhost;Database=HranitelPRO;User Id=sa;Password=YSPASS!Gaqt4;TrustServerCertificate=True;");
            }
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<PassRequest>()
                .HasIndex(p => p.Status);

            modelBuilder.Entity<PassRequest>()
                .HasOne(p => p.Department)
                .WithMany(d => d.PassRequests)
                .HasForeignKey(p => p.DepartmentId)
                .OnDelete(DeleteBehavior.Restrict);

            // PassVisitor -> PassRequest
            modelBuilder.Entity<PassVisitor>()
                .HasOne(v => v.PassRequest)
                .WithMany(p => p.Visitors)
                .HasForeignKey(v => v.PassRequestId)
                .OnDelete(DeleteBehavior.Cascade);

            // FileAttachment relation constraints: either PassRequestId or PassVisitorId
            modelBuilder.Entity<FileAttachment>()
                .HasOne(f => f.PassRequest)
                .WithMany(p => p.Attachments)
                .HasForeignKey(f => f.PassRequestId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<FileAttachment>()
                .HasOne(f => f.PassVisitor)
                .WithMany(v => v.Attachments)
                .HasForeignKey(f => f.PassVisitorId)
                .OnDelete(DeleteBehavior.Cascade);

            // BlacklistEntry added by employee
            modelBuilder.Entity<BlacklistEntry>()
                .HasOne(b => b.AddedByEmployee)
                .WithMany()
                .HasForeignKey(b => b.AddedByEmployeeId)
                .OnDelete(DeleteBehavior.SetNull);

            // Employee - Department
            modelBuilder.Entity<Employee>()
                .HasOne(e => e.Department)
                .WithMany(d => d.Employees)
                .HasForeignKey(e => e.DepartmentId)
                .OnDelete(DeleteBehavior.SetNull);

            // VisitRecord relationships
            modelBuilder.Entity<VisitRecord>()
                .HasOne(vr => vr.PassRequest)
                .WithMany(p => p.VisitRecords)
                .HasForeignKey(vr => vr.PassRequestId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<VisitRecord>()
                .HasOne(vr => vr.Department)
                .WithMany()
                .HasForeignKey(vr => vr.DepartmentId)
                .OnDelete(DeleteBehavior.SetNull);

            // AuditLog -> User
            modelBuilder.Entity<AuditLog>()
                .HasOne(a => a.User)
                .WithMany(u => u.AuditLogs)
                .HasForeignKey(a => a.UserId)
                .OnDelete(DeleteBehavior.SetNull);

            // Notification -> User
            modelBuilder.Entity<Notification>()
                .HasOne(n => n.User)
                .WithMany(u => u.Notifications)
                .HasForeignKey(n => n.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // SecurityLog relationships
            modelBuilder.Entity<SecurityLog>()
                .HasOne(s => s.Employee)
                .WithMany()
                .HasForeignKey(s => s.EmployeeId)
                .OnDelete(DeleteBehavior.SetNull);

            // AccessEvent -> Employee, PassRequest
            modelBuilder.Entity<AccessEvent>()
                .HasOne(a => a.Employee)
                .WithMany()
                .HasForeignKey(a => a.EmployeeId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<AccessEvent>()
                .HasOne(a => a.PassRequest)
                .WithMany()
                .HasForeignKey(a => a.PassRequestId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Employee>()
                .HasOne(e => e.Department)
                .WithMany()
                .HasForeignKey(e => e.DepartmentId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<PassRequest>()
                .HasOne<ApplicationStatus>()
                .WithMany()
                .HasForeignKey("StatusID")
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<FileAttachment>()
                .HasOne<FileType>()
                .WithMany()
                .HasForeignKey("FileTypeID")
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<BlacklistEntry>()
                .HasOne<ViolationReason>()
                .WithMany()
                .HasForeignKey("ReasonID")
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<AccessToken>()
                .HasOne(a => a.User)
                .WithMany()
                .HasForeignKey(a => a.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Связь AccessToken → Employee
            modelBuilder.Entity<AccessToken>()
                .HasOne(a => a.Employee)
                .WithMany()
                .HasForeignKey(a => a.EmployeeId)
                .OnDelete(DeleteBehavior.Cascade);

            // Role seed (optional) - можно раскомментировать и использовать при миграциях
            // modelBuilder.Entity<Role>().HasData(
            //     new Role { Id = 1, Name = "Guest" },
            //     new Role { Id = 2, Name = "Security" },
            //     new Role { Id = 3, Name = "CommonDept" },
            //     new Role { Id = 4, Name = "Division" },
            //     new Role { Id = 5, Name = "Administrator" }
            // );
        }
    }

    #endregion
}
