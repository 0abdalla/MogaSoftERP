import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Store } from '../../../../core/models/system-settings/stores';
import { SysSettingsService } from '../../../../core/services/sys-settings.service';
import { MessageService } from 'primeng/api';
import Swal from 'sweetalert2';
import { StoreType } from '../../../../core/models/system-settings/storeType';
declare var bootstrap: any;
@Component({
  selector: 'app-stores',
  templateUrl: './stores.component.html',
  styleUrl: './stores.component.css',
  providers:[MessageService]
})
export class StoresComponent {
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
  currentStoreId!:any;
  isFilter: boolean = false;
  // 
  stores:Store[] = [];
  storeTypes:StoreType[] = [];
  // pagination state
  searchTerm: string = '';
  currentPage: number = 1;
  pageSize: number = 2;
  totalCount: number = 0;
  // 
  storesForm!: FormGroup;
  // 
  constructor(private fb: FormBuilder , private service : SysSettingsService , private messageService : MessageService){
    this.storesForm = this.fb.group({
      code:['' , Validators.required],
      name:['' , Validators.required],
      address:[''],
      phoneNumber:[''],
      storeTypeId: ['', Validators.required]
    });
  }
  ngOnInit(): void {
    this.loadStores();
    this.loadStoreTypes();
  }
  trackByStore(index: number, store: any): number {
    return store.id;
  }
  search(){
    this.currentPage = 1;
    this.loadStores();
  }
  resetSearch(){
    this.searchTerm = '';
    this.loadStores();
  }
  // CRUD
  loadStores(){
    this.service.getAllStores(this.searchTerm).subscribe({
      next: (res:any) => {
        this.stores = res.result;
        console.log(this.stores);
      },
      error: (err) => {
        console.log(err);
      }
    })
  }
  loadStoreTypes(){
    this.service.getAllStoreTypes().subscribe({
      next: (res:any) => {
        this.storeTypes = res.result;
        console.log(this.storeTypes);
      },
      error: (err) => {
        console.log(err);
      }
    })
  }
  addStore() {
    if (this.storesForm.invalid) {
      this.storesForm.markAllAsTouched();
      return;
    }
  
    const formData = this.storesForm.value;
  
    if (this.isEditMode && this.currentStoreId !== null) {
      this.service.updateStore(this.currentStoreId, formData).subscribe({
        next: () => {
          this.loadStores();
          this.storesForm.reset();
          this.isEditMode = false;
          this.currentStoreId = null;
          this.messageService.add({
            severity: 'success',
            summary: 'تم التعديل بنجاح',
            detail: 'تم تعديل المخزن بنجاح',
          });
          setTimeout(() => {
            const modalElement = document.getElementById('addStoreModal');
            if (modalElement) {
              const modalInstance = bootstrap.Modal.getInstance(modalElement);
              modalInstance?.hide();
            }
            this.resetForm();
          }, 1000);
        },
        error: (err) => {
          console.error('فشل التعديل:', err);
        }
      });
    } else {
      this.service.addStore(formData).subscribe({
        next: (res:any) => {
          this.loadStores();
          this.storesForm.reset();
          if(res.isSuccess==true){
            this.messageService.add({
              severity: 'success',
              summary: 'تم الإضافة بنجاح',
              detail: 'تم إضافة المخزن بنجاح',
            });
          }else{
            this.messageService.add({
              severity: 'error',
              summary: 'فشل الإضافة',
              detail: 'فشل إضافة المخزن',
            });
          }
        },
        error: (err) => {
          console.error('فشل الإضافة:', err);
        }
      });
    }
  }
  store!:any;
  editStore(id: number) {
    this.isEditMode = true;
    this.currentStoreId = id;
  
    this.service.getStoreById(id).subscribe({
      next: (data:any) => {
        this.store=data.result;
        this.storesForm.patchValue({
          name: this.store.name,
          code: this.store.code,
          storeTypeId: this.store.storeTypeId,
        });
  
        const modal = new bootstrap.Modal(document.getElementById('addStoreModal')!);
        modal.show();
      },
      error: (err) => {
        console.error('فشل تحميل بيانات الخزنة:', err);
      }
    });
  }
  deleteStore(id: number) {
    Swal.fire({
      title: 'هل أنت متأكد؟',
      text: 'هل تريد حذف هذا المخزن؟',
      icon: 'warning',
      showCancelButton: true,
      confirmButtonColor: '#3085d6',
      cancelButtonColor: '#d33',
      confirmButtonText: 'نعم، حذف',
      cancelButtonText: 'إلغاء'
    }).then((result) => {
      if (result.isConfirmed) {
        this.service.deleteStore(id).subscribe({
          next: (res:any) => {
            this.loadStores();
            if(res.isSuccess==true){
              this.messageService.add({
                severity: 'success',
                summary: 'تم الحذف بنجاح',
                detail: 'تم حذف المخزن بنجاح',
              });
            }else{
              this.messageService.add({
                severity: 'error',
                summary: 'فشل الحذف',
                detail: 'فشل حذف المخزن',
              });
            }
          },
          error: (err) => {
            console.error('فشل الحذف:', err);
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
    this.loadStores();
  }
  // 
  resetForm(){
    this.storesForm.reset();
    this.isEditMode = false;
  }
}
