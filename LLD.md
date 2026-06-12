# Low-Level Design (LLD) - Workforce Management System

## 1. Design Patterns
- **Repository Pattern**: Abstracted database access (`IGenericRepository<T>`).
- **Dependency Injection**: Registered Scoped lifetimes for all Services and Repositories.
- **DTO Pattern**: AutoMapper profiles enforce strict decoupling between Domain Entities and API Responses.

## 2. Database Schema Details
- **Employee**: Linked 1:1 with `UserLogin` and N:1 with `Department`.
- **Attendance**: N:1 with `Employee`. `TotalHours` bounded explicitly at Checkout.
- **Leave**: Enforces Date boundaries (`ToDate` >= `FromDate`).
- **AuditLogs**: Generates automatically upon `SaveChangesAsync()` via `ChangeTracker`.

## 3. API Layer
- Fully segregated into Controllers: `AuthController`, `EmployeeController`, `DashboardController`, etc.
- Gated using `[Authorize(Roles="...")]`.

## 4. Frontend Layer
- **Services**: Inject `HttpClient` and `AuthService` to fetch Bearer tokens automatically via `JwtInterceptor`.
- **Components**: Employ `MatTable` for structured data, reacting to state changes.
- **Guards**: `AuthGuard` and `RoleGuard` prevent UI state leakage.
