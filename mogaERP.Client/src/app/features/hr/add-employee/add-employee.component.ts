import { Component } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import { MessageService } from 'primeng/api';
import { forkJoin } from 'rxjs';
import { HrService } from '../../../core/services/hr.service';

@Component({
  selector: 'app-add-employee',
  templateUrl: './add-employee.component.html',
  styleUrl: './add-employee.component.css',
  providers:[MessageService]
})
export class AddEmployeeComponent {
  employeeForm: FormGroup;
  selectedFiles: any[] = [];
  // 
  jobDepartments: any;
  jobTitles: any;
  jobTypes: any;
  jobLevels: any;
  branches: any;
  filteredJobTitles: any;
  netSalary!: number;
  // 
  employeeId!: number;
  constructor(
    private fb: FormBuilder,
    private hrService: HrService,
    private messageService: MessageService,
    private router: Router,
    private route: ActivatedRoute,
  ) {
    this.route.paramMap.subscribe(params => {
      const id = params.get('id');
      if (id) {
        this.employeeId = +id;
        this.loadEmployeeData(this.employeeId);
      }
    });
    this.employeeForm = this.fb.group({
      fullName: ['', Validators.required],
      nationalId: ['', [Validators.required, Validators.pattern(/^[0-9]{14}$/)]],
      gender: ['', Validators.required],
      maritalStatus: ['', Validators.required],
      birthDate: ['', Validators.required],
      // 
      phoneNumber: ['', [Validators.required, Validators.pattern(/^01[0125][0-9]{8}$/)]],
      email: ['', [Validators.required, Validators.email]],
      address: ['', Validators.required],
      // 
      branchId: [''],
      basicSalary: [null, Validators.required],
      allowances: [null , [Validators.required , Validators.min(0)]],
      tax: [null, [Validators.required, Validators.min(0), Validators.max(100)]],
      insurance: [null, [Validators.required, Validators.min(0), Validators.max(100)]],
      vacationDays: [null, [Validators.required, Validators.min(0), Validators.max(30)]],
      // 
      status: ['Active', Validators.required],
      notes: [''],
      Files: [null],
      // 
      JobDepartmentId: ['', Validators.required],
      JobLevelId: ['', Validators.required],
      JobTitleId: [{ value: '', disabled: true }, Validators.required],
      JobTypeId: ['', Validators.required],
      hireDate: ['', Validators.required],
      // 
      isAuthorized: [null, Validators.required],
      userName: [''],
      password: ['' , Validators.minLength(8)],
    });
    this.employeeForm.get('basicSalary')!.valueChanges.subscribe(() => this.calculateNetSalary());
    this.employeeForm.get('tax')!.valueChanges.subscribe(() => this.calculateNetSalary());
    this.employeeForm.get('insurance')!.valueChanges.subscribe(() => this.calculateNetSalary());
    this.employeeForm.get('allowances')!.valueChanges.subscribe(() => this.calculateNetSalary());
  }
  calculateNetSalary() {
    const salary = Number(this.employeeForm.get('basicSalary')?.value);
    const taxPercent = Number(this.employeeForm.get('tax')?.value);
    const insurancePercent = Number(this.employeeForm.get('insurance')?.value);
    const allowances = Number(this.employeeForm.get('allowances')?.value);
    if (!salary || isNaN(taxPercent) || isNaN(insurancePercent)) {
      this.netSalary = 0;
      return;
    }
    const taxAmount = salary * (taxPercent / 100);
    const insuranceAmount = salary * (insurancePercent / 100);
    this.netSalary = salary - taxAmount - insuranceAmount + allowances;
    const shouldDisable = this.netSalary <= 0;
    const isDisabled = this.employeeForm.disabled;
    if (shouldDisable) {
      this.employeeForm.get('basicSalary')?.setErrors({ 'invalid': true });
    } else {
      this.employeeForm.get('basicSalary')?.setErrors(null);
    }
  }

  ngOnInit() {
    this.employeeForm.get('isAuthorized')?.valueChanges.subscribe(value => {
      const userNameControl = this.employeeForm.get('userName');
      const passwordControl = this.employeeForm.get('password');
      if (value === true) {
        userNameControl?.setValidators([Validators.required]);
        passwordControl?.setValidators([Validators.required]);
      } else {
        userNameControl?.clearValidators();
        passwordControl?.clearValidators();
        userNameControl?.reset();
        passwordControl?.reset();
      }
      userNameControl?.updateValueAndValidity();
      passwordControl?.updateValueAndValidity();
    });
    this.employeeForm.get('JobDepartmentId')?.valueChanges.subscribe(value => {
      const jobTitleControl = this.employeeForm.get('JobTitleId');
      jobTitleControl?.setValue('');
      if (value) {
        this.filteredJobTitles = this.jobTitles.filter((title: any) => title.jobDepartmentId === parseInt(value));
        jobTitleControl?.enable();
      } else {
        this.filteredJobTitles = [];
        jobTitleControl?.disable();
      }
    });
    this.loadStaffData();
  }

  onFileSelected(event: Event) {
    const target = event.target as HTMLInputElement;
    const files: FileList = target.files as FileList;
    this.selectedFiles = [];

    if (files && files.length > 0) {
      for (let i = 0; i < files.length; i++) {
        const file = files[i];
        const validTypes = ['image/png', 'image/jpeg', 'application/pdf'];
        if (validTypes.includes(file.type)) {
          this.selectedFiles.push(file);
        } else {
          this.messageService.add({
            severity: 'error',
            summary: 'خطأ',
            detail: `الملف ${file.name} غير مدعوم. يُسمح فقط بـ PNG, JPG, JPEG, PDF.`,
          });
        }
      }
    }
  }
  
  onDragOver(event: DragEvent) {
    event.preventDefault();
  }

  onDrop(event: DragEvent) {
    event.preventDefault();
    if (event.dataTransfer?.files) {
      this.handleFiles(event.dataTransfer.files);
    }
  }

  handleFiles(files: FileList) {
    this.selectedFiles = [];
    Array.from(files).forEach((file:any) => {
      if (file.type.startsWith('image/')) {
        const reader = new FileReader();
        reader.onload = (e: any) => {
          this.selectedFiles.push({ 
            name: file.name, 
            type: file.type, 
            preview: e.target.result 
          });
        };
        reader.readAsDataURL(file);
      } else {
        this.selectedFiles.push({ 
          name: file.name, 
          type: file.type 
        });
      }
    });
  }

  removeFile(index: number) {
    this.selectedFiles.splice(index, 1);
  }
  

  onSubmit() {
    console.log(this.employeeForm.value);
    console.log(this.employeeId);
    
    if (this.employeeForm.invalid) {
      this.messageService.add({
        severity: 'error',
        summary: 'فشل',
        detail: 'يرجى ملء جميع الحقول المطلوبة بشكل صحيح',
      });
      return;
    }
    const formData = new FormData();
    this.selectedFiles?.forEach((file, index) => {
      formData.append(`Files[${index}]`, file, file.name);
    });
    Object.keys(this.employeeForm.controls).forEach(key => {
      formData.append(key, this.employeeForm.get(key)?.value ?? '');
    });
    if (this.employeeId) {
      this.hrService.updateEmployee(this.employeeId, formData).subscribe({
        next: (res: any) => {
          if (res.isSuccess === true) {
            this.messageService.add({
              severity: 'success',
              summary: 'نجاح',
              detail: 'تم تعديل بيانات الموظف بنجاح',
            });
            setTimeout(() => {
              this.router.navigate(['/hr/employees']);
            }, 1000);
          } else {
            this.messageService.add({
              severity: 'error',
              summary: 'فشل',
              detail: 'فشل في تعديل بيانات الموظف',
            });
            console.log('update', res);
          }
        },
        error: () => {
          this.messageService.add({
            severity: 'error',
            summary: 'فشل',
            detail: 'فشل في تعديل بيانات الموظف',
          });
        },
      });
    }
    
    else {
      this.hrService.addEmployee(formData).subscribe({
        next: (res: any) => {
          if (res.isSuccess === true) {
            this.messageService.add({
              severity: 'success',
              summary: 'نجاح',
              detail: 'تم إضافة الموظف بنجاح',
            });
            setTimeout(() => {
              this.router.navigate(['/hr/employees']);
            }, 1000);
          }else{
            this.messageService.add({
              severity: 'error',
              summary: 'فشل',
              detail: 'فشل في إضافة الموظف',
            });
            console.log('add',res);
            console.log(res.message);
            console.log(res.errors);
            
          }
        },
        error: () => {
          this.messageService.add({
            severity: 'error',
            summary: 'فشل',
            detail: 'فشل في إضافة الموظف',
          });
        }
      });
    }
  }
  
  loadStaffData() { 
    forkJoin({
      jobDepartments: this.hrService.getDepartments(''),
      jobTitles: this.hrService.getTitles(''),
      jobTypes: this.hrService.getTypes(''),
      jobLevels: this.hrService.getLevels(''),
      // branches: this.hrService.GetBranches()
    }).subscribe({
      next: (data:any) => {
        this.jobDepartments = data.jobDepartments.result;
        this.jobTypes = data.jobTypes.result;
        this.jobTitles = data.jobTitles.result;
        this.jobLevels = data.jobLevels.result;
        // this.branches = data.branches.results;
        console.log(data);
        
        if (this.employeeData?.jobDepartmentId) {
          this.onDepartmentChange(this.employeeData.jobDepartmentId);
          this.employeeForm.patchValue({
            JobTitleId: this.employeeData.jobTitleId
          });
        }
      },
      error: (error) => {
        console.error(error);
      }
    })
  }
  
  
  // 
  employeeData: any;
  loadEmployeeData(id: number) {
    this.hrService.getEmployeeById(id).subscribe({
      next: (res: any) => {
        this.employeeData = res;
        this.employeeId = this.employeeData.id;
        console.log(this.employeeData);
        this.employeeForm.patchValue({
          fullName: this.employeeData.fullName,
          nationalId: this.employeeData.nationalId,
          gender: this.employeeData.gender,
          phoneNumber: this.employeeData.phoneNumber,
          email: this.employeeData.email,
          address: this.employeeData.address,
          type: this.employeeData.type ?? '', 
          hireDate: this.employeeData.hireDate,
          branchId: this.employeeData.branchId,
          basicSalary: this.employeeData.basicSalary ?? 0,
          tax: this.employeeData.tax ?? 0,
          insurance: this.employeeData.insurance ?? 0,
          allowances: this.employeeData.allowances ?? 0,
          vacationDays: this.employeeData.vacationDays ?? 0,
          status: this.employeeData.status,
          maritalStatus: this.employeeData.maritalStatus,
          notes: this.employeeData.notes,
          JobDepartmentId: this.employeeData.jobDepartmentId,
          JobLevelId: this.employeeData.jobLevelId,
          JobTypeId: this.employeeData.jobTypeId,
          isAuthorized: this.employeeData.isAuthorized ?? null,
          userName: this.employeeData.userName ?? '',
          password: '',
        });
      },
      error: (err) => {
        console.error(err);
        this.messageService.add({ severity: 'error', summary: 'خطأ', detail: 'فشل في تحميل بيانات الموظف' });
      }
    });
  }
  onDepartmentChange(deptId: number) {
    if (!this.jobTitles) {
      this.filteredJobTitles = [];
      this.employeeForm.get('JobTitleId')?.disable();
      return;
    }
    this.filteredJobTitles = this.jobTitles.filter((x: any) => x.jobDepartmentId === deptId);
  
    if (this.filteredJobTitles.length > 0) {
      this.employeeForm.get('JobTitleId')?.enable();
    } else {
      this.employeeForm.get('JobTitleId')?.disable();
    }
  }   
}
