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
import { LeaveService } from '../shared/services/leave.service';
import { AuthService } from '../shared/services/auth.service';
import { Leave } from '../shared/models/models';

@Component({
  selector: 'app-leave-form-dialog',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, MatDialogModule, MatFormFieldModule,
    MatInputModule, MatSelectModule, MatButtonModule, MatDatepickerModule, MatNativeDateModule, MatSnackBarModule],
  template: `
    <h2 mat-dialog-title>{{ data ? 'Manage' : 'Apply' }} Leave</h2>
    <mat-dialog-content>
      <form [formGroup]="form" style="display:flex;flex-direction:column;gap:12px;padding-top:8px;min-width:380px;">
        <mat-form-field>
          <mat-label>Employee ID</mat-label>
          <input matInput formControlName="empId" type="number">
          <mat-error>Required</mat-error>
        </mat-form-field>
        <mat-form-field>
          <mat-label>Leave Type</mat-label>
          <mat-select formControlName="leaveType">
            <mat-option value="Sick">Sick</mat-option>
            <mat-option value="Casual">Casual</mat-option>
            <mat-option value="Earned">Earned</mat-option>
          </mat-select>
        </mat-form-field>
        <mat-form-field>
          <mat-label>From Date</mat-label>
          <input matInput [matDatepicker]="fromDp" formControlName="fromDate" [min]="today">
          <mat-datepicker-toggle matSuffix [for]="fromDp"></mat-datepicker-toggle>
          <mat-datepicker #fromDp></mat-datepicker>
          <mat-error *ngIf="form.get('fromDate')?.hasError('required')">Required</mat-error>
          <mat-error *ngIf="form.get('fromDate')?.hasError('matDatepickerMin')">From Date cannot be in the past.</mat-error>
        </mat-form-field>
        <mat-form-field>
          <mat-label>To Date</mat-label>
          <input matInput [matDatepicker]="toDp" formControlName="toDate" [min]="form.get('fromDate')?.value || today">
          <mat-datepicker-toggle matSuffix [for]="toDp"></mat-datepicker-toggle>
          <mat-datepicker #toDp></mat-datepicker>
          <mat-error *ngIf="form.get('toDate')?.hasError('required')">Required</mat-error>
          <mat-error *ngIf="form.get('toDate')?.hasError('matDatepickerMin')">To Date cannot be earlier than From Date.</mat-error>
        </mat-form-field>
        <mat-form-field>
          <mat-label>Reason</mat-label>
          <textarea matInput formControlName="reason" rows="2"></textarea>
        </mat-form-field>
        <mat-form-field *ngIf="data">
          <mat-label>Status (Approval)</mat-label>
          <mat-select formControlName="status">
            <mat-option value="Pending">Pending</mat-option>
            <mat-option value="Approved">Approved</mat-option>
            <mat-option value="Rejected">Rejected</mat-option>
            <mat-option value="Cancelled">Cancelled</mat-option>
          </mat-select>
        </mat-form-field>
        <mat-form-field *ngIf="data">
          <mat-label>Approved By (Manager Emp ID)</mat-label>
          <input matInput formControlName="approvedBy" type="number">
        </mat-form-field>
      </form>
    </mat-dialog-content>
    <mat-dialog-actions align="end">
      <button mat-button mat-dialog-close>Cancel</button>
      <button mat-raised-button color="primary" (click)="save()" [disabled]="form.invalid || formHasDateErrors()">Save</button>
    </mat-dialog-actions>
  `
})
export class LeaveFormDialogComponent implements OnInit {
  today = new Date();
  
  form = this.fb.group({
    empId: [null as any, Validators.required],
    leaveType: ['Sick', Validators.required],
    fromDate: [null as any, Validators.required],
    toDate: [null as any, Validators.required],
    reason: [''],
    status: ['Pending'],
    approvedBy: [null as any]
  });

  constructor(private fb: FormBuilder, private svc: LeaveService, private snack: MatSnackBar,
    private ref: MatDialogRef<LeaveFormDialogComponent>, private auth: AuthService,
    @Inject(MAT_DIALOG_DATA) public data: Leave | null) {
      
    // Remove the time portion from today to prevent strict same-day blocking
    this.today.setHours(0, 0, 0, 0);
  }

  ngOnInit() {
    // When FromDate changes, if ToDate is earlier, reset ToDate
    this.form.get('fromDate')?.valueChanges.subscribe(newFromDate => {
      const currentToDate = this.form.get('toDate')?.value;
      if (newFromDate && currentToDate && new Date(currentToDate) < new Date(newFromDate)) {
        this.form.get('toDate')?.setValue(null as any);
      }
    });

    if (this.data) this.form.patchValue({
      ...this.data,
      fromDate: new Date(this.data.fromDate),
      toDate: new Date(this.data.toDate)
    });
    
    const role = this.auth.getRole();
    if (role === 'Employee') {
      const empId = this.auth.getEmpId();
      if (empId) {
        this.form.patchValue({ empId: empId });
      }
      this.form.get('empId')?.disable();
    }
  }

  formHasDateErrors(): boolean {
    const from = this.form.get('fromDate')?.value;
    const to = this.form.get('toDate')?.value;
    if (from && to && new Date(to) < new Date(from)) return true;
    return false;
  }

  save() {
    if (this.form.invalid || this.formHasDateErrors()) return;
    const v = this.form.value;
    const payload: any = { ...v, empId: this.form.get('empId')?.value, fromDate: this.fmt(v.fromDate), toDate: this.fmt(v.toDate) };
    
    let obs: any;
    if (this.data && this.data.status !== v.status) {
      if (v.status === 'Approved') obs = this.svc.approve(this.data.leaveId);
      else if (v.status === 'Rejected') obs = this.svc.reject(this.data.leaveId);
      else if (v.status === 'Cancelled') obs = this.svc.cancel(this.data.leaveId);
      else obs = this.svc.update({ ...payload, leaveId: this.data.leaveId });
    } else {
      obs = this.data ? this.svc.update({ ...payload, leaveId: this.data.leaveId }) : this.svc.create(payload);
    }

    obs.subscribe({ next: () => { this.snack.open('Saved','',{duration:2000}); this.ref.close(true); },
      error: (e: any) => this.snack.open(e.error?.message || 'Error saving','',{duration:3000}) });
  }

  private fmt(d: any) { return d ? new Date(d).toISOString().split('T')[0] : ''; }
}
