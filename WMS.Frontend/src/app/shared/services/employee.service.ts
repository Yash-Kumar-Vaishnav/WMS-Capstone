import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { CreateEmployeeDto, Employee, UpdateEmployeeDto } from '../models/models';

@Injectable({ providedIn: 'root' })
export class EmployeeService {
  private base = 'http://localhost:5176/api/employee';
  constructor(private http: HttpClient) {}
  getAll() { return this.http.get<Employee[]>(this.base); }
  getById(id: number) { return this.http.get<Employee>(`${this.base}/${id}`); }
  search(name?: string, departmentId?: number, roleId?: number, status?: string) {
    let params = new HttpParams();
    if (name) params = params.set('name', name);
    if (departmentId) params = params.set('departmentId', departmentId.toString());
    if (roleId) params = params.set('roleId', roleId.toString());
    if (status) params = params.set('status', status);
    return this.http.get<Employee[]>(`${this.base}/search`, { params });
  }
  create(dto: CreateEmployeeDto) { return this.http.post<number>(this.base, dto); }
  update(dto: UpdateEmployeeDto) { return this.http.put(this.base, dto); }
  delete(id: number) { return this.http.delete(`${this.base}/${id}`); }
}
