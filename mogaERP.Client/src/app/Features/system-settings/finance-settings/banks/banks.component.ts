import { Component } from '@angular/core';
import { Banks } from '../../../../core/models/system-settings/banks';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { MessageService } from 'primeng/api';
import { SysSettingsService } from '../../../../core/services/sys-settings.service';
import { FinanceService } from '../../../../core/services/finance.service';
import Swal from 'sweetalert2';
declare var bootstrap : any;
@Component({
  selector: 'app-banks',
  templateUrl: './banks.component.html',
  styleUrl: './banks.component.css',
  providers:[MessageService]
})
export class BanksComponent {
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
  currentBankId!:any;
  isFilter: boolean = false;
  // 
  banks:Banks[] = [];
  // pagination state
  searchTerm: string = '';
  currentPage: number = 1;
  pageSize: number = 16;
  totalCount: number = 0;
  // 
  addBankForm: FormGroup;
  // 
  constructor(private fb: FormBuilder , private service : FinanceService , private toastService : MessageService){
    this.addBankForm = this.fb.group({
      name: ['', Validators.required],
      code: ['', Validators.required],
      accountNumber: ['', Validators.required],
      currency: ['', Validators.required],
      initialBalance: ['', Validators.required]
    });
  }
  ngOnInit(): void {
    this.loadBanks();
  }
  // CRUD
  loadBanks(){
    this.service.getAllBanks(this.currentPage, this.pageSize, this.searchTerm, true).subscribe((res) => {
      this.banks = res.result.data;
      this.totalCount = res.result.totalCount;
      console.log(this.banks);
    });
  }
  trackByProvider(index: number, provider: any): number {
    return provider.id;
  }
  addBank() {
    if (this.addBankForm.invalid) {
      this.addBankForm.markAllAsTouched();
      return;
    }
  
    const formData = this.addBankForm.value;
  
    if (this.isEditMode && this.currentBankId !== null) {
      this.service.updateBank(this.currentBankId, formData).subscribe({
        next: (res:any) => {
          this.loadBanks();
          this.addBankForm.reset();
          this.isEditMode = false;
          this.currentBankId = null;
          this.toastService.add({
            severity: 'success',
            summary: 'تم التعديل بنجاح',
            detail: 'تم تعديل البنك بنجاح',
          });
          setTimeout(() => {
            const modalElement = document.getElementById('addItemModal');
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
      this.service.addBank(formData).subscribe({
        next: (res) => {
          console.log(res);
          console.log(this.addBankForm.value);
          
          this.loadBanks();
          // this.addBankForm.reset();
          if(res.isSuccess == true){
            this.toastService.add({
              severity: 'success',
              summary: 'تم الإضافة بنجاح',
              detail: 'تم إضافة البنك بنجاح',
            });
          }else{
            this.toastService.add({
              severity: 'error',
              summary: 'فشل الإضافة',
              detail: 'فشل إضافة البنك',
            });
          } 
        },
        error: (err) => {
          console.error('فشل الإضافة:', err);
        }
      });
    }
  }
  Bank!:any;
  editBank(id: number) {
    this.isEditMode = true;
    this.currentBankId = id;
  
    this.service.getBankById(id).subscribe({
      next: (data) => {
        this.Bank=data.results;
        this.addBankForm.patchValue({
          name: this.Bank.name,
          code: this.Bank.code,
          accountNumber: this.Bank.accountNumber,
          currency: this.Bank.currency,
          initialBalance: this.Bank.initialBalance,
        });
  
        const modal = new bootstrap.Modal(document.getElementById('addItemModal')!);
        modal.show();
      },
      error: (err) => {
        console.error('فشل تحميل بيانات البنك:', err);
      }
    });
  }
  deleteBank(id: number) {
    Swal.fire({
      title: 'هل أنت متأكد؟',
      text: 'هل تريد حذف هذا البنك؟',
      icon: 'warning',
      showCancelButton: true,
      confirmButtonColor: '#3085d6',
      cancelButtonColor: '#d33',
      confirmButtonText: 'نعم، حذف',
      cancelButtonText: 'إلغاء'
    }).then((result) => {
      if (result.isConfirmed) {
        this.service.deleteBank(id).subscribe({
          next: (res:any) => {
            this.loadBanks();
            if(res.isSuccess == true){
              this.toastService.add({
                severity: 'success',
                summary: 'تم الحذف بنجاح',
                detail: 'تم حذف البنك بنجاح',
              });
            }else{
              this.toastService.add({
                severity: 'error',
                summary: 'فشل الحذف',
                detail: 'فشل حذف البنك',
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
  search(){
    this.loadBanks();
  }
  get math(){
    return Math;
  }
  goToPage(page: number){
    this.currentPage = page;
    this.loadBanks();
  }
  // 
  resetForm(){
    this.addBankForm.reset();
    this.isEditMode = false;
  }
}
