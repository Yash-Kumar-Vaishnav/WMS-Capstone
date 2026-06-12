import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';
import { MatDividerModule } from '@angular/material/divider';
import { EmployeeService } from '../shared/services/employee.service';
import { AuthService } from '../shared/services/auth.service';
import { Employee } from '../shared/models/models';

@Component({
  selector: 'app-profile',
  standalone: true,
  imports: [CommonModule, MatCardModule, MatIconModule, MatDividerModule],
  template: `
    <div class="page-container" style="display:flex; justify-content:center; padding-top:40px;">
      <mat-card style="max-width: 500px; width: 100%;">
        <mat-card-header>
          <div mat-card-avatar style="background-color: #3f51b5; color: white; display: flex; align-items: center; justify-content: center; border-radius: 50%;">
            <mat-icon>person</mat-icon>
          </div>
          <mat-card-title>{{ employee?.firstName }} {{ employee?.lastName }}</mat-card-title>
          <mat-card-subtitle>{{ employee?.email }}</mat-card-subtitle>
        </mat-card-header>
        <mat-divider></mat-divider>
        <mat-card-content style="padding-top: 16px;">
          <div style="display:grid; grid-template-columns: 1fr 1fr; gap: 16px;">
            <div>
              <strong>Employee ID:</strong><br/>
              {{ employee?.employeeId }}
            </div>
            <div>
              <strong>Role ID:</strong><br/>
              {{ employee?.roleId }}
            </div>
            <div>
              <strong>Department ID:</strong><br/>
              {{ employee?.departmentId }}
            </div>
            <div>
              <strong>Status:</strong><br/>
              <span [style.color]="employee?.status === 'Active' ? 'green' : 'red'">{{ employee?.status }}</span>
            </div>
            <div>
              <strong>Phone:</strong><br/>
              {{ employee?.phoneNumber }}
            </div>
            <div>
              <strong>Date of Join:</strong><br/>
              {{ employee?.doj | date }}
            </div>
            <div>
              <strong>Date of Birth:</strong><br/>
              {{ employee?.dob | date }}
            </div>
          </div>
        </mat-card-content>
      </mat-card>
    </div>
  `
})
export class ProfileComponent implements OnInit {
  employee: Employee | null = null;

  constructor(private svc: EmployeeService, private auth: AuthService) {}

  ngOnInit() {
    const empId = this.auth.getEmpId();
    if (empId) {
      this.svc.getById(empId).subscribe(e => this.employee = e);
    }
  }
}
