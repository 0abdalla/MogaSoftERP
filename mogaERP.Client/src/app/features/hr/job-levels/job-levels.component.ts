import { Component } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { MessageService } from 'primeng/api';
import { HrService } from '../../../core/services/hr.service';
import Swal from 'sweetalert2';
declare var bootstrap : any

@Component({
  selector: 'app-job-levels',
  templateUrl: './job-levels.component.html',
  styleUrl: './job-levels.component.css',
  providers:[MessageService]
})
export class JobLevelsComponent {
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
  currentLevelId: number | null = null;
  isFilter: boolean = false;
  // 
  levels!: any;

  // pagination state
  // pageSize: number = 2;
  // totalCount: number = 100;
  // currentPage: number = 1;
  searchTerm: string = '';
  // 
  addLevelForm!: FormGroup;
  constructor(private fb : FormBuilder , private hrService : HrService , private messageService : MessageService){
    this.addLevelForm = this.fb.group({
      name: ['', Validators.required],
      status: ['Active', Validators.required],
      description: [''],
    });
  }
  ngOnInit(){
    this.loadJobLevels();
  }
  // CRUD
  loadJobLevels(){
    this.hrService.getLevels(this.searchTerm).subscribe({
      next: (res:any) => {
        this.levels = res.result;
        console.log('levels',this.levels);
        console.log(res);
      },
      error: (err) => {
        console.error('Error fetching entries', err);
      }
    });
  }
  trackByLevel(index : number , item : any){
    return item.id;
  }
  addJobLevel() {
    if (this.addLevelForm.invalid) {
      this.addLevelForm.markAllAsTouched();
      return;
    }
  
    const formData = this.addLevelForm.value;
  
    if (this.isEditMode && this.currentLevelId !== null) {
      this.hrService.updateLevel(this.currentLevelId, formData).subscribe({
        next: (data:any) => {
          console.log(data);
          this.loadJobLevels();
          this.addLevelForm.reset();
          this.isEditMode = false;
          this.currentLevelId = null;
          this.messageService.add({
            severity: 'success',
            summary: 'تم التعديل بنجاح',
            detail: 'تم تعديل المستوى الوظيفي بنجاح',
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
      this.hrService.addLevel(formData).subscribe({
        next: (res:any) => {
          console.log(res);
          this.loadJobLevels();
          this.addLevelForm.reset();
          if(res.isSuccess==true){
            this.messageService.add({
              severity: 'success',
              summary: 'تم الإضافة بنجاح',
              detail: 'تم إضافة المستوى الوظيفي بنجاح',
            });
          }else{
            this.messageService.add({
              severity: 'error',
              summary: 'فشل الإضافة',
              detail: 'فشل إضافة المستوى الوظيفي',
            });
          }
        },
        error: (err) => {
          console.error('فشل الإضافة:', err);
        }
      });
    }
  }
  level!:any;
  editJobLevel(id: number) {
    this.isEditMode = true;
    this.currentLevelId = id;
    this.hrService.getLevelById(id).subscribe({
      next: (data:any) => {
        // this.jobLevelForm.reset();
        this.addLevelForm.get('name')?.setValue('');
        this.addLevelForm.get('description')?.setValue('');
        this.level=data.result;
        let statusValue = this.level.status === 'نشط' ? 'Active' : 'Inactive';
        this.addLevelForm.patchValue({
          name: this.level.name,
          status: statusValue,
          description: this.level.description,
        });
        console.log('Form value after patch:', this.addLevelForm.value);
        console.log('typeof statusValue:', typeof statusValue);
        console.log('statusValue set to:', statusValue);
        const modal = new bootstrap.Modal(document.getElementById('addjobLevelModal')!);
        modal.show();
      },
      error: (err) => {
        console.error('فشل تحميل بيانات المستوى الوظيفي:', err);
      }
    });
  }
  deleteJobLevel(id: number) {
    Swal.fire({
      title: 'هل أنت متأكد؟',
      text: 'هل تريد حذف هذا المستوى الوظيفي؟',
      icon: 'warning',
      showCancelButton: true,
      confirmButtonColor: '#3085d6',
      cancelButtonColor: '#d33',
      confirmButtonText: 'نعم، حذف',
      cancelButtonText: 'إلغاء'
    }).then((result) => {
      if (result.isConfirmed) {
        this.hrService.deleteLevel(id).subscribe({
          next: (res:any) => {
            this.loadJobLevels();
            if(res.isSuccess==true){
              this.messageService.add({
                severity: 'success',
                summary: 'تم الحذف بنجاح',
                detail: 'تم حذف المستوى الوظيفي بنجاح',
              });
            }else{
              this.messageService.add({
                severity: 'error',
                summary: 'فشل الحذف',
                detail: 'فشل حذف المستوى الوظيفي',
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
    this.loadJobLevels();
  }
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
    this.addLevelForm.reset();
    this.isEditMode = false;
    this.currentLevelId = null;
  }
}
