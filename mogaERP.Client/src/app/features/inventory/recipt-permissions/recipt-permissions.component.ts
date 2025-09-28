import { Component } from '@angular/core';
import { MessageService } from 'primeng/api';
import { ReciptPermission } from '../../../core/models/inventory/reciptPermissions';
import { FormArray, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { InventoryService } from '../../../core/services/inventory.service';
import Swal from 'sweetalert2';
import { forkJoin } from 'rxjs';
import { SysSettingsService } from '../../../core/services/sys-settings.service';
import { PurchaseService } from '../../../core/services/purchase.service';
import { Item, PurchaseOrder } from '../../../core/models/purchase/purchase-order';
import { Store } from '../../../core/models/system-settings/stores';
import { Provider } from '../../../core/models/system-settings/providers';
import { Items } from '../../../core/models/system-settings/item';
declare var bootstrap : any;
@Component({
  selector: 'app-recipt-permissions',
  templateUrl: './recipt-permissions.component.html',
  styleUrl: './recipt-permissions.component.css',
  providers:[MessageService]
})
export class ReciptPermissionsComponent {
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
  currentReciptPermissionId: number | null = null;
  isFilter: boolean = false;
  // 
  reciptPermissions: ReciptPermission[] = [];
  // 
  allItems: Items[] = [];
  allStores: Store[] = [];
  allSuppliers: Provider[] = [];
  purchaseOrders: PurchaseOrder[] = [];
  // 
  // pagination state
  pageSize: number = 2;
  totalCount: number = 100;
  currentPage: number = 1;
  searchTerm: string = '';
  get math() {
    return Math;
  }
  // 
  addPermissionForm!: FormGroup;
  constructor(private fb : FormBuilder , private messageService : MessageService , private inventoryService : InventoryService , private systemService : SysSettingsService , private purchaseService : PurchaseService){
    this.addPermissionForm = this.fb.group({
      documentNumber: ['' , Validators.required],
      permissionDate: [new Date().toISOString().substring(0, 10)],
      notes: [''],
      items: this.fb.array([
        this.createItemGroup()
      ]),
      storeId: [''],
      supplierId: [''],
      purchaseOrderId: [''],
    }); 
  }
  ngOnInit(): void {
    this.loadReciptPermissions();
    this.loadFormData();
    this.setupQuotationSelectionListener();
  }
  createItemGroup(): FormGroup {
    return this.fb.group({
      id: ['', Validators.required],
      unit: ['', Validators.required],
      quantity: [1, Validators.required],
      unitPrice: [1, Validators.required],
      totalPrice: [''],
      notes: ['']
    });
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

  trackByReciptPermission(index: number, reciptPermission: ReciptPermission): number {
    return reciptPermission.id;
  }
  // 
  savedOrderData!: any;
  // CRUD
  loadReciptPermissions() {
    this.inventoryService.getReciptPermissions(this.currentPage, this.pageSize, this.searchTerm).subscribe((res:any) => {
      this.reciptPermissions = res.result.data;
      this.totalCount = res.result.totalCount;
      console.log('Recipt Permissions:', this.reciptPermissions);
    });
  }
  loadFormData(){
    forkJoin({
      allItems: this.systemService.getAllItems(1 , 100 , ''),
      allStores: this.systemService.getAllStores(''),
      allSuppliers: this.systemService.getAllProviders(1 , 100 , ''),
      purchaseOrders: this.purchaseService.getPurchaseOrders(1 , 100 , '')
    }).subscribe((res:any) => {
      this.allItems = res.allItems.result.data;
      this.allStores = res.allStores.result;
      this.allSuppliers = res.allSuppliers.result.data;
      this.purchaseOrders = res.purchaseOrders.result.data;
      console.log(this.allItems);
      console.log(this.allStores);
      console.log(this.allSuppliers);
      console.log(this.purchaseOrders);
    });
  }

  addPermission() {
    if (this.addPermissionForm.invalid) {
      this.addPermissionForm.markAllAsTouched();
      return;
    }
  
    const formData = this.addPermissionForm.value;

    const supplierName = this.getSupplierName(formData.supplierId);
    const storeName = this.getStoreName(formData.storeId);
    
    const itemsWithNames = formData.items.map((item: any) => {
      const itemName = this.getItemName(item.id);
      const totalPrice = item.quantity * item.unitPrice;
      return { ...item, itemName, totalPrice };
    });
    
    const totalAmount = itemsWithNames.reduce((sum: number, i: any) => sum + i.totalPrice, 0);
    
    this.savedOrderData = {
      ...formData,
      supplierName,
      storeName,
      items: itemsWithNames,
      totalAmount
    };
      
    if (this.isEditMode && this.currentReciptPermissionId) {
      this.inventoryService.updateReciptPermission(this.currentReciptPermissionId, formData).subscribe({
        next: () => this.loadReciptPermissions(),
        error: (err) => console.error('فشل التعديل:', err)
      });
    } else {
      this.inventoryService.addReciptPermission(formData).subscribe({
        next: (res: any) => {
          const formData = this.addPermissionForm.value;
          console.log('Full Data:' , res);
          console.log(formData);
          console.log(formData);
          if (!this.allItems?.length || !this.allStores?.length || !this.allSuppliers?.length) {
            console.error('Required data not loaded');
            return;
          }
          this.loadReciptPermissions();
          const supplier = this.getSupplierName(formData.supplierId);
          const store = this.getStoreName(formData.storeId);
  
          const itemsWithNames = formData.items.map((item: any) => {
            const itemName = this.getItemName(item.id)
            const totalPrice = item.quantity * item.unitPrice;
            return { ...item, itemName, totalPrice };
          });
  
          const totalAmount = itemsWithNames.reduce((sum: number, i: any) => sum + i.totalPrice, 0);
  
          this.savedOrderData = {
            ...formData,
            supplierName: supplier,
            storeName: store,
            items: itemsWithNames,
            totalAmount,
            accountingGuidanceName: res.results.accountingGuidanceName,
            restrictionNumber: res.results.restrictionNumber,
            number: res.results.number
          };    
          // this.addPermissionForm.reset();
          if(res.isSuccess === true){
            this.messageService.add({
              severity: 'success',
              summary: 'تم الإضافة',
              detail: `${res.message}`
            });
          }else{
            this.messageService.add({
              severity: 'error',
              summary: 'فشل الإضافة',
              detail: `${res.message}`
            });
          console.log(formData);
          }
        },
        error: (err) => {
          console.error('فشل الإضافة:', err);
          console.log(formData);
          
        }
      });
    }
  }
  
  permission!:any;
  editReciptPermission(id: number) {
    this.isEditMode = true;
    this.currentReciptPermissionId = id;
  
    this.inventoryService.getReciptPermissionById(id).subscribe({
      next: (data:any) => {
        this.permission = data.result;
        const itemsArray = this.addPermissionForm.get('items') as FormArray;
        itemsArray.clear();
        this.permission.items.forEach((item: any) => {
          itemsArray.push(this.fb.group({
            id: [item.id, Validators.required],
            unit: [item.unit, Validators.required],
            quantity: [item.quantity, Validators.required],
            unitPrice: [item.unitPrice, Validators.required],
            totalPrice: [item.totalPrice, Validators.required],
          }));
        });
        this.addPermissionForm.patchValue({
          supplierId: this.permission.supplierId,
          documentNumber: this.permission.documentNumber,
          permissionDate: this.permission.permissionDate,
          notes: this.permission.notes,
          storeId: this.permission.storeId,
          purchaseOrderId: this.permission.purchaseOrderId
        });
        const modal = new bootstrap.Modal(document.getElementById('addReciptPermissionModal')!);
        modal.show();
      },
      error: (err) => {
        console.error('فشل تحميل بيانات إذن الإستلام:', err);
      }
    });
  }
  
  deleteReciptPermission(id: number) {
    Swal.fire({
      title: 'هل أنت متأكد؟',
      text: 'هل أنت متأكد من حذف إذن الإستلام؟',
      icon: 'warning',
      showCancelButton: true,
      confirmButtonColor: '#3085d6',
      cancelButtonColor: '#d33',
      confirmButtonText: 'نعم، حذف',
      cancelButtonText: 'إلغاء'
    }).then((result) => {
      if (result.isConfirmed) {
        this.inventoryService.deleteReciptPermission(id).subscribe({
          next: () => {
            this.loadReciptPermissions();
          },
          error: (err) => {
            console.error('فشل حذف إذن الإستلام:', err);
          }
        });
      }
    });
  }

  generateReciptPermissionPDFById(id:number){}

  // Helpers
  
  setupQuotationSelectionListener() {
    this.addPermissionForm.get('purchaseOrderId')?.valueChanges.subscribe((id: number) => {
      if (id) {
        this.purchaseService.getPurchaseOrderById(id).subscribe((res: any) => {
          const order = res.result;
          console.log(order);
          
          this.addPermissionForm.patchValue({
            supplierId: order.supplierId,
            notes: order.description || ''
          });
          this.items.clear();
          order.items.forEach((item: any) => {
            this.items.push(this.fb.group({
              id: [item.itemId, Validators.required],
              unit: [item.unit || '', Validators.required],
              quantity: [item.requestedQuantity || 1, Validators.required],
              unitPrice: [item.unitPrice || 0, Validators.required],
              totalPrice: [item.requestedQuantity * item.unitPrice],
              notes: ['']
            }));
          });
        });
      }
    });
  }
  
  getTotal(): number {
    return this.items.value.reduce((sum: number, item: any) => sum + (+item.totalPrice || 0), 0);
  }
  
  getItemName(id: number): string {
    const item = this.allItems?.find(i => +i.id === +id);
    return item?.name || '---';
  }  
  
  getSupplierName(id: number): string {
    const supplier = this.allSuppliers?.find(s => +s.id === +id);
    return supplier?.name || '---';
  }
  
  getStoreName(id: number): string {
    const store = this.allStores?.find(s => +s.id === +id);
    return store?.name || '---';
  }  
  // pagination handlers
  goToPage(page: number) {
    this.currentPage = page;
    this.loadReciptPermissions();
  }

  search() {
    this.currentPage = 1;
    this.loadReciptPermissions();
  }
  resetSearch(){
    this.searchTerm = '';
    this.loadReciptPermissions();
  }
  // 
  resetForm(){
    this.addPermissionForm.reset();
    this.isEditMode = false;
    this.currentReciptPermissionId = null;
  }
}
