import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';
import { MatDividerModule } from '@angular/material/divider';
import { DashboardService } from '../shared/services/dashboard.service';
import { AuthService } from '../shared/services/auth.service';
import { DashboardSummary } from '../shared/models/models';
import { NgChartsModule } from 'ng2-charts';
import { ChartData, ChartOptions } from 'chart.js';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [CommonModule, MatCardModule, MatIconModule, MatDividerModule, NgChartsModule],
  template: `
    <div class="page-container">
      <h2 style="margin-bottom:20px;">Dashboard</h2>

      <!-- KPI Cards -->
      <div class="card-grid">
        <mat-card class="summary-card" *ngIf="isAdminOrManager">
          <mat-card-content>
            <mat-icon style="font-size:40px;color:#3f51b5;">people</mat-icon>
            <mat-card-title>{{ summary?.totalEmployees ?? 0 }}</mat-card-title>
            <mat-card-subtitle>Total Employees</mat-card-subtitle>
          </mat-card-content>
        </mat-card>
        <mat-card class="summary-card">
          <mat-card-content>
            <mat-icon style="font-size:40px;color:#4caf50;">access_time</mat-icon>
            <mat-card-title>{{ summary?.todayAttendanceCount ?? 0 }}</mat-card-title>
            <mat-card-subtitle>Today's Attendance</mat-card-subtitle>
          </mat-card-content>
        </mat-card>
        <mat-card class="summary-card">
          <mat-card-content>
            <mat-icon style="font-size:40px;color:#ff9800;">event_busy</mat-icon>
            <mat-card-title>{{ summary?.pendingLeaveRequests ?? 0 }}</mat-card-title>
            <mat-card-subtitle>Pending Leaves</mat-card-subtitle>
          </mat-card-content>
        </mat-card>
        <mat-card class="summary-card">
          <mat-card-content>
            <mat-icon style="font-size:40px;color:#2196f3;">folder</mat-icon>
            <mat-card-title>{{ summary?.activeProjects ?? 0 }}</mat-card-title>
            <mat-card-subtitle>Active Projects</mat-card-subtitle>
          </mat-card-content>
        </mat-card>
        <mat-card class="summary-card" *ngIf="isAdminOrManager">
          <mat-card-content>
            <mat-icon style="font-size:40px;color:#9c27b0;">business</mat-icon>
            <mat-card-title>{{ summary?.totalDepartments ?? 0 }}</mat-card-title>
            <mat-card-subtitle>Departments</mat-card-subtitle>
          </mat-card-content>
        </mat-card>
      </div>

      <!-- Charts Row -->
      <div style="display:grid;grid-template-columns:1fr 1fr;gap:16px;flex-wrap:wrap;">
        <mat-card>
          <mat-card-header><mat-card-title>Leave Statistics</mat-card-title></mat-card-header>
          <mat-card-content style="height:280px;">
            <canvas *ngIf="leaveChartData.datasets[0].data.length" baseChart
              [data]="leaveChartData" [type]="'doughnut'" [options]="pieOptions"></canvas>
          </mat-card-content>
        </mat-card>
        <mat-card>
          <mat-card-header><mat-card-title>Attendance (Last 30 Days)</mat-card-title></mat-card-header>
          <mat-card-content style="height:280px;">
            <canvas *ngIf="attendanceChartData.labels?.length" baseChart
              [data]="attendanceChartData" [type]="'bar'" [options]="barOptions"></canvas>
          </mat-card-content>
        </mat-card>
      </div>
    </div>
  `
})
export class DashboardComponent implements OnInit {
  summary: DashboardSummary | null = null;

  leaveChartData: ChartData<'doughnut'> = {
    labels: ['Pending', 'Approved', 'Rejected'],
    datasets: [{ data: [], backgroundColor: ['#ff9800','#4caf50','#f44336'] }]
  };
  pieOptions: ChartOptions<'doughnut'> = { responsive: true, maintainAspectRatio: false };

  attendanceChartData: ChartData<'bar'> = {
    labels: [], datasets: [{ label: 'Count', data: [], backgroundColor: '#3f51b5' }]
  };
  barOptions: ChartOptions<'bar'> = { responsive: true, maintainAspectRatio: false };
  isAdminOrManager = false;

  constructor(private svc: DashboardService, private auth: AuthService) {}

  ngOnInit() {
    const role = this.auth.getRole();
    this.isAdminOrManager = role === 'Admin' || role === 'Manager';
    this.svc.getSummary().subscribe(d => this.summary = d);
    this.svc.getLeaveStats().subscribe(stats => {
      const order = ['Pending','Approved','Rejected'];
      this.leaveChartData = { ...this.leaveChartData,
        datasets: [{ data: order.map(s => stats.find(x => x.status===s)?.count ?? 0),
          backgroundColor: ['#ff9800','#4caf50','#f44336'] }]
      };
    });
    this.svc.getAttendanceChart().subscribe(data => {
      this.attendanceChartData = {
        labels: data.map(d => new Date(d.date).toLocaleDateString('en-IN',{month:'short',day:'numeric'})),
        datasets: [{ label: 'Attendance', data: data.map(d => d.count), backgroundColor: '#3f51b5' }]
      };
    });
  }
}
