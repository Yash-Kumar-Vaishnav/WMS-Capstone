import { environment } from '../../../environments/environment';
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, tap } from 'rxjs';
import { LoginDto, LoginResponseDto } from '../models/models';

@Injectable({ providedIn: 'root' })
export class AuthService {
  private baseUrl = '${environment.apiUrl}/auth';
  private _user = new BehaviorSubject<LoginResponseDto | null>(this.loadUser());

  user$ = this._user.asObservable();

  constructor(private http: HttpClient) {}

  private loadUser(): LoginResponseDto | null {
    const raw = localStorage.getItem('wms_user');
    return raw ? JSON.parse(raw) : null;
  }

  login(dto: LoginDto) {
    return this.http.post<LoginResponseDto>(`${this.baseUrl}/login`, dto).pipe(
      tap(res => {
        localStorage.setItem('wms_user', JSON.stringify(res));
        localStorage.setItem('wms_token', res.token);
        this._user.next(res);
      })
    );
  }

  logout() {
    localStorage.removeItem('wms_user');
    localStorage.removeItem('wms_token');
    this._user.next(null);
  }

  getToken(): string | null { return localStorage.getItem('wms_token'); }
  isLoggedIn(): boolean { return !!this.getToken(); }
  getRole(): string { return this._user.value?.role ?? ''; }
  getUsername(): string { return this._user.value?.username ?? ''; }
  getEmpId(): number | null { return this._user.value?.empId ?? null; }
}


