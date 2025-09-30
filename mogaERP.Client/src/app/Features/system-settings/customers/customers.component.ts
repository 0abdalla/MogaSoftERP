import { Component, Provider } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { MessageService } from 'primeng/api';
import { SysSettingsService } from '../../../core/services/sys-settings.service';
import { Customers } from '../../../core/models/system-settings/customers';
import Swal from 'sweetalert2';
declare var bootstrap : any;
@Component({
  selector: 'app-customers',
  templateUrl: './customers.component.html',
  styleUrl: './customers.component.css',
  providers:[MessageService]
})
export class CustomersComponent {
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
  currentCustomerId!:any;
  isFilter: boolean = false;
  // 
  customers:Customers[] = [];
  // pagination state
  searchTerm: string = '';
  currentPage: number = 1;
  pageSize: number = 16;
  totalCount: number = 0;
  // 
  addCustomerForm: FormGroup;
  // 
  constructor(private fb: FormBuilder , private service : SysSettingsService , private toastService : MessageService){
    this.addCustomerForm = this.fb.group({
      name: [null, Validators.required],
      accountCode: [null, Validators.required],
      address: [null, Validators.required],
      phoneNumber: [null, Validators.required],
      taxNumber: [''],
      commercialRegistration: [''],
      paymentType: ['Cash'],
      creditLimit: ['']
    });
  }
  ngOnInit(): void {
    this.loadCustomers();
  }
  // CRUD
  loadCustomers(){
    this.service.getCustomers(this.currentPage, this.pageSize, this.searchTerm, true).subscribe((res) => {
      this.customers = res.result.data;
      this.totalCount = res.result.totalCount;
      console.log(this.customers);
    });
  }
  trackByProvider(index: number, provider: any): number {
    return provider.id;
  }
  addCustomer() {
      if (this.addCustomerForm.invalid) {
        this.addCustomerForm.markAllAsTouched();
        return;
      }
    
      const formData = this.addCustomerForm.value;
    
      if (this.isEditMode && this.currentCustomerId) {
        this.service.updateCustomer(this.currentCustomerId, formData).subscribe({
          next: () => {
            this.loadCustomers();
            this.addCustomerForm.reset();
            this.isEditMode = false;
            this.currentCustomerId = null;
          },
          error: (err) => {
            console.error('فشل التعديل:', err);
          }
        });
      } else {
        this.service.addCustomer(formData).subscribe({
          next: () => {
            this.loadCustomers();
            this.addCustomerForm.reset();
          },
          error: (err) => {
            console.error('فشل الإضافة:', err);
          }
        });
      }
    }
    customerData!:any;
    editCustomer(id: number) {
      this.isEditMode = true;
      this.currentCustomerId = id;
    
      this.service.getCustomerById(id).subscribe({
        next: (data:any) => {
          console.log(data);
          this.customerData=data.result;
          console.log(this.customerData);
          this.addCustomerForm.patchValue(this.customerData);
          const modal = new bootstrap.Modal(document.getElementById('addCustomerModal')!);
          modal.show();
        },
        error: (err) => {
          console.error('فشل تحميل بيانات العميل:', err);
        }
      });
    }
    deleteCustomer(id:number){
      Swal.fire({
        title: 'هل أنت متأكد؟',
        text: 'هل أنت متأكد من حذف هذا العميل؟',
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'نعم',
        cancelButtonText: 'إلغاء'
      }).then((result) => {
        if (result.isConfirmed) {
          this.service.deleteCustomer(id).subscribe({
            next: () => {
              this.loadCustomers();
              this.toastService.add({ severity: 'success', summary: 'تم حذف العميل بنجاح' });
            },
            error: (err) => {
              console.error('فشل الحذف:', err);
              this.toastService.add({ severity: 'error', summary: 'فشل الحذف' });
            }
          });
        }
      });
    }
    // Paginatoin helpers
    search(){
      this.loadCustomers();
    }
    get math(){
      return Math;
    }
    goToPage(page: number){
      this.currentPage = page;
      this.loadCustomers();
    }
    // 
    resetForm(){
      this.addCustomerForm.reset();
      this.isEditMode = false;
    }
}
