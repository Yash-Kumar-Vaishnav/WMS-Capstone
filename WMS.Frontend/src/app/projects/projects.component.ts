import { Component, OnInit, ViewChild } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatTableModule, MatTableDataSource } from '@angular/material/table';
import { MatPaginatorModule, MatPaginator } from '@angular/material/paginator';
import { MatSortModule, MatSort } from '@angular/material/sort';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatCardModule } from '@angular/material/card';
import { MatDialogModule, MatDialog } from '@angular/material/dialog';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { ProjectService } from '../shared/services/project.service';
import { AuthService } from '../shared/services/auth.service';
import { Project } from '../shared/models/models';
import { ProjectFormDialogComponent } from './project-form-dialog.component';

@Component({
  selector: 'app-projects',
  standalone: true,
  imports: [CommonModule, MatTableModule, MatPaginatorModule, MatSortModule,
    MatButtonModule, MatIconModule, MatCardModule, MatDialogModule, MatSnackBarModule,
    MatFormFieldModule, MatInputModule],
  template: `
    <div class="page-container">
      <div style="display:flex;justify-content:space-between;align-items:center;margin-bottom:16px;">
        <h2>Project Management</h2>
        <button *ngIf="role !== 'Employee'" mat-raised-button color="primary" (click)="openForm()">
          <mat-icon>add</mat-icon> Add Project
        </button>
      </div>
      <mat-form-field class="full-width search-field">
        <mat-label>Search projects...</mat-label>
        <input matInput (keyup)="applyFilter($event)">
        <mat-icon matSuffix>search</mat-icon>
      </mat-form-field>
      <mat-card>
        <table mat-table [dataSource]="dataSource" matSort>
          <ng-container matColumnDef="projectId">
            <th mat-header-cell *matHeaderCellDef>#</th>
            <td mat-cell *matCellDef="let p">{{p.projectId}}</td>
          </ng-container>
          <ng-container matColumnDef="projectName">
            <th mat-header-cell *matHeaderCellDef mat-sort-header>Project Name</th>
            <td mat-cell *matCellDef="let p">{{p.projectName}}</td>
          </ng-container>
          <ng-container matColumnDef="startDate">
            <th mat-header-cell *matHeaderCellDef>Start Date</th>
            <td mat-cell *matCellDef="let p">{{p.startDate | date:'dd/MM/yyyy'}}</td>
          </ng-container>
          <ng-container matColumnDef="endDate">
            <th mat-header-cell *matHeaderCellDef>End Date</th>
            <td mat-cell *matCellDef="let p">{{p.endDate | date:'dd/MM/yyyy'}}</td>
          </ng-container>
          <ng-container matColumnDef="status">
            <th mat-header-cell *matHeaderCellDef>Status</th>
            <td mat-cell *matCellDef="let p">
              <span [class]="p.status==='Active' ? 'chip-active' : 'chip-inactive'"
                style="padding:4px 10px;border-radius:12px;font-size:12px;">{{p.status}}</span>
            </td>
          </ng-container>
          <ng-container matColumnDef="actions">
            <th mat-header-cell *matHeaderCellDef>Actions</th>
            <td mat-cell *matCellDef="let p">
              <button *ngIf="role !== 'Employee'" mat-icon-button color="primary" (click)="openForm(p)"><mat-icon>edit</mat-icon></button>
              <button *ngIf="role !== 'Employee'" mat-icon-button color="warn" (click)="delete(p.projectId)"><mat-icon>delete</mat-icon></button>
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
export class ProjectsComponent implements OnInit {
  columns = ['projectId','projectName','startDate','endDate','status','actions'];
  dataSource = new MatTableDataSource<Project>();
  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;

  role = '';

  constructor(private svc: ProjectService, private auth: AuthService, private dialog: MatDialog, private snack: MatSnackBar) {
    this.role = this.auth.getRole();
    if (this.role === 'Employee') {
      this.columns = ['projectId','projectName','startDate','endDate','status'];
    }
  }

  ngOnInit() { this.load(); }

  load() {
    this.svc.getAll().subscribe(d => {
      this.dataSource.data = d;
      this.dataSource.paginator = this.paginator;
      this.dataSource.sort = this.sort;
      this.dataSource.filterPredicate = (p, f) => p.projectName.toLowerCase().includes(f);
    });
  }

  applyFilter(e: Event) {
    this.dataSource.filter = (e.target as HTMLInputElement).value.trim().toLowerCase();
  }

  openForm(proj?: Project) {
    const ref = this.dialog.open(ProjectFormDialogComponent, { width: '480px', data: proj ?? null });
    ref.afterClosed().subscribe(r => { if (r) this.load(); });
  }

  delete(id: number) {
    if (!confirm('Delete this project?')) return;
    this.svc.delete(id).subscribe({ next: () => { this.snack.open('Deleted','',{duration:2000}); this.load(); } });
  }
}
