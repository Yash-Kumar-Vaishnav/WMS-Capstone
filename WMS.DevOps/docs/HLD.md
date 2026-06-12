# High-Level Design (HLD) — Workforce Management System

## 1. System Overview
The WMS is a multi-tier web application that automates HR operations for a mid-to-large enterprise. It follows Clean Architecture principles with separation of concerns across Domain, Application, Infrastructure, and Presentation layers.

## 2. Architecture Diagram
```
┌─────────────────────────────────────────────────────────┐
│                     CLIENT LAYER                        │
│           Angular 17 SPA (Angular Material)             │
│    Auth | Dashboard | Employees | Attendance | Leaves   │
│         Departments | Projects | Announcements          │
└──────────────────────┬──────────────────────────────────┘
                       │ HTTPS + JWT Bearer
┌──────────────────────▼──────────────────────────────────┐
│                   API LAYER (WMS.API)                   │
│         ASP.NET Core 8 Web API + Swagger UI             │
│  Controllers: Employee | Attendance | Leave | Auth...   │
│         JWT Authentication Middleware + CORS            │
└──────────────────────┬──────────────────────────────────┘
                       │
┌──────────────────────▼──────────────────────────────────┐
│             APPLICATION LAYER (WMS.Application)         │
│    Services | DTOs | Interfaces | AutoMapper Profiles   │
└──────────────────────┬──────────────────────────────────┘
                       │
┌──────────────────────▼──────────────────────────────────┐
│             DOMAIN LAYER (WMS.Domain)                   │
│          Entities | BaseEntity | Interfaces             │
└──────────────────────┬──────────────────────────────────┘
                       │
┌──────────────────────▼──────────────────────────────────┐
│          INFRASTRUCTURE LAYER (WMS.Infrastructure)      │
│    EF Core DbContext | Repositories | Migrations        │
└──────────────────────┬──────────────────────────────────┘
                       │
┌──────────────────────▼──────────────────────────────────┐
│                  SQL SERVER DATABASE                    │
│     WMSDB: Employee | Attendance | Leave | Project...   │
└─────────────────────────────────────────────────────────┘
```

## 3. Module Breakdown

| Module       | Backend Controller      | Frontend Component       |
|--------------|------------------------|--------------------------|
| Auth         | AuthController          | LoginComponent           |
| Employee     | EmployeeController      | EmployeesComponent       |
| Attendance   | AttendanceController    | AttendanceComponent      |
| Leave        | LeaveController         | LeavesComponent          |
| Department   | DepartmentController    | DepartmentsComponent     |
| Project      | ProjectController       | ProjectsComponent        |
| Dashboard    | DashboardController     | DashboardComponent       |
| Announcement | AnnouncementController  | (Admin)                  |

## 4. Security Design
- **Authentication**: JWT Bearer tokens (HS256, 60-min expiry)
- **Authorization**: Role-based ([Authorize(Roles="Admin,Manager")])
- **Transport**: HTTPS enforced in production
- **CORS**: Angular origin whitelisted
- **Passwords**: BCrypt hashed (cost factor 11)
- **Secrets**: Store in Azure Key Vault / environment variables in production

## 5. Data Flow
1. User submits credentials → AuthController validates → BCrypt verify → JWT issued
2. Angular stores JWT in localStorage → HTTP Interceptor attaches to every request
3. API validates JWT on each request → role claims extracted → business logic executed
4. Repository pattern isolates DB from business logic → AutoMapper transforms entities to DTOs

## 6. Non-Functional Compliance
- **Performance**: EF Core with Include() navigation, indexed PKs — target <200ms
- **Scalability**: Stateless JWT, horizontal scaling ready
- **Auditability**: AuditLog table captures all Insert/Update/Delete actions
