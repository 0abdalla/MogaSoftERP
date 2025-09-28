import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, tap } from 'rxjs';
import { environment } from '../../env/environment';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  baseUrl = environment.baseUrl;
  private allowedPagesCache = new Map<string, Observable<any>>();

  constructor(private http: HttpClient) { }

  login(data: { email: string; password: string }): Observable<any> {
    return this.http.post<any>(`${this.baseUrl}Auth/Login`, data).pipe(
      tap(response => {
        if (response && response.result) {
          const user = response.result;
  
          sessionStorage.setItem('userId', user.userId ?? '');
          sessionStorage.setItem('token', user.token ?? '');
          sessionStorage.setItem('userName', user.userName ?? '');
          sessionStorage.setItem('role', user.role ?? '');
          sessionStorage.setItem('pages', user.pages ?? '');
          sessionStorage.setItem('fullName', user.fullName ?? '');
        } else {
          console.error("Login API response is invalid:", response);
        }
      })
    );
  }

  logout() {
    sessionStorage.clear();
  }

  isLoggedIn(): boolean {
    return sessionStorage.getItem('token') !== null;
  }

  register(data: any) {
    return this.http.post<any>(`${this.baseUrl}Auth/register`, data);
  }
}
