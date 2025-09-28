import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MessageService } from 'primeng/api';
import { FinanceService } from '../../../../core/services/finance.service';
import { forkJoin } from 'rxjs';
import Swal from 'sweetalert2';
declare var bootstrap : any
@Component({
  selector: 'app-addition-notifications',
  templateUrl: './addition-notifications.component.html',
  styleUrl: './addition-notifications.component.css',
  providers:[MessageService]
})
export class AdditionNotificationsComponent {
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
  currentAdditionNotificationId: number | null = null;
  isFilter: boolean = false;
  // 
  additionNotifications!: any[];
  banks!: any;
  accounts!: any;

  // pagination state
  pageSize: number = 2;
  totalCount: number = 100;
  currentPage: number = 1;
  searchTerm: string = '';
  // 
  addDebitNotificationForm!: FormGroup;
  constructor(private fb : FormBuilder , private finTreeService : FinanceService , private messageService : MessageService){
    this.addDebitNotificationForm = this.fb.group({
      date: [new Date().toISOString().substring(0, 10)],
      bankId: ['' , Validators.required],
      accountId: ['' , Validators.required],
      checkNumber: ['' , Validators.required],
      amount: [0 , Validators.required],
      notes: ['']
    }); 
  }
  ngOnInit(){
    this.loadAdditionNotifications();
    this.loadFormData();
  }
  // CRUD
  loadAdditionNotifications(){
    this.finTreeService.getAdditionNotifications(this.currentPage , this.pageSize , this.searchTerm).subscribe({
      next : (response : any) => {
        this.additionNotifications = response.result.data;
        this.totalCount = response.result.totalCount;
        console.log(this.additionNotifications);
      },
      error : (error) => {
        console.log(error);
      }
    })
  }
  loadFormData(){
    forkJoin({
      banks : this.finTreeService.getAllBanks(),
    }).subscribe({
      next : (response : any) => {
        this.banks = response.banks.result.data;
        console.log('banks',this.banks);
      },
      error : (error) => {
        console.log(error);
      }
    })
  }
  trackByAdditionNotification(index : number , item : any){
    return item.id;
  }
    addAdditionNotification() {
      if (this.addDebitNotificationForm.invalid) {
        this.addDebitNotificationForm.markAllAsTouched();
        return;
      }
    
      const formData = this.addDebitNotificationForm.value;
    
      if (this.isEditMode && this.currentAdditionNotificationId) {
        this.finTreeService.updateAdditionNotification(this.currentAdditionNotificationId, formData).subscribe({
          next: (res:any) => {
            this.loadAdditionNotifications();
            if(res.isSuccess){
              this.messageService.add({ severity: 'success', summary: 'تم التعديل بنجاح' , detail: 'تم تعديل الإشعار بنجاح' });
            }else{
              this.messageService.add({ severity: 'error', summary: 'فشل التعديل' , detail: 'فشل تعديل الإشعار' });
            }
          },
          error: (err) => {
            console.error('فشل التعديل:', err);
            this.messageService.add({ severity: 'error', summary: 'فشل التعديل' , detail: 'فشل تعديل الإشعار' });
          }
        });
      } else {
        this.finTreeService.addAdditionNotification(formData).subscribe({
          next: (res:any) => {
            this.loadAdditionNotifications();
            this.addDebitNotificationForm.reset();
            if(res.isSuccess){
              this.messageService.add({ severity: 'success', summary: 'تم الحفظ بنجاح' , detail: 'تم حفظ الإشعار بنجاح' });
            }else{
              this.messageService.add({ severity: 'error', summary: 'فشل الحفظ' , detail: 'فشل حفظ الإشعار' });
            }
          },
          error: (err) => {
            console.error('فشل الإضافة:', err);
            this.messageService.add({ severity: 'error', summary: 'فشل الحفظ' , detail: 'فشل حفظ الإشعار' });
          }
        });
      }
    }
    order!:any;
    editAdditionNotification(id: number) {
      this.isEditMode = true;
      this.currentAdditionNotificationId = id;
    
      this.finTreeService.getAdditionNotificationById(id).subscribe({
        next: (data:any) => {
          this.order=data.result;
          console.log(this.order);
          this.addDebitNotificationForm.patchValue({
            date: this.order.date,
            bankId: this.order.bankId,
            accountId: this.order.accountId,
            checkNumber: this.order.checkNumber,
            amount: this.order.amount,
            notes: this.order.notes
          });
    
          const modal = new bootstrap.Modal(document.getElementById('addnoticePermissionModal')!);
          modal.show();
        },
        error: (err) => {
          console.error('فشل تحميل بيانات إشعار إضافة:', err);
        }
      });
    }
    deleteAdditionNotification(id: number) {
      Swal.fire({
        title: 'هل أنت متأكد؟',
        text: 'هل أنت متأكد من حذف إشعار إضافة؟',
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'نعم، حذف',
        cancelButtonText: 'إلغاء'
      }).then((result) => {
        if (result.isConfirmed) {
          this.finTreeService.deleteAdditionNotification(id).subscribe({
            next: (res:any) => {
              this.loadAdditionNotifications();
              if(res.isSuccess){
                this.messageService.add({ severity: 'success', summary: 'تم الحذف بنجاح' , detail: 'تم حذف الإشعار بنجاح' });
              }else{
                this.messageService.add({ severity: 'error', summary: 'فشل الحذف' , detail: 'فشل حذف الإشعار' });
              }
            },
            error: (err) => {
              console.error('فشل حذف إشعار إضافة:', err);
              this.messageService.add({ severity: 'error', summary: 'فشل الحذف' });
            }
          });
        }
      });
    }
    // 
    generateAdditionNotificationPDFById(id: number) {
      // this.finTreeService.generateAdditionNotificationPDFById(id).subscribe({
      //   next: (res:any) => {
      //     if(res.isSuccess){
      //       this.messageService.add({ severity: 'success', summary: 'تم الطباعة بنجاح' , detail: 'تم طباعة الإشعار بنجاح' });
      //     }else{
      //       this.messageService.add({ severity: 'error', summary: 'فشل الطباعة' , detail: 'فشل طباعة الإشعار' });
      //     }
      //   },
      //   error: (err) => {
      //     console.error('فشل الطباعة:', err);
      //     this.messageService.add({ severity: 'error', summary: 'فشل الطباعة' });
      //   }
      // });
    }
    // 
    get math(){
      return Math;
    }
    // pagination handlers
    search(){
      this.currentPage = 1;
      this.loadAdditionNotifications();
    }
    goToPage(page: number) {
      this.currentPage = page;
      this.loadAdditionNotifications();
    }
    // 
    resetForm(){
      this.addDebitNotificationForm.reset();
      this.isEditMode = false;
      this.currentAdditionNotificationId = null;
    }
}
