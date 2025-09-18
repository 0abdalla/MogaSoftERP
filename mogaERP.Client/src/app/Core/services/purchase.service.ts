import { Injectable, signal } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { catchError, Observable, of, tap } from 'rxjs';
import { environment } from '../../../environments/environment';
import { PurchaseRequest } from '../../Models/purchases/purchase-request';


@Injectable({ providedIn: 'root' })
export class PurchaseService {
  private baseUrl = `${environment.baseUrl}`;

  // ======== Signals Usage ========
  purchaseRequests = signal<PurchaseRequest[]>([]);
  loading = signal<boolean>(false);
  error = signal<string | null>(null);

  constructor(private http: HttpClient) {}
  // ============================================== Purchase Requests ============================================== 
  getPurchaseRequests(
    pageNumber: number,
    pageSize: number,
    searchTerm: string = '',
    sortDescending: boolean = true
  ): Observable<any> {
    const url = `${this.baseUrl}PurchaseRequests?PageNumber=${pageNumber}&PageSize=${pageSize}&SearchTerm=${searchTerm}&SortDescending=${sortDescending}`;
    return this.http.get<any>(url).pipe(
      catchError(err => {
        console.error('Error fetching purchase requests', err);
        return of({ result: { data: [], totalCount: 0 } });
      })
    );
  }

  getPurchaseRequestById(id: number) {
    return this.http.get<PurchaseRequest>(`${this.baseUrl}PurchaseRequests/${id}`);
  }

  addPurchaseRequest(request: Partial<PurchaseRequest>) {
    return this.http.post<PurchaseRequest>(`${this.baseUrl}PurchaseRequests`, request).pipe(
      tap(newReq => this.purchaseRequests.update(list => [...list, newReq]))
    );
  }

  updatePurchaseRequest(id: number, request: Partial<PurchaseRequest>) {
    return this.http.put<PurchaseRequest>(`${this.baseUrl}PurchaseRequests/${id}`, request).pipe(
      tap(updated =>
        this.purchaseRequests.update(list =>
          list.map(req => (req.id === id ? updated : req))
        )
      )
    );
  }

  deletePurchaseRequest(id: number) {
    return this.http.delete(`${this.baseUrl}PurchaseRequests/${id}`).pipe(
      tap(() =>
        this.purchaseRequests.update(list =>
          list.filter(req => req.id !== id)
        )
      )
    );
  }
  // ============================================== End Purchase Requests ============================================== 
}
