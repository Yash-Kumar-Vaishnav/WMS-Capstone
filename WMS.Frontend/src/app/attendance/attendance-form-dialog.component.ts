import { Component, Inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, Validators } from '@angular/forms';
import { MatDialogModule, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatButtonModule } from '@angular/material/button';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { AttendanceService } from '../shared/services/attendance.service';
import { AuthService } from '../shared/services/auth.service';
import { Attendance } from '../shared/models/models';

@Component({
  selector: 'app-attendance-form-dialog',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, MatDialogModule, MatFormFieldModule,
    MatInputModule, MatSelectModule, MatButtonModule, MatDatepickerModule, MatNativeDateModule, MatSnackBarModule],
  template: `
    <h2 mat-dialog-title>{{ data ? 'Edit Attendance' : 'Check-In' }}</h2>
    <mat-dialog-content>
      <form [formGroup]="form" style="display:flex;flex-direction:column;gap:12px;padding-top:8px;min-width:360px;">
        <mat-form-field>
          <mat-label>Employee ID</mat-label>
          <input matInput formControlName="empId" type="number">
          <mat-error>Required</mat-error>
        </mat-form-field>
        <ng-container *ngIf="data">
          <mat-form-field>
            <mat-label>Attendance Date</mat-label>
            <input matInput [matDatepicker]="dp" formControlName="attendanceDate">
            <mat-datepicker-toggle matSuffix [for]="dp"></mat-datepicker-toggle>
            <mat-datepicker #dp></mat-datepicker>
          </mat-form-field>
          <mat-form-field>
            <mat-label>Check-In Time (HH:mm)</mat-label>
            <input matInput formControlName="checkInTime" placeholder="09:00">
            <mat-error>Required</mat-error>
          </mat-form-field>
          <mat-form-field>
            <mat-label>Check-Out Time (HH:mm, optional)</mat-label>
            <input matInput formControlName="checkOutTime" placeholder="18:00">
          </mat-form-field>
        </ng-container>
        <mat-form-field>
          <mat-label>Work Mode</mat-label>
          <mat-select formControlName="workMode">
            <mat-option value="WFO">WFO</mat-option>
            <mat-option value="WFH">WFH</mat-option>
            <mat-option value="Hybrid">Hybrid</mat-option>
          </mat-select>
        </mat-form-field>
      </form>
    </mat-dialog-content>
    <mat-dialog-actions align="end">
      <button mat-button mat-dialog-close>Cancel</button>
      <button mat-raised-button color="primary" (click)="save()" [disabled]="form.invalid">Save</button>
    </mat-dialog-actions>
  `
})
export class AttendanceFormDialogComponent implements OnInit {
  form = this.fb.group({
    empId: [null as any, Validators.required],
    attendanceDate: [new Date()],
    checkInTime: ['09:00'],
    checkOutTime: [''],
    workMode: ['WFO', Validators.required]
  });

  constructor(private fb: FormBuilder, private svc: AttendanceService,
    private snack: MatSnackBar, private ref: MatDialogRef<AttendanceFormDialogComponent>,
    private auth: AuthService,
    @Inject(MAT_DIALOG_DATA) public data: Attendance | null) {}

  ngOnInit() {
    if (this.data) {
      const ci = new Date(this.data.checkIn);
      const co = this.data.checkOut ? new Date(this.data.checkOut) : null;
      this.form.patchValue({
        empId: this.data.empId,
        attendanceDate: new Date(this.data.attendanceDate),
        checkInTime: this.toHHMM(ci),
        checkOutTime: co ? this.toHHMM(co) : '',
        workMode: this.data.workMode || 'WFO'
      });
    }
    
    const role = this.auth.getRole();
    if (role === 'Employee') {
      const empId = this.auth.getEmpId();
      if (empId) {
        this.form.patchValue({ empId: empId });
      }
      this.form.get('empId')?.disable();
    }
  }

  save() {
    if (this.form.invalid) return;
    const v = this.form.value;
    const dateStr = this.fmt(v.attendanceDate as any);
    const checkIn = `${dateStr}T${v.checkInTime}:00`;
    const checkOut = v.checkOutTime ? `${dateStr}T${v.checkOutTime}:00` : null;

    if (this.data) {
      const payload = {
        attendanceId: this.data.attendanceId, empId: this.form.get('empId')?.value,
        checkIn, checkOut, workMode: v.workMode,
        attendanceDate: dateStr
      };
      this.svc.update(payload).subscribe({
        next: () => { this.snack.open('Updated', '', { duration: 2000 }); this.ref.close(true); },
        error: () => this.snack.open('Error', '', { duration: 3000 })
      });
    } else {
      const payload: any = { empId: this.form.get('empId')?.value, workMode: v.workMode };
      this.svc.checkIn(payload).subscribe({
        next: () => { this.snack.open('Checked in', '', { duration: 2000 }); this.ref.close(true); },
        error: (err) => this.snack.open(err.error?.message || 'Error checking in', '', { duration: 3000 })
      });
    }
  }

  private fmt(d: any): string { return new Date(d).toISOString().split('T')[0]; }
  private toHHMM(d: Date): string {
    return `${String(d.getHours()).padStart(2,'0')}:${String(d.getMinutes()).padStart(2,'0')}`;
  }
}
