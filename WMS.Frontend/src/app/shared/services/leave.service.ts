import { environment } from '../../../environments/environment';
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { CreateLeaveDto, Leave, UpdateLeaveDto } from '../models/models';

@Injectable({ providedIn: 'root' })
export class LeaveService {
  private base = '${environment.apiUrl}/leave';
  constructor(private http: HttpClient) {}
  getAll() { return this.http.get<Leave[]>(this.base); }
  getById(id: number) { return this.http.get<Leave>(`${this.base}/${id}`); }
  getByEmployee(empId: number) { return this.http.get<Leave[]>(`${this.base}/employee/${empId}`); }
  create(dto: CreateLeaveDto) { return this.http.post<number>(this.base, dto); }
  update(dto: UpdateLeaveDto) { return this.http.put(this.base, dto); }
  approve(id: number) { return this.http.put(`${this.base}/${id}/approve`, {}); }
  reject(id: number) { return this.http.put(`${this.base}/${id}/reject`, {}); }
  cancel(id: number) { return this.http.put(`${this.base}/${id}/cancel`, {}); }
  delete(id: number) { return this.http.delete(`${this.base}/${id}`); }
}


