# WMS Test Cases

## Module 1: Authentication

| TC# | Test Case                          | Input                              | Expected Output              | Status |
|-----|------------------------------------|------------------------------------|------------------------------|--------|
| TC1 | Valid login                        | admin / Admin@123                  | 200 + JWT token              | PASS   |
| TC2 | Invalid password                   | admin / wrong                      | 401 Unauthorized             | PASS   |
| TC3 | Non-existent user                  | unknown / pass                     | 401 Unauthorized             | PASS   |
| TC4 | Missing fields                     | {} (empty body)                    | 400 Bad Request              | PASS   |
| TC5 | Access protected route without JWT | GET /api/employee (no token)       | 401 Unauthorized             | PASS   |
| TC6 | Access protected route with JWT    | GET /api/employee (valid token)    | 200 OK                       | PASS   |

## Module 2: Employee Management

| TC# | Test Case                         | Input                              | Expected Output              | Status |
|-----|-----------------------------------|------------------------------------|------------------------------|--------|
| TC7 | Get all employees                 | GET /api/employee                  | 200 + array                  | PASS   |
| TC8 | Get employee by valid ID          | GET /api/employee/1                | 200 + EmployeeDto            | PASS   |
| TC9 | Get employee by invalid ID        | GET /api/employee/9999             | 404 Not Found                | PASS   |
| TC10| Create employee                   | POST valid CreateEmployeeDto       | 201 Created                  | PASS   |
| TC11| Create with missing required field| POST without FirstName             | 400 Bad Request              | PASS   |
| TC12| Update employee                   | PUT valid UpdateEmployeeDto        | 200 OK                       | PASS   |
| TC13| Delete employee (Admin)           | DELETE /api/employee/1             | 200 OK                       | PASS   |
| TC14| Search by name                    | GET /api/employee/search?name=Alice| 200 + filtered list          | PASS   |
| TC15| Search by department              | GET /api/employee/search?departmentId=1 | 200 + filtered list     | PASS   |

## Module 3: Attendance Management

| TC# | Test Case                         | Input                              | Expected Output              | Status |
|-----|-----------------------------------|------------------------------------|------------------------------|--------|
| TC16| Check-In                          | POST /api/attendance/checkin       | 200 + {AttendanceId}         | PASS   |
| TC17| Check-Out                         | PUT /api/attendance/checkout/{id}  | 200 OK                       | PASS   |
| TC18| Check-Out invalid ID              | PUT /api/attendance/checkout/9999  | 404 Not Found                | PASS   |
| TC19| Get monthly attendance            | GET /api/attendance/monthly/1/2026/6 | 200 + monthly list         | PASS   |
| TC20| Get by employee                   | GET /api/attendance/employee/1     | 200 + list                   | PASS   |
| TC21| TotalHours calculated             | Create with checkIn+checkOut       | TotalHours = difference      | PASS   |

## Module 4: Leave Management

| TC# | Test Case                         | Input                              | Expected Output              | Status |
|-----|-----------------------------------|------------------------------------|------------------------------|--------|
| TC22| Apply for leave                   | POST /api/leave valid body         | 200 + LeaveId                | PASS   |
| TC23| Apply with invalid dates          | fromDate > toDate                  | 400 / business rule          | PASS   |
| TC24| Approve leave (Manager)           | PUT /api/leave/1/approve [Manager] | 200 OK, status=Approved      | PASS   |
| TC25| Approve leave (Employee role)     | PUT /api/leave/1/approve [Employee]| 403 Forbidden                | PASS   |
| TC26| Reject leave                      | PUT /api/leave/1/reject [Manager]  | 200 OK, status=Rejected      | PASS   |
| TC27| Cancel pending leave              | PUT /api/leave/1/cancel            | 200 OK, status=Cancelled     | PASS   |
| TC28| Cancel approved leave             | PUT /api/leave/1/cancel (approved) | 400 / false                  | PASS   |
| TC29| Get leaves by employee            | GET /api/leave/employee/1          | 200 + list                   | PASS   |

## Module 5: Department Management

| TC# | Test Case                         | Input                              | Expected Output              | Status |
|-----|-----------------------------------|------------------------------------|------------------------------|--------|
| TC30| Get all departments               | GET /api/department                | 200 + list                   | PASS   |
| TC31| Create department                 | POST valid body                    | 200 + ID                     | PASS   |
| TC32| Create without name               | POST without departmentName        | 400 Bad Request              | PASS   |
| TC33| Update department                 | PUT valid body                     | 200 OK                       | PASS   |
| TC34| Delete department                 | DELETE /api/department/1           | 200 OK                       | PASS   |

## Module 6: Dashboard

| TC# | Test Case                         | Input                              | Expected Output              | Status |
|-----|-----------------------------------|------------------------------------|------------------------------|--------|
| TC35| Get dashboard summary             | GET /api/dashboard/summary         | 200 + KPI object             | PASS   |
| TC36| Get leave stats                   | GET /api/dashboard/leave-stats     | 200 + [{Status, Count}]      | PASS   |
| TC37| Get attendance chart              | GET /api/dashboard/attendance-chart| 200 + [{Date, Count}]        | PASS   |
| TC38| Get project counts                | GET /api/dashboard/project-counts  | 200 + {Active, Completed}    | PASS   |

## Unit Test Coverage (xUnit)

| Test Class                  | Method Tested                          | Coverage |
|-----------------------------|----------------------------------------|----------|
| EmployeeServiceTests        | GetAllAsync_ReturnsAllEmployees        | ✅       |
| EmployeeServiceTests        | GetByIdAsync_ReturnsEmployee           | ✅       |
| EmployeeServiceTests        | GetByIdAsync_ReturnsNull_WhenNotFound  | ✅       |
| EmployeeServiceTests        | CreateAsync_ReturnsNewId               | ✅       |
| EmployeeServiceTests        | DeleteAsync_ReturnsFalse               | ✅       |
| EmployeeServiceTests        | UpdateAsync_ReturnsFalse               | ✅       |
| EmployeeServiceTests        | SearchAsync_ReturnsFilteredEmployees   | ✅       |
| LeaveServiceTests           | GetAllAsync_ReturnsAllLeaves           | ✅       |
| LeaveServiceTests           | ApproveAsync_SetsApprovedStatus        | ✅       |
| LeaveServiceTests           | RejectAsync_SetsRejectedStatus         | ✅       |
| LeaveServiceTests           | CancelAsync_ReturnsFalse_IfApproved    | ✅       |
| LeaveServiceTests           | CancelAsync_ReturnsTrue_IfPending      | ✅       |
| LeaveServiceTests           | DeleteAsync_ReturnsFalse               | ✅       |
| AttendanceServiceTests      | GetAllAsync_ReturnsAttendanceList      | ✅       |
| AttendanceServiceTests      | GetByIdAsync_ReturnsNull               | ✅       |
| AttendanceServiceTests      | CheckInAsync_ReturnsId                 | ✅       |
| AttendanceServiceTests      | CheckOutAsync_ReturnsFalse_NotFound    | ✅       |
| AttendanceServiceTests      | CheckOutAsync_CalculatesHours          | ✅       |
