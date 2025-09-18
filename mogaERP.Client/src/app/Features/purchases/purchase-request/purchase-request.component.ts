import { Component, OnInit } from '@angular/core';
import { FormArray, FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { MessageService } from 'primeng/api';
import { forkJoin } from 'rxjs';
declare var bootstrap: any;
import Swal from 'sweetalert2';
import { PurchaseRequest } from '../../../Models/purchases/purchase-request';
import { Store } from '../../../Models/system-settings/stores/stores';
import { Items } from '../../../Models/system-settings/stores/items';
import { MainGroups } from '../../../Models/system-settings/stores/maingroup';
import { ItemsUnits } from '../../../Models/system-settings/stores/itemsUnits';
import { PurchaseService } from '../../../Core/services/purchase.service';
import { SystemSettingsService } from '../../../Core/services/system-settings.service';
import { ToastModule } from "primeng/toast";
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-purchase-request',
  standalone: true,
  templateUrl: './purchase-request.component.html',
  styleUrl: './purchase-request.component.css',
  providers:[MessageService],
  imports: [ToastModule,FormsModule,CommonModule,ReactiveFormsModule],
  // changeDetection: ChangeDetectionStrategy.OnPush
})
export class PurchaseRequestComponent implements OnInit {
  userName: string = '';
  get today(): string {
    const date = new Date();
    const dateStr = date.toLocaleDateString('ar-EG', {
      weekday: 'long',
      year: 'numeric',
      month: 'long',
      day: 'numeric'
    });
    const timeStr = date.toLocaleTimeString('ar-EG', {
      hour: 'numeric',
      minute: '2-digit',
      hour12: true
    });
    return `${dateStr} - الساعة ${timeStr}`;
  }
  // 
  isEditMode: boolean = false;
  isFilter: boolean = false;
  // 
  purchaseRequests: PurchaseRequest[] = [];
  totalCount: number = 0;

  // pagination state
  pageNumber: number = 1;
  pageSize: number = 2;
  searchTerm: string = '';
  // 
  stores: Store[] = [];
  allItems: Items[] = [];
  groups: MainGroups[] = [];
  units: ItemsUnits[] = [];
  // 
  purchaseRequestForm: FormGroup;
  itemForm: FormGroup;
  // 
  purNumber: string = '';
  constructor(private fb: FormBuilder , private purchaseService : PurchaseService , private systemSettingsService : SystemSettingsService){
    this.purchaseRequestForm = this.fb.group({
      requestDate: [new Date().toISOString().substring(0, 10)], // Today Date Validator Required
      purpose: [null, Validators.required],
      storeId: [null, Validators.required],
      notes: [null],
      items: this.fb.array([
        this.createItemGroup()
      ]),
    })

    this.itemForm = this.fb.group({
      nameAr: ['', Validators.required],
      nameEn: ['', Validators.required],
      unitId: [null, Validators.required],
      groupId: [null, Validators.required],
      orderLimit: [0],
      cost: [0],
      openingBalance: [0],
      salesTax: [0],
      price: [0],
      hasBarcode: [true],
      typeId: ['1']
    });
  }
  ngOnInit(): void {
    this.loadPurchaseRequests();
    this.loadInitialData();
  }
  // 
  createItemGroup(): FormGroup {
    return this.fb.group({
      itemId: [null, Validators.required],
      quantity: [0, [Validators.required, Validators.min(1)]],
      notes: ['']
    });
  }

  get items(): FormArray {
    return this.purchaseRequestForm.get('items') as FormArray;
  }

  addItemRow() {
    this.items.push(this.createItemGroup());
  }

  removeItemRow(index: number) {
    if (this.items.length > 1) {
      this.items.removeAt(index);
    }
  }
  // 
  trackByRequest(index: number, order: any): number {
    return order.id;
  }  

  getStatusName(type: string): string {
    const map: { [key: string]: string } = {
      Approved: 'تم الموافقة',
      Pending: 'قيد الانتظار',
      Rejected: 'مرفوض'
    };
    return map[type] || type;
  }
  getStatusColor(type: string): string {
    const map: { [key: string]: string } = {
      Approved: '#198654',
      Pending: '#FFA500',
      Rejected: '#dc3545'
    };
    return map[type] || '#000000';
  }
  getItemName(itemId: number | string): string {
    const item = this.allItems?.find(i => i.id == itemId);
    return item ? item.name : '—';
  }
  getStoreName(storeId: number | string): string {
    const store = this.stores?.find(s => s.id == storeId);
    return store ? store.name : '—';
  }
  get math() {
    return Math;
  }
  
  // 
  loadPurchaseRequests() {
    this.purchaseService.getPurchaseRequests(this.pageNumber, this.pageSize, this.searchTerm).subscribe((res) => {
      this.purchaseRequests = res.result.data;
      this.totalCount = res.result.totalCount;
      console.log('Purchase Requests:', this.purchaseRequests);
    });
  }

  loadInitialData() {
    forkJoin({
      items: this.systemSettingsService.getAllItems(),
      units: this.systemSettingsService.getAllItemsUnits(),
      stores: this.systemSettingsService.getAllStores(),
      groups: this.systemSettingsService.getAllItemsGroups()
    }).subscribe(({ items, units, stores, groups }) => {
      this.allItems = items.result.data;
      this.units = units.result;
      this.stores = stores.result;
      this.groups = groups.result;
      console.log('All Items:', this.allItems);
      console.log('All Units:', this.units);
      console.log('All Stores:', this.stores);
      console.log('All Groups:', this.groups);
    });
  }
  
  // pagination handlers
  goToPage(page: number) {
    this.pageNumber = page;
    this.loadPurchaseRequests();
  }

  search() {
    this.pageNumber = 1;
    this.loadPurchaseRequests();
  }
  resetSearch(){
    this.searchTerm = '';
    this.loadPurchaseRequests();
  }
  // 
  currentPurchaseRequestId: number | null = null;
  request: any | null = null;
  addPurchaseRequest() {
    if (this.purchaseRequestForm.invalid) {
      this.purchaseRequestForm.markAllAsTouched();
      return;
    }
    const formData = {
      ...this.purchaseRequestForm.value,
      notes: this.purchaseRequestForm.value.notes ?? '',
      storeId: Number(this.purchaseRequestForm.value.storeId),
      items: this.purchaseRequestForm.value.items.map((item: any) => ({
        ...item,
        itemId: Number(item.itemId),
        notes: item.notes ?? ''
      }))
    };
  
    if (this.isEditMode && this.currentPurchaseRequestId) {
      this.purchaseService.updatePurchaseRequest(this.currentPurchaseRequestId, formData).subscribe({
        next: (res: any) => {
          if (res.isSuccess === true) {
            const index = this.purchaseRequests.findIndex(p => p.id === this.currentPurchaseRequestId);
            if (index !== -1) {
              this.purchaseRequests[index] = { ...this.purchaseRequests[index], ...formData };
            }
          } else {
            console.error('فشل التعديل:', res);
          }
        },
        error: (err) => {
          console.error('فشل التعديل:', err);
        }
      });
    } else {
      this.purchaseService.addPurchaseRequest(formData).subscribe({
        next: (res: any) => {
          this.purNumber = res.result;
          if (res.isSuccess === true) {
            this.purchaseRequests = [res.result, ...this.purchaseRequests];
            this.totalCount++;
          } else {
            console.error('فشل الإضافة:', res);
          }
        },
        error: (err) => {
          console.error('فشل الإضافة:', err);
          console.log('FormData sent:', formData);
        }
      });
    }
  }
  
  generatePurchaseRequestPDFById(id: number){
    
  }
  editPurchaseRequest(id: number){
    this.isEditMode = true;
    this.currentPurchaseRequestId = id;

    this.purchaseService.getPurchaseRequestById(id).subscribe({
      next: (data : any) => {
        this.request = data.result;
        console.log(this.request);
        this.purchaseRequestForm.patchValue({
          purpose: this.request.purpose || '',
          storeId: this.request.storeId || '',
          notes: this.request.notes || '',
          items: this.request.items || []
        });

        const modal = new bootstrap.Modal(document.getElementById('addPurchaseRequestModal')!);
        modal.show();
      },
      error: (err) => {
        console.error('فشل تحميل بيانات طلب الشراء:', err);
      }
    });
  }
  deletePurchaseRequest(id: number){
    Swal.fire({
      title: 'هل أنت متأكد؟',
      text: 'هل أنت متأكد من حذف طلب الشراء؟',
      icon: 'warning',
      showCancelButton: true,
      confirmButtonColor: '#3085d6',
      cancelButtonColor: '#d33',
      confirmButtonText: 'نعم، حذف',
      cancelButtonText: 'إلغاء'
    }).then((result) => {
      if (result.isConfirmed) {
        this.purchaseService.deletePurchaseRequest(id).subscribe({
          next: () => {
            this.loadPurchaseRequests();
          },
          error: (err) => {
            console.error('فشل حذف طلب الشراء:', err);
          }
        });
      }
    });
  }
  addItem(){

  }
  resetForm(){

  }
}
