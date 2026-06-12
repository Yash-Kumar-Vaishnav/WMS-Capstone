import { Component, Inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, Validators } from '@angular/forms';
import { MatDialogModule, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { DepartmentService } from '../shared/services/department.service';
import { Department } from '../shared/models/models';

@Component({
  selector: 'app-department-form-dialog',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, MatDialogModule, MatFormFieldModule,
    MatInputModule, MatButtonModule, MatSnackBarModule],
  template: `
    <h2 mat-dialog-title>{{ data ? 'Edit' : 'Add' }} Department</h2>
    <mat-dialog-content>
      <form [formGroup]="form" style="display:flex;flex-direction:column;gap:12px;padding-top:8px;min-width:340px;">
        <mat-form-field>
          <mat-label>Department Name</mat-label>
          <input matInput formControlName="departmentName">
          <mat-error>Required</mat-error>
        </mat-form-field>
        <mat-form-field>
          <mat-label>Description</mat-label>
          <textarea matInput formControlName="description" rows="3"></textarea>
        </mat-form-field>
      </form>
    </mat-dialog-content>
    <mat-dialog-actions align="end">
      <button mat-button mat-dialog-close>Cancel</button>
      <button mat-raised-button color="primary" (click)="save()" [disabled]="form.invalid">Save</button>
    </mat-dialog-actions>
  `
})
export class DepartmentFormDialogComponent implements OnInit {
  form = this.fb.group({
    departmentName: ['', Validators.required],
    description: ['']
  });

  constructor(private fb: FormBuilder, private svc: DepartmentService, private snack: MatSnackBar,
    private ref: MatDialogRef<DepartmentFormDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: Department | null) {}

  ngOnInit() { if (this.data) this.form.patchValue(this.data); }

  save() {
    if (this.form.invalid) return;
    const obs = this.data
      ? this.svc.update({ ...this.form.value, departmentId: this.data.departmentId } as any)
      : this.svc.create(this.form.value as any);
    obs.subscribe({ next: () => { this.snack.open('Saved','',{duration:2000}); this.ref.close(true); },
      error: () => this.snack.open('Error','',{duration:3000}) });
  }
}
