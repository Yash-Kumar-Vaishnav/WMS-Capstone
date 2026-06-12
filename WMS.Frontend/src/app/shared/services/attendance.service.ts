import { environment } from '../../../environments/environment';
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Attendance, CreateAttendanceDto } from '../models/models';

export interface CheckInDto { empId: number; checkIn: string; workMode: string; attendanceDate: string; }
export interface CheckOutDto { checkOut: string; }

@Injectable({ providedIn: 'root' })
export class AttendanceService {
  private base = '${environment.apiUrl}/attendance';
  constructor(private http: HttpClient) {}
  getAll() { return this.http.get<Attendance[]>(this.base); }
  getById(id: number) { return this.http.get<Attendance>(`${this.base}/${id}`); }
  getByEmployee(empId: number) { return this.http.get<Attendance[]>(`${this.base}/employee/${empId}`); }
  getMonthly(empId: number, year: number, month: number) {
    return this.http.get<Attendance[]>(`${this.base}/monthly/${empId}/${year}/${month}`);
  }
  exportTimesheet(year: number, month: number) {
    return this.http.get(`${this.base}/timesheet/export/${year}/${month}`, { responseType: 'blob' });
  }
  checkIn(dto: CheckInDto) { return this.http.post<any>(`${this.base}/checkin`, dto); }
  checkOut(id: number, dto: CheckOutDto) { return this.http.put(`${this.base}/checkout/${id}`, dto); }
  create(dto: CreateAttendanceDto) { return this.http.post<number>(this.base, dto); }
  update(dto: any) { return this.http.put(this.base, dto); }
  delete(id: number) { return this.http.delete(`${this.base}/${id}`); }
}


