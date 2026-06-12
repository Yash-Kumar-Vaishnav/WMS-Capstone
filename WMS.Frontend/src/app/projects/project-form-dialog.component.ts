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
import { ProjectService } from '../shared/services/project.service';
import { Project } from '../shared/models/models';

@Component({
  selector: 'app-project-form-dialog',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, MatDialogModule, MatFormFieldModule,
    MatInputModule, MatSelectModule, MatButtonModule, MatDatepickerModule, MatNativeDateModule, MatSnackBarModule],
  template: `
    <h2 mat-dialog-title>{{ data ? 'Edit' : 'Add' }} Project</h2>
    <mat-dialog-content>
      <form [formGroup]="form" style="display:flex;flex-direction:column;gap:12px;padding-top:8px;min-width:380px;">
        <div *ngIf="form.errors?.['pastStartDate']" style="color:red;font-size:12px;">Project Start Date cannot be earlier than today.</div>
        <div *ngIf="form.errors?.['invalidEndDate']" style="color:red;font-size:12px;">End Date cannot be earlier than Start Date.</div>
        <mat-form-field>
          <mat-label>Project Name</mat-label>
          <input matInput formControlName="projectName">
          <mat-error>Required</mat-error>
        </mat-form-field>
        <mat-form-field>
          <mat-label>Start Date</mat-label>
          <input matInput [matDatepicker]="sp" formControlName="startDate" [min]="today">
          <mat-datepicker-toggle matSuffix [for]="sp"></mat-datepicker-toggle>
          <mat-datepicker #sp></mat-datepicker>
        </mat-form-field>
        <mat-form-field>
          <mat-label>End Date</mat-label>
          <input matInput [matDatepicker]="ep" formControlName="endDate" [min]="form.value.startDate || today">
          <mat-datepicker-toggle matSuffix [for]="ep"></mat-datepicker-toggle>
          <mat-datepicker #ep></mat-datepicker>
        </mat-form-field>
        <mat-form-field>
          <mat-label>Status</mat-label>
          <mat-select formControlName="status">
            <mat-option value="Active">Active</mat-option>
            <mat-option value="Completed">Completed</mat-option>
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
export class ProjectFormDialogComponent implements OnInit {
  today = new Date();

  form = this.fb.group({
    projectName: ['', Validators.required],
    startDate: [null as any],
    endDate: [null as any],
    status: ['Active']
  }, { validators: this.dateValidator });

  constructor(private fb: FormBuilder, private svc: ProjectService, private snack: MatSnackBar,
    private ref: MatDialogRef<ProjectFormDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: Project | null) {}

  dateValidator(group: any) {
    const start = group.get('startDate')?.value;
    const end = group.get('endDate')?.value;
    
    if (!start) return null;
    
    const errors: any = {};
    const today = new Date();
    today.setHours(0, 0, 0, 0);
    const startDate = new Date(start);
    startDate.setHours(0, 0, 0, 0);

    if (startDate < today) errors.pastStartDate = true;

    if (end) {
      const endDate = new Date(end);
      endDate.setHours(0, 0, 0, 0);
      if (endDate < startDate) errors.invalidEndDate = true;
    }

    return Object.keys(errors).length ? errors : null;
  }

  ngOnInit() {
    if (this.data) this.form.patchValue({
      ...this.data,
      startDate: this.data.startDate ? new Date(this.data.startDate) : null,
      endDate: this.data.endDate ? new Date(this.data.endDate) : null
    });
  }

  save() {
    if (this.form.invalid) return;
    const v = this.form.value;
    const payload: any = { ...v, startDate: this.fmt(v.startDate), endDate: this.fmt(v.endDate) };
    const obs = this.data ? this.svc.update({ ...payload, projectId: this.data.projectId }) : this.svc.create(payload);
    obs.subscribe({ next: () => { this.snack.open('Saved','',{duration:2000}); this.ref.close(true); },
      error: () => this.snack.open('Error','',{duration:3000}) });
  }

  private fmt(d: any) { return d ? new Date(d).toISOString().split('T')[0] : null; }
}
