import { Component, OnInit, ViewChild } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatTableModule, MatTableDataSource } from '@angular/material/table';
import { MatPaginatorModule, MatPaginator } from '@angular/material/paginator';
import { MatSortModule, MatSort } from '@angular/material/sort';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatCardModule } from '@angular/material/card';
import { MatDialogModule, MatDialog } from '@angular/material/dialog';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { MatTooltipModule } from '@angular/material/tooltip';
import { LeaveService } from '../shared/services/leave.service';
import { AuthService } from '../shared/services/auth.service';
import { Leave } from '../shared/models/models';
import { LeaveFormDialogComponent } from './leave-form-dialog.component';

@Component({
  selector: 'app-leaves',
  standalone: true,
  imports: [CommonModule, MatTableModule, MatPaginatorModule, MatSortModule,
    MatFormFieldModule, MatInputModule, MatButtonModule, MatIconModule,
    MatCardModule, MatDialogModule, MatSnackBarModule, MatTooltipModule],
  template: `
    <div class="page-container">
      <div style="display:flex;justify-content:space-between;align-items:center;margin-bottom:16px;">
        <h2>Leave Management</h2>
        <button mat-raised-button color="primary" (click)="openForm()">
          <mat-icon>add</mat-icon> Apply Leave
        </button>
      </div>
      <mat-form-field class="full-width search-field">
        <mat-label>Search by Emp ID, type or status...</mat-label>
        <input matInput (keyup)="applyFilter($event)">
        <mat-icon matSuffix>search</mat-icon>
      </mat-form-field>
      <mat-card>
        <table mat-table [dataSource]="dataSource" matSort>
          <ng-container matColumnDef="leaveId">
            <th mat-header-cell *matHeaderCellDef>#</th>
            <td mat-cell *matCellDef="let l">{{l.leaveId}}</td>
          </ng-container>
          <ng-container matColumnDef="empId">
            <th mat-header-cell *matHeaderCellDef mat-sort-header>Emp ID</th>
            <td mat-cell *matCellDef="let l">{{l.empId}}</td>
          </ng-container>
          <ng-container matColumnDef="leaveType">
            <th mat-header-cell *matHeaderCellDef>Type</th>
            <td mat-cell *matCellDef="let l">{{l.leaveType}}</td>
          </ng-container>
          <ng-container matColumnDef="fromDate">
            <th mat-header-cell *matHeaderCellDef>From</th>
            <td mat-cell *matCellDef="let l">{{l.fromDate | date:'dd/MM/yyyy'}}</td>
          </ng-container>
          <ng-container matColumnDef="toDate">
            <th mat-header-cell *matHeaderCellDef>To</th>
            <td mat-cell *matCellDef="let l">{{l.toDate | date:'dd/MM/yyyy'}}</td>
          </ng-container>
          <ng-container matColumnDef="status">
            <th mat-header-cell *matHeaderCellDef>Status</th>
            <td mat-cell *matCellDef="let l">
              <span [style.background]="statusBg(l.status)"
                style="padding:3px 10px;border-radius:12px;font-size:12px;color:#fff;font-weight:500;">
                {{l.status}}
              </span>
            </td>
          </ng-container>
          <ng-container matColumnDef="actions">
            <th mat-header-cell *matHeaderCellDef>Actions</th>
            <td mat-cell *matCellDef="let l">
              <button mat-icon-button color="primary" (click)="openForm(l)" matTooltip="Edit"><mat-icon>edit</mat-icon></button>
              <button mat-icon-button color="accent" (click)="approve(l)"
                *ngIf="isManager && l.status==='Pending'" matTooltip="Approve">
                <mat-icon>check_circle</mat-icon>
              </button>
              <button mat-icon-button color="warn" (click)="reject(l)"
                *ngIf="isManager && l.status==='Pending'" matTooltip="Reject">
                <mat-icon>cancel</mat-icon>
              </button>
              <button mat-icon-button (click)="cancel(l)"
                *ngIf="l.status==='Pending'" matTooltip="Cancel Leave"
                style="color:#ff9800;">
                <mat-icon>event_busy</mat-icon>
              </button>
              <button mat-icon-button color="warn" (click)="delete(l.leaveId)" matTooltip="Delete"><mat-icon>delete</mat-icon></button>
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
export class LeavesComponent implements OnInit {
  columns = ['leaveId','empId','leaveType','fromDate','toDate','status','actions'];
  dataSource = new MatTableDataSource<Leave>();
  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;
  isManager = false;

  constructor(private svc: LeaveService, private auth: AuthService,
    private dialog: MatDialog, private snack: MatSnackBar) {}

  ngOnInit() {
    const role = this.auth.getRole();
    this.isManager = role === 'Admin' || role === 'Manager';
    this.load();
  }

  load() {
    if (!this.isManager) {
      const empId = this.auth.getEmpId();
      if (empId) {
        this.svc.getByEmployee(empId).subscribe(d => {
          this.dataSource.data = d;
          this.dataSource.paginator = this.paginator;
          this.dataSource.sort = this.sort;
          this.dataSource.filterPredicate = (l, f) =>
            `${l.empId} ${l.leaveType} ${l.status}`.toLowerCase().includes(f);
        });
      }
    } else {
      this.svc.getAll().subscribe(d => {
        this.dataSource.data = d;
        this.dataSource.paginator = this.paginator;
        this.dataSource.sort = this.sort;
        this.dataSource.filterPredicate = (l, f) =>
          `${l.empId} ${l.leaveType} ${l.status}`.toLowerCase().includes(f);
      });
    }
  }

  applyFilter(e: Event) {
    this.dataSource.filter = (e.target as HTMLInputElement).value.trim().toLowerCase();
  }

  statusBg(s: string): string {
    return s === 'Approved' ? '#4caf50' : s === 'Rejected' ? '#f44336' : s === 'Cancelled' ? '#9e9e9e' : '#ff9800';
  }

  openForm(leave?: Leave) {
    const ref = this.dialog.open(LeaveFormDialogComponent, { width: '480px', data: leave ?? null });
    ref.afterClosed().subscribe(r => { if (r) this.load(); });
  }

  approve(l: Leave) {
    this.svc.approve(l.leaveId).subscribe({
      next: () => { this.snack.open('Leave approved', '', { duration: 2000 }); this.load(); },
      error: () => this.snack.open('Error approving leave', '', { duration: 3000 })
    });
  }

  reject(l: Leave) {
    this.svc.reject(l.leaveId).subscribe({
      next: () => { this.snack.open('Leave rejected', '', { duration: 2000 }); this.load(); },
      error: () => this.snack.open('Error rejecting leave', '', { duration: 3000 })
    });
  }

  cancel(l: Leave) {
    if (!confirm('Cancel this leave request?')) return;
    this.svc.cancel(l.leaveId).subscribe({
      next: () => { this.snack.open('Leave cancelled', '', { duration: 2000 }); this.load(); },
      error: () => this.snack.open('Error cancelling leave', '', { duration: 3000 })
    });
  }

  delete(id: number) {
    if (!confirm('Delete this leave record?')) return;
    this.svc.delete(id).subscribe({ next: () => { this.snack.open('Deleted', '', { duration: 2000 }); this.load(); } });
  }
}
