import { Routes } from '@angular/router';
import { authGuard } from './shared/guards/auth.guard';
import { roleGuard } from './shared/guards/role.guard';

export const routes: Routes = [
  { path: 'login', loadComponent: () => import('./auth/login.component').then(m => m.LoginComponent) },
  {
    path: '',
    loadComponent: () => import('./layout/shell.component').then(m => m.ShellComponent),
    canActivate: [authGuard],
    children: [
      { path: '', redirectTo: 'dashboard', pathMatch: 'full' },
      { path: 'dashboard', loadComponent: () => import('./dashboard/dashboard.component').then(m => m.DashboardComponent) },
      { path: 'profile', loadComponent: () => import('./employees/profile.component').then(m => m.ProfileComponent) },
      { path: 'announcements', loadComponent: () => import('./announcements/announcements.component').then(m => m.AnnouncementsComponent) },
      { path: 'employees', loadComponent: () => import('./employees/employees.component').then(m => m.EmployeesComponent), canActivate: [roleGuard(['Admin', 'Manager'])] },
      { path: 'attendance', loadComponent: () => import('./attendance/attendance.component').then(m => m.AttendanceComponent) },
      { path: 'leaves', loadComponent: () => import('./leaves/leaves.component').then(m => m.LeavesComponent) },
      { path: 'departments', loadComponent: () => import('./departments/departments.component').then(m => m.DepartmentsComponent), canActivate: [roleGuard(['Admin', 'Manager'])] },
      { path: 'projects', loadComponent: () => import('./projects/projects.component').then(m => m.ProjectsComponent) },
      { path: 'allocations', loadComponent: () => import('./allocations/allocations.component').then(m => m.AllocationsComponent), canActivate: [roleGuard(['Admin', 'Manager', 'Employee'])] },
      { path: 'audit-logs', loadComponent: () => import('./audit-logs/audit-logs.component').then(m => m.AuditLogsComponent), canActivate: [roleGuard(['Admin'])] },
    ]
  },
  { path: '**', redirectTo: '' }
];
