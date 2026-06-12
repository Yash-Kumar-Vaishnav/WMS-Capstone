import { environment } from '../../../environments/environment';
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Client } from '../models/models';

@Injectable({ providedIn: 'root' })
export class ClientService {
  private base = environment.apiUrl + '/client';
  constructor(private http: HttpClient) {}
  getAll() { return this.http.get<Client[]>(this.base); }
  getById(id: number) { return this.http.get<Client>(`${this.base}/${id}`); }
  create(dto: any) { return this.http.post<number>(this.base, dto); }
  update(dto: any) { return this.http.put(this.base, dto); }
  delete(id: number) { return this.http.delete(`${this.base}/${id}`); }
}




