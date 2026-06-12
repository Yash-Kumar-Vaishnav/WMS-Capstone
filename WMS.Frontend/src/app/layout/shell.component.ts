import { Component } from '@angular/core';
import { Router, RouterLink, RouterLinkActive, RouterOutlet } from '@angular/router';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatListModule } from '@angular/material/list';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { CommonModule } from '@angular/common';
import { AuthService } from '../shared/services/auth.service';

@Component({
  selector: 'app-shell',
  standalone: true,
  imports: [CommonModule, RouterOutlet, RouterLink, RouterLinkActive,
    MatToolbarModule, MatSidenavModule, MatListModule, MatIconModule, MatButtonModule],
  template: `
    <mat-sidenav-container class="sidenav-container">
      <mat-sidenav mode="side" opened class="sidenav">
        <mat-toolbar color="primary" style="font-size:14px;">WMS</mat-toolbar>
        <mat-nav-list>
          <a mat-list-item routerLink="/dashboard" routerLinkActive="mdc-list-item--activated">
            <mat-icon matListItemIcon>dashboard</mat-icon> Dashboard
          </a>
          <a *ngIf="role === 'Employee'" mat-list-item routerLink="/profile" routerLinkActive="mdc-list-item--activated">
            <mat-icon matListItemIcon>person</mat-icon> My Profile
          </a>
          <a *ngIf="role === 'Admin' || role === 'Manager'" mat-list-item routerLink="/employees" routerLinkActive="mdc-list-item--activated">
            <mat-icon matListItemIcon>people</mat-icon> Employees
          </a>
          <a mat-list-item routerLink="/attendance" routerLinkActive="mdc-list-item--activated">
            <mat-icon matListItemIcon>access_time</mat-icon> Attendance
          </a>
          <a mat-list-item routerLink="/leaves" routerLinkActive="mdc-list-item--activated">
            <mat-icon matListItemIcon>event_busy</mat-icon> Leaves
          </a>
          <a *ngIf="role === 'Admin' || role === 'Manager'" mat-list-item routerLink="/departments" routerLinkActive="mdc-list-item--activated">
            <mat-icon matListItemIcon>business</mat-icon> Departments
          </a>
          <a mat-list-item routerLink="/projects" routerLinkActive="mdc-list-item--activated">
            <mat-icon matListItemIcon>folder</mat-icon> Projects
          </a>
          <a mat-list-item routerLink="/allocations" routerLinkActive="mdc-list-item--activated">
            <mat-icon matListItemIcon>assignment_ind</mat-icon> {{ role === 'Employee' ? 'My Projects' : 'Allocations' }}
          </a>
          <a *ngIf="role === 'Admin'" mat-list-item routerLink="/audit-logs" routerLinkActive="mdc-list-item--activated">
            <mat-icon matListItemIcon>history</mat-icon> Audit Logs
          </a>
          <a mat-list-item routerLink="/announcements" routerLinkActive="mdc-list-item--activated">
            <mat-icon matListItemIcon>campaign</mat-icon> Announcements
          </a>
        </mat-nav-list>
      </mat-sidenav>
      <mat-sidenav-content>
        <mat-toolbar color="primary">
          <span class="spacer"></span>
          <span style="margin-right:8px;">{{ username }} ({{ role }})</span>
          <button mat-icon-button (click)="logout()"><mat-icon>logout</mat-icon></button>
        </mat-toolbar>
        <div class="main-content">
          <router-outlet></router-outlet>
        </div>
      </mat-sidenav-content>
    </mat-sidenav-container>
  `
})
export class ShellComponent {
  username = this.auth.getUsername();
  role = this.auth.getRole();
  constructor(private auth: AuthService, private router: Router) {}
  logout() { this.auth.logout(); this.router.navigate(['/login']); }
}
