import { Component } from '@angular/core';
import { FormGroup, FormBuilder, Validators, FormArray } from '@angular/forms';
import { InventoryService } from '../../../core/services/inventory.service';
import { MessageService } from 'primeng/api';
import { SysSettingsService } from '../../../core/services/sys-settings.service';
import Swal from 'sweetalert2';
import { ExchangeRequest } from '../../../core/models/inventory/exchangeRequest';
declare var bootstrap: any;
@Component({
  selector: 'app-exchange-request',
  templateUrl: './exchange-request.component.html',
  styleUrl: './exchange-request.component.css',
  providers:[MessageService]
})
export class ExchangeRequestComponent {
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
    currentExchangePermissionId!: number;
    // 
    exchangeRequests: ExchangeRequest[] = [];
    allItems: any[] = [];
    jobDeps: any[] = [];
  
    // pagination state
    pageSize: number = 1
    totalCount: number = 0;
    currentPage: number = 1;
    searchTerm: string = '';
    // 
    exchangePermissionForm!: FormGroup;
  constructor(private inventoryService : InventoryService , private fb : FormBuilder , private sysService : SysSettingsService , private messageService : MessageService){
    this.exchangePermissionForm = this.fb.group({
      jobDepartmentId: ['' , Validators.required],
      date: [new Date().toISOString().substring(0, 10)],
      notes: [''],
      items: this.fb.array([
        this.createItemGroup()
      ]),
    })
  }
  ngOnInit(){
    this.loadExchangeRequests();
    this.loadAllItems();
    this.loadJobDepartments();
  }
  createItemGroup(): FormGroup {
    return this.fb.group({
      itemId: ['', Validators.required],
      quantity: [1, [Validators.required , Validators.min(1)]]
    });
  }
  get items(): FormArray {
      return this.exchangePermissionForm.get('items') as FormArray;
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
  loadExchangeRequests(){
    this.inventoryService.getExchangeRequest(this.currentPage , this.pageSize , this.searchTerm , true)
    .subscribe({
      next : (response : any) => {
        this.exchangeRequests = response.result.data;
        this.totalCount = response.result.totalCount;
        console.log(this.exchangeRequests);
        console.log('Response' , response);
        
      },
      error : (error) => {
        console.log(error);
      }
    })
  }
  loadAllItems(){
    this.sysService.getAllItems(1 , 100 , '')
    .subscribe({
      next : (response : any) => {
        this.allItems = response.result.data;
        console.log(this.allItems);
      },
      error : (error) => {
        console.log(error);
      }
    })
  }
  loadJobDepartments(){
    this.sysService.getDepartments(1 , 100 , '')
    .subscribe({
      next : (response : any) => {
        this.jobDeps = response.result;
        console.log(this.jobDeps);
      },
      error : (error) => {
        console.log(error);
      }
    })
  }
  trackByExchangePermission(index : number , item : ExchangeRequest){
    return item.id;
  }
  savedExchangePermissionData!:any;
  addExchangePermission() {
    if (this.exchangePermissionForm.invalid) {
      this.exchangePermissionForm.markAllAsTouched();
      return;
    }
    const formData = this.exchangePermissionForm.value;
    const enrichedItems = formData.items.map((item:any) => {
      const fullItem = this.allItems.find((i:any) => i.id === item.itemId);
      return {
        ...item,
        itemNameAr: fullItem?.nameAr || '---',
        itemName: fullItem?.nameEn || '---'
      };
    });

    const enrichedData = {
      ...formData,
      items: enrichedItems
    };

    if (this.isEditMode && this.currentExchangePermissionId) {
      this.inventoryService.updateExchangeRequest(this.currentExchangePermissionId, formData).subscribe({
        next: () => {
          this.loadExchangeRequests();
          this.messageService.add({
            severity: 'success',
            summary: 'تم التعديل',
            detail: 'تم تعديل طلب الصرف بنجاح'
          });
        },
        error: (err) => console.error('فشل التعديل:', err)
      });
    } else {
      this.inventoryService.addExchangeRequest(formData).subscribe({
        next: (res: any) => {
          this.loadExchangeRequests();
          this.savedExchangePermissionData = enrichedData;
          console.log('Data ready for printing:', this.savedExchangePermissionData);
          // this.printExchangePermissionForm();
          this.exchangePermissionForm.reset();
          this.messageService.add({
            severity: 'success',
            summary: 'تم الإضافة',
            detail: 'تم إضافة طلب الصرف بنجاح'
          });
        },
        error: (err) => console.error('فشل الإضافة:', err)
      });
    }
  }
  issueRequest!:any;
  editExchangePermission(id: number) {
    this.isEditMode = true;
    this.currentExchangePermissionId = id;
  
    this.inventoryService.getExchangeRequestById(id).subscribe({
      next: (data:any) => {
        this.issueRequest=data.result;
        console.log(this.issueRequest);
        this.exchangePermissionForm.patchValue({
          supplierId: this.issueRequest.supplierId,
          documentNumber: this.issueRequest.documentNumber,
          date: this.issueRequest.date,
          notes: this.issueRequest.notes,
          items: this.issueRequest.items,
          storeId: this.issueRequest.storeId,
          jobDepartmentId: this.issueRequest.jobDepartmentId,
          purchaseOrderId: this.issueRequest.purchaseOrderId
        });
  
        const modal = new bootstrap.Modal(document.getElementById('addExchangePermissionModal')!);
        modal.show();
      },
      error: (err) => {
        console.error('فشل تحميل بيانات طلب الصرف:', err);
      }
    });
  }
  deleteExchangePermission(id: number) {
    Swal.fire({
      title: 'هل أنت متأكد؟',
      text: 'هل أنت متأكد من حذف طلب الصرف؟',
      icon: 'warning',
      showCancelButton: true,
      confirmButtonColor: '#3085d6',
      cancelButtonColor: '#d33',
      confirmButtonText: 'نعم، حذف',
      cancelButtonText: 'إلغاء'
    }).then((result) => {
      if (result.isConfirmed) {
        this.inventoryService.deleteExchangeRequest(id).subscribe({
          next: () => {
            this.loadExchangeRequests();
            this.messageService.add({
              severity: 'success',
              summary: 'تم الحذف',
              detail: 'تم حذف طلب الصرف بنجاح'
            });
          },
          error: (err) => {
            console.error('فشل حذف طلب الصرف:', err);
            this.messageService.add({
              severity: 'error',
              summary: 'فشل الحذف',
              detail: 'فشل حذف طلب الصرف'
            });
          }
        });
      }
    });
  }
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
    this.loadExchangeRequests();
  }
  goToPage(page: number) {
    this.currentPage = page;
    this.loadExchangeRequests();
  }
  onPageChange(event: any) {
    this.currentPage = event.pageIndex + 1;
    this.pageSize = event.pageSize;
    this.loadExchangeRequests();
  }
  get math(){
    return Math;
  }
  // 
  resetForm(){
    this.exchangePermissionForm.reset();
    this.items.clear();
    this.addItemRow();
  }
}
