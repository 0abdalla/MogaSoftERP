import { Component, inject, signal } from '@angular/core';
import { MessageService } from 'primeng/api';
import { SysSettingsService } from '../../../../core/services/sys-settings.service';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ItemsUnits } from '../../../../core/models/system-settings/itemUnits';
import Swal from 'sweetalert2';
declare var bootstrap : any
@Component({
  selector: 'app-item-units',
  templateUrl: './item-units.component.html',
  styleUrl: './item-units.component.css',
  providers:[MessageService]
})
export class ItemUnitsComponent {
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
  currentItemId!:any;
  isFilter: boolean = false;
  // 
  units: any;
  loading = signal<boolean>(false);
  error = signal<string | null>(null);
  // pagination state
  pageSize: number = 2;
  totalCount: number = 100;
  currentPage: number = 1;
  searchTerm:string = '';
  // 
  unitForm!: FormGroup;
  constructor(private fb : FormBuilder , private service : SysSettingsService , private messageService : MessageService){
    this.unitForm=this.fb.group({
      name:['' , Validators.required],
    })
  }
  ngOnInit(): void {
    this.loadItemsUnits()
  }
  // CRUD
  loadItemsUnits() {
    this.loading.set(true);
    this.service.getAllItemsUnits(this.currentPage, this.pageSize, this.searchTerm , true).subscribe({
      next: (res:any) => {
        this.units = res.result;
        this.totalCount = res.result.totalCount;
        console.log(this.units);
        
        this.loading.set(false);
      },
      error: () => {
        this.messageService.add({ severity: 'error', summary: 'خطأ', detail: 'فشل في تحميل الوحدات' });
        this.loading.set(false);
      }
    });
  }
  addUnit() {
    if (this.unitForm.invalid) {
      this.unitForm.markAllAsTouched();
      return;
    }
  
    const formData = this.unitForm.value;
  
    if (this.isEditMode && this.currentItemId !== null) {
      this.service.updateItemUnit(this.currentItemId, formData).subscribe({
        next: () => {
          this.loadItemsUnits();
          this.unitForm.reset();
          this.isEditMode = false;
          this.currentItemId = null;
          this.messageService.add({
            severity: 'success',
            summary: 'تم التعديل بنجاح',
            detail: 'تم تعديل الوحدة بنجاح',
          });
          setTimeout(() => {
            const modalElement = document.getElementById('addUnitModal');
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
      this.service.addItemUnit(formData).subscribe({
        next: (res:any) => {
          console.log(res);
          console.log(this.unitForm.value);
          this.unitForm.reset();
          this.loadItemsUnits();
          if(res.isSuccess == true){
            this.messageService.add({
              severity: 'success',
              summary: 'تم الإضافة بنجاح',
              detail: 'تم إضافة الوحدة بنجاح',
            });
          }else{
            this.messageService.add({
              severity: 'error',
              summary: 'فشل الإضافة',
              detail: 'فشل إضافة الوحدة',
            });
          }
          this.unitForm.reset();
        },
        error: (err) => {
          console.error('فشل الإضافة:', err);
        }
      });
    }
  }
  Unit!:any;
  editUnit(id: number) {
    this.isEditMode = true;
    this.currentItemId = id;
  
    this.service.getItemUnitById(id).subscribe({
      next: (data:any) => {
        this.Unit=data.result;
        this.unitForm.patchValue({
          name: this.Unit.name,
        });
  
        const modal = new bootstrap.Modal(document.getElementById('addUnitModal')!);
        modal.show();
      },
      error: (err) => {
        console.error('فشل تحميل بيانات المجموعة:', err);
      }
    });
  }
  deleteUnit(id: number) {
    Swal.fire({
      title: 'هل أنت متأكد؟',
      text: 'هل تريد حذف هذه الوحدة؟',
      icon: 'warning',
      showCancelButton: true,
      confirmButtonColor: '#3085d6',
      cancelButtonColor: '#d33',
      confirmButtonText: 'نعم، حذف',
      cancelButtonText: 'إلغاء'
    }).then((result) => {
      if (result.isConfirmed) {
        this.service.deleteItemUnit(id).subscribe({
          next: () => {
            this.loadItemsUnits();
          },
          error: (err) => {
            console.error('فشل الحذف:', err);
          }
        });
      }
    });
  }
  // 
  resetForm(){
    this.unitForm.reset();
    this.isEditMode = false;
    this.currentItemId = null;
  }
  get math(){
    return Math;
  }
  search(){
    this.loadItemsUnits();
  }
  goToPage(page: number) {
    this.currentPage = page;
    this.loadItemsUnits();
  }
}
