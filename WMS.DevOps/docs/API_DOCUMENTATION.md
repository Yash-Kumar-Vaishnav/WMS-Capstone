# API Documentation

The WMS API is built using ASP.NET Core 8 with Clean Architecture. All endpoints are protected via JWT Bearer Authentication unless otherwise specified.

## Base URL
`https://localhost:5001/api`

## Authentication (`/api/auth`)
- `POST /login`: Authenticates a user and returns a JWT token.
  - **Body**: `{ "username": "admin", "password": "password" }`
  - **Response**: `{ "token": "eyJhb...", "role": "Admin", "userId": 1 }`

## Employees (`/api/employee`)
- `GET /`: Retrieves all employees.
- `GET /{id}`: Retrieves an employee by ID.
- `POST /`: Creates a new employee.
- `PUT /`: Updates an existing employee.
- `DELETE /{id}`: Deletes an employee.

## Attendance (`/api/attendance`)
- `GET /`: Retrieves all attendance records.
- `GET /employee/{empId}`: Retrieves attendance for a specific employee.
- `GET /monthly/{empId}/{year}/{month}`: Retrieves monthly attendance.
- `POST /checkin`: Records a check-in.
- `PUT /checkout/{id}`: Records a check-out and calculates total hours.
- `GET /timesheet/export/{year}/{month}`: Exports monthly timesheet as CSV.

## Leaves (`/api/leave`)
- `GET /`: Retrieves all leave requests.
- `POST /`: Applies for a new leave.
- `PUT /{id}/approve`: Approves a leave request (Manager/Admin).
- `PUT /{id}/reject`: Rejects a leave request (Manager/Admin).
- `PUT /{id}/cancel`: Cancels a leave request.

## Departments (`/api/department`)
- `GET /`: Retrieves all departments.
- `POST /`: Creates a new department.
- `PUT /`: Updates a department.
- `DELETE /{id}`: Deletes a department.

## Projects (`/api/project`)
- `GET /`: Retrieves all projects.
- `POST /`: Creates a new project.
- `PUT /`: Updates a project.
- `DELETE /{id}`: Deletes a project.

## Dashboard (`/api/dashboard`)
- `GET /summary`: Retrieves KPI summary metrics (Employee Count, Project Count, etc.).
- `GET /leave-stats`: Retrieves leave statistics for charts.
- `GET /attendance-chart`: Retrieves 30-day attendance trends.

*For full interactive documentation, run the API and navigate to the Swagger UI at `/swagger`.*
