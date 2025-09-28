import { Component } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { MessageService } from 'primeng/api';
import { HrService } from '../../../core/services/hr.service';
import Swal from 'sweetalert2';
declare var bootstrap : any
@Component({
  selector: 'app-job-categories',
  templateUrl: './job-categories.component.html',
  styleUrl: './job-categories.component.css',
  providers:[MessageService]
})
export class JobCategoriesComponent {
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
  currentCategoryId: number | null = null;
  isFilter: boolean = false;
  // 
  categories!: any;

  // pagination state
  // pageSize: number = 2;
  // totalCount: number = 100;
  // currentPage: number = 1;
  searchTerm: string = '';
  // 
  addCategoryForm!: FormGroup;
  constructor(private fb : FormBuilder , private hrService : HrService , private messageService : MessageService){
    this.addCategoryForm = this.fb.group({
      name: ['', Validators.required],
      status: ['Active', Validators.required],
      description: [''],
    });
  }
  ngOnInit(){
    this.loadJobCategories();
  }
  // 
  loadJobCategories(){
    this.hrService.getTypes(this.searchTerm).subscribe({
      next: (res:any) => {
        this.categories = res.result;
        console.log('Categories',this.categories);
        console.log(res);
      },
      error: (err) => {
        console.error('Error fetching entries', err);
      }
    });
  }
  trackByCategory(index : number , item : any){
    return item.id;
  }
  addJobCategory() {
      if (this.addCategoryForm.invalid) {
        this.addCategoryForm.markAllAsTouched();
        return;
      }
    
      const formData = this.addCategoryForm.value;
    
      if (this.isEditMode && this.currentCategoryId !== null) {
        this.hrService.updateType(this.currentCategoryId, formData).subscribe({
          next: (data:any) => {
            console.log(data);
            this.loadJobCategories();
            this.addCategoryForm.reset();
            this.isEditMode = false;
            this.currentCategoryId = null;
            this.messageService.add({
              severity: 'success',
              summary: 'تم التعديل بنجاح',
              detail: 'تم تعديل النوع الوظيفي بنجاح',
            });
            setTimeout(() => {
              const modalElement = document.getElementById('addjobLevelModal');
              if (modalElement) {
                const modalInstance = bootstrap.Modal.getInstance(modalElement);
                modalInstance?.hide();
              }
              this.resetFormOnClose();
            }, 1000);
          },
          error: (err) => {
            console.error('فشل التعديل:', err);
          }
        });
      } else {
        this.hrService.addType(formData).subscribe({
          next: (res:any) => {
            console.log(res);
            this.loadJobCategories();
            this.addCategoryForm.reset();
            if(res.isSuccess==true){
              this.messageService.add({
                severity: 'success',
                summary: 'تم الإضافة بنجاح',
                detail: 'تم إضافة النوع الوظيفي بنجاح',
              });
            }else{
              this.messageService.add({
                severity: 'error',
                summary: 'فشل الإضافة',
                detail: 'فشل إضافة النوع الوظيفي',
              });
            }
          },
          error: (err) => {
            console.error('فشل الإضافة:', err);
          }
        });
      }
  }
  category!:any;
  editJobCategory(id: number) {
    this.isEditMode = true;
    this.currentCategoryId = id;
    this.hrService.getTypeById(id).subscribe({
      next: (data:any) => {
        // this.jobLevelForm.reset();
        this.addCategoryForm.get('name')?.setValue('');
        this.addCategoryForm.get('description')?.setValue('');
        this.category=data.result;
        let statusValue = this.category.status === 'نشط' ? 'Active' : 'Inactive';
        this.addCategoryForm.patchValue({
          name: this.category.name,
          status: statusValue,
          description: this.category.description,
        });
        console.log('Form value after patch:', this.addCategoryForm.value);
        console.log('typeof statusValue:', typeof statusValue);
        console.log('statusValue set to:', statusValue);
        const modal = new bootstrap.Modal(document.getElementById('addjobCategoryModal')!);
        modal.show();
      },
      error: (err) => {
        console.error('فشل تحميل بيانات النوع الوظيفي:', err);
      }
    });
  }
  deleteJobCategory(id: number) {
      Swal.fire({
        title: 'هل أنت متأكد؟',
        text: 'هل تريد حذف هذا النوع الوظيفي؟',
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'نعم، حذف',
        cancelButtonText: 'إلغاء'
      }).then((result) => {
        if (result.isConfirmed) {
          this.hrService.deleteType(id).subscribe({
            next: (res:any) => {
              this.loadJobCategories();
              if(res.isSuccess==true){
                this.messageService.add({
                  severity: 'success',
                  summary: 'تم الحذف بنجاح',
                  detail: 'تم حذف النوع الوظيفي بنجاح',
                });
              }else{
                this.messageService.add({
                  severity: 'error',
                  summary: 'فشل الحذف',
                  detail: 'فشل حذف النوع الوظيفي',
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
  // 
  search(){
    this.loadJobCategories();
  }
  getStatusDotClass(status: string): string {
    return status === 'نشط' ? 'bg-success' : 'bg-danger';
  }
  getStatusName(status: string): string {
    return status === 'نشط' ? 'نشط' : 'غير نشط';
  }
  resetFormOnClose(){
    this.addCategoryForm.reset();
    this.isEditMode = false;
    this.currentCategoryId = null;
  }
}
