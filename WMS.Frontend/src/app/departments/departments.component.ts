import { Component, OnInit, ViewChild } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, Validators } from '@angular/forms';
import { MatTableModule, MatTableDataSource } from '@angular/material/table';
import { MatPaginatorModule, MatPaginator } from '@angular/material/paginator';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatCardModule } from '@angular/material/card';
import { MatDialogModule, MatDialog } from '@angular/material/dialog';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { DepartmentService } from '../shared/services/department.service';
import { AuthService } from '../shared/services/auth.service';
import { Department } from '../shared/models/models';
import { DepartmentFormDialogComponent } from './department-form-dialog.component';

@Component({
  selector: 'app-departments',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, MatTableModule, MatPaginatorModule,
    MatFormFieldModule, MatInputModule, MatButtonModule, MatIconModule,
    MatCardModule, MatDialogModule, MatSnackBarModule],
  template: `
    <div class="page-container">
      <div style="display:flex;justify-content:space-between;align-items:center;margin-bottom:16px;">
        <h2>Department Management</h2>
        <button *ngIf="isAdmin" mat-raised-button color="primary" (click)="openForm()">
          <mat-icon>add</mat-icon> Add Department
        </button>
      </div>
      <mat-form-field class="full-width search-field" style="width:100%;">
        <mat-label>Search Departments...</mat-label>
        <input matInput (keyup)="applyFilter($event)">
        <mat-icon matSuffix>search</mat-icon>
      </mat-form-field>
      <mat-card>
        <table mat-table [dataSource]="dataSource">
          <ng-container matColumnDef="departmentId">
            <th mat-header-cell *matHeaderCellDef>ID</th>
            <td mat-cell *matCellDef="let d">{{d.departmentId}}</td>
          </ng-container>
          <ng-container matColumnDef="departmentName">
            <th mat-header-cell *matHeaderCellDef>Department Name</th>
            <td mat-cell *matCellDef="let d">{{d.departmentName}}</td>
          </ng-container>
          <ng-container matColumnDef="description">
            <th mat-header-cell *matHeaderCellDef>Description</th>
            <td mat-cell *matCellDef="let d">{{d.description}}</td>
          </ng-container>
          <ng-container matColumnDef="createdOn">
            <th mat-header-cell *matHeaderCellDef>Created On</th>
            <td mat-cell *matCellDef="let d">{{d.createdOn | date:'dd/MM/yyyy'}}</td>
          </ng-container>
          <ng-container matColumnDef="actions">
            <th mat-header-cell *matHeaderCellDef>Actions</th>
            <td mat-cell *matCellDef="let d">
              <button mat-icon-button color="primary" (click)="openForm(d)"><mat-icon>edit</mat-icon></button>
              <button mat-icon-button color="warn" (click)="delete(d.departmentId)"><mat-icon>delete</mat-icon></button>
            </td>
          </ng-container>
          <tr mat-header-row *matHeaderRowDef="columns"></tr>
          <tr mat-row *matRowDef="let row; columns: columns;"></tr>
        </table>
        <mat-paginator [pageSizeOptions]="[10,25]" showFirstLastButtons></mat-paginator>
      </mat-card>
    </div>
  `
})
export class DepartmentsComponent implements OnInit {
  columns = ['departmentId','departmentName','description','createdOn','actions'];
  dataSource = new MatTableDataSource<Department>();
  isAdmin = false;
  @ViewChild(MatPaginator) paginator!: MatPaginator;

  constructor(private svc: DepartmentService, private auth: AuthService, private dialog: MatDialog, private snack: MatSnackBar) {}

  ngOnInit() { 
    this.isAdmin = this.auth.getRole() === 'Admin';
    if (!this.isAdmin) {
      this.columns = ['departmentId','departmentName','description','createdOn'];
    }
    this.load(); 
  }

  load() {
    this.svc.getAll().subscribe(d => {
      this.dataSource.data = d;
      this.dataSource.paginator = this.paginator;
      this.dataSource.filterPredicate = (data, filter) => 
        data.departmentName.toLowerCase().includes(filter) || 
        data.description.toLowerCase().includes(filter);
    });
  }

  applyFilter(e: Event) {
    this.dataSource.filter = (e.target as HTMLInputElement).value.trim().toLowerCase();
  }

  openForm(dept?: Department) {
    const ref = this.dialog.open(DepartmentFormDialogComponent, { width: '420px', data: dept ?? null });
    ref.afterClosed().subscribe(r => { if (r) this.load(); });
  }

  delete(id: number) {
    if (!confirm('Delete this department?')) return;
    this.svc.delete(id).subscribe({ next: () => { this.snack.open('Deleted','',{duration:2000}); this.load(); } });
  }
}
