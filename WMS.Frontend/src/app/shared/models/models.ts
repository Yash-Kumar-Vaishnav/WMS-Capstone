export interface LoginDto { username: string; password: string; }
export interface LoginResponseDto {
  token: string;
  username: string;
  role: string;
  empId?: number;
}

export interface Employee {
  employeeId: number; firstName: string; lastName: string;
  email: string; phoneNumber: string; gender: string;
  dob: string; doj: string; departmentId: number; roleId: number;
  status: string; departmentName?: string; roleName?: string;
}
export interface CreateEmployeeDto {
  firstName: string; lastName: string; email: string;
  phoneNumber: string; gender: string; dob: string; doj: string;
  departmentId: number; roleId: number; status: string;
}
export interface UpdateEmployeeDto extends CreateEmployeeDto { employeeId: number; }

export interface Attendance {
  attendanceId: number; empId: number; checkIn: string; checkOut?: string;
  totalHours?: number; workMode: string; attendanceDate: string;
}
export interface CreateAttendanceDto {
  empId: number; checkIn: string; checkOut?: string;
  workMode: string; attendanceDate: string;
}

export interface Leave {
  leaveId: number; empId: number; leaveType: string; reason: string;
  fromDate: string; toDate: string; status: string;
  appliedOn: string; approvedBy?: number; approvedOn?: string;
}
export interface CreateLeaveDto {
  empId: number; leaveType: string; reason: string;
  fromDate: string; toDate: string;
}
export interface UpdateLeaveDto extends CreateLeaveDto {
  leaveId: number; status: string; approvedBy?: number;
}

export interface Department {
  departmentId: number; departmentName: string; description: string; createdOn?: string;
}
export interface CreateDepartmentDto { departmentName: string; description: string; }
export interface UpdateDepartmentDto extends CreateDepartmentDto { departmentId: number; }

export interface Project {
  projectId: number; projectName: string; clientId?: number;
  startDate: string; endDate: string; status: string;
}
export interface CreateProjectDto {
  projectName: string; clientId?: number;
  startDate: string; endDate: string; status: string;
}

export interface Client {
  clientId: number; clientName: string; clientAddress: string;
  clientPhoneNumber: string; clientLocation: string; status: boolean;
}

export interface Announcement {
  announcementId: number; title: string; message: string;
  createdBy: number; createdOn: string; isActive: boolean;
}

export interface DashboardSummary {
  totalEmployees: number; todayAttendanceCount: number;
  pendingLeaveRequests: number; activeProjects: number; totalDepartments: number;
}

export interface Role { roleId: number; roleName: string; }

export interface EmployeeProjectAllocation {
  allocationId: number; empId: number; projectId: number;
  assignedOn: string; status: boolean; createdBy: string;
  updatedBy?: string; updatedDate?: string;
}

export interface CreateAllocationDto {
  empId: number; projectId: number; assignedOn: string; createdBy: string;
}

export interface UpdateAllocationDto extends CreateAllocationDto {
  allocationId: number; status: boolean; updatedBy?: string;
}
