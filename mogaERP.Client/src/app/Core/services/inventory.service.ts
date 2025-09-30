import { Injectable, signal } from '@angular/core';
import { environment } from '../../../env/environment';
import { HttpClient } from '@angular/common/http';
import { PagedResult, ReciptPermission  } from '../models/inventory/reciptPermissions';
import { Observable, of } from 'rxjs';
import { catchError} from 'rxjs/operators';
import { ExchangeRequest } from '../models/inventory/exchangeRequest';
import { ExchangePermission } from '../models/inventory/exchangePermissions';

@Injectable({
  providedIn: 'root'
})
export class InventoryService {
 private baseUrl = `${environment.baseUrl}`;

  // ======== Signals Usage ========
  reciptPermissions = signal<ReciptPermission[]>([]);
  exchangeRequests = signal<ExchangeRequest[]>([]);
  exchangePermissions = signal<ExchangePermission[]>([]);
  loading = signal<boolean>(false);
  error = signal<string | null>(null);

  constructor(private http: HttpClient) {}
  // ============================================== Recipt Permissions ============================================== 
  getReciptPermissions(
    pageNumber: number,
    pageSize: number,
    searchTerm: string = '',
    sortDescending: boolean = true
  ): Observable<PagedResult<ReciptPermission>> {
    this.loading.set(true);
    const url = `${this.baseUrl}ReceiptPermissions?PageNumber=${pageNumber}&PageSize=${pageSize}&SearchTerm=${searchTerm}&SortDescending=${sortDescending}`;
    
    return this.http.get<PagedResult<ReciptPermission>>(url).pipe(
      catchError(err => {
        console.error('Error fetching recipt permissions', err);
        return of({ data: [], totalCount: 0 });
      })
    );
  }
  getReciptPermissionById(id: number) {
    return this.http.get<ReciptPermission>(`${this.baseUrl}ReceiptPermissions/${id}`);
  }
  addReciptPermission(reciptPermission: ReciptPermission) {
    return this.http.post<ReciptPermission>(`${this.baseUrl}ReceiptPermissions`, reciptPermission);
  }
  updateReciptPermission(id: number, reciptPermission: ReciptPermission) {
    return this.http.put<ReciptPermission>(`${this.baseUrl}ReceiptPermissions/${id}`, reciptPermission);
  }
  deleteReciptPermission(id: number) {
    return this.http.delete<ReciptPermission>(`${this.baseUrl}ReceiptPermissions/${id}`);
  }
  // ============================================== End Recipt Permissions ============================================== 
  getExchangeRequest(
    pageNumber: number,
    pageSize: number,
    searchTerm: string = '',
    sortDescending: boolean = true
  ): Observable<PagedResult<any>> {
    this.loading.set(true);
    const url = `${this.baseUrl}DisbursementRequest?PageNumber=${pageNumber}&PageSize=${pageSize}&SearchTerm=${searchTerm}&SortDescending=${sortDescending}`;
    
    return this.http.get<PagedResult<any>>(url).pipe(
      catchError(err => {
        console.error('Error fetching recipt permissions', err);
        return of({ data: [], totalCount: 0 });
      })
    );
  }
  getApprovedExchangeRequest(
    pageNumber: number,
    pageSize: number,
    searchTerm: string = '',
    sortDescending: boolean = true
  ): Observable<PagedResult<any>> {
    this.loading.set(true);
    const url = `${this.baseUrl}DisbursementRequest/approved?PageNumber=${pageNumber}&PageSize=${pageSize}&SearchTerm=${searchTerm}&SortDescending=${sortDescending}`;
    
    return this.http.get<PagedResult<any>>(url).pipe(
      catchError(err => {
        console.error('Error fetching recipt permissions', err);
        return of({ data: [], totalCount: 0 });
      })
    );
  }
  getExchangeRequestById(id: number) {
    return this.http.get<any>(`${this.baseUrl}DisbursementRequest/${id}`);
  }
  addExchangeRequest(exchangeRequest: any) {
    return this.http.post<any>(`${this.baseUrl}DisbursementRequest`, exchangeRequest);
  }
  updateExchangeRequest(id: number, exchangeRequest: any) {
    return this.http.put<any>(`${this.baseUrl}DisbursementRequest/${id}`, exchangeRequest);
  }
  deleteExchangeRequest(id: number) {
    return this.http.delete<any>(`${this.baseUrl}DisbursementRequest/${id}`);
  }

  // ============================================== Exchange Permissions ============================================== 
  getExchangePermissions(
    pageNumber: number,
    pageSize: number,
    searchTerm: string = '',
    sortDescending: boolean = true
  ): Observable<PagedResult<ExchangePermission>> {
    this.loading.set(true);
    const url = `${this.baseUrl}MaterialIssuePermissions?PageNumber=${pageNumber}&PageSize=${pageSize}&SearchTerm=${searchTerm}&SortDescending=${sortDescending}`;
    
    return this.http.get<PagedResult<ExchangePermission>>(url).pipe(
      catchError(err => {
        console.error('Error fetching recipt permissions', err);
        return of({ data: [], totalCount: 0 });
      })
    );
  }
  getExchangePermissionById(id: number) {
    return this.http.get<ExchangePermission>(`${this.baseUrl}MaterialIssuePermissions/${id}`);
  }
  addExchangePermission(exchangePermission: ExchangePermission) {
    return this.http.post<ExchangePermission>(`${this.baseUrl}MaterialIssuePermissions`, exchangePermission);
  }
  updateExchangePermission(id: number, exchangePermission: ExchangePermission) {
    return this.http.put<ExchangePermission>(`${this.baseUrl}MaterialIssuePermissions/${id}`, exchangePermission);
  }
  deleteExchangePermission(id: number) {
    return this.http.delete<ExchangePermission>(`${this.baseUrl}MaterialIssuePermissions/${id}`);
  }
  // ============================================== End Exchange Permissions ============================================== 
}
