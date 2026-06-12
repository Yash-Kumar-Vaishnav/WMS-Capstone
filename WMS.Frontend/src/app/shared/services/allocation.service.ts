import { environment } from '../../../environments/environment';
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { EmployeeProjectAllocation, CreateAllocationDto, UpdateAllocationDto } from '../models/models';

@Injectable({ providedIn: 'root' })
export class AllocationService {
  private base = '${environment.apiUrl}/employeeprojectallocation';

  constructor(private http: HttpClient) {}

  getAll() { return this.http.get<EmployeeProjectAllocation[]>(this.base); }
  getById(id: number) { return this.http.get<EmployeeProjectAllocation>(`${this.base}/${id}`); }
  getByProject(projectId: number) { return this.http.get<EmployeeProjectAllocation[]>(`${this.base}/project/${projectId}`); }
  getByEmployee(empId: number) { return this.http.get<EmployeeProjectAllocation[]>(`${this.base}/employee/${empId}`); }
  create(dto: CreateAllocationDto) { return this.http.post<any>(this.base, dto); }
  update(dto: UpdateAllocationDto) { return this.http.put(this.base, dto); }
  delete(id: number) { return this.http.delete(`${this.base}/${id}`); }
}


