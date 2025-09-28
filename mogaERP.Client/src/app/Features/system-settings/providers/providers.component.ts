import { Component, OnInit } from '@angular/core';
import { signal } from '@angular/core';
import { Provider } from '../../../core/models/system-settings/providers';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { SysSettingsService } from '../../../core/services/sys-settings.service';
import Swal from 'sweetalert2';
import { MessageService } from 'primeng/api';
declare var bootstrap: any;
@Component({
  selector: 'app-providers',
  templateUrl: './providers.component.html',
  styleUrl: './providers.component.css',
  providers:[MessageService]
})
export class ProvidersComponent implements OnInit {
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
  currentProviderId!:any;
  isFilter: boolean = false;
  // 
  providers:Provider[] = [];
  // pagination state
  searchTerm: string = '';
  currentPage: number = 1;
  pageSize: number = 2;
  totalCount: number = 0;
  // 
  providersForm: FormGroup;
  // 
  constructor(private fb: FormBuilder , private service : SysSettingsService , private toastService : MessageService){
    this.providersForm = this.fb.group({
      accountCode: [null, Validators.required],
      name: [null, Validators.required],
      address: [null, Validators.required],
      responsibleName1: [null, Validators.required],
      responsibleName2: [null, Validators.required],
      phone1: [null, Validators.required],
      phone2: [null],
      taxNumber: [null],
      job: [null],
      fax1: [null],
      fax2: [null],
      email: [null],
      website: [null],
      notes: [null],
      paymentType: ['Cash'],
      creditLimit: [null]
    });
  }
  ngOnInit(): void {
    this.loadProviders();
  }
  trackByProvider(index: number, provider: any): number {
    return provider.id;
  }
  search(){
    this.currentPage = 1;
    this.loadProviders();
  }
  resetSearch(){
    this.searchTerm = '';
    this.loadProviders();
  }
  // CRUD
  loadProviders(){
    this.service.getAllProviders(this.currentPage, this.pageSize, this.searchTerm, true).subscribe((res) => {
      this.providers = res.result.data;
      this.totalCount = res.result.totalCount;
      console.log(this.providers);
    });
  }
  addProvider() {
    if (this.providersForm.invalid) {
      this.providersForm.markAllAsTouched();
      return;
    }
  
    const formData = this.providersForm.value;
  
    if (this.isEditMode && this.currentProviderId) {
      this.service.updateProvider(this.currentProviderId, formData).subscribe({
        next: () => {
          this.loadProviders();
          this.providersForm.reset();
          this.isEditMode = false;
          this.currentProviderId = null;
        },
        error: (err) => {
          console.error('فشل التعديل:', err);
        }
      });
    } else {
      this.service.addProvider(formData).subscribe({
        next: () => {
          this.loadProviders();
          this.providersForm.reset();
        },
        error: (err) => {
          console.error('فشل الإضافة:', err);
        }
      });
    }
  }
  providerData!:any;
  editProvider(id: number) {
    this.isEditMode = true;
    this.currentProviderId = id;
  
    this.service.getProviderById(id).subscribe({
      next: (data:any) => {
        console.log(data);
        this.providerData=data.result;
        console.log(this.providerData);
        this.providersForm.patchValue(this.providerData);
        const modal = new bootstrap.Modal(document.getElementById('addProviderModal')!);
        modal.show();
      },
      error: (err) => {
        console.error('فشل تحميل بيانات المورد:', err);
      }
    });
  }
  deleteProvider(id:number){
    Swal.fire({
      title: 'هل أنت متأكد؟',
      text: 'هل أنت متأكد من حذف هذا المورد؟',
      icon: 'warning',
      showCancelButton: true,
      confirmButtonColor: '#3085d6',
      cancelButtonColor: '#d33',
      confirmButtonText: 'نعم',
      cancelButtonText: 'إلغاء'
    }).then((result) => {
      if (result.isConfirmed) {
        this.service.deleteProvider(id).subscribe({
          next: () => {
            this.loadProviders();
            this.toastService.add({ severity: 'success', summary: 'تم حذف المورد بنجاح' });
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
  get math(){
    return Math;
  }
  goToPage(page: number){
    this.currentPage = page;
    this.loadProviders();
  }
  // 
  resetForm(){
    this.providersForm.reset();
    this.isEditMode = false;
  }
}
