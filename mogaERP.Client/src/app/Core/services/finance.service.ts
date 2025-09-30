import { Injectable, signal } from '@angular/core';
import { environment } from '../../../env/environment';
import { HttpClient } from '@angular/common/http';
import { debitNotification } from '../models/fin-tree/banks/debit';
import { catchError , Observable, of } from 'rxjs';
import { DailyEntry } from '../models/fin-tree/entries';
import { Banks } from '../models/system-settings/banks';

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
  banks = signal<Banks[]>([])
  accounts = signal<any[]>([])
  dailyEntries = signal<DailyEntry[]>([])
  constructor(private http : HttpClient) { }

  // ============================================== Banks ============================================== 
  getAllBanks(
    pageNumber: number,
    pageSize: number,
    searchTerm: string = '',
    sortDescending: boolean = true
  ): Observable<any> {
    this.loading.set(true);
    const url = `${this.baseUrl}Banks?PageNumber=${pageNumber}&PageSize=${pageSize}&SearchTerm=${searchTerm}&SortDescending=${sortDescending}`;
    
    return this.http.get<any>(url).pipe(
      catchError(err => {
        console.error('Error fetching banks', err);
        return of({ data: [], totalCount: 0 });
      })
    );
  }
  getBankById(id: number) {
    return this.http.get<any>(`${this.baseUrl}Banks/${id}`);
  }
  addBank(bank: any) {
    return this.http.post<any>(`${this.baseUrl}Banks`, bank);
  }
  updateBank(id: number, bank: any) {
    return this.http.put<any>(`${this.baseUrl}Banks/${id}`, bank);
  }
  deleteBank(id: number) {
    return this.http.delete<any>(`${this.baseUrl}Banks/${id}`);
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
  // ============================================== Daily Entries ============================================== 
  getDailyEntries(
    pageNumber: number,
    pageSize: number,
    searchTerm: string = '',
    sortDescending: boolean = true
  ): Observable<{ data: DailyEntry[]; totalCount: number }> {
    this.loading.set(true);
    const url = `${this.baseUrl}DailyRestrictions?PageNumber=${pageNumber}&PageSize=${pageSize}&SearchTerm=${searchTerm}&SortDescending=${sortDescending}`;
    return this.http.get<{ data: DailyEntry[]; totalCount: number }>(url).pipe(
      catchError(err => {
        console.error('Error fetching daily entries', err);
        return of({ data: [], totalCount: 0 });
      })
    );
  }  
  getDailyEntryById(id: number) {
    return this.http.get<DailyEntry>(`${this.baseUrl}DailyRestrictions/${id}`);
  }
  addDailyEntry(dailyEntry: DailyEntry) {
    return this.http.post<DailyEntry>(`${this.baseUrl}DailyRestrictions`, dailyEntry);
  }
  updateDailyEntry(id: number, dailyEntry: DailyEntry) {
    return this.http.put<DailyEntry>(`${this.baseUrl}DailyRestrictions/${id}`, dailyEntry);
  }
  deleteDailyEntry(id: number) {
    return this.http.delete<DailyEntry>(`${this.baseUrl}DailyRestrictions/${id}`);
  }
}
