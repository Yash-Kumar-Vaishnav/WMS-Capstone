import { Component, OnInit, ViewChild } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, Validators } from '@angular/forms';
import { MatTableModule, MatTableDataSource } from '@angular/material/table';
import { MatPaginatorModule, MatPaginator } from '@angular/material/paginator';
import { MatSortModule, MatSort } from '@angular/material/sort';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatDialogModule, MatDialog } from '@angular/material/dialog';
import { MatSelectModule } from '@angular/material/select';
import { MatCardModule } from '@angular/material/card';
import { MatChipsModule } from '@angular/material/chips';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';
import { EmployeeService } from '../shared/services/employee.service';
import { DepartmentService } from '../shared/services/department.service';
import { Employee, Department } from '../shared/models/models';
import { EmployeeFormDialogComponent } from './employee-form-dialog.component';

@Component({
  selector: 'app-employees',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, MatTableModule, MatPaginatorModule,
    MatSortModule, MatFormFieldModule, MatInputModule, MatButtonModule, MatIconModule,
    MatDialogModule, MatSelectModule, MatCardModule, MatChipsModule, MatSnackBarModule,
    MatDatepickerModule, MatNativeDateModule],
  template: `
    <div class="page-container">
      <div style="display:flex;justify-content:space-between;align-items:center;margin-bottom:16px;">
        <h2>Employee Management</h2>
        <button mat-raised-button color="primary" (click)="openForm()">
          <mat-icon>add</mat-icon> Add Employee
        </button>
      </div>

      <mat-form-field class="full-width search-field">
        <mat-label>Search by name, ID, department...</mat-label>
        <input matInput (keyup)="applyFilter($event)">
        <mat-icon matSuffix>search</mat-icon>
      </mat-form-field>

      <mat-card>
        <table mat-table [dataSource]="dataSource" matSort>
          <ng-container matColumnDef="employeeId">
            <th mat-header-cell *matHeaderCellDef mat-sort-header>ID</th>
            <td mat-cell *matCellDef="let e">{{e.employeeId}}</td>
          </ng-container>
          <ng-container matColumnDef="name">
            <th mat-header-cell *matHeaderCellDef mat-sort-header>Name</th>
            <td mat-cell *matCellDef="let e">{{e.firstName}} {{e.lastName}}</td>
          </ng-container>
          <ng-container matColumnDef="email">
            <th mat-header-cell *matHeaderCellDef>Email</th>
            <td mat-cell *matCellDef="let e">{{e.email}}</td>
          </ng-container>
          <ng-container matColumnDef="departmentName">
            <th mat-header-cell *matHeaderCellDef>Department</th>
            <td mat-cell *matCellDef="let e">{{e.departmentName}}</td>
          </ng-container>
          <ng-container matColumnDef="roleName">
            <th mat-header-cell *matHeaderCellDef>Role</th>
            <td mat-cell *matCellDef="let e">{{e.roleName}}</td>
          </ng-container>
          <ng-container matColumnDef="status">
            <th mat-header-cell *matHeaderCellDef>Status</th>
            <td mat-cell *matCellDef="let e">
              <span [class]="e.status==='Active' ? 'chip-active' : 'chip-inactive'"
                style="padding:4px 10px;border-radius:12px;font-size:12px;">{{e.status}}</span>
            </td>
          </ng-container>
          <ng-container matColumnDef="actions">
            <th mat-header-cell *matHeaderCellDef>Actions</th>
            <td mat-cell *matCellDef="let e">
              <button mat-icon-button color="primary" (click)="openForm(e)"><mat-icon>edit</mat-icon></button>
              <button mat-icon-button color="warn" (click)="delete(e.employeeId)"><mat-icon>delete</mat-icon></button>
            </td>
          </ng-container>
          <tr mat-header-row *matHeaderRowDef="columns"></tr>
          <tr mat-row *matRowDef="let row; columns: columns;"></tr>
        </table>
        <mat-paginator [pageSizeOptions]="[10,25,50]" showFirstLastButtons></mat-paginator>
      </mat-card>
    </div>
  `
})
export class EmployeesComponent implements OnInit {
  columns = ['employeeId','name','email','departmentName','roleName','status','actions'];
  dataSource = new MatTableDataSource<Employee>();
  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;

  constructor(private svc: EmployeeService, private dialog: MatDialog, private snack: MatSnackBar) {}

  ngOnInit() { this.load(); }

  load() {
    this.svc.getAll().subscribe(data => {
      this.dataSource.data = data;
      this.dataSource.paginator = this.paginator;
      this.dataSource.sort = this.sort;
      this.dataSource.filterPredicate = (e, f) =>
        `${e.firstName} ${e.lastName} ${e.employeeId} ${e.departmentName} ${e.roleName}`.toLowerCase().includes(f);
    });
  }

  applyFilter(e: Event) {
    this.dataSource.filter = (e.target as HTMLInputElement).value.trim().toLowerCase();
  }

  openForm(emp?: Employee) {
    const ref = this.dialog.open(EmployeeFormDialogComponent, { width: '560px', data: emp ?? null });
    ref.afterClosed().subscribe(r => { if (r) this.load(); });
  }

  delete(id: number) {
    if (!confirm('Delete this employee?')) return;
    this.svc.delete(id).subscribe({ next: () => { this.snack.open('Deleted','',{duration:2000}); this.load(); } });
  }
}
