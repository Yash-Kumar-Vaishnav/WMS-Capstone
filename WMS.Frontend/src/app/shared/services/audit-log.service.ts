import { environment } from '../../../environments/environment';
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';


export interface AuditLog {
  auditId: number;
  entityName: string;
  recordId: number;
  action: string;
  createdBy: number;
  createdOn: string;
}

@Injectable({
  providedIn: 'root'
})
export class AuditLogService {
  private apiUrl = '${environment.apiUrl}/auditlog';

  constructor(private http: HttpClient) {}

  getAll(): Observable<AuditLog[]> {
    return this.http.get<AuditLog[]>(this.apiUrl);
  }
}


