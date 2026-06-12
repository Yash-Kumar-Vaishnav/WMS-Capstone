import { Component, Inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA, MatDialogModule } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { AnnouncementService } from '../shared/services/announcement.service';

@Component({
  selector: 'app-announcement-form-dialog',
  standalone: true,
  imports: [CommonModule, FormsModule, MatDialogModule, MatFormFieldModule, MatInputModule, MatButtonModule],
  template: `
    <h2 mat-dialog-title>{{ data ? 'Edit' : 'Create' }} Announcement</h2>
    <mat-dialog-content>
      <mat-form-field appearance="outline" fullWidth style="width:100%; margin-top:10px;">
        <mat-label>Title</mat-label>
        <input matInput [(ngModel)]="dto.title" required>
      </mat-form-field>
      <mat-form-field appearance="outline" fullWidth style="width:100%;">
        <mat-label>Message</mat-label>
        <textarea matInput [(ngModel)]="dto.message" required rows="4"></textarea>
      </mat-form-field>
    </mat-dialog-content>
    <mat-dialog-actions align="end">
      <button mat-button mat-dialog-close>Cancel</button>
      <button mat-raised-button color="primary" (click)="save()" [disabled]="!dto.title || !dto.message">Save</button>
    </mat-dialog-actions>
  `
})
export class AnnouncementFormDialogComponent {
  dto: any = { title: '', message: '', isActive: true };

  constructor(
    private dialogRef: MatDialogRef<AnnouncementFormDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any,
    private svc: AnnouncementService
  ) {
    if (data) {
      this.dto = { ...data };
    }
  }

  save() {
    if (this.data) {
      this.svc.update(this.dto).subscribe(() => this.dialogRef.close(true));
    } else {
      this.svc.create(this.dto).subscribe(() => this.dialogRef.close(true));
    }
  }
}
