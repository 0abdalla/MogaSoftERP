import { Component, OnInit } from '@angular/core';
import { FormArray, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MessageService } from 'primeng/api';
import { PriceQuotation } from '../../../core/models/purchase/priceQuotation';
import { Store } from '../../../core/models/system-settings/stores';
import { Items } from '../../../core/models/system-settings/item';
import { ItemsUnits } from '../../../core/models/system-settings/itemUnits';
import { MainGroups } from '../../../core/models/system-settings/maingroup';
import { PurchaseService } from '../../../core/services/purchase.service';
import { PurchaseRequest } from '../../../core/models/purchase/purchase-request';
import { Provider } from '../../../core/models/system-settings/providers';
import { SysSettingsService } from '../../../core/services/sys-settings.service';
import Swal from 'sweetalert2';
import { forkJoin } from 'rxjs';
declare var bootstrap: any;
@Component({
  selector: 'app-price-qoutations',
  templateUrl: './price-qoutations.component.html',
  styleUrl: './price-qoutations.component.css',
    providers:[MessageService]
})
export class PriceQoutationsComponent implements OnInit {
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
    currentPriceQuotationId: number | null = null;
    isFilter: boolean = false;
    // 
    priceQuotations: PriceQuotation[] = [];
    // purchaseRequests: PurchaseRequest[] = [];
    approvedPurchaseRequests: PurchaseRequest[] = [];
  
    // pagination state
    pageSize: number = 16;
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
    allSuppliers: Provider[] = [];
    // 
    priceQuotationForm: FormGroup;
    requestForm: FormGroup;
    // 
    quotationData: any[] = [];
    supplierNames: string[] = [];
    uniqueItems: string[] = [];
    structuredTable: any[] = [];
    selectedPurchaseRequestId!: number;
    quotationDate: string = '';
    purNumber: string = '';
    constructor(private fb : FormBuilder , private purchaseService : PurchaseService , private systemSettingsService : SysSettingsService , private messageService : MessageService){
      this.priceQuotationForm = this.fb.group({
        quotationDate:[new Date().toISOString().split('T')[0]],
        supplierId:[null,Validators.required],
        purchaseRequestId:[null,Validators.required], 
        notes:[''],
        items: this.fb.array([
          this.createItemGroup()
        ]),
      })
      this.requestForm = this.fb.group({
        purchaseRequestId:[null,Validators.required],
      })
    }
    ngOnInit(): void {
      this.loadPriceQuotations();
      this.loadFormData();
      this.priceQuotationForm.get('purchaseRequestId')?.valueChanges.subscribe(id => {
        if (id) {
          this.purchaseRequestSelected(id);
        }
      });
    }
    createItemGroup(): FormGroup {
      return this.fb.group({
        itemId: [null, Validators.required],
        unitPrice: [0, [Validators.required, Validators.min(1)]],
        quantity: [0, [Validators.required, Validators.min(1)]],
        notes: [''],
      });
    }
    
    get items(): FormArray {
      return this.priceQuotationForm.get('items') as FormArray;
    }
    
    addItemRow() {
      this.items.push(this.createItemGroup());
    }
    
    removeItemRow(index: number) {
      if (this.items.length > 1) {
        this.items.removeAt(index);
      }
    }
    goToPage(page: number) {
      this.currentPage = page;
      this.loadPriceQuotations();
    }
  
    search() {
      this.currentPage = 1;
      this.loadPriceQuotations();
    }
    resetSearch(){
      this.searchTerm = '';
      this.loadPriceQuotations();
    }
    trackByQoute(index: number, order: any): number {
      return order.id;
    }
    getStatusDotClass(status: string): string {
      switch (status) {
        case 'Approved': return 'bg-success';
        case 'Pending': return 'bg-warning';
        case 'Rejected': return 'bg-danger';
        default: return 'bg-secondary';
      }
    }  
    
  getStatusName(type: string): string {
    const map: { [key: string]: string } = {
      Approved: 'تمت الموافقة',
      Pending: 'قيد الانتظار',
      Rejected: 'مرفوض'
    };
    return map[type] || type;
  }
  // 
  loadPriceQuotations() {
    this.purchaseService.getPriceQuotations(this.currentPage, this.pageSize, this.searchTerm).subscribe((res) => {
      this.priceQuotations = res.result.data;
      this.totalCount = res.result.totalCount;
      console.log('Price Quotations:', this.priceQuotations);
    });
  }
  loadFormData(){
    forkJoin({
      purchaseRequests: this.purchaseService.getapprovedPurchaseRequests(),
      suppliers: this.systemSettingsService.getAllProviders(1,100,''),
      items: this.systemSettingsService.getAllItems(1,100,''),
    }).subscribe((res) => {
      this.approvedPurchaseRequests = res.purchaseRequests.result;
      console.log(this.approvedPurchaseRequests);
      this.allSuppliers = res.suppliers.result.data;
      this.allItems = res.items.result.data;
    })
  }
  
  addPriceQoutation() {
    if (this.priceQuotationForm.invalid) {
      this.priceQuotationForm.markAllAsTouched();
      return;
    }
  
    const formData = this.priceQuotationForm.value;
  
    if (this.isEditMode && this.currentPriceQuotationId) {
      this.purchaseService.updatePriceQuotation(this.currentPriceQuotationId, formData).subscribe({
        next: (res:any) => {
          this.loadPriceQuotations();
          if(res.isSuccess === true){
            this.messageService.add({
              severity: 'success',
              summary: 'تم التعديل',
              detail: `${res.message}`
            });
            // this.priceQuotationForm.reset();
          }else{
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
      this.purchaseService.addPriceQuotation(formData).subscribe({
        next: (res : any) => {
          console.log(res);
          this.loadPriceQuotations();
          if(res.isSuccess === true){
            this.messageService.add({
              severity: 'success',
              summary: 'تمت الإضافة',
              detail: `${res.message}`
            });
            // this.priceQuotationForm.reset();
            // this.resetForm();
          }else{
            this.messageService.add({
              severity: 'error',
              summary: 'فشل الإضافة',
              detail: `${res.message}`
            });
            console.log(this.priceQuotationForm.value);
            
          }
          // this.priceQuotationForm.reset();
        },
        error: (err) => {
          console.error('فشل الإضافة:', err);
          console.log(this.priceQuotationForm.value);
        }
      });
    }
  }
  order!:any;
  editPriceQoutation(id: number) {
    this.isEditMode = true;
    this.currentPriceQuotationId = id;
  
    this.purchaseService.getPriceQuotationById(id).subscribe({
      next: (data:any) => {
        this.order = data.result;
        const itemsArray = this.priceQuotationForm.get('items') as FormArray;
        itemsArray.clear();
        this.order.items.forEach((item: any) => {
          itemsArray.push(this.fb.group({
            itemId: [item.itemId ?? item.id, Validators.required],
            quantity: [item.quantity, Validators.required],
            unitPrice: [item.unitPrice, Validators.required],
            notes: [item.notes || '']
          }));
        });
        this.priceQuotationForm.patchValue({
          purchaseRequestId: this.order.purchaseRequestId,
          supplierId: this.order.supplierId,
          notes: this.order.notes
        });
        const modal = new bootstrap.Modal(document.getElementById('addOfferModal')!);
        modal.show();
      },
      error: (err) => {
        console.error('فشل تحميل بيانات عرض السعر:', err);
      }
    });
  }
  
  deletePriceQoutation(id: number) {
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
        this.purchaseService.deletePriceQuotation(id).subscribe({
          next: () => {
            this.loadPriceQuotations();
          },
          error: (err) => {
            console.error('فشل حذف طلب الشراء:', err);
          }
        });
      }
    });
  }
  generatePriceQoutationPDFById(id:number){}
  // 
  resetForm(){
    // this.priceQuotationForm.reset();
    this.priceQuotationForm.get('quotationDate')?.setValue(new Date().toISOString().split('T')[0]);
    this.priceQuotationForm.get('supplierId')?.setValue(null);
    this.priceQuotationForm.get('notes')?.setValue('');
    this.items.clear();
    this.addItemRow();
  }
  // 
  purchaseRequestSelected(id: number) {
    this.purchaseService.getPurchaseRequestById(id).subscribe((res: any) => {
      const request = res.result; // تأكد إنها فعلاً result
      console.log('Full API response:', res);
      console.log('Request object:', request);
  
      this.items.clear();
  
      if (request && request.items && request.items.length > 0) {
        request.items.forEach((item: any) => {
          this.items.push(this.fb.group({
            itemId: [item.itemId || item.id, Validators.required],
            quantity: [item.quantity, Validators.required],
            unitPrice: [item.unitPrice || 0, Validators.required],
            notes: [item.notes || '']
          }));
        });
      } else {
        this.addItemRow();
      }
  
      this.priceQuotationForm.patchValue({
        notes: request.notes || ''
      });
    });
  }
  
  loadQuotationsByRequestId(purchaseRequestId: number) {
    this.selectedPurchaseRequestId = purchaseRequestId;
  
    this.purchaseService.getPriceQuotationByPurchaseRequestId(purchaseRequestId).subscribe((res: any) => {
      const quotations = res.result || [];
  
      if (!quotations.length) {
        Swal.fire({
          icon: "error",
          title: "حدث خطأ",
          text: "لا يوجد عروض أسعار لطلب الشراء المحدد",
          confirmButtonText: 'حسنًا',
          confirmButtonColor: '#3085d6',
          timer: 5000
        });
        return;
      }
  
      this.quotationData = quotations;
      this.quotationDate = quotations[0]?.quotationDate; // أول تاريخ عرض سعر
      this.purNumber = quotations[0]?.purchaseRequestNumber;
  
      // أسماء الموردين
      this.supplierNames = quotations.map((q: any) => q.supplierName);
  
      // كل الأصناف (باسمها العربي name من الريسبونس)
      const allNames = quotations.flatMap((q: any) => q.items.map((i: any) => i.name));
      this.uniqueItems = Array.from(new Set(allNames));
  
      // بناء جدول منسق
      this.structuredTable = this.uniqueItems.map(name => {
        const row: any = { name };
        quotations.forEach((quotation: any) => {
          const item = quotation.items.find((i: any) => i.name === name);
          row[quotation.supplierName] = {
            quantity: item?.quantity || 0,
            unitPrice: item?.unitPrice || 0,
            total: item?.totalPrice || 0
          };
        });
        return row;
      });
  
      // إغلاق وفتح المودالز
      const selectModalEl = document.getElementById('selectRequestModal');
      const selectModal = bootstrap.Modal.getInstance(selectModalEl!);
      selectModal?.hide();
  
      setTimeout(() => {
        document.querySelectorAll('.modal-backdrop').forEach(el => el.remove());
        document.body.classList.add('modal-open');
        const detailsModalEl = document.getElementById('requestDetailsModal');
        const detailsModal = new bootstrap.Modal(detailsModalEl!);
        detailsModal.show();
      }, 300);
  
    }, error => {
      console.error('فشل تحميل عروض الأسعار:', error);
      alert('حدث خطأ أثناء تحميل عروض الأسعار.');
    });
  }
  
  // 
  
  getTotalForSupplier(supplier: string): number {
    if (!this.structuredTable) return 0;
  
    return this.structuredTable.reduce((sum, row) => {
      return sum + (row[supplier]?.total || 0);
    }, 0);
  }
  getWinningSupplier(): string {
    if (!this.supplierNames || this.supplierNames.length === 0) return 'لا يوجد بيانات';
  
    const totals = this.supplierNames.map(name => {
      const total = this.getTotalForSupplier(name);
      return {
        name,
        total: total ?? Infinity
      };
    });
  
    if (totals.length === 0) return 'لا يوجد بيانات';
  
    const winner = totals.reduce((min, curr) => curr.total < min.total ? curr : min, totals[0]);
  
    return winner.total !== Infinity ? winner.name : 'لا يوجد بيانات';
  }
  postPriceQuotation(){
    this.purchaseService.putPriceQutataion(this.selectedPurchaseRequestId, '').subscribe({
      next: () => {
        this.loadPriceQuotations();
      },
      error: (err) => {
        console.error('فشل حفظ العرض:', err);
      }
    });
  }
  printOffers(){}
}
