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
import { EmployeeService } from '../shared/services/employee.service';
import { DepartmentService } from '../shared/services/department.service';
import { Employee, Department } from '../shared/models/models';

@Component({
  selector: 'app-employee-form-dialog',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, MatDialogModule, MatFormFieldModule,
    MatInputModule, MatSelectModule, MatButtonModule, MatDatepickerModule, MatNativeDateModule, MatSnackBarModule],
  template: `
    <h2 mat-dialog-title>{{ data ? 'Edit' : 'Add' }} Employee</h2>
    <mat-dialog-content style="min-width:500px;">
      <form [formGroup]="form" style="display:grid;grid-template-columns:1fr 1fr;gap:12px;padding-top:8px;">
        <mat-form-field>
          <mat-label>First Name</mat-label>
          <input matInput formControlName="firstName">
          <mat-error>Required</mat-error>
        </mat-form-field>
        <mat-form-field>
          <mat-label>Last Name</mat-label>
          <input matInput formControlName="lastName">
          <mat-error>Required</mat-error>
        </mat-form-field>
        <mat-form-field>
          <mat-label>Email</mat-label>
          <input matInput formControlName="email" type="email">
          <mat-error>Valid email required</mat-error>
        </mat-form-field>
        <mat-form-field>
          <mat-label>Phone Number</mat-label>
          <input matInput formControlName="phoneNumber" maxlength="15">
          <mat-error>Required</mat-error>
        </mat-form-field>
        <mat-form-field>
          <mat-label>Gender</mat-label>
          <mat-select formControlName="gender">
            <mat-option value="M">Male</mat-option>
            <mat-option value="F">Female</mat-option>
            <mat-option value="O">Other</mat-option>
          </mat-select>
        </mat-form-field>
        <mat-form-field>
          <mat-label>Date of Birth</mat-label>
          <input matInput [matDatepicker]="dobPicker" formControlName="dob" [max]="today">
          <mat-datepicker-toggle matSuffix [for]="dobPicker"></mat-datepicker-toggle>
          <mat-datepicker #dobPicker></mat-datepicker>
          <mat-error *ngIf="form.get('dob')?.hasError('required')">Required</mat-error>
          <mat-error *ngIf="form.get('dob')?.hasError('matDatepickerMax')">Future dates are not allowed</mat-error>
        </mat-form-field>
        <mat-form-field>
          <mat-label>Date of Joining</mat-label>
          <input matInput [matDatepicker]="dojPicker" formControlName="doj" [max]="today">
          <mat-datepicker-toggle matSuffix [for]="dojPicker"></mat-datepicker-toggle>
          <mat-datepicker #dojPicker></mat-datepicker>
          <mat-error *ngIf="form.get('doj')?.hasError('required')">Required</mat-error>
          <mat-error *ngIf="form.get('doj')?.hasError('matDatepickerMax')">Future dates are not allowed</mat-error>
        </mat-form-field>
        <mat-form-field>
          <mat-label>Department</mat-label>
          <mat-select formControlName="departmentId">
            <mat-option *ngFor="let d of departments" [value]="d.departmentId">{{d.departmentName}}</mat-option>
          </mat-select>
        </mat-form-field>
        <mat-form-field>
          <mat-label>Role</mat-label>
          <mat-select formControlName="roleId">
            <mat-option [value]="1">Admin</mat-option>
            <mat-option [value]="2">Manager</mat-option>
            <mat-option [value]="3">Employee</mat-option>
          </mat-select>
        </mat-form-field>
        <mat-form-field>
          <mat-label>Status</mat-label>
          <mat-select formControlName="status">
            <mat-option value="Active">Active</mat-option>
            <mat-option value="Inactive">Inactive</mat-option>
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
export class EmployeeFormDialogComponent implements OnInit {
  today = new Date();
  departments: Department[] = [];
  form = this.fb.group({
    firstName: ['', Validators.required],
    lastName: ['', Validators.required],
    email: ['', [Validators.required, Validators.email]],
    phoneNumber: ['', Validators.required],
    gender: ['M', Validators.required],
    dob: [null as any, Validators.required],
    doj: [null as any, Validators.required],
    departmentId: [null as any, Validators.required],
    roleId: [null as any, Validators.required],
    status: ['Active']
  });

  constructor(private fb: FormBuilder, private svc: EmployeeService,
    private deptSvc: DepartmentService, private snack: MatSnackBar,
    private ref: MatDialogRef<EmployeeFormDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: Employee | null) {}

  ngOnInit() {
    this.deptSvc.getAll().subscribe(d => this.departments = d);
    if (this.data) this.form.patchValue({ ...this.data, dob: new Date(this.data.dob), doj: new Date(this.data.doj) });
  }

  save() {
    if (this.form.invalid) return;
    const v = this.form.value;
    const payload: any = { ...v, dob: this.fmt(v.dob), doj: this.fmt(v.doj) };
    const obs = this.data
      ? this.svc.update({ ...payload, employeeId: this.data.employeeId })
      : this.svc.create(payload);
    obs.subscribe({ next: () => { this.snack.open('Saved','',{duration:2000}); this.ref.close(true); },
      error: e => this.snack.open(e.error?.message ?? 'Error','',{duration:3000}) });
  }

  private fmt(d: any): string {
    if (!d) return '';
    const dt = new Date(d);
    return dt.toISOString().split('T')[0];
  }
}
