import { Component, inject } from '@angular/core';
import { MessageService } from 'primeng/api';
import { SysSettingsService } from '../../../../core/services/sys-settings.service';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MainGroups } from '../../../../core/models/system-settings/maingroup';
import { ItemsGroups } from '../../../../core/models/system-settings/itemGroup';
import Swal from 'sweetalert2';
declare var bootstrap: any;
@Component({
  selector: 'app-items-groups',
  templateUrl: './items-groups.component.html',
  styleUrl: './items-groups.component.css',
  providers:[MessageService]
})
export class ItemsGroupsComponent {
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
  currentitemGroupFormId!:any;
  isFilter: boolean = false;
  // 
  mainGroups: MainGroups[] = [];
  itemsGroups: ItemsGroups[] = [];
  // 
  loading = this.service.loading;
  error = this.service.error;
  // 
  itemGroupForm:FormGroup;
  searchTerm: string = '';
  constructor(private service : SysSettingsService , private messageService : MessageService , private fb : FormBuilder){
    this.itemGroupForm=this.fb.group({
      mainGroupId:['' , Validators.required],
      name:[''  , Validators.required],
      description:[],
    })
  }
  ngOnInit(): void {
    this.loadItemsGroups();
    this.loadMainGroups();
  }
  trackByItemsGroup(index: number, itemsGroup: any): number {
    return itemsGroup.id;
  }
  search(){
    this.loadItemsGroups();
  }
  resetSearch(){
    this.searchTerm = '';
    this.loadItemsGroups();
  }
  // CRUD
  loadMainGroups(){
    this.loading.set(true);
    this.service.getAllMainGroups('').subscribe({
      next: (res) => {
          this.mainGroups = res.result;
          console.log(this.mainGroups);
          this.loading.set(false);
        },
        error: () => {
          this.messageService.add({ severity: 'error', summary: 'خطأ', detail: 'فشل في تحميل المجموعات' });
        }
      });
  }
  loadItemsGroups(searchTerm: string = '') {
    this.loading.set(true);
    this.service.getAllItemsGroups(searchTerm).subscribe({
      next: (res) => {
          this.itemsGroups = res.result;
          console.log(this.itemsGroups);
          this.loading.set(false);
        },
        error: () => {
          this.messageService.add({ severity: 'error', summary: 'خطأ', detail: 'فشل في تحميل المجموعات' });
        }
      });
  }
  
  addItemGroupForm() {
    if (this.itemGroupForm.invalid) {
      this.itemGroupForm.markAllAsTouched();
      return;
    }
  
    const formData = this.itemGroupForm.value;
  
    if (this.isEditMode && this.currentitemGroupFormId !== null) {
      this.service.updateItemGroup(this.currentitemGroupFormId, formData).subscribe({
        next: () => {
          this.loadItemsGroups();
          this.itemGroupForm.reset();
          this.isEditMode = false;
          this.currentitemGroupFormId = null;
          this.messageService.add({
            severity: 'success',
            summary: 'تم التعديل بنجاح',
            detail: 'تم تعديل المجموعة بنجاح',
          });
          setTimeout(() => {
            const modalElement = document.getElementById('addItemGropModal');
            if (modalElement) {
              const modalInstance = bootstrap.Modal.getInstance(modalElement);
              modalInstance?.hide();
            }
            this.resetForm();
          }, 1000);
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
      this.service.addItemGroup(formData).subscribe({
        next: (res:any) => {
          console.log(res);
          console.log(this.itemGroupForm.value);
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
          this.loadItemsGroups();
          this.itemGroupForm.reset();
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
  itemGroupFormRow!:any;
  editItemGroupForm(id: number) {
    this.isEditMode = true;
    this.currentitemGroupFormId = id;
    
    this.service.getItemGroupById(id).subscribe({
        next: (data:any) => {
          this.itemGroupFormRow=data.result;
          this.itemGroupForm.patchValue({
            name: this.itemGroupFormRow.name,
            mainGroupId: this.itemGroupFormRow.mainGroupId,
            description: this.itemGroupFormRow.description,
          });
    
          const modal = new bootstrap.Modal(document.getElementById('addItemGropModal')!);
          modal.show();
        },
        error: (err) => {
          console.error('فشل تحميل بيانات المجموعة:', err);
        }
      });
  }
  deleteItemGroupForm(id: number) {
      Swal.fire({
        title: 'هل أنت متأكد؟',
        text: 'هل تريد حذف هذه المجموعة؟',
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'نعم، حذف',
        cancelButtonText: 'إلغاء'
      }).then((result) => {
        if (result.isConfirmed) {
          this.service.deleteItemGroup(id).subscribe({
            next: () => {
              this.loadItemsGroups();
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
    this.itemGroupForm.reset();
    this.isEditMode = false;
    this.currentitemGroupFormId = null;
  }
}
