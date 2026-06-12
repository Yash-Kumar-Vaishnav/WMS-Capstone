import { environment } from '../../../environments/environment';
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { CreateProjectDto, Project } from '../models/models';

@Injectable({ providedIn: 'root' })
export class ProjectService {
  private base = environment.apiUrl + '/project';
  constructor(private http: HttpClient) {}
  getAll() { return this.http.get<Project[]>(this.base); }
  getById(id: number) { return this.http.get<Project>(`${this.base}/${id}`); }
  create(dto: CreateProjectDto) { return this.http.post<number>(this.base, dto); }
  update(dto: any) { return this.http.put(this.base, dto); }
  delete(id: number) { return this.http.delete(`${this.base}/${id}`); }
}




