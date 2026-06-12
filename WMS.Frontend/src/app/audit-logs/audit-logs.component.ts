import { Component, OnInit, ViewChild } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatTableModule, MatTableDataSource } from '@angular/material/table';
import { MatPaginatorModule, MatPaginator } from '@angular/material/paginator';
import { MatSortModule, MatSort } from '@angular/material/sort';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatIconModule } from '@angular/material/icon';
import { AuditLogService, AuditLog } from '../shared/services/audit-log.service';

@Component({
  selector: 'app-audit-logs',
  standalone: true,
  imports: [CommonModule, MatTableModule, MatPaginatorModule, MatSortModule,
    MatCardModule, MatFormFieldModule, MatInputModule, MatIconModule],
  template: `
    <div class="page-container">
      <div style="display:flex;justify-content:space-between;align-items:center;margin-bottom:16px;">
        <h2>Audit Logs</h2>
      </div>

      <mat-form-field class="full-width search-field">
        <mat-label>Search logs...</mat-label>
        <input matInput (keyup)="applyFilter($event)">
        <mat-icon matSuffix>search</mat-icon>
      </mat-form-field>

      <mat-card>
        <table mat-table [dataSource]="dataSource" matSort>
          <ng-container matColumnDef="auditId">
            <th mat-header-cell *matHeaderCellDef mat-sort-header>ID</th>
            <td mat-cell *matCellDef="let log">{{log.auditId}}</td>
          </ng-container>

          <ng-container matColumnDef="entityName">
            <th mat-header-cell *matHeaderCellDef mat-sort-header>Entity</th>
            <td mat-cell *matCellDef="let log">{{log.entityName}}</td>
          </ng-container>

          <ng-container matColumnDef="recordId">
            <th mat-header-cell *matHeaderCellDef mat-sort-header>Record ID</th>
            <td mat-cell *matCellDef="let log">{{log.recordId}}</td>
          </ng-container>

          <ng-container matColumnDef="action">
            <th mat-header-cell *matHeaderCellDef mat-sort-header>Action</th>
            <td mat-cell *matCellDef="let log">
              <span [ngStyle]="{'color': log.action === 'Create' ? 'green' : (log.action === 'Update' ? 'blue' : 'red')}">
                {{log.action}}
              </span>
            </td>
          </ng-container>

          <ng-container matColumnDef="createdBy">
            <th mat-header-cell *matHeaderCellDef mat-sort-header>User ID</th>
            <td mat-cell *matCellDef="let log">{{log.createdBy}}</td>
          </ng-container>

          <ng-container matColumnDef="createdOn">
            <th mat-header-cell *matHeaderCellDef mat-sort-header>Date/Time</th>
            <td mat-cell *matCellDef="let log">{{log.createdOn | date:'short'}}</td>
          </ng-container>

          <tr mat-header-row *matHeaderRowDef="columns"></tr>
          <tr mat-row *matRowDef="let row; columns: columns;"></tr>
        </table>

        <mat-paginator [pageSizeOptions]="[10,25,50]" showFirstLastButtons></mat-paginator>
      </mat-card>
    </div>
  `
})
export class AuditLogsComponent implements OnInit {
  columns = ['auditId', 'entityName', 'recordId', 'action', 'createdBy', 'createdOn'];
  dataSource = new MatTableDataSource<AuditLog>();

  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;

  constructor(private service: AuditLogService) {}

  ngOnInit(): void {
    this.service.getAll().subscribe(logs => {
      this.dataSource.data = logs;
      this.dataSource.paginator = this.paginator;
      this.dataSource.sort = this.sort;
      this.dataSource.filterPredicate = (data, filter) => 
        data.entityName.toLowerCase().includes(filter) || 
        data.action.toLowerCase().includes(filter) ||
        data.createdBy.toString().includes(filter);
    });
  }

  applyFilter(event: Event) {
    this.dataSource.filter = (event.target as HTMLInputElement).value.trim().toLowerCase();
  }
}
