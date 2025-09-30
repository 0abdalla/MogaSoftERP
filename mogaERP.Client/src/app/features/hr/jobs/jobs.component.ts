import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MessageService } from 'primeng/api';
import { HrService } from '../../../core/services/hr.service';
import { JobTitle } from '../../../core/models/hr/jobTitle';
import { Department } from '../../../core/models/system-settings/departments';
import Swal from 'sweetalert2';
declare var bootstrap : any
@Component({
  selector: 'app-jobs',
  templateUrl: './jobs.component.html',
  styleUrl: './jobs.component.css',
  providers:[MessageService]
})
export class JobsComponent {
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
  currentJobId: number | null = null;
  isFilter: boolean = false;
  // 
  jobs!: JobTitle[];
  jobDepartments!: Department[];
  // pagination state
  // pageSize: number = 2;
  // totalCount: number = 100;
  // currentPage: number = 1;
  searchTerm: string = '';
  // 
  addJobForm!: FormGroup;
  constructor(private fb : FormBuilder , private hrService : HrService , private messageService : MessageService){
    this.addJobForm = this.fb.group({
      name: ['', Validators.required],
      status: ['Active', Validators.required],
      jobDepartmentId: ['', Validators.required],
      description: [''],
    });
  }
  ngOnInit(){
    this.loadJobs();
    this.loadJobDepartments();
  }
  // CRUD
  loadJobs(){
    this.hrService.getTitles(this.searchTerm).subscribe({
      next: (res:any) => {
        this.jobs = res.result;
        console.log('jobs',this.jobs);
        console.log(res);
      },
      error: (err) => {
        console.error('Error fetching entries', err);
      }
    });
  }
  loadJobDepartments(){
    this.hrService.getDepartments().subscribe({
      next: (res:any) => {
        this.jobDepartments = res.result;
        console.log('jobDepartments',this.jobDepartments);
        console.log(res);
      },
      error: (err) => {
        console.error('Error fetching entries', err);
      }
    });
  }
  trackByJob(index : number , item : any){
    return item.id;
  }
  addJobTitle() {
    if (this.addJobForm.invalid) {
      this.addJobForm.markAllAsTouched();
      return;
    }
  
    const formData = this.addJobForm.value;
  
    if (this.isEditMode && this.currentJobId !== null) {
      this.hrService.updateTitle(this.currentJobId, formData).subscribe({
        next: (data:any) => {
          console.log(data);
          this.loadJobs();
          this.addJobForm.reset();
          this.isEditMode = false;
          this.currentJobId = null;
          this.messageService.add({
            severity: 'success',
            summary: 'تم التعديل بنجاح',
            detail: 'تم تعديل الوظيفة بنجاح',
          });
          setTimeout(() => {
            const modalElement = document.getElementById('addjobTitleModal');
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
      this.hrService.addTitle(formData).subscribe({
        next: (res:any) => {
          console.log(res);
          this.loadJobs();
          this.addJobForm.reset();
          if(res.isSuccess==true){
            this.messageService.add({
              severity: 'success',
              summary: 'تم الإضافة بنجاح',
              detail: 'تم إضافة الوظيفة بنجاح',
            });
          }else{
            this.messageService.add({
              severity: 'error',
              summary: 'فشل الإضافة',
              detail: 'فشل إضافة الوظيفة',
            });
          }
        },
        error: (err) => {
          console.error('فشل الإضافة:', err);
        }
      });
    }
  }
  title!:any;
  editJobTitle(id: number) {
    this.isEditMode = true;
    this.currentJobId = id;
  
    this.hrService.getTitleById(id).subscribe({
      next: (data : any) => {
        this.title = data.result;
        console.log('title:', this.title);
        // this.addJobForm.reset();
        this.addJobForm.get('name')?.setValue('');
        this.addJobForm.get('jobDepartmentId')?.setValue('');
        this.addJobForm.get('description')?.setValue('');
        let statusValue = this.title.status === 'نشط' ? 'Active' : 'Inactive';
        this.addJobForm.patchValue({
          name: this.title.name,
          jobDepartmentId: this.title.jobDepartmentId,
          status: statusValue,
          description: this.title.description,
        });
        console.log('Form value after patch:', this.addJobForm.value);
        console.log('typeof statusValue:', typeof statusValue);
        console.log('statusValue set to:', statusValue);
        const modal = new bootstrap.Modal(document.getElementById('addjobTitleModal')!);
        modal.show();
      },
      error: (err) => {
        console.error('فشل تحميل بيانات القسم:', err);
      }
    });
  }
  deleteJobTitle(id: number) {
    Swal.fire({
      title: 'هل أنت متأكد؟',
      text: 'هل تريد حذف هذه الوظيفة',
      icon: 'warning',
      showCancelButton: true,
      confirmButtonColor: '#3085d6',
      cancelButtonColor: '#d33',
      confirmButtonText: 'نعم، حذف',
      cancelButtonText: 'إلغاء'
    }).then((result) => {
      if (result.isConfirmed) {
        this.hrService.deleteTitle(id).subscribe({
          next: (res:any) => {
            this.loadJobs();
            if(res.isSuccess==true){
              this.messageService.add({
                severity: 'success',
                summary: 'تم الحذف بنجاح',
                detail: 'تم حذف الوظيفة بنجاح',
              });
            }else{
              this.messageService.add({
                severity: 'error',
                summary: 'فشل الحذف',
                detail: 'فشل حذف الوظيفة',
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
    this.loadJobs();
  }
  resetFormOnClose() {
    this.addJobForm.reset();
    this.isEditMode = false;
    this.currentJobId = null;
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
}
