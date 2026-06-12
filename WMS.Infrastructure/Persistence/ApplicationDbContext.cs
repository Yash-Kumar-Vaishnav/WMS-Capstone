using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using WMS.Domain.Entities;

namespace WMS.Infrastructure.Persistence;

public class ApplicationDbContext : DbContext
{
    private readonly IHttpContextAccessor? _httpContextAccessor;

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IHttpContextAccessor? httpContextAccessor = null)
        : base(options) 
    { 
        _httpContextAccessor = httpContextAccessor;
    }

    public DbSet<Employee>                  Employees                  { get; set; }
    public DbSet<Department>                Departments                { get; set; }
    public DbSet<Role>                      Roles                      { get; set; }
    public DbSet<Attendance>                Attendances                { get; set; }
    public DbSet<Leave>                     Leaves                     { get; set; }
    public DbSet<Announcement>              Announcements              { get; set; }
    public DbSet<Project>                   Projects                   { get; set; }
    public DbSet<Client>                    Clients                    { get; set; }
    public DbSet<EmployeeProjectAllocation> EmployeeProjectAllocations { get; set; }
    public DbSet<UserLogin>                 UserLogins                 { get; set; }
    public DbSet<AuditLog>                  AuditLogs                  { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // ── Announcement.CreatedBy → UserLogin.UserId ─────────────────────
        modelBuilder.Entity<Announcement>()
            .HasOne<UserLogin>()
            .WithMany()
            .HasForeignKey(a => a.CreatedBy)
            .OnDelete(DeleteBehavior.Restrict);

        // ── Restrict Delete Behaviors to prevent multiple cascade paths ───
        modelBuilder.Entity<Leave>()
            .HasOne(l => l.Employee)
            .WithMany(e => e.Leaves)
            .HasForeignKey(l => l.EmpId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Leave>()
            .HasOne(l => l.ApprovedByUser)
            .WithMany()
            .HasForeignKey(l => l.ApprovedBy)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<EmployeeProjectAllocation>()
            .HasOne(epa => epa.Employee)
            .WithMany(e => e.EmployeeProjectAllocations)
            .HasForeignKey(epa => epa.EmpId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<EmployeeProjectAllocation>()
            .HasOne(epa => epa.Project)
            .WithMany(p => p.EmployeeProjectAllocations)
            .HasForeignKey(epa => epa.ProjectId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Attendance>()
            .HasOne(a => a.Employee)
            .WithMany(e => e.Attendances)
            .HasForeignKey(a => a.EmpId)
            .OnDelete(DeleteBehavior.Cascade);

        // ── Seed Roles ────────────────────────────────────────────────────────
        modelBuilder.Entity<Role>().HasData(
            new Role { RoleId = 1, RoleName = "Admin",    Description = "System Administrator" },
            new Role { RoleId = 2, RoleName = "Manager",  Description = "Department Manager"   },
            new Role { RoleId = 3, RoleName = "Employee", Description = "Regular Employee"     }
        );

        // ── Seed Departments ──────────────────────────────────────────────────
        modelBuilder.Entity<Department>().HasData(
            new Department { DepartmentId = 1, DepartmentName = "IT",         Description = "Information Technology", CreatedOn = new DateTime(2026, 1, 1) },
            new Department { DepartmentId = 2, DepartmentName = "HR",         Description = "Human Resources",        CreatedOn = new DateTime(2026, 1, 1) },
            new Department { DepartmentId = 3, DepartmentName = "Finance",    Description = "Finance Department",     CreatedOn = new DateTime(2026, 1, 1) },
            new Department { DepartmentId = 4, DepartmentName = "Operations", Description = "Operations Department",  CreatedOn = new DateTime(2026, 1, 1) }
        );

        // ── Seed UserLogins  ──────────────────────────────────────────────────
        string adminHash = "$2a$11$fBzVsc515Mpa25JqOKMX4uO6.cFT4Aqho9eRmtOzsaE6dIDCcQ9ay"; // Admin@123
        string managerHash = "$2a$11$jShHrfEX3hmjHHBDWYkOuu7sRhIbvLKZk1VYVVsq8sLPK4RdEYEaW"; // Manager@123
        string employeeHash = "$2a$11$0YWVoOiAnhC5/1hSLKH1J.Jknbt6oqNKUDiagArnDHq4sQahPfXVK"; // Employee@123

        modelBuilder.Entity<UserLogin>().HasData(
            new UserLogin { UserId = 1, Username = "admin",   PasswordHash = adminHash,    RoleId = 1 },
            new UserLogin { UserId = 2, Username = "manager", PasswordHash = managerHash,  RoleId = 2 },
            new UserLogin { UserId = 3, Username = "rahul",   PasswordHash = employeeHash, RoleId = 3 },
            new UserLogin { UserId = 4, Username = "priya",   PasswordHash = employeeHash, RoleId = 3 },
            new UserLogin { UserId = 5, Username = "amit",    PasswordHash = employeeHash, RoleId = 3 }
        );

        // ── Seed Employees (Linked to UserLogin.UserId via ID matching) ────────
        modelBuilder.Entity<Employee>().HasData(
            new Employee { EmployeeId = 1, FirstName = "System", LastName = "Admin",   Email = "admin@wms.com",   PhoneNumber = "1111111111", Gender = "M", DOB = new DateTime(1980,1,1), DOJ = new DateTime(2026,1,1), DepartmentId = 1, RoleId = 1, Status = "Active", CreatedOn = new DateTime(2026,1,1) },
            new Employee { EmployeeId = 2, FirstName = "System", LastName = "Manager", Email = "manager@wms.com", PhoneNumber = "2222222222", Gender = "F", DOB = new DateTime(1985,1,1), DOJ = new DateTime(2026,1,1), DepartmentId = 2, RoleId = 2, Status = "Active", CreatedOn = new DateTime(2026,1,1) },
            new Employee { EmployeeId = 3, FirstName = "Rahul",  LastName = "Sharma",  Email = "rahul@wms.com",   PhoneNumber = "3333333333", Gender = "M", DOB = new DateTime(1990,1,1), DOJ = new DateTime(2026,1,1), DepartmentId = 1, RoleId = 3, Status = "Active", CreatedOn = new DateTime(2026,1,1) },
            new Employee { EmployeeId = 4, FirstName = "Priya",  LastName = "Singh",   Email = "priya@wms.com",   PhoneNumber = "4444444444", Gender = "F", DOB = new DateTime(1992,1,1), DOJ = new DateTime(2026,1,1), DepartmentId = 3, RoleId = 3, Status = "Active", CreatedOn = new DateTime(2026,1,1) },
            new Employee { EmployeeId = 5, FirstName = "Amit",   LastName = "Kumar",   Email = "amit@wms.com",    PhoneNumber = "5555555555", Gender = "M", DOB = new DateTime(1994,1,1), DOJ = new DateTime(2026,1,1), DepartmentId = 4, RoleId = 3, Status = "Active", CreatedOn = new DateTime(2026,1,1) }
        );

        // ── Seed Clients ──────────────────────────────────────────────────────
        modelBuilder.Entity<Client>().HasData(
            new Client { ClientId = 1, ClientName = "TechCorp",      ClientLocation = "New York", ClientPhoneNumber = 9999999991, ClientAddress = "123 Tech Lane", Status = true },
            new Client { ClientId = 2, ClientName = "GlobalFinance", ClientLocation = "London",   ClientPhoneNumber = 9999999992, ClientAddress = "456 Finance Blvd", Status = true }
        );

        // ── Seed Projects ─────────────────────────────────────────────────────
        modelBuilder.Entity<Project>().HasData(
            new Project { ProjectId = 1, ProjectName = "WMS Project",       ClientId = 1, StartDate = new DateTime(2026,1,1), EndDate = new DateTime(2026,12,31), Status = "Active" },
            new Project { ProjectId = 2, ProjectName = "HR Portal",         ClientId = 1, StartDate = new DateTime(2026,1,1), EndDate = new DateTime(2026,6,30),  Status = "Inactive" },
            new Project { ProjectId = 3, ProjectName = "Finance Dashboard", ClientId = 2, StartDate = new DateTime(2026,3,1), EndDate = new DateTime(2026,9,30),  Status = "Active" }
        );

        // ── Seed Allocations ──────────────────────────────────────────────────
        modelBuilder.Entity<EmployeeProjectAllocation>().HasData(
            new EmployeeProjectAllocation { AllocationId = 1, EmpId = 3, ProjectId = 1, AssignedOn = new DateTime(2026,1,1), CreateDate = new DateTime(2026,1,1), CreatedBy = "Admin", Status = true },
            new EmployeeProjectAllocation { AllocationId = 2, EmpId = 4, ProjectId = 2, AssignedOn = new DateTime(2026,1,1), CreateDate = new DateTime(2026,1,1), CreatedBy = "Admin", Status = false },
            new EmployeeProjectAllocation { AllocationId = 3, EmpId = 5, ProjectId = 3, AssignedOn = new DateTime(2026,1,1), CreateDate = new DateTime(2026,1,1), CreatedBy = "Admin", Status = true }
        );

        // ── Seed Leaves ───────────────────────────────────────────────────────
        modelBuilder.Entity<Leave>().HasData(
            new Leave { LeaveId = 1, EmpId = 3, LeaveType = "Sick",     FromDate = DateTime.Today.AddDays(1), ToDate = DateTime.Today.AddDays(2), Reason = "Fever",       Status = "Pending",  AppliedOn = DateTime.UtcNow },
            new Leave { LeaveId = 2, EmpId = 4, LeaveType = "Vacation", FromDate = DateTime.Today.AddDays(7), ToDate = DateTime.Today.AddDays(9), Reason = "Trip",        Status = "Approved", AppliedOn = DateTime.UtcNow, ApprovedBy = 2, ApprovedOn = DateTime.UtcNow },
            new Leave { LeaveId = 3, EmpId = 5, LeaveType = "Casual",   FromDate = DateTime.Today.AddDays(3), ToDate = DateTime.Today.AddDays(3), Reason = "Family event",Status = "Rejected", AppliedOn = DateTime.UtcNow, ApprovedBy = 2, ApprovedOn = DateTime.UtcNow }
        );

        // ── Seed Attendances ──────────────────────────────────────────────────
        var attendances = new List<Attendance>();
        int attendanceId = 1;
        for (int i = 0; i < 5; i++)
        {
            var date = DateTime.Today.AddDays(-i);
            if (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday) continue;

            attendances.Add(new Attendance { AttendanceId = attendanceId++, EmpId = 3, AttendanceDate = date, CheckIn = date.AddHours(9), CheckOut = date.AddHours(17), TotalHours = 8, WorkMode = "Office" });
            attendances.Add(new Attendance { AttendanceId = attendanceId++, EmpId = 4, AttendanceDate = date, CheckIn = date.AddHours(9), CheckOut = date.AddHours(17), TotalHours = 8, WorkMode = "Remote" });
            attendances.Add(new Attendance { AttendanceId = attendanceId++, EmpId = 5, AttendanceDate = date, CheckIn = date.AddHours(9), CheckOut = date.AddHours(17), TotalHours = 8, WorkMode = "Office" });
        }
        modelBuilder.Entity<Attendance>().HasData(attendances);

        // ── Seed Announcements ────────────────────────────────────────────────
        modelBuilder.Entity<Announcement>().HasData(
            new Announcement { AnnouncementId = 1, Title = "Welcome to WMS", Message = "Welcome to the new Workforce Management System!", CreatedBy = 1, IsActive = true, CreatedOn = DateTime.UtcNow },
            new Announcement { AnnouncementId = 2, Title = "Legacy Shutdown", Message = "The legacy system will be shut down next week.", CreatedBy = 1, IsActive = false, CreatedOn = DateTime.UtcNow.AddDays(-10) }
        );

        // ── Seed AuditLogs ────────────────────────────────────────────────────
        modelBuilder.Entity<AuditLog>().HasData(
            new AuditLog { AuditId = 1, EntityName = "Employee", Action = "Added",    RecordId = 3, CreatedBy = 1, CreatedOn = DateTime.UtcNow.AddDays(-5) },
            new AuditLog { AuditId = 2, EntityName = "Leave",    Action = "Modified", RecordId = 2, CreatedBy = 2, CreatedOn = DateTime.UtcNow.AddDays(-2) },
            new AuditLog { AuditId = 3, EntityName = "EmployeeProjectAllocation", Action = "Added", RecordId = 1, CreatedBy = 1, CreatedOn = DateTime.UtcNow.AddDays(-4) }
        );
    }

    public override int SaveChanges()
    {
        OnBeforeSaveChanges();
        return base.SaveChanges();
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        OnBeforeSaveChanges();
        return base.SaveChangesAsync(cancellationToken);
    }

    private void OnBeforeSaveChanges()
    {
        ChangeTracker.DetectChanges();
        var auditEntries = new List<AuditLog>();
        
        int userId = 0;
        if (_httpContextAccessor?.HttpContext?.User?.Identity?.IsAuthenticated == true)
        {
            var idClaim = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (int.TryParse(idClaim, out int parsedId))
            {
                userId = parsedId;
            }
        }

        foreach (var entry in ChangeTracker.Entries())
        {
            if (entry.Entity is AuditLog || entry.State == EntityState.Detached || entry.State == EntityState.Unchanged)
                continue;

            var auditLog = new AuditLog
            {
                EntityName = entry.Entity.GetType().Name,
                Action = entry.State.ToString(),
                CreatedBy = userId,
                CreatedOn = DateTime.UtcNow
            };

            // Attempt to get the RecordId if it has a property like "Id" or EntityName + "Id"
            var primaryKey = entry.Properties.FirstOrDefault(p => p.Metadata.IsPrimaryKey());
            if (primaryKey != null && primaryKey.CurrentValue is int pkValue)
            {
                auditLog.RecordId = pkValue;
            }

            auditEntries.Add(auditLog);
        }

        if (auditEntries.Any())
        {
            AuditLogs.AddRange(auditEntries);
        }
    }
}
