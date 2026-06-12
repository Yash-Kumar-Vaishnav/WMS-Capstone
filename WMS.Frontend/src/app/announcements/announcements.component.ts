import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatTableModule } from '@angular/material/table';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { AnnouncementService } from '../shared/services/announcement.service';
import { AuthService } from '../shared/services/auth.service';
import { Announcement } from '../shared/models/models';
import { AnnouncementFormDialogComponent } from './announcement-form-dialog.component';

@Component({
  selector: 'app-announcements',
  standalone: true,
  imports: [CommonModule, MatTableModule, MatButtonModule, MatIconModule, MatDialogModule, MatSnackBarModule],
  template: `
    <div style="padding:20px;">
      <div style="display:flex;justify-content:space-between;align-items:center;margin-bottom:20px;">
        <h2>Announcements</h2>
        <button *ngIf="isAdmin" mat-raised-button color="primary" (click)="openDialog()">
          <mat-icon>campaign</mat-icon> New Announcement
        </button>
      </div>

      <table mat-table [dataSource]="announcements" class="mat-elevation-z2" style="width:100%;">
        <ng-container matColumnDef="announcementId">
          <th mat-header-cell *matHeaderCellDef> ID </th>
          <td mat-cell *matCellDef="let a"> {{ a.announcementId }} </td>
        </ng-container>

        <ng-container matColumnDef="title">
          <th mat-header-cell *matHeaderCellDef> Title </th>
          <td mat-cell *matCellDef="let a"> <strong>{{ a.title }}</strong> </td>
        </ng-container>

        <ng-container matColumnDef="message">
          <th mat-header-cell *matHeaderCellDef> Message </th>
          <td mat-cell *matCellDef="let a"> {{ a.message }} </td>
        </ng-container>

        <ng-container matColumnDef="createdOn">
          <th mat-header-cell *matHeaderCellDef> Date </th>
          <td mat-cell *matCellDef="let a"> {{ a.createdOn | date:'mediumDate' }} </td>
        </ng-container>

        <ng-container matColumnDef="status">
          <th mat-header-cell *matHeaderCellDef> Status </th>
          <td mat-cell *matCellDef="let a"> {{ a.isActive ? 'Active' : 'Inactive' }} </td>
        </ng-container>

        <ng-container matColumnDef="actions">
          <th mat-header-cell *matHeaderCellDef> Actions </th>
          <td mat-cell *matCellDef="let a">
            <button *ngIf="isAdmin" mat-icon-button color="primary" (click)="openDialog(a)"><mat-icon>edit</mat-icon></button>
            <button *ngIf="isAdmin && a.isActive" mat-icon-button color="warn" (click)="deactivate(a)"><mat-icon>block</mat-icon></button>
          </td>
        </ng-container>

        <tr mat-header-row *matHeaderRowDef="cols"></tr>
        <tr mat-row *matRowDef="let row; columns: cols;"></tr>
      </table>
    </div>
  `
})
export class AnnouncementsComponent implements OnInit {
  announcements: Announcement[] = [];
  isAdmin = false;
  cols = ['title', 'message', 'createdOn'];

  constructor(
    private svc: AnnouncementService, 
    private auth: AuthService,
    private dialog: MatDialog, 
    private snack: MatSnackBar
  ) {}

  ngOnInit() { 
    this.isAdmin = this.auth.getRole() === 'Admin';
    if (this.isAdmin) {
      this.cols = ['announcementId', 'title', 'message', 'createdOn', 'status', 'actions'];
    }
    this.load(); 
  }

  load() {
    if (this.isAdmin) {
      this.svc.getAll().subscribe(data => this.announcements = data);
    } else {
      this.svc.getActive().subscribe(data => this.announcements = data);
    }
  }

  openDialog(a?: Announcement) {
    const d = this.dialog.open(AnnouncementFormDialogComponent, { width: '400px', data: a });
    d.afterClosed().subscribe(res => { if (res) this.load(); });
  }

  deactivate(a: Announcement) {
    if (confirm('Deactivate this announcement?')) {
      const dto = { ...a, isActive: false };
      this.svc.update(dto).subscribe({
        next: () => { this.snack.open('Deactivated', '', { duration: 2000 }); this.load(); },
        error: () => this.snack.open('Error deactivating', '', { duration: 3000 })
      });
    }
  }
}
