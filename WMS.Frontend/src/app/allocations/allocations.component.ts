import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatTableModule } from '@angular/material/table';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { AllocationService } from '../shared/services/allocation.service';
import { AuthService } from '../shared/services/auth.service';
import { EmployeeProjectAllocation } from '../shared/models/models';
import { AllocationFormDialogComponent } from './allocation-form-dialog.component';

@Component({
  selector: 'app-allocations',
  standalone: true,
  imports: [CommonModule, MatTableModule, MatButtonModule, MatIconModule, MatDialogModule, MatSnackBarModule],
  template: `
    <div style="padding:20px;">
      <div style="display:flex;justify-content:space-between;align-items:center;margin-bottom:20px;">
        <h2>{{ isAdminOrManager ? 'Employee-Project Allocations' : 'My Projects' }}</h2>
        <button *ngIf="isAdminOrManager" mat-raised-button color="primary" (click)="openDialog()">
          <mat-icon>add</mat-icon> Assign Employee
        </button>
      </div>

      <table mat-table [dataSource]="allocations" class="mat-elevation-z2" style="width:100%;">
        <ng-container matColumnDef="allocationId">
          <th mat-header-cell *matHeaderCellDef> ID </th>
          <td mat-cell *matCellDef="let a"> {{ a.allocationId }} </td>
        </ng-container>

        <ng-container matColumnDef="empId">
          <th mat-header-cell *matHeaderCellDef> Employee ID </th>
          <td mat-cell *matCellDef="let a"> {{ a.empId }} </td>
        </ng-container>

        <ng-container matColumnDef="projectId">
          <th mat-header-cell *matHeaderCellDef> Project ID </th>
          <td mat-cell *matCellDef="let a"> {{ a.projectId }} </td>
        </ng-container>

        <ng-container matColumnDef="assignedOn">
          <th mat-header-cell *matHeaderCellDef> Assigned On </th>
          <td mat-cell *matCellDef="let a"> {{ a.assignedOn | date }} </td>
        </ng-container>

        <ng-container matColumnDef="status">
          <th mat-header-cell *matHeaderCellDef> Status </th>
          <td mat-cell *matCellDef="let a"> {{ a.status ? 'Active' : 'Inactive' }} </td>
        </ng-container>

        <ng-container matColumnDef="actions">
          <th mat-header-cell *matHeaderCellDef> Actions </th>
          <td mat-cell *matCellDef="let a">
            <button mat-icon-button color="primary" (click)="openDialog(a)"><mat-icon>edit</mat-icon></button>
            <button mat-icon-button color="warn" (click)="deactivate(a)"><mat-icon>block</mat-icon></button>
          </td>
        </ng-container>

        <tr mat-header-row *matHeaderRowDef="cols"></tr>
        <tr mat-row *matRowDef="let row; columns: cols;"></tr>
      </table>
    </div>
  `
})
export class AllocationsComponent implements OnInit {
  allocations: EmployeeProjectAllocation[] = [];
  cols = ['allocationId', 'empId', 'projectId', 'assignedOn', 'status', 'actions'];
  isAdminOrManager = false;

  constructor(
    private svc: AllocationService, 
    private auth: AuthService,
    private dialog: MatDialog, 
    private snack: MatSnackBar
  ) {}

  ngOnInit() { 
    const role = this.auth.getRole();
    this.isAdminOrManager = role === 'Admin' || role === 'Manager';
    if (!this.isAdminOrManager) {
      this.cols = ['allocationId', 'projectId', 'assignedOn', 'status']; // hide empId and actions
    }
    this.load(); 
  }

  load() {
    if (this.isAdminOrManager) {
      this.svc.getAll().subscribe(data => this.allocations = data);
    } else {
      const empId = this.auth.getEmpId();
      if (empId) {
        this.svc.getByEmployee(empId).subscribe(data => this.allocations = data);
      }
    }
  }

  openDialog(a?: EmployeeProjectAllocation) {
    const d = this.dialog.open(AllocationFormDialogComponent, { width: '400px', data: a });
    d.afterClosed().subscribe(res => { if (res) this.load(); });
  }

  deactivate(a: EmployeeProjectAllocation) {
    if (confirm('Deactivate this allocation?')) {
      const dto = { ...a, status: false };
      this.svc.update(dto).subscribe({
        next: () => { this.snack.open('Deactivated', '', { duration: 2000 }); this.load(); },
        error: () => this.snack.open('Error deactivating', '', { duration: 3000 })
      });
    }
  }
}
