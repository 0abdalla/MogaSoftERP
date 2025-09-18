import { Injectable, signal } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { catchError, of, tap } from 'rxjs';
import { environment } from '../../../environments/environment';
import { MainGroups } from '../../Models/system-settings/stores/maingroup';
import { ItemsGroups } from '../../Models/system-settings/stores/itemgroup';
import { ItemsUnits } from '../../Models/system-settings/stores/itemsUnits';
import { Store } from '../../Models/system-settings/stores/stores';
import { Items } from '../../Models/system-settings/stores/items';

@Injectable({
  providedIn: 'root'
})
export class SystemSettingsService {
  private baseUrl = `${environment.baseUrl}`;

  // =========== Signals Usage ===========
  mainGroups = signal<MainGroups[]>([]);
  itemsGroups = signal<ItemsGroups[]>([]);
  items = signal<Items[]>([]);
  itemsUnits = signal<ItemsUnits[]>([]);
  stores = signal<Store[]>([]);
  loading = signal<boolean>(false);
  error = signal<string | null>(null);
  constructor(private http: HttpClient) { }
  // ============================================== Main Groups ============================================== 
  getAllMainGroups() {
    this.loading.set(true);
    return this.http.get<any>(`${this.baseUrl}MainGroups`).pipe(
      tap(res => {
        if (res.isSuccess) {
          this.mainGroups.set(res.result);
        } else {
          this.error.set(res.message || 'Error loading main groups');
        }
      }),
      catchError(err => {
        this.error.set('Error loading main groups');
        return of(null);
      }),
      tap(() => this.loading.set(false))
    );
  }  

  getMainGroupById(id: number) {
      return this.http.get<MainGroups>(`${this.baseUrl}MainGroups/${id}`);
  }

  addMainGroup(request: Partial<MainGroups>) {
    return this.http.post<MainGroups>(`${this.baseUrl}MainGroups`, request).pipe(
      tap(newReq => this.mainGroups.update(list => [...list, newReq]))
    );
  }

  updateMainGroup(id: number, request: Partial<MainGroups>) {
    return this.http.put<MainGroups>(`${this.baseUrl}MainGroups/${id}`, request).pipe(
      tap(updated =>
        this.mainGroups.update(list =>
          list.map(req => (req.id === id ? updated : req))
        )
      )
    );
  }

  deleteMainGroup(id: number) {
    return this.http.delete(`${this.baseUrl}MainGroups/${id}`).pipe(
      tap(() =>
        this.mainGroups.update(list =>
          list.filter(req => req.id !== id)
        )
      )
    );
  }
  // ============================================== Items Groups ==============================================
  getAllItemsGroups() {
    this.loading.set(true);
    return this.http.get<any>(`${this.baseUrl}ItemGroups`).pipe(
      tap(res => {
        if (res.isSuccess) {
          this.itemsGroups.set(res.result);
        } else {
          this.error.set(res.message || 'Error loading items groups');
        }
      }),
      catchError(err => {
        this.error.set('Error loading items groups');
        return of(null);
      }),
      tap(() => this.loading.set(false))
    );
  }

  getItemGroupById(id: number) {
    return this.http.get<ItemsGroups>(`${this.baseUrl}ItemGroups/${id}`);
  }

  addItemGroup(request: Partial<ItemsGroups>) {
    return this.http.post<ItemsGroups>(`${this.baseUrl}ItemGroups`, request).pipe(
      tap(newReq => this.itemsGroups.update(list => [...list, newReq]))
    );
  }

  updateItemGroup(id: number, request: Partial<ItemsGroups>) {
    return this.http.put<ItemsGroups>(`${this.baseUrl}ItemGroups/${id}`, request).pipe(
      tap(updated =>
        this.itemsGroups.update(list =>
          list.map(req => (req.id === id ? updated : req))
        )
      )
    );
  }

  deleteItemGroup(id: number) {
    return this.http.delete(`${this.baseUrl}ItemGroups/${id}`).pipe(
      tap(() =>
        this.itemsGroups.update(list =>
          list.filter(req => req.id !== id)
        )
      )
    );
  }
  // ============================================== Items =====================================================
  getAllItems() {
    this.loading.set(true);
    return this.http.get<any>(`${this.baseUrl}Items`).pipe(
      tap(res => {
        if (res.isSuccess) {
          this.items.set(res.result.data);
        } else {
          this.error.set(res.message || 'Error loading items');
        }
      }),
      catchError(err => {
        this.error.set('Error loading items');
        return of(null);
      }),
      tap(() => this.loading.set(false))
    );
  }

  getItemById(id: number) {
    return this.http.get<Items>(`${this.baseUrl}Items/${id}`);
  }

  addItem(request: Partial<Items>) {
    return this.http.post<Items>(`${this.baseUrl}Items`, request).pipe(
      tap(newReq => this.items.update(list => [...list, newReq]))
    );
  }

  updateItem(id: number, request: Partial<Items>) {
    return this.http.put<Items>(`${this.baseUrl}Items/${id}`, request).pipe(
      tap(updated =>
        this.items.update(list =>
          list.map(req => (req.id === id ? updated : req))
        )
      )
    );
  }

  deleteItem(id: number) {
    return this.http.delete(`${this.baseUrl}Items/${id}`).pipe(
      tap(() =>
        this.items.update(list =>
          list.filter(req => req.id !== id)
        )
      )
    );
  }
  // ============================================== Items Units ================================================
  getAllItemsUnits() {
    this.loading.set(true);
    return this.http.get<any>(`${this.baseUrl}ItemUnits`).pipe(
      tap(res => {
        if (res.isSuccess) {
          this.itemsUnits.set(res.result);
        } else {
          this.error.set(res.message || 'Error loading items units');
        }
      }),
      catchError(err => {
        this.error.set('Error loading items groups');
        return of(null);
      }),
      tap(() => this.loading.set(false))
    );
  }

  getItemUnitById(id: number) {
    return this.http.get<ItemsUnits>(`${this.baseUrl}ItemUnits/${id}`);
  }

  addItemUnit(request: Partial<ItemsUnits>) {
    return this.http.post<ItemsUnits>(`${this.baseUrl}ItemUnits`, request).pipe(
      tap(newReq => this.itemsUnits.update(list => [...list, newReq]))
    );
  }

  updateItemUnit(id: number, request: Partial<ItemsUnits>) {
    return this.http.put<ItemsUnits>(`${this.baseUrl}ItemUnits/${id}`, request).pipe(
      tap(updated =>
        this.itemsUnits.update(list =>
          list.map(req => (req.id === id ? updated : req))
        )
      )
    );
  }

  deleteItemUnit(id: number) {
    return this.http.delete(`${this.baseUrl}ItemUnits/${id}`).pipe(
      tap(() =>
        this.itemsUnits.update(list =>
          list.filter(req => req.id !== id)
        )
      )
    );
  }
  // ============================================== Stores ======================================================
  getAllStores(){
    this.loading.set(true);
    return this.http.get<any>(`${this.baseUrl}Stores`).pipe(
      tap(res => {
        if (res.isSuccess) {
          this.stores.set(res.result);
        } else {
          this.error.set(res.message || 'Error loading stores');
        }
      }),
      catchError(err => {
        this.error.set('Error loading stores');
        return of(null);
      }),
      tap(() => this.loading.set(false))
    );
  }

  getStoreById(id: number) {
    return this.http.get<Store>(`${this.baseUrl}Stores/${id}`);
  }

  addStore(request: Partial<Store>) {
    return this.http.post<Store>(`${this.baseUrl}Stores`, request).pipe(
      tap(newReq => this.stores.update(list => [...list, newReq]))
    );
  }

  updateStore(id: number, request: Partial<Store>) {
    return this.http.put<Store>(`${this.baseUrl}Stores/${id}`, request).pipe(
      tap(updated =>
        this.stores.update(list =>
          list.map(req => (req.id === id ? updated : req))
        )
      )
    );
  }

  deleteStore(id: number) {
    return this.http.delete(`${this.baseUrl}Stores/${id}`).pipe(
      tap(() =>
        this.stores.update(list =>
          list.filter(req => req.id !== id)
        )
      )
    );
  }
}
