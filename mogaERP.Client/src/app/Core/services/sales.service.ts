import { Injectable } from '@angular/core';
import { environment } from '../../../env/environment';
import { signal } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { tap, catchError, of } from 'rxjs';
@Injectable({
  providedIn: 'root'
})
export class SalesService {
  private baseUrl = `${environment.baseUrl}`;
  
  // =========== Signals Usage ===========
  quotations = signal<any[]>([]);
  loading = signal<boolean>(false);
  error = signal<string | null>(null);
  constructor(private http: HttpClient) { }

  getAllQuotations(pageNumber: number, pageSize: number, searchTerm: string = '', sortDescending: boolean = true) {
    this.loading.set(true);
    return this.http.get<any>(`${this.baseUrl}SalesQuotations?SearchTerm=${searchTerm}&pageNumber=${pageNumber}&pageSize=${pageSize}&sortDescending=${sortDescending}`).pipe(
      tap(res => {
        if (res.isSuccess) {
          this.quotations.set(res.result.data);
        } else {
          this.error.set(res.message || 'Error loading quotations');
        }
      }),
      catchError(err => {
        this.error.set('Error loading quotations');
        return of(null);
      }),
      tap(() => this.loading.set(false))
    );
  }
  getQuotationById(id: number) {
    return this.http.get<any>(`${this.baseUrl}SalesQuotations/${id}`);
  }
  addQuotation(request: Partial<any>) {
    return this.http.post<any>(`${this.baseUrl}SalesQuotations`, request).pipe(
      tap(newReq => this.quotations.update(list => [...list, newReq]))
    );
  }
  updateQuotation(id: number, request: Partial<any>) {
    return this.http.put<any>(`${this.baseUrl}SalesQuotations/${id}`, request).pipe(
      tap(updated =>
        this.quotations.update(list =>
          list.map(req => (req.id === id ? updated : req))
        )
      )
    );
  }
  deleteQuotation(id: number) {
    return this.http.delete(`${this.baseUrl}SalesQuotations/${id}`).pipe(
      tap(() =>
        this.quotations.update(list =>
          list.filter(req => req.id !== id)
        )
      )
    );
  }
}
