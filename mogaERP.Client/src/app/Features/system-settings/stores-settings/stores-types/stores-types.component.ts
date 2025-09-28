import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MessageService } from 'primeng/api';
import { StoreType } from '../../../../core/models/system-settings/storeType';
import { SysSettingsService } from '../../../../core/services/sys-settings.service';
import Swal from 'sweetalert2';
declare var bootstrap: any;
@Component({
  selector: 'app-stores-types',
  templateUrl: './stores-types.component.html',
  styleUrl: './stores-types.component.css',
    providers:[MessageService]
})
export class StoresTypesComponent {
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
  currentStoreTypeId!:any;
  isFilter: boolean = false;
  // 
  storeTypes:StoreType[] = [];
  // pagination state
  searchTerm: string = '';
  currentPage: number = 1;
  pageSize: number = 2;
  totalCount: number = 0;
  // 
  storeTypesForm!: FormGroup;
  // 
  constructor(private fb: FormBuilder , private service : SysSettingsService , private messageService : MessageService){
    this.storeTypesForm = this.fb.group({
      name: ['', Validators.required],
    });
  }
  ngOnInit(): void {
    this.loadStoreTypes();
  }
  trackByStoreType(index: number, storeType: any): number {
    return storeType.id;
  }
  search(){
    this.currentPage = 1;
    this.loadStoreTypes();
  }
  resetSearch(){
    this.searchTerm = '';
    this.loadStoreTypes();
  }
  // CRUD
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
  addStoreType() {
    if (this.storeTypesForm.invalid) {
      this.storeTypesForm.markAllAsTouched();
      return;
    }
  
    const formData = this.storeTypesForm.value;
  
    if (this.isEditMode && this.currentStoreTypeId !== null) {
      this.service.updateStoreType(this.currentStoreTypeId, formData).subscribe({
        next: () => {
          this.loadStoreTypes()
          this.storeTypesForm.reset();
          this.isEditMode = false;
          this.currentStoreTypeId = null;
          this.messageService.add({
            severity: 'success',
            summary: 'تم التعديل بنجاح',
            detail: 'تم تعديل نوع المخزن بنجاح',
          });
          setTimeout(() => {
            const modalElement = document.getElementById('addStoretypeModal');
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
      this.service.addStoreType(formData).subscribe({
        next: (res:any) => {
          this.loadStoreTypes()
          this.storeTypesForm.reset();
          if(res.isSuccess == true){
            this.messageService.add({
              severity: 'success',
              summary: 'تم الإضافة بنجاح',
              detail: 'تم إضافة نوع المخزن بنجاح',
            });
          }else{
            this.messageService.add({
              severity: 'error',
              summary: 'فشل الإضافة',
              detail: 'فشل إضافة نوع المخزن',
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
  editStoreType(id: number) {
    this.isEditMode = true;
    this.currentStoreTypeId = id;
  
    this.service.getStoreTypeById(id).subscribe({
      next: (data : any) => {
        this.store=data.result;
        this.storeTypesForm.patchValue({
          name: this.store.name,
          accountCode: this.store.accountCode,
          branchId: this.store.branchId,
          currency: this.store.currency,
          openingBalance: this.store.openingBalance
        });
  
        const modal = new bootstrap.Modal(document.getElementById('addStoretypeModal')!);
        modal.show();
      },
      error: (err) => {
        console.error('فشل تحميل بيانات النوع:', err);
      }
    });
  }
  deleteStoreType(id: number) {
    Swal.fire({
      title: 'هل أنت متأكد؟',
      text: 'هل تريد حذف هذا النوع؟',
      icon: 'warning',
      showCancelButton: true,
      confirmButtonColor: '#3085d6',
      cancelButtonColor: '#d33',
      confirmButtonText: 'نعم، حذف',
      cancelButtonText: 'إلغاء'
    }).then((result) => {
      if (result.isConfirmed) {
        this.service.deleteStoreType(id).subscribe({
          next: (res:any) => {
            this.loadStoreTypes();
            if(res.isSuccess == true){
              this.messageService.add({
                severity: 'success',
                summary: 'تم الحذف بنجاح',
                detail: 'تم حذف النوع بنجاح',
              });
            }else{
              this.messageService.add({
                severity: 'error',
                summary: 'فشل الحذف',
                detail: 'فشل حذف النوع',
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
    this.loadStoreTypes();
  }
  // 
  resetForm(){
    this.storeTypesForm.reset();
    this.isEditMode = false;
  }
}
