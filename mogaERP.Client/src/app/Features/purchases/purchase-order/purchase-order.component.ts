import { Component, OnInit } from '@angular/core';
import { MessageService } from 'primeng/api';
import { Items } from '../../../core/models/system-settings/item';
import { MainGroups } from '../../../core/models/system-settings/maingroup';
import { ItemsUnits } from '../../../core/models/system-settings/itemUnits';
import { Store } from '../../../core/models/system-settings/stores';
import { PurchaseOrder } from '../../../core/models/purchase/purchase-order';
import { FormArray, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { PurchaseService } from '../../../core/services/purchase.service';
import { PurchaseRequest } from '../../../core/models/purchase/purchase-request';
import { Provider } from '../../../core/models/system-settings/providers';
import { SysSettingsService } from '../../../core/services/sys-settings.service';
import { forkJoin } from 'rxjs';
import Swal from 'sweetalert2';
declare var bootstrap : any;
@Component({
  selector: 'app-purchase-order',
  templateUrl: './purchase-order.component.html',
  styleUrl: './purchase-order.component.css',
  providers:[MessageService]
})
export class PurchaseOrderComponent implements OnInit {
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
  currentPurchaseRequestId: number | null = null;
  isFilter: boolean = false;
  // 
  purchaseOrders: PurchaseOrder[] = [];
  // pagination state
  pageSize: number = 2;
  totalCount: number = 100;
  currentPage: number = 1;
  searchTerm: string = '';
  get math() {
    return Math;
  }
  // 
  stores: Store[] = [];
  allItems: Items[] = [];
  groups: MainGroups[] = [];
  units: ItemsUnits[] = [];
  purchaseRequests: PurchaseRequest[] = [];
  allSuppliers: Provider[] = [];
  // 
  purchaseOrderForm: FormGroup;
  constructor(private fb : FormBuilder , private purchaseService : PurchaseService,private messageService : MessageService , private sysService : SysSettingsService){
    this.purchaseOrderForm=this.fb.group({
      orderDate:[new Date().toISOString()],
      purchaseRequestId:[null,Validators.required],
      supplierId:[null,Validators.required],
      description:[''],
      items: this.fb.array([
        this.createItemGroup()
      ]),
    })
  }
  ngOnInit(): void {
    this.loadPurchaseOrders();
    this.loadFormData();
    this.setupQuotationSelectionListener();
  }
  createItemGroup(): FormGroup {
    return this.fb.group({
      itemId: [null, Validators.required],
      unitId: [null, Validators.required],
      requestedQuantity: [0, [Validators.required, Validators.min(1)]],
      unitPrice: [0, [Validators.required, Validators.min(1)]],
      totalPrice: [0, [Validators.required, Validators.min(1)]]
    });
  }
  
  get items(): FormArray {
    return this.purchaseOrderForm.get('items') as FormArray;
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
  goToPage(page: number) {
    this.currentPage = page;
    this.loadPurchaseOrders();
  }

  search() {
    this.currentPage = 1;
    this.loadPurchaseOrders();
  }
  resetSearch(){
    this.searchTerm = '';
    this.loadPurchaseOrders();
  }
  trackByOrder(index: number, order: any): number {
    return order.id;
  }
  // 
  loadPurchaseOrders(){
    this.purchaseService.getPurchaseOrders(this.currentPage, this.pageSize, this.searchTerm).subscribe((res) => {
      this.purchaseOrders = res.result.data;
      this.totalCount = res.result.totalCount;
      console.log('Purchase Orders:', this.purchaseOrders);
    });
  }
  loadFormData(){
    forkJoin({
      items:this.sysService.getAllItems(1 ,100),
      groups:this.sysService.getAllMainGroups(),
      units:this.sysService.getAllItemsUnits(1 ,100),
      purchaseRequests:this.purchaseService.getPurchaseRequests(1 ,100),
      allSuppliers:this.sysService.getAllProviders(1 ,100)
    }).subscribe((res) => {
      this.allItems = res.items.result.data;
      this.groups = res.groups.result.data;
      this.units = res.units.result.data;
      this.purchaseRequests = res.purchaseRequests.result.data;
      this.allSuppliers = res.allSuppliers.result.data;
      console.log('Items:', res.items);
      console.log('Groups:', res.groups);
      console.log('Units:', res.units);
      console.log('Purchase Requests:', res.purchaseRequests);
      console.log('Suppliers:', res.allSuppliers);
    })
  }
  addPurchaseOrder() {
    if (this.purchaseOrderForm.invalid) {
      this.purchaseOrderForm.markAllAsTouched();
      return;
    }
  
    const formData = this.purchaseOrderForm.value;
  
    if (this.isEditMode && this.currentPurchaseRequestId) {
      this.purchaseService.updatePurchaseOrder(this.currentPurchaseRequestId, formData).subscribe({
        next: () => {
          this.loadPurchaseOrders();
        },
        error: (err) => {
          console.error('فشل التعديل:', err);
        }
      });
    } else {
      this.purchaseService.addPurchaseOrder(formData).subscribe({
        next: (res: any) => {
          if (res.isSuccess) {
            this.loadPurchaseOrders();
            this.messageService.add({
              severity: 'success',
              summary: 'تم الإضافة',
              detail: `${res.message}`
            });
          }
        },
        error: (err) => {
          console.error('فشل الإضافة:', err);
        }
      });
    }
  }
  order!:any;
  editPurchaseOrder(id: number) {
    this.isEditMode = true;
    this.currentPurchaseRequestId = id;
  
    this.purchaseService.getPurchaseOrderById(id).subscribe({
      next: (data:any) => {
        this.order=data.results;
        console.log(this.order);
        const itemsArray = this.purchaseOrderForm.get('items') as FormArray;
        itemsArray.clear();
        this.order.items.forEach((item: any) => {
          itemsArray.push(this.fb.group({
            itemId: [item.itemId ?? item.id, Validators.required],
            requestedQuantity: [item.requestedQuantity ?? item.quantity, Validators.required],
            unitPrice: [item.unitPrice, Validators.required],
            totalPrice: [item.totalPrice ?? (item.unitPrice * item.requestedQuantity), Validators.required],
          }));          
        });
        this.purchaseOrderForm.patchValue({
          purchaseRequestId: this.order.purchaseRequestId,
          supplierId: this.order.supplierId,
          notes: this.order.notes
        });
  
        const modal = new bootstrap.Modal(document.getElementById('addPurchaseorderModal')!);
        modal.show();
      },
      error: (err) => {
        console.error('فشل تحميل بيانات طلب الشراء:', err);
      }
    });
  }
  deletePurchaseOrder(id: number) {
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
        this.purchaseService.deletePurchaseOrder(id).subscribe({
          next: () => {
            this.loadPurchaseOrders();
          },
          error: (err) => {
            console.error('فشل حذف طلب الشراء:', err);
          }
        });
      }
    });
  }
  generatePurchaseOrderPDFById(id:number){}
  // 
  resetForm(){
    this.purchaseOrderForm.reset();
    this.items.clear();
    this.addItemRow();
  }
  // 
  setupQuotationSelectionListener() {
    this.purchaseOrderForm.get('purchaseRequestId')?.valueChanges.subscribe((id: number) => {
      if (id) {
        this.purchaseService.getPurchaseRequestById(id).subscribe((res: any) => {
          console.log(res);
          const price = res.result.priceQuotation;
          console.log(price);
          this.purchaseOrderForm.patchValue({
            supplierId: price.supplierId,
            description: price.notes
          });
  
          this.items.clear();
          price.items.forEach((item: any) => {
            this.items.push(this.fb.group({
              itemId: [item.id],
              requestedQuantity: [item.quantity],
              unitPrice: [item.unitPrice],
              totalPrice: [item.total]
            }));
          });
        });
      }
    });
  }
}
