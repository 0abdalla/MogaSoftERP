import { HttpClient } from '@angular/common/http';
import { Injectable, signal } from '@angular/core';
import { tap, catchError, of, Observable } from 'rxjs';
import { environment } from '../../../env/environment';
import { Items } from '../models/system-settings/item';
import { ItemsGroups } from '../models/system-settings/itemGroup';
import { MainGroups } from '../models/system-settings/maingroup';
import { Store } from '../models/system-settings/stores';
import { ItemsUnits } from '../models/system-settings/itemUnits';
import { Provider } from '../models/system-settings/providers';
import { StoreType } from '../models/system-settings/storeType';
import { PagedResult } from '../models/inventory/reciptPermissions';
import { Department } from '../models/system-settings/departments';

@Injectable({
  providedIn: 'root'
})
export class SysSettingsService {
  private baseUrl = `${environment.baseUrl}`;

  // =========== Signals Usage ===========
  mainGroups = signal<MainGroups[]>([]);
  itemsGroups = signal<ItemsGroups[]>([]);
  items = signal<Items[]>([]);
  itemsUnits = signal<ItemsUnits[]>([]);
  stores = signal<Store[]>([]);
  storeTypes = signal<StoreType[]>([]);
  providers = signal<Provider[]>([]);
  departments = signal<Department[]>([]);
  loading = signal<boolean>(false);
  error = signal<string | null>(null);
  constructor(private http: HttpClient) { }
  // ============================================== Main Groups ============================================== 
  getAllMainGroups(searchTerm: string = '') {
    this.loading.set(true);
    return this.http.get<any>(`${this.baseUrl}MainGroups?SearchTerm=${searchTerm}`).pipe(
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
  getAllItemsGroups(searchTerm: string = '') {
    this.loading.set(true);
    return this.http.get<any>(`${this.baseUrl}ItemGroups?SearchTerm=${searchTerm}`).pipe(
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
  getAllItems(
    pageNumber: number,
    pageSize: number,
    searchTerm: string = '',
    sortDescending: boolean = true
  ) {
    this.loading.set(true);
    return this.http.get<any>(`${this.baseUrl}Items?SearchTerm=${searchTerm}&pageNumber=${pageNumber}&pageSize=${pageSize}&sortDescending=${sortDescending}`).pipe(
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
  getAllItemsUnits(pageNumber: number, pageSize: number, searchTerm: string = '', sortDescending: boolean = true) {
    this.loading.set(true);
    return this.http.get<any>(`${this.baseUrl}ItemUnits?SearchTerm=${searchTerm}&pageNumber=${pageNumber}&pageSize=${pageSize}&sortDescending=${sortDescending}`).pipe(
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
  getAllStores(SearchTerm:string){
    this.loading.set(true);
    return this.http.get<any>(`${this.baseUrl}Stores?SearchTerm=${SearchTerm}`).pipe(
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
  // ================================================= Stores Types ===================================================
  getAllStoreTypes(){
    this.loading.set(true);
    return this.http.get<any>(`${this.baseUrl}StoreTypes`).pipe(
      tap(res => {
        if (res.isSuccess) {
          this.storeTypes.set(res.result);
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

  getStoreTypeById(id: number) {
    return this.http.get<StoreType>(`${this.baseUrl}StoreTypes/${id}`);
  }

  addStoreType(request: Partial<StoreType>) {
    return this.http.post<StoreType>(`${this.baseUrl}StoreTypes`, request).pipe(
      tap(newReq => this.storeTypes.update(list => [...list, newReq]))
    );
  }

  updateStoreType(id: number, request: Partial<StoreType>) {
    return this.http.put<StoreType>(`${this.baseUrl}StoreTypes/${id}`, request).pipe(
      tap(updated =>
        this.storeTypes.update(list =>
          list.map(req => (req.id === id ? updated : req))
        )
      )
    );
  }

  deleteStoreType(id: number) {
    return this.http.delete(`${this.baseUrl}StoreTypes/${id}`).pipe(
      tap(() =>
        this.storeTypes.update(list =>
          list.filter(req => req.id !== id)
        )
      )
    );
  }
  // ========================================================================================================
  getAllProviders(
    pageNumber: number,
    pageSize: number,
    searchTerm: string = '',
    sortDescending: boolean = true
  ){
    this.loading.set(true);
    return this.http.get<any>(`${this.baseUrl}Suppliers?PageNumber=${pageNumber}&PageSize=${pageSize}&SearchTerm=${searchTerm}&SortDescending=${sortDescending}`).pipe(
      tap(res => {
        if (res.isSuccess) {
          this.providers.set(res.result);
        } else {
          this.error.set(res.message || 'Error loading providers');
        }
      }),
      catchError(err => {
        this.error.set('Error loading providers');
        return of(null);
      }),
      tap(() => this.loading.set(false))
    );
  }

  getProviderById(id: number) {
    return this.http.get<Provider>(`${this.baseUrl}Suppliers/${id}`);
  }

  addProvider(request: Partial<Provider>) {
    return this.http.post<Provider>(`${this.baseUrl}Suppliers`, request).pipe(
      tap(newReq =>
        this.providers.update(list =>
          Array.isArray(list) ? [...list, newReq] : [newReq]
        )
      )
    );
  }  

  updateProvider(id: number, request: Partial<Provider>) {
    return this.http.put<Provider>(`${this.baseUrl}Suppliers/${id}`, request).pipe(
      tap(updated =>
        this.providers.update(list =>
          Array.isArray(list)
            ? list.map(req => (req.id === id ? updated : req))
            : [updated]
        )
      )
    );
  }

  deleteProvider(id: number) {
    return this.http.delete(`${this.baseUrl}Suppliers/${id}`).pipe(
      tap(() =>
        this.providers.update(list =>
          Array.isArray(list) ? list.filter(req => req.id !== id) : []
        )
      )
    );
  }  
  // 
  getDepartments(
    pageNumber: number,
    pageSize: number,
    searchTerm: string = '',
    sortDescending: boolean = true
  ): Observable<any> {
    this.loading.set(true);
    const url = `${this.baseUrl}JobDepartments?PageNumber=${pageNumber}&PageSize=${pageSize}&SearchTerm=${searchTerm}&SortDescending=${sortDescending}`;
    
    return this.http.get<any>(url).pipe(
      catchError(err => {
        console.error('Error fetching departments', err);
        return of({ data: [], totalCount: 0 });
      })
    );
  }
  getDepartmentById(id: number) {
    return this.http.get<any>(`${this.baseUrl}JobDepartments/${id}`);
  }
  addDepartment(request: Partial<any>) {
    return this.http.post<any>(`${this.baseUrl}JobDepartments`, request).pipe(
      tap(newReq => this.departments.update(list => [...list, newReq]))
    );
  }
  updateDepartment(id: number, request: Partial<any>) {
    return this.http.put<any>(`${this.baseUrl}JobDepartments/${id}`, request).pipe(
      tap(updated =>
        this.departments.update(list =>
          list.map(req => (req.id === id ? updated : req))
        )
      )
    );
  }
  deleteDepartment(id: number) {
    return this.http.delete(`${this.baseUrl}JobDepartments/${id}`).pipe(
      tap(() =>
        this.departments.update(list =>
          list.filter(req => req.id !== id)
        )
      )
    );
  }
}
