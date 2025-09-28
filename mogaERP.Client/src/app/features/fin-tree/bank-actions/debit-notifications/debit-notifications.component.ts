import { Component } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { debitNotification } from '../../../../core/models/fin-tree/banks/debit';
import { FinanceService } from '../../../../core/services/finance.service';
import { Validators } from '@angular/forms';
import Swal from 'sweetalert2';
import { MessageService } from 'primeng/api';
import { forkJoin } from 'rxjs';
declare var bootstrap : any

@Component({
  selector: 'app-debit-notifications',
  templateUrl: './debit-notifications.component.html',
  styleUrl: './debit-notifications.component.css',
  providers:[MessageService]
})
export class DebitNotificationsComponent {
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
  currentDebitNotificationId: number | null = null;
  isFilter: boolean = false;
  // 
  debitNotifications!: debitNotification[];
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
    this.loadDebitNotifications();
    this.loadFormData();
  }

  // CRUD
  loadDebitNotifications(){
    this.finTreeService.getDebitNotifications(this.currentPage , this.pageSize , this.searchTerm).subscribe({
      next : (response : any) => {
        this.debitNotifications = response.result.data;
        this.totalCount = response.result.totalCount;
        console.log(this.debitNotifications);
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
        console.log('reponse:' , response);
      },
      error : (error) => {
        console.log(error);
      }
    })
  }
  trackByDebitNotification(index : number , item : debitNotification){
    return item.id;
  }
  addDebitNotification() {
    if (this.addDebitNotificationForm.invalid) {
      this.addDebitNotificationForm.markAllAsTouched();
      return;
    }
  
    const formData = this.addDebitNotificationForm.value;
  
    if (this.isEditMode && this.currentDebitNotificationId) {
      this.finTreeService.updateDebitNotification(this.currentDebitNotificationId, formData).subscribe({
        next: (res:any) => {
          this.loadDebitNotifications();
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
      this.finTreeService.addDebitNotification(formData).subscribe({
        next: (res:any) => {
          this.loadDebitNotifications();
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
  editDebitNotification(id: number) {
    this.isEditMode = true;
    this.currentDebitNotificationId = id;
  
    this.finTreeService.getDebitNotificationById(id).subscribe({
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
  
        const modal = new bootstrap.Modal(document.getElementById('debitNotificationModal')!);
        modal.show();
      },
      error: (err) => {
        console.error('فشل تحميل بيانات إشعار خصم:', err);
      }
    });
  }
  deleteDebitNotification(id: number) {
    Swal.fire({
      title: 'هل أنت متأكد؟',
      text: 'هل أنت متأكد من حذف إشعار خصم؟',
      icon: 'warning',
      showCancelButton: true,
      confirmButtonColor: '#3085d6',
      cancelButtonColor: '#d33',
      confirmButtonText: 'نعم، حذف',
      cancelButtonText: 'إلغاء'
    }).then((result) => {
      if (result.isConfirmed) {
        this.finTreeService.deleteDebitNotification(id).subscribe({
          next: (res:any) => {
            this.loadDebitNotifications();
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
  generateDebitNotificationPDFById(id:number){}
  get math(){
    return Math;
  }
  // pagination handlers
  search(){
    this.currentPage = 1;
    this.loadDebitNotifications();
  }
  goToPage(page: number) {
    this.currentPage = page;
    this.loadDebitNotifications();
  }
  // 
  resetForm(){
    this.addDebitNotificationForm.reset();
    this.isEditMode = false;
    this.currentDebitNotificationId = null;
  }
}
