# Low-Level Design (LLD) — Workforce Management System

## 1. Database Schema

### Employee Table
| Column       | Type         | Constraints                    |
|--------------|--------------|-------------------------------|
| EmployeeId   | INT          | PK, Identity                  |
| FirstName    | VARCHAR(50)  | NOT NULL                      |
| LastName     | VARCHAR(50)  | NOT NULL                      |
| Email        | VARCHAR(80)  | UNIQUE, NOT NULL               |
| PhoneNumber  | VARCHAR(15)  | NOT NULL                      |
| Gender       | CHAR(1)      | CHECK (M/F/O)                 |
| DOB          | DATE         | NOT NULL                      |
| DOJ          | DATE         | NOT NULL                      |
| DepartmentId | INT          | FK → Department                |
| RoleId       | INT          | FK → Role                     |
| Status       | VARCHAR(20)  | DEFAULT 'Active'              |
| CreatedOn    | DATETIME     | DEFAULT GETDATE()             |
| UpdatedOn    | DATETIME     | NULL                          |

### Attendance Table
| Column         | Type        | Constraints                |
|----------------|-------------|---------------------------|
| AttendanceId   | INT         | PK, Identity              |
| EmpId          | INT         | FK → Employee             |
| CheckIn        | DATETIME    | NOT NULL                  |
| CheckOut       | DATETIME    | NULL                      |
| TotalHours     | FLOAT       | Computed                  |
| WorkMode       | VARCHAR(20) | WFO/WFH/Hybrid            |
| AttendanceDate | DATE        | NOT NULL                  |

### Leave Table
| Column     | Type        | Constraints              |
|------------|-------------|--------------------------|
| LeaveId    | INT         | PK, Identity             |
| EmpId      | INT         | FK → Employee            |
| LeaveType  | VARCHAR(30) | Sick/Casual/Earned       |
| Reason     | VARCHAR(255)| NULL                     |
| FromDate   | DATE        | NOT NULL                 |
| ToDate     | DATE        | NOT NULL                 |
| Status     | VARCHAR(20) | DEFAULT 'Pending'        |
| AppliedOn  | DATETIME    | DEFAULT GETDATE()        |
| ApprovedBy | INT         | NULL, FK → Employee      |
| ApprovedOn | DATETIME    | NULL                     |

## 2. API Endpoints

### Authentication
- `POST /api/auth/login` → LoginDto → LoginResponseDto (JWT)

### Employee
- `GET /api/employee` → List<EmployeeDto>
- `GET /api/employee/{id}` → EmployeeDto
- `GET /api/employee/search?name=&departmentId=&roleId=&status=` → List<EmployeeDto>
- `POST /api/employee` → CreateEmployeeDto → 201
- `PUT /api/employee` → UpdateEmployeeDto → 200
- `DELETE /api/employee/{id}` → 200 [Admin only]

### Attendance
- `GET /api/attendance` → List<AttendanceDto>
- `GET /api/attendance/employee/{empId}` → List<AttendanceDto>
- `GET /api/attendance/monthly/{empId}/{year}/{month}` → List<AttendanceDto>
- `POST /api/attendance/checkin` → CheckInDto → {AttendanceId}
- `PUT /api/attendance/checkout/{id}` → CheckOutDto → 200
- `POST /api/attendance` → CreateAttendanceDto → 200
- `PUT /api/attendance` → UpdateAttendanceDto → 200
- `DELETE /api/attendance/{id}` → 200

### Leave
- `GET /api/leave` → List<LeaveDto>
- `GET /api/leave/{id}` → LeaveDto
- `GET /api/leave/employee/{empId}` → List<LeaveDto>
- `POST /api/leave` → CreateLeaveDto → LeaveId
- `PUT /api/leave` → UpdateLeaveDto → 200
- `PUT /api/leave/{id}/approve` [Admin/Manager] → 200
- `PUT /api/leave/{id}/reject` [Admin/Manager] → 200
- `PUT /api/leave/{id}/cancel` → 200
- `DELETE /api/leave/{id}` → 200

### Dashboard
- `GET /api/dashboard/summary` → {totalEmployees, todayAttendanceCount, pendingLeaveRequests, activeProjects, totalDepartments}
- `GET /api/dashboard/leave-stats` → [{Status, Count}]
- `GET /api/dashboard/attendance-chart` → [{Date, Count}]
- `GET /api/dashboard/project-counts` → {Active, Completed}

## 3. Service Layer Patterns

### Repository Pattern
```
IGenericRepository<T> (base CRUD)
  └── IEmployeeRepository : SearchAsync()
  └── IAttendanceRepository : GetByEmployeeAsync(), GetMonthlyAsync(), GetOpenCheckInAsync()
  └── ILeaveRepository : GetByEmployeeAsync()
  └── IDepartmentRepository
  └── IProjectRepository
```

### Service Layer
Each service wraps a repository + AutoMapper:
- GetAllAsync / GetByIdAsync
- CreateAsync (returns new ID)
- UpdateAsync / DeleteAsync (returns bool)
- Domain-specific methods (ApproveAsync, CheckInAsync, etc.)

## 4. JWT Token Structure
```json
{
  "sub": "<UserId>",
  "name": "<Username>",
  "role": "<Admin|Manager|Employee>",
  "iss": "WMS.API",
  "aud": "WMS.Client",
  "exp": <Unix timestamp>
}
```

## 5. Angular Architecture
```
app/
├── auth/               login.component.ts
├── dashboard/          dashboard.component.ts (Chart.js charts)
├── employees/          employees.component.ts + employee-form-dialog
├── attendance/         attendance.component.ts + attendance-form-dialog
├── leaves/             leaves.component.ts + leave-form-dialog
├── departments/        departments.component.ts + department-form-dialog
├── projects/           projects.component.ts + project-form-dialog
├── layout/             shell.component.ts (sidenav + toolbar)
└── shared/
    ├── guards/         auth.guard.ts (isLoggedIn check)
    ├── interceptors/   auth.interceptor.ts (JWT header injection)
    ├── models/         models.ts (all TypeScript interfaces)
    └── services/       one service per module (HttpClient)
```

## 6. Seed Data
- Roles: Admin (1), Manager (2), Employee (3)
- Departments: IT (1), HR (2), Finance (3), Operations (4)
- Admin user: username=admin, password=Admin@123 (BCrypt hashed)
