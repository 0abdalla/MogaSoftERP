import { Injectable, signal } from '@angular/core';
import { environment } from '../../../env/environment';
import { HttpClient } from '@angular/common/http';
import { debitNotification } from '../models/fin-tree/banks/debit';
import { catchError , Observable, of } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class FinanceService {
  private baseUrl = `${environment.baseUrl}`;
  loading = signal<boolean>(false);
  error = signal<string | null>(null);

  // ======== Signals Usage ========
  debitNotifications = signal<debitNotification[]>([])
  additionNotifications = signal<any[]>([])
  banks = signal<any[]>([])
  accounts = signal<any[]>([])
  constructor(private http : HttpClient) { }

  // ============================================== Banks ============================================== 
  getAllBanks(): Observable<any> {
    return this.http.get<any>(`${this.baseUrl}Banks`);
  }
  // ============================================== Accounts ============================================== 
  getAllAccounts(): Observable<any> {
    return this.http.get<any>(`${this.baseUrl}Accounts`);
  }
  // ============================================== Debit Notifications ============================================== 
  getDebitNotifications(
      pageNumber: number,
      pageSize: number,
      searchTerm: string = '',
      sortDescending: boolean = true
    ): Observable<any> {
      this.loading.set(true);
      const url = `${this.baseUrl}DebitNotification?PageNumber=${pageNumber}&PageSize=${pageSize}&SearchTerm=${searchTerm}&SortDescending=${sortDescending}`;
      
      return this.http.get<any>(url).pipe(
        catchError(err => {
          console.error('Error fetching debit notifications', err);
          return of({ data: [], totalCount: 0 });
        })
      );
  }
  getDebitNotificationById(id: number) {
    return this.http.get<debitNotification>(`${this.baseUrl}DebitNotification/${id}`);
  }
  addDebitNotification(debitNotification: debitNotification) {
    return this.http.post<debitNotification>(`${this.baseUrl}DebitNotification`, debitNotification);
  }
  updateDebitNotification(id: number, debitNotification: debitNotification) {
    return this.http.put<debitNotification>(`${this.baseUrl}DebitNotification/${id}`, debitNotification);
  }
  deleteDebitNotification(id: number) {
    return this.http.delete<debitNotification>(`${this.baseUrl}DebitNotification/${id}`);
  }
  // ============================================== Addition Notifications ============================================== 
  getAdditionNotifications(
      pageNumber: number,
      pageSize: number,
      searchTerm: string = '',
      sortDescending: boolean = true
    ): Observable<any> {
      this.loading.set(true);
      const url = `${this.baseUrl}AdditionNotification?PageNumber=${pageNumber}&PageSize=${pageSize}&SearchTerm=${searchTerm}&SortDescending=${sortDescending}`;
      
      return this.http.get<any>(url).pipe(
        catchError(err => {
          console.error('Error fetching addition notifications', err);
          return of({ data: [], totalCount: 0 });
        })
      );
  }
  getAdditionNotificationById(id: number) {
    return this.http.get<any>(`${this.baseUrl}AdditionNotification/${id}`);
  }
  addAdditionNotification(additionNotification: any) {
    return this.http.post<any>(`${this.baseUrl}AdditionNotification`, additionNotification);
  }
  updateAdditionNotification(id: number, additionNotification: any) {
    return this.http.put<any>(`${this.baseUrl}AdditionNotification/${id}`, additionNotification);
  }
  deleteAdditionNotification(id: number) {
    return this.http.delete<any>(`${this.baseUrl}AdditionNotification/${id}`);
  }
}
