import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Announcement } from '../models/models';

@Injectable({ providedIn: 'root' })
export class AnnouncementService {
  private base = 'http://localhost:5176/api/announcement';
  constructor(private http: HttpClient) {}
  getAll() { return this.http.get<Announcement[]>(this.base); }
  getActive() { return this.http.get<Announcement[]>(`${this.base}/active`); }
  getById(id: number) { return this.http.get<Announcement>(`${this.base}/${id}`); }
  create(dto: any) { return this.http.post<number>(this.base, dto); }
  update(dto: any) { return this.http.put(this.base, dto); }
  delete(id: number) { return this.http.delete(`${this.base}/${id}`); }
}
