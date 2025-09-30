import { Component } from '@angular/core';
import { FormGroup, FormBuilder, Validators, FormArray } from '@angular/forms';
import { forkJoin } from 'rxjs';
import Swal from 'sweetalert2';
import { PurchaseRequest } from '../../../core/models/purchase/purchase-request';
import { Items } from '../../../core/models/system-settings/item';
import { ItemsUnits } from '../../../core/models/system-settings/itemUnits';
import { MainGroups } from '../../../core/models/system-settings/maingroup';
import { Store } from '../../../core/models/system-settings/stores';
import { PurchaseService } from '../../../core/services/purchase.service';
import { SysSettingsService } from '../../../core/services/sys-settings.service';
import { MessageService } from 'primeng/api';
declare var bootstrap : any;
@Component({
  selector: 'app-purchase-request',
  templateUrl: './purchase-request.component.html',
  styleUrl: './purchase-request.component.css',
  providers:[MessageService]
})
export class PurchaseRequestComponent {
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

  // pagination state
  pageSize: number = 2;
  totalCount: number = 100;
  currentPage: number = 1;

  onPageChange(event: any) {
    this.pageSize = event.rows;
    this.currentPage = event.page + 1;
    this.loadPurchaseRequests();
  }
  

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
  constructor(private fb: FormBuilder , private purchaseService : PurchaseService , private systemSettingsService : SysSettingsService , private messageService : MessageService){
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
      Approved: 'تمت الموافقة',
      Pending: 'قيد الانتظار',
      Rejected: 'مرفوض'
    };
    return map[type] || type;
  }
  // getStatusColor(type: string): string {
  //   const map: { [key: string]: string } = {
  //     Approved: '#198654',
  //     Pending: '#FFA500',
  //     Rejected: '#dc3545'
  //   };
  //   return map[type] || '#000000';
  // }
  getStatusDotClass(status: string): string {
    switch (status) {
      case 'Approved': return 'bg-success';
      case 'Pending': return 'bg-warning';
      case 'Rejected': return 'bg-danger';
      default: return 'bg-secondary';
    }
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
    this.purchaseService.getPurchaseRequests(this.currentPage, this.pageSize, this.searchTerm).subscribe((res) => {
      this.purchaseRequests = res.result.data;
      this.totalCount = res.result.totalCount;
      console.log('Purchase Requests:', this.purchaseRequests);
    });
  }

  loadInitialData() {
    forkJoin({
      items: this.systemSettingsService.getAllItems(1, 100, '', true),
      units: this.systemSettingsService.getAllItemsUnits(1, 100, '', true),
      stores: this.systemSettingsService.getAllStores(''),
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
    this.currentPage = page;
    this.loadPurchaseRequests();
  }

  search() {
    this.currentPage = 1;
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

    const formData = this.purchaseRequestForm.value;

    if (this.isEditMode && this.currentPurchaseRequestId) {
      this.purchaseService.updatePurchaseRequest(this.currentPurchaseRequestId, formData).subscribe({
        next: (res: any) => {
          if (res.isSuccess === true) {
            this.messageService.add({
              severity: 'success',
              summary: 'تم التعديل',
              detail: `${res.message}`
            });
            // this.CloseModal();
            this.loadPurchaseRequests();
            console.log(res);
          } else {
            this.messageService.add({
              severity: 'error',
              summary: 'فشل التعديل',
              detail: `${res.message}`
            });
          }
        },
        error: (err) => {
          console.error('فشل التعديل:', err);
        }
      });
    } else {
      this.purchaseService.addPurchaseRequest(formData).subscribe({
        next: (res: any) => {
          this.purNumber = res.results;
          if (res.isSuccess === true) {
            this.messageService.add({
              severity: 'success',
              summary: 'تم الإضافة',
              detail: `${res.message}`
            });
            // this.CloseModal();
            this.purchaseRequestForm.reset();
            console.log(res);
            this.loadPurchaseRequests();
            this.generatePurchaseRequestPDFById(res.results);
          } else {
            this.messageService.add({
              severity: 'error',
              summary: 'فشل الإضافة',
              detail: `${res.message}`
            });
            console.log(res);
            console.log(formData);
            
          }
        },
        error: (err) => {
          console.error('فشل الإضافة:', err);
        }
      });
    }
  }
  editPurchaseRequest(id: number) {
    this.isEditMode = true;
    this.currentPurchaseRequestId = id;

    this.purchaseService.getPurchaseRequestById(id).subscribe({
      next: (res:any) => {
        this.request = res.result;
        console.log(this.request);
        this.purchaseRequestForm.patchValue({
          purpose: this.request.purpose,
          storeId: this.request.storeId,
          notes: this.request.notes,
          items: this.request.items
        });

        const modal = new bootstrap.Modal(document.getElementById('addPurchaseRequestModal')!);
        modal.show();
      },
      error: (err) => {
        console.error('فشل تحميل بيانات طلب الشراء:', err);
      }
    });
  }
  deletePurchaseRequest(id: number) {
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

  generatePurchaseRequestPDFById(id: number){
    
  }
  addItem(){

  }
  resetForm(){

  }
}
