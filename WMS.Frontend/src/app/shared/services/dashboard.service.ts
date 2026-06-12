import { environment } from '../../../environments/environment';
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { DashboardSummary } from '../models/models';

@Injectable({ providedIn: 'root' })
export class DashboardService {
  private base = environment.apiUrl + '/dashboard';
  constructor(private http: HttpClient) {}
  getSummary() { return this.http.get<DashboardSummary>(`${this.base}/summary`); }
  getLeaveStats() { return this.http.get<any[]>(`${this.base}/leave-stats`); }
  getAttendanceChart() { return this.http.get<any[]>(`${this.base}/attendance-chart`); }
  getProjectCounts() { return this.http.get<any>(`${this.base}/project-counts`); }
}




