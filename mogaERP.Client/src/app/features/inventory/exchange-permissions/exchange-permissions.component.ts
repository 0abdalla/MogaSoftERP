import { Component, OnInit } from '@angular/core';
import { InventoryService } from '../../../core/services/inventory.service';
import { FormArray, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MessageService } from 'primeng/api';
import { ExchangePermission } from '../../../core/models/inventory/exchangePermissions';
import { ExchangeRequest } from '../../../core/models/inventory/exchangeRequest';
import { Store } from '../../../core/models/system-settings/stores';
import { Department } from '../../../core/models/system-settings/departments';
import { Items } from '../../../core/models/system-settings/item';
import { forkJoin } from 'rxjs';
import { SysSettingsService } from '../../../core/services/sys-settings.service';

@Component({
  selector: 'app-exchange-permissions',
  templateUrl: './exchange-permissions.component.html',
  styleUrl: './exchange-permissions.component.css',
  providers : [MessageService]
})
export class ExchangePermissionsComponent implements OnInit {
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
    exchangePermissions: ExchangePermission[] = [];
    approvedExchangeRequests: ExchangeRequest[] = [];
    stores: Store[] = [];
    jobDepartments: Department[] = [];
    allItems: any;
  
    // pagination state
    pageSize: number = 2;
    totalCount: number = 100;
    currentPage: number = 1;
    searchTerm: string = '';
    // 
    addPermissionForm!: FormGroup;
  constructor(private inventoryService : InventoryService , private fb : FormBuilder , private sysService : SysSettingsService){
    this.addPermissionForm = this.fb.group({
      permissionDate: [new Date().toISOString().substring(0, 10)],
      disbursementRequestId: ['' , Validators.required],
      storeId: ['' , Validators.required],
      jobDepartmentId : ['' , Validators.required],
      items: this.fb.array([
        this.createItemGroup()
      ]),
      notes: ['']
    });  
  }
  ngOnInit(){
    this.loadExchangePermissions();
    this.loadFormData();
  }
  createItemGroup(item: any = null): FormGroup {
    const unitPriceValue = item?.priceAfterTax ?? 1;
    const quantityValue = item?.quantity ?? 1;
    const group = this.fb.group({
      itemId: [item?.itemId ?? '', Validators.required],
      unit: [item?.unit ?? '', Validators.required],
      quantity: [quantityValue, [Validators.required , Validators.min(1)]],
      unitPrice: [{ value: unitPriceValue, disabled: true }, Validators.required],
      totalPrice: [quantityValue * unitPriceValue, Validators.required]
    });
    group.get('quantity')?.valueChanges.subscribe(qty => {
      const price = group.get('unitPrice')?.value || 0;
      group.get('totalPrice')?.setValue(qty * price, { emitEvent: false });
    });
    return group;
  }
  get items(): FormArray {
      return this.addPermissionForm.get('items') as FormArray;
  }
  addItemRow() {
    this.items.push(this.createItemGroup());
  }
    
  removeItemRow(index: number) {
    if (this.items.length > 1) {
    this.items.removeAt(index);
    }
  }
  // CRUD
  loadExchangePermissions(){
    this.inventoryService.getExchangePermissions(this.currentPage , this.pageSize , this.searchTerm , true)
    .subscribe({
      next : (response : any) => {
        this.exchangePermissions = response.result.data;
        this.totalCount = response.result.totalCount;
        console.log(this.exchangePermissions);
      },
      error : (error) => {
        console.log(error);
      }
    })
  }
  loadFormData(){
    forkJoin({
      stores : this.sysService.getAllStores(''),
      jobDepartments : this.sysService.getDepartments(1 , 100 , ''),
      items : this.sysService.getAllItems(1 , 100 , '')
    }).subscribe({
      next : (response : any) => {
        this.stores = response.stores.result;
        this.jobDepartments = response.jobDepartments.result;
        this.allItems = response.items.result.data;
        console.log('stores',this.stores);
        console.log('jobDepartments',this.jobDepartments);
        console.log('items',this.allItems);
        this.loadExchangePermissions();
        
      },
      error : (error) => {
        console.log(error);
      }
    })
  }
  trackByExchangePermission(index : number , item : ExchangePermission){
    return item.id;
  }
  addPermission(){}
  editExchangePermission(id:number){}
  deleteExchangePermission(id:number){}
  // 
  
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
  generateExchangePermissionPDFById(id:number){}
  // Pagination handlers
  search(){
    this.currentPage = 1;
    this.loadExchangePermissions();
  }
  goToPage(page: number) {
    this.currentPage = page;
    this.loadExchangePermissions();
  }
  onPageChange(event: any) {
    this.currentPage = event.pageIndex + 1;
    this.pageSize = event.pageSize;
    this.loadExchangePermissions();
  }
  get math(){
    return Math;
  }
  // 
  resetForm(){
    this.addPermissionForm.reset();
  }
}
