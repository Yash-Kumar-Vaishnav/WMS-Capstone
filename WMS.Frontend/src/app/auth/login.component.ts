import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { AuthService } from '../shared/services/auth.service';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, MatCardModule, MatFormFieldModule,
    MatInputModule, MatButtonModule, MatIconModule, MatSnackBarModule],
  template: `
    <div style="display:flex;justify-content:center;align-items:center;height:100vh;background:#3f51b5;">
      <mat-card style="width:380px;padding:24px;">
        <mat-card-header style="justify-content:center;margin-bottom:16px;">
          <mat-card-title style="font-size:24px;">Workforce Management</mat-card-title>
          <mat-card-subtitle>Sign in to your account</mat-card-subtitle>
        </mat-card-header>
        <mat-card-content>
          <form [formGroup]="form" (ngSubmit)="login()">
            <mat-form-field class="full-width">
              <mat-label>Username</mat-label>
              <input matInput formControlName="username" autocomplete="username">
              <mat-error *ngIf="form.get('username')?.hasError('required')">Username is required</mat-error>
            </mat-form-field>
            <mat-form-field class="full-width" style="margin-top:8px;">
              <mat-label>Password</mat-label>
              <input matInput [type]="hide ? 'password' : 'text'" formControlName="password" autocomplete="current-password">
              <button mat-icon-button matSuffix type="button" (click)="hide=!hide">
                <mat-icon>{{hide ? 'visibility_off' : 'visibility'}}</mat-icon>
              </button>
              <mat-error *ngIf="form.get('password')?.hasError('required')">Password is required</mat-error>
            </mat-form-field>
            <button mat-raised-button color="primary" class="full-width" style="margin-top:16px;" type="submit" [disabled]="loading">
              {{ loading ? 'Signing in...' : 'Login' }}
            </button>
          </form>
        </mat-card-content>
      </mat-card>
    </div>
  `
})
export class LoginComponent {
  hide = true; loading = false;
  form = this.fb.group({
    username: ['', Validators.required],
    password: ['', Validators.required]
  });
  constructor(private fb: FormBuilder, private auth: AuthService,
    private router: Router, private snack: MatSnackBar) {}

  login() {
    if (this.form.invalid) return;
    this.loading = true;
    this.auth.login(this.form.value as any).subscribe({
      next: () => this.router.navigate(['/']),
      error: () => {
        this.snack.open('Invalid username or password', 'Close', { duration: 3000 });
        this.loading = false;
      }
    });
  }
}
