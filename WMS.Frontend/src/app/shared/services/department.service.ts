import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { CreateDepartmentDto, Department, UpdateDepartmentDto } from '../models/models';

@Injectable({ providedIn: 'root' })
export class DepartmentService {
  private base = 'http://localhost:5176/api/department';
  constructor(private http: HttpClient) {}
  getAll() { return this.http.get<Department[]>(this.base); }
  getById(id: number) { return this.http.get<Department>(`${this.base}/${id}`); }
  create(dto: CreateDepartmentDto) { return this.http.post<number>(this.base, dto); }
  update(dto: UpdateDepartmentDto) { return this.http.put(this.base, dto); }
  delete(id: number) { return this.http.delete(`${this.base}/${id}`); }
}
