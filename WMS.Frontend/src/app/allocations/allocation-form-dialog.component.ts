import { Component, Inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, Validators } from '@angular/forms';
import { MatDialogModule, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatSelectModule } from '@angular/material/select';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { AllocationService } from '../shared/services/allocation.service';
import { EmployeeProjectAllocation } from '../shared/models/models';
import { AuthService } from '../shared/services/auth.service';

@Component({
  selector: 'app-allocation-form-dialog',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, MatDialogModule, MatFormFieldModule,
    MatInputModule, MatSelectModule, MatButtonModule, MatDatepickerModule, MatNativeDateModule, MatSnackBarModule],
  template: `
    <h2 mat-dialog-title>{{ data ? 'Edit' : 'Assign' }} Allocation</h2>
    <mat-dialog-content>
      <form [formGroup]="form" style="display:flex;flex-direction:column;gap:12px;padding-top:8px;min-width:300px;">
        <div *ngIf="form.errors?.['pastAssignedDate']" style="color:red;font-size:12px;">Project assignment date cannot be earlier than today.</div>
        <mat-form-field>
          <mat-label>Employee ID</mat-label>
          <input matInput formControlName="empId" type="number">
          <mat-error>Required</mat-error>
        </mat-form-field>

        <mat-form-field>
          <mat-label>Project ID</mat-label>
          <input matInput formControlName="projectId" type="number">
          <mat-error>Required</mat-error>
        </mat-form-field>

        <mat-form-field>
          <mat-label>Assigned On</mat-label>
          <input matInput [matDatepicker]="picker" formControlName="assignedOn" [min]="today">
          <mat-datepicker-toggle matSuffix [for]="picker"></mat-datepicker-toggle>
          <mat-datepicker #picker></mat-datepicker>
          <mat-error>Required</mat-error>
        </mat-form-field>

        <mat-form-field *ngIf="data">
          <mat-label>Status</mat-label>
          <mat-select formControlName="status">
            <mat-option [value]="true">Active</mat-option>
            <mat-option [value]="false">Inactive</mat-option>
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
export class AllocationFormDialogComponent implements OnInit {
  today = new Date();

  form = this.fb.group({
    empId: [null as any, Validators.required],
    projectId: [null as any, Validators.required],
    assignedOn: [new Date(), Validators.required],
    status: [true]
  }, { validators: this.dateValidator });

  dateValidator(group: any) {
    const assigned = group.get('assignedOn')?.value;
    if (!assigned) return null;

    const today = new Date();
    today.setHours(0, 0, 0, 0);
    const assignedDate = new Date(assigned);
    assignedDate.setHours(0, 0, 0, 0);

    return assignedDate < today ? { pastAssignedDate: true } : null;
  }

  constructor(private fb: FormBuilder, private svc: AllocationService, private snack: MatSnackBar,
    private ref: MatDialogRef<AllocationFormDialogComponent>,
    private auth: AuthService,
    @Inject(MAT_DIALOG_DATA) public data: EmployeeProjectAllocation | null) {}

  ngOnInit() {
    if (this.data) {
      this.form.patchValue({
        empId: this.data.empId,
        projectId: this.data.projectId,
        assignedOn: new Date(this.data.assignedOn),
        status: this.data.status
      });
    }
  }

  save() {
    if (this.form.invalid) return;
    const v = this.form.value as any;
    v.assignedOn = v.assignedOn ? new Date(v.assignedOn).toISOString() : new Date().toISOString();
    v.createdBy = this.auth.getUsername();
    
    const obs = this.data 
      ? this.svc.update({ ...v, allocationId: this.data.allocationId }) 
      : this.svc.create(v);
      
    obs.subscribe({
      next: () => { this.snack.open('Saved','',{duration:2000}); this.ref.close(true); },
      error: () => this.snack.open('Error saving','',{duration:3000})
    });
  }
}
