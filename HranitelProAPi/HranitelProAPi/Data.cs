using HranitelPRO.API.Models;
using Microsoft.EntityFrameworkCore;

namespace HranitelPro.API.Data
{
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
                    "Server=127.0.0.1;Database=HranitelPRO;User Id=sa;Password=YSPASS!Gaqt4;TrustServerCertificate=True;");
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

            modelBuilder.Entity<PassRequest>()
                .HasOne(p => p.ApplicantUser)
                .WithMany(u => u.PassRequests)
                .HasForeignKey(p => p.ApplicantUserId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<PassRequest>()
                .HasOne(p => p.CheckedByUser)
                .WithMany()
                .HasForeignKey(p => p.CheckedByUserId)
                .OnDelete(DeleteBehavior.SetNull);

            // PassVisitor -> PassRequest
            modelBuilder.Entity<PassVisitor>()
                .HasOne(v => v.PassRequest)
                .WithMany(p => p.Visitors)
                .HasForeignKey(v => v.PassRequestId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<PassVisitor>()
                .HasOne(v => v.Group)
                .WithMany(g => g.Visitors)
                .HasForeignKey(v => v.GroupId)
                .OnDelete(DeleteBehavior.SetNull);

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

            modelBuilder.Entity<FileAttachment>()
                .HasOne(f => f.Department)
                .WithMany(d => d.Attachments)
                .HasForeignKey(f => f.DepartmentId)
                .OnDelete(DeleteBehavior.Cascade);

            // BlacklistEntry added by employee
            modelBuilder.Entity<BlacklistEntry>()
                .HasOne(b => b.AddedByEmployee)
                .WithMany()
                .HasForeignKey(b => b.AddedByEmployeeId)
                .OnDelete(DeleteBehavior.SetNull);

            // Employee - Department (match SQL schema foreign key behavior)
            modelBuilder.Entity<Employee>()
                .HasOne(e => e.Department)
                .WithMany(d => d.Employees)
                .HasForeignKey(e => e.DepartmentId)
                .OnDelete(DeleteBehavior.Restrict);

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

            modelBuilder.Entity<PassRequest>()
                .HasOne(p => p.StatusRef)
                .WithMany()
                .HasForeignKey(p => p.StatusID)
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
