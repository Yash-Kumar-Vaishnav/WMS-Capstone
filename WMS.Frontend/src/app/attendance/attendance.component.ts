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
import { MatCardModule } from '@angular/material/card';
import { MatSelectModule } from '@angular/material/select';
import { MatDialogModule, MatDialog } from '@angular/material/dialog';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { AttendanceService } from '../shared/services/attendance.service';
import { AuthService } from '../shared/services/auth.service';
import { Attendance } from '../shared/models/models';
import { AttendanceFormDialogComponent } from './attendance-form-dialog.component';

@Component({
  selector: 'app-attendance',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, MatTableModule, MatPaginatorModule,
    MatSortModule, MatFormFieldModule, MatInputModule, MatButtonModule, MatIconModule,
    MatCardModule, MatSelectModule, MatDialogModule, MatSnackBarModule],
  template: `
    <div class="page-container">
      <div style="display:flex;justify-content:space-between;align-items:center;margin-bottom:16px;">
        <h2>Attendance Management</h2>
        <div>
          <button *ngIf="isAdminOrManager" mat-stroked-button color="primary" style="margin-right: 10px;" (click)="exportTimesheet()">
            <mat-icon>download</mat-icon> Export Timesheet
          </button>
          <button mat-raised-button color="primary" (click)="openForm()">
            <mat-icon>add</mat-icon> Check-In
          </button>
        </div>
      </div>
      <mat-form-field class="full-width search-field">
        <mat-label>Search by Employee ID or Date...</mat-label>
        <input matInput (keyup)="applyFilter($event)">
        <mat-icon matSuffix>search</mat-icon>
      </mat-form-field>
      <mat-card>
        <table mat-table [dataSource]="dataSource" matSort>
          <ng-container matColumnDef="attendanceId">
            <th mat-header-cell *matHeaderCellDef>#</th>
            <td mat-cell *matCellDef="let a">{{a.attendanceId}}</td>
          </ng-container>
          <ng-container matColumnDef="empId">
            <th mat-header-cell *matHeaderCellDef mat-sort-header>Emp ID</th>
            <td mat-cell *matCellDef="let a">{{a.empId}}</td>
          </ng-container>
          <ng-container matColumnDef="attendanceDate">
            <th mat-header-cell *matHeaderCellDef mat-sort-header>Date</th>
            <td mat-cell *matCellDef="let a">{{a.attendanceDate | date:'dd/MM/yyyy'}}</td>
          </ng-container>
          <ng-container matColumnDef="checkIn">
            <th mat-header-cell *matHeaderCellDef>Check-In</th>
            <td mat-cell *matCellDef="let a">{{a.checkIn | date:'HH:mm'}}</td>
          </ng-container>
          <ng-container matColumnDef="checkOut">
            <th mat-header-cell *matHeaderCellDef>Check-Out</th>
            <td mat-cell *matCellDef="let a">
              <ng-container *ngIf="a.checkOut">{{a.checkOut | date:'HH:mm'}}</ng-container>
              <button *ngIf="!a.checkOut" mat-stroked-button color="accent" (click)="doCheckOut(a.attendanceId)">Check Out</button>
            </td>
          </ng-container>
          <ng-container matColumnDef="totalHours">
            <th mat-header-cell *matHeaderCellDef>Hours</th>
            <td mat-cell *matCellDef="let a">{{a.totalHours ? (a.totalHours | number:'1.1-1') : '-'}}</td>
          </ng-container>
          <ng-container matColumnDef="workMode">
            <th mat-header-cell *matHeaderCellDef>Mode</th>
            <td mat-cell *matCellDef="let a">{{a.workMode}}</td>
          </ng-container>
          <ng-container matColumnDef="actions">
            <th mat-header-cell *matHeaderCellDef>Actions</th>
            <td mat-cell *matCellDef="let a">
              <span *ngIf="a.checkOut" style="color: green; font-weight: bold; margin-right: 8px;">Completed</span>
              <button mat-icon-button color="primary" (click)="openForm(a)"><mat-icon>edit</mat-icon></button>
              <button mat-icon-button color="warn" (click)="delete(a.attendanceId)"><mat-icon>delete</mat-icon></button>
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
export class AttendanceComponent implements OnInit {
  columns = ['attendanceId','empId','attendanceDate','checkIn','checkOut','totalHours','workMode','actions'];
  dataSource = new MatTableDataSource<Attendance>();
  isAdminOrManager = false;
  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;

  constructor(private svc: AttendanceService, private auth: AuthService, private dialog: MatDialog, private snack: MatSnackBar) {}

  ngOnInit() { 
    const role = this.auth.getRole();
    this.isAdminOrManager = role === 'Admin' || role === 'Manager';
    this.load(); 
  }

  load() {
    if (!this.isAdminOrManager) {
      const empId = this.auth.getEmpId();
      if (empId) {
        this.svc.getByEmployee(empId).subscribe(d => {
          this.dataSource.data = d;
          this.dataSource.paginator = this.paginator;
          this.dataSource.sort = this.sort;
          this.dataSource.filterPredicate = (a, f) =>
            `${a.empId} ${a.attendanceDate}`.toLowerCase().includes(f);
        });
      }
    } else {
      this.svc.getAll().subscribe(d => {
        this.dataSource.data = d;
        this.dataSource.paginator = this.paginator;
        this.dataSource.sort = this.sort;
        this.dataSource.filterPredicate = (a, f) =>
          `${a.empId} ${a.attendanceDate}`.toLowerCase().includes(f);
      });
    }
  }

  applyFilter(e: Event) {
    this.dataSource.filter = (e.target as HTMLInputElement).value.trim().toLowerCase();
  }

  openForm(att?: Attendance) {
    const ref = this.dialog.open(AttendanceFormDialogComponent, { width: '480px', data: att ?? null });
    ref.afterClosed().subscribe(r => { if (r) this.load(); });
  }

  delete(id: number) {
    if (!confirm('Delete this record?')) return;
    this.svc.delete(id).subscribe({ next: () => { this.snack.open('Deleted','',{duration:2000}); this.load(); } });
  }

  doCheckOut(id: number) {
    if (!confirm('Are you sure you want to Check Out now?')) return;
    this.svc.checkOut(id, { checkOut: new Date().toISOString() }).subscribe({
      next: () => { this.snack.open('Checked out successfully', '', { duration: 2000 }); this.load(); },
      error: () => this.snack.open('Error checking out', '', { duration: 3000 })
    });
  }

  exportTimesheet() {
    const now = new Date();
    this.svc.exportTimesheet(now.getFullYear(), now.getMonth() + 1).subscribe(blob => {
      const url = window.URL.createObjectURL(blob);
      const a = document.createElement('a');
      a.href = url;
      a.download = `Timesheet_${now.getFullYear()}_${now.getMonth() + 1}.csv`;
      a.click();
      window.URL.revokeObjectURL(url);
    });
  }
}
