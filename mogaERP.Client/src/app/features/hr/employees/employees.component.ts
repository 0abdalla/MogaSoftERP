import { Component, ElementRef, ViewChild } from '@angular/core';
import { Employee } from '../../../core/models/hr/employee';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { MessageService } from 'primeng/api';
import { HrService } from '../../../core/services/hr.service';

@Component({
  selector: 'app-employees',
  templateUrl: './employees.component.html',
  styleUrl: './employees.component.css',
  providers: [MessageService]
})
export class EmployeesComponent {
  @ViewChild('employeeDetailsPDF', { static: false }) employeeDetailsPDF!: ElementRef;
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
  employees : Employee[] = [];
  selectedEmployee!: any;
  // pagination state
  pageSize: number = 16;
  totalCount: number = 100;
  currentPage: number = 1;
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
    this.loadEmployees();
  }
  // CRUD
  loadEmployees(){
    this.hrService.getEmployees(this.currentPage,this.pageSize,this.searchTerm).subscribe({
      next: (res:any) => {
        this.employees = res.result.data;
        this.totalCount = res.result.totalCount;
        console.log('employees',this.employees);
        console.log(res);
      },
      error: (err) => {
        console.error('Error fetching entries', err);
      }
    });
  }
  trackByEmployee(index : number , item : any){
    return item.id;
  }
  openStaffModal(id: number) {
    this.getEmployeeById(id);
  }
  getEmployeeById(id: number) {
    this.hrService.getEmployeeById(id).subscribe({
      next: (res:any) => {
        this.selectedEmployee = res.result;
        console.log('selectedEmployee',this.selectedEmployee);
        console.log(res);
      },
      error: (err) => {
        console.error('Error fetching entries', err);
      }
    });
  }
  addEmployee(){
    
  }
  deleteEmployee(id: number){
    
  }
  // 
  get math(){
    return Math;
  }
  goToPage(page: number) {
    this.currentPage = page;
    this.loadEmployees();
  }
  search(){
    this.loadEmployees();
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
  getArabicGender(gender: string) {
    if (gender === 'Male') {
      return 'ذكر';
    } else if (gender === 'Female') {
      return 'أنثى';
    }
    return '';
  }
  getArabicMaritalStatus(maritalStatus: string) {
    if (maritalStatus === 'Single') {
      return 'أعزب';
    } else if (maritalStatus === 'Married') {
      return 'متزوج';
    } else if (maritalStatus === 'Divorced') {
      return 'مطلق';
    } else if (maritalStatus === 'Widowed') {
      return 'أرمل';
    }
    return '';
  }
  getArabicStatus(status: string) {
    if (status === 'Active') {
      return 'نشط';
    } else if (status === 'Inactive') {
      return 'غير نشط';
    }
    return '';
  }
  // 
  editEmployee(){
    
  }
  exportToPDF(){

  }
  suspendEmployee(){

  }
}
