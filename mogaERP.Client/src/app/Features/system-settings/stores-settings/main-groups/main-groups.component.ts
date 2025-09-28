import { Component, inject, OnInit, signal } from '@angular/core';
import { MessageService } from 'primeng/api';
import { SysSettingsService } from '../../../../core/services/sys-settings.service';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MainGroups } from '../../../../core/models/system-settings/maingroup';
import Swal from 'sweetalert2';
declare var bootstrap: any;

@Component({
  selector: 'app-main-groups',
  templateUrl: './main-groups.component.html',
  styleUrl: './main-groups.component.css',
  providers: [MessageService]
})
export class MainGroupsComponent implements OnInit {
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
  currentMainGroupId!:any;
  isFilter: boolean = false;
  // 
  mainGroups: MainGroups[] = [];
  loading = this.service.loading;
  error = this.service.error;
  // 
  mainGroupsForm!: FormGroup;
  // 
  searchTerm: string = '';

  constructor(private service : SysSettingsService , private fb : FormBuilder , private messageService : MessageService){
    this.mainGroupsForm=this.fb.group({
      name:['' , Validators.required],
      description:[],
    })
  }
  ngOnInit(): void {
    this.loadMainGroups()
  }
  trackByMainGroup(index: number, mainGroup: any): number {
    return mainGroup.id;
  }
  search(){
    this.loadMainGroups();
  }
  resetSearch(){
    this.searchTerm = '';
    this.loadMainGroups();
  }
  // CRUD
  loadMainGroups() {
    this.loading.set(true);
    this.service.getAllMainGroups(this.searchTerm).subscribe({
      next: (res:any) => {
        this.mainGroups = res.result;
        console.log(this.mainGroups);
        this.loading.set(false);
      },
      error: () => {
        this.messageService.add({ severity: 'error', summary: 'خطأ', detail: 'فشل في تحميل المجموعات الرئيسية' });
      }
    });
  }
  addMainGroup() {
    if (this.mainGroupsForm.invalid) {
      this.mainGroupsForm.markAllAsTouched();
      return;
    }
  
    const formData = this.mainGroupsForm.value;
  
    if (this.isEditMode && this.currentMainGroupId !== null) {
      this.service.updateMainGroup(this.currentMainGroupId, formData).subscribe({
        next: () => {
          this.loadMainGroups();
          this.messageService.add({
            severity: 'success',
            summary: 'تم التعديل بنجاح',
            detail: 'تم تعديل المجموعة بنجاح',
          });
          setTimeout(() => {
            const modalElement = document.getElementById('addMainGroupModal');
            if (modalElement) {
              const modalInstance = bootstrap.Modal.getInstance(modalElement);
              modalInstance?.hide();
            }
            this.resetForm();
          }, 2000);
        },
        error: (err) => {
          console.error('فشل التعديل:', err);
          this.messageService.add({
            severity: 'error',
            summary: 'فشل التعديل',
            detail: 'فشل تعديل المجموعة',
          });
        }
      });
    } else {
      this.service.addMainGroup(formData).subscribe({
        next: (res : any) => {
          console.log(res);
          console.log(this.mainGroupsForm.value);
          if (res.isSuccess == true) {
            this.messageService.add({
              severity: 'success',
              summary: 'تم الإضافة بنجاح',
              detail: 'تم إضافة المجموعة بنجاح',
            });
            
          } else {
            this.messageService.add({
              severity: 'error',
              summary: 'فشل الإضافة',
              detail: 'فشل إضافة المجموعة',
            });
          }
          this.loadMainGroups();
          this.resetForm();
        },
        error: (err) => {
          console.error('فشل الإضافة:', err);
          this.messageService.add({
            severity: 'error',
            summary: 'فشل الإضافة',
            detail: 'فشل إضافة المجموعة',
          });
        }
      });
    }
  }
  mainGroup!:any;
  editMainGroup(id: number) {
    this.isEditMode = true;
    this.currentMainGroupId = id;
    this.service.getMainGroupById(id).subscribe({
      next: (data: any) => {
        this.mainGroup=data.result;
        this.mainGroupsForm.patchValue({
          name: this.mainGroup.name,
          description: this.mainGroup.description,
        });
        const modal = new bootstrap.Modal(document.getElementById('addMainGroupModal')!);
        modal.show();
      },
      error: (err) => {
        console.error('فشل تحميل بيانات المجموعة:', err);
      }
    });
  }
  
  deleteMainGroup(id: number) {
    Swal.fire({
      title: 'هل أنت متأكد؟',
      text: 'هل تريد حذف هذا المجموعة؟',
      icon: 'warning',
      showCancelButton: true,
      confirmButtonColor: '#3085d6',
      cancelButtonColor: '#d33',
      confirmButtonText: 'نعم، حذف',
      cancelButtonText: 'إلغاء'
    }).then((result) => {
      if (result.isConfirmed) {
        this.service.deleteMainGroup(id).subscribe({
          next: () => {
            this.loadMainGroups();
            this.messageService.add({
              severity: 'success',
              summary: 'تم الحذف بنجاح',
              detail: 'تم حذف المجموعة بنجاح',
            });
          },
          error: (err) => {
            console.error('فشل الحذف:', err);
            this.messageService.add({
              severity: 'error',
              summary: 'فشل الحذف',
              detail: 'فشل حذف المجموعة',
            });
          }
        });
      }
    });
  }
  // 
  resetForm(){
    this.mainGroupsForm.reset();
    this.isEditMode = false;
    this.currentMainGroupId = null;
  }
}
