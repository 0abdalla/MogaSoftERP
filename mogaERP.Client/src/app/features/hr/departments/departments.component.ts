import { Component , OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { HrService } from '../../../core/services/hr.service';
import Swal from 'sweetalert2';
import { MessageService } from 'primeng/api';
declare var bootstrap : any
@Component({
  selector: 'app-departments',
  templateUrl: './departments.component.html',
  styleUrl: './departments.component.css',
  providers:[MessageService]
})
export class DepartmentsComponent implements OnInit {
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
  currentDepartmentId: number | null = null;
  isFilter: boolean = false;
  // 
  departments!: any;

  // pagination state
  // pageSize: number = 2;
  // totalCount: number = 100;
  // currentPage: number = 1;
  searchTerm: string = '';
  // 
  addDepartmentForm!: FormGroup;
  constructor(private fb : FormBuilder , private hrService : HrService , private messageService : MessageService){
    this.addDepartmentForm = this.fb.group({
      name: ['', Validators.required],
      status: ['Active', Validators.required],
      description: ['', Validators.required],
    });
  }
  ngOnInit(){
    this.loadDepartments();
  }
  // CRUD
  loadDepartments(){
    this.hrService.getDepartments(this.searchTerm).subscribe({
      next: (res:any) => {
        this.departments = res.result;
        console.log('departments',this.departments);
        console.log(res);
      },
      error: (err) => {
        console.error('Error fetching entries', err);
      }
    });
  }
  trackByDepartment(index : number , item : any){
    return item.id;
  }
  addDep() {
    if (this.addDepartmentForm.invalid) {
      this.addDepartmentForm.markAllAsTouched();
      return;
    }
  
    const formData = this.addDepartmentForm.value;
  
    if (this.isEditMode && this.currentDepartmentId !== null) {
      this.hrService.updateDepartment(this.currentDepartmentId, formData).subscribe({
        next: (data:any) => {
          console.log(data);
          this.loadDepartments();
          // this.addDepartmentForm.reset();
          this.addDepartmentForm.get('name')?.setValue('');
          this.addDepartmentForm.get('description')?.setValue('');
          this.isEditMode = false;
          this.currentDepartmentId = null;
          this.messageService.add({
            severity: 'success',
            summary: 'تم التعديل بنجاح',
            detail: 'تم تعديل القسم بنجاح',
          });
          setTimeout(() => {
            const modalElement = document.getElementById('addDepartmentModal');
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
      this.hrService.addDepartment(formData).subscribe({
        next: (res:any) => {
          console.log(res);
          this.loadDepartments();
          // this.addDepartmentForm.reset();
          this.addDepartmentForm.get('name')?.setValue('');
          this.addDepartmentForm.get('description')?.setValue('');
          if(res.isSuccess==true){
            this.messageService.add({
              severity: 'success',
              summary: 'تم الإضافة بنجاح',
              detail: 'تم إضافة القسم بنجاح',
            });
          }else{
            this.messageService.add({
              severity: 'error',
              summary: 'فشل الإضافة',
              detail: 'فشل إضافة القسم',
            });
          }
        },
        error: (err) => {
          console.error('فشل الإضافة:', err);
        }
      });
    }
  }
  dep!:any;
  editDepartment(id: number) {
    this.isEditMode = true;
    this.currentDepartmentId = id;
    this.hrService.getDepartmentById(id).subscribe({
      next: (data : any) => {
        this.dep = data.result;
        console.log('dep:', this.dep);
        // this.addDepartmentForm.reset();
        this.addDepartmentForm.get('name')?.setValue('');
        this.addDepartmentForm.get('description')?.setValue('');
        let statusValue = this.dep.status === 'نشط' ? 'Active' : 'Inactive';
        this.addDepartmentForm.patchValue({
          name: this.dep.name,
          status: statusValue,
          description: this.dep.description,
        });
        console.log('Form value after patch:', this.addDepartmentForm.value);
        console.log('typeof statusValue:', typeof statusValue);
        console.log('statusValue set to:', statusValue);
        const modal = new bootstrap.Modal(document.getElementById('addDepartmentModal')!);
        modal.show();
      },
      error: (err) => {
        console.error('فشل تحميل بيانات القسم:', err);
      }
    });
  }
  deleteDepartment(id: number) {
    Swal.fire({
      title: 'هل أنت متأكد؟',
      text: 'هل تريد حذف هذا القسم؟',
      icon: 'warning',
      showCancelButton: true,
      confirmButtonColor: '#3085d6',
      cancelButtonColor: '#d33',
      confirmButtonText: 'نعم، حذف',
      cancelButtonText: 'إلغاء'
    }).then((result) => {
      if (result.isConfirmed) {
        this.hrService.deleteDepartment(id).subscribe({
          next: (res:any) => {
            this.loadDepartments();
            if(res.isSuccess==true){
              this.messageService.add({
                severity: 'success',
                summary: 'تم الحذف بنجاح',
                detail: 'تم حذف القسم بنجاح',
              });
            }else{
              this.messageService.add({
                severity: 'error',
                summary: 'فشل الحذف',
                detail: 'فشل حذف القسم',
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
  // pagination handlers
  search(){
    this.loadDepartments();
  }
  // 
  getStatusName(type: string): string {
    const map: { [key: string]: string } = {
      Active: 'نشط',
      Inactive: 'غير نشط'
    };
    return map[type] || type;
  }
  getStatusDotClass(status: string): string {
    switch (status) {
      case 'Active': return 'bg-success';
      case 'Inactive': return 'bg-danger';
      default: return 'bg-secondary';
    }
  }  
  resetFormOnClose(){
    this.addDepartmentForm.reset();
    this.isEditMode = false;
    this.currentDepartmentId = null;
  }
}
