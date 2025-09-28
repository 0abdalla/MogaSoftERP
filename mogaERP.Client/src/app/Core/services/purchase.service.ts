import { HttpClient } from '@angular/common/http';
import { Injectable, signal } from '@angular/core';
import { Observable, catchError, of, tap } from 'rxjs';
import { environment } from '../../../env/environment';
import { PurchaseRequest } from '../models/purchase/purchase-request';
import { PriceQuotation } from '../models/purchase/priceQuotation';
import { PurchaseOrder } from '../models/purchase/purchase-order';

@Injectable({
  providedIn: 'root'
})
export class PurchaseService {
  private baseUrl = `${environment.baseUrl}`;

  // ======== Signals Usage ========
  purchaseRequests = signal<PurchaseRequest[]>([]);
  priceQoutations = signal<PriceQuotation[]>([]);
  purchaseOrders = signal<PurchaseOrder[]>([]);
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
  getapprovedPurchaseRequests(){
    return this.http.get<any>(`${this.baseUrl}PurchaseRequests/approved`);
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
  // ============================================== Price Quotations ============================================== 
  getPriceQuotations(
    pageNumber: number,
    pageSize: number,
    searchTerm: string = '',
    sortDescending: boolean = true
  ): Observable<any> {
    const url = `${this.baseUrl}PriceQuotations?PageNumber=${pageNumber}&PageSize=${pageSize}&SearchTerm=${searchTerm}&SortDescending=${sortDescending}`;
    return this.http.get<any>(url).pipe(
      catchError(err => {
        console.error('Error fetching price quotations', err);
        return of({ result: { data: [], totalCount: 0 } });
      })
    );
  }

  getPriceQuotationById(id: number) {
    return this.http.get<PriceQuotation>(`${this.baseUrl}PriceQuotations/${id}`);
  }

  getPriceQuotationByPurchaseRequestId(id: number) {
    return this.http.get<PriceQuotation>(`${this.baseUrl}PriceQuotations/get-by-purchase-request/${id}`);
  }
  
  addPriceQuotation(quotation: Partial<PriceQuotation>) {
    return this.http.post<PriceQuotation>(`${this.baseUrl}PriceQuotations`, quotation).pipe(
      tap(newQuot => this.priceQoutations.update(list => [...list, newQuot]))
    );
  }

  updatePriceQuotation(id: number, quotation: Partial<PriceQuotation>) {
    return this.http.put<PriceQuotation>(`${this.baseUrl}PriceQuotations/${id}`, quotation).pipe(
      tap(updated =>
        this.priceQoutations.update(list =>
          list.map(req => (req.id === id ? updated : req))
        )
      )
    );
  }

  deletePriceQuotation(id: number) {
    return this.http.delete(`${this.baseUrl}PriceQuotations/${id}`).pipe(
      tap(() =>
        this.priceQoutations.update(list =>
          list.filter(req => req.id !== id)
        )
      )
    );
  }

  putPriceQutataion(id:number , price:any){
    return this.http.put<any>(this.baseUrl + `PriceQuotations/submit-price-quotation/${id}`, price);
  }
  // ============================================== End Price Quotations ============================================== 
  // ============================================== Purchase Orders ============================================== 
  getPurchaseOrders(
    pageNumber: number,
    pageSize: number,
    searchTerm: string = '',
    sortDescending: boolean = true
  ): Observable<any> {
    const url = `${this.baseUrl}PurchaseOrders?PageNumber=${pageNumber}&PageSize=${pageSize}&SearchTerm=${searchTerm}&SortDescending=${sortDescending}`;
    return this.http.get<any>(url).pipe(
      catchError(err => {
        console.error('Error fetching purchase orders', err);
        return of({ result: { data: [], totalCount: 0 } });
      })
    );
  }

  
  getPurchaseOrderById(id: number) {
    return this.http.get<PurchaseOrder>(`${this.baseUrl}PurchaseOrders/${id}`);
  }

  addPurchaseOrder(order: Partial<PurchaseOrder>) {
    return this.http.post<PurchaseOrder>(`${this.baseUrl}PurchaseOrders`, order).pipe(
      tap(newOrder => this.purchaseOrders.update(list => [...list, newOrder]))
    );
  }

  updatePurchaseOrder(id: number, order: Partial<PurchaseOrder>) {
    return this.http.put<PurchaseOrder>(`${this.baseUrl}PurchaseOrders/${id}`, order).pipe(
      tap(updated =>
        this.purchaseOrders.update(list =>
          list.map(req => (req.id === id ? updated : req))
        )
      )
    );
  }

  deletePurchaseOrder(id: number) {
    return this.http.delete(`${this.baseUrl}PurchaseOrders/${id}`).pipe(
      tap(() =>
        this.purchaseOrders.update(list =>
          list.filter(req => req.id !== id)
        )
      )
    );
  }
}
