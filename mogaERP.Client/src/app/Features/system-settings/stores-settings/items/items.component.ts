import { Component, inject, signal } from '@angular/core';
import { MessageService } from 'primeng/api';
import { SysSettingsService } from '../../../../core/services/sys-settings.service';
import { Items } from '../../../../core/models/system-settings/item';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import Swal from 'sweetalert2';
import { forkJoin } from 'rxjs';
declare var bootstrap:any;

@Component({
  selector: 'app-items',
  templateUrl: './items.component.html',
  styleUrl: './items.component.css',
  providers:[MessageService]
})
export class ItemsComponent {
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
  items: Items[] = [];
  groups: any[] = [];
  units: any[] = [];
  loading = signal<boolean>(false);
  error = signal<string | null>(null);
  // pagination state
  pageSize: number = 2;
  totalCount: number = 100;
  currentPage: number = 1;
  searchTerm:string = '';
  // 
  itemForm!: FormGroup;
  constructor(private service : SysSettingsService , private messageService : MessageService , private fb : FormBuilder){
    this.itemForm = this.fb.group({
    name: [null, Validators.required],
    orderLimit: [null, Validators.required],
    cost: [null, Validators.required],
    openingBalance: [null, Validators.required],
    salesTax: [null, Validators.required],
    price: [null, Validators.required],
    hasBarcode: [true, Validators.required],
    unitId: [null, Validators.required],
    groupId: [null, Validators.required]
    });
  }
  ngOnInit(): void {
    this.loadItems();
    this.loadAnotherData();
  }
  // CRUD
  loadAnotherData(){
    forkJoin({
      groups: this.service.getAllItemsGroups(),
      units: this.service.getAllItemsUnits(1 , 100 , '' , true)
    }).subscribe({
      next: (res:any) => {
        this.groups = res.groups.result;
        this.units = res.units.result;
      },
      error: () => {
        this.messageService.add({ severity: 'error', summary: 'خطأ', detail: 'فشل في تحميل الأصناف' });
      }
    })
  }
  loadItems() {
    this.loading.set(true);
    this.service.getAllItems(this.currentPage, this.pageSize, this.searchTerm , true).subscribe({
      next: (res:any) => {
        this.items = res.result.data;
        this.totalCount = res.result.totalCount;
        this.loading.set(false);
      },
      error: () => {
        this.messageService.add({ severity: 'error', summary: 'خطأ', detail: 'فشل في تحميل الأصناف' });
        this.loading.set(false);
      }
    });
  }

  addItem() {
    if (this.itemForm.invalid) {
      this.itemForm.markAllAsTouched();
      return;
    }
    const rawForm = this.itemForm.value;
    const formData = {
      name: rawForm.name,
      orderLimit: Number(rawForm.orderLimit),
      cost: Number(rawForm.cost),
      openingBalance: Number(rawForm.openingBalance),
      salesTax: Number(rawForm.salesTax),
      price: Number(rawForm.price),
      hasBarcode: rawForm.hasBarcode === true || rawForm.hasBarcode === 'true',
      unitId: rawForm.unitId,
      groupId: Number(rawForm.groupId),
    };
    if (this.isEditMode && this.currentItemId) {
      this.service.updateItem(this.currentItemId, formData).subscribe({
        next: (res) => {
          console.log('تم تعديل الصنف:', res);
          this.loadItems();
          this.itemForm.reset();
          this.isEditMode = false;
          this.currentItemId = null;
          this.messageService.add({
            severity: 'success',
            summary: 'تم التعديل بنجاح',
            detail: 'تم تعديل الصنف بنجاح',
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
          console.error('خطأ أثناء التعديل:', err);
        }
      });
    } else {
      this.service.addItem(formData).subscribe({
        next: (res : any) => {
          console.log('تم إضافة الصنف:', res);
          this.loadItems();
          console.log(JSON.stringify(formData));
          if(res.isSuccess == true){
            this.messageService.add({
              severity: 'success',
              summary: 'تم الإضافة بنجاح',
              detail: 'تم إضافة الصنف بنجاح',
            });
          }else{
            this.messageService.add({
              severity: 'error',
              summary: 'فشل الإضافة',
              detail: 'فشل إضافة الصنف',
            });
          }
          this.itemForm.reset();
        },
        error: (err) => {
          console.error('خطأ أثناء الإضافة:', err);
        }
      });
    }
  }
  itemRes:any;
  editItem(id: number) {
    this.isEditMode = true;
    this.currentItemId = id;
    this.service.getItemById(id).subscribe({
      next: (itemData:any) => {
        console.log(itemData);
        this.itemRes = itemData.result;
        this.itemForm.patchValue({
          name: this.itemRes.name || "",
          unitId: this.itemRes.unitId,
          groupId: this.itemRes.groupId,
          orderLimit: this.itemRes.orderLimit,
          cost: this.itemRes.cost,
          openingBalance: this.itemRes.openingBalance,
          salesTax: this.itemRes.salesTax,
          price: this.itemRes.price,
          hasBarcode: this.itemRes.hasBarcode,
          typeId: this.itemRes.typeId
        });
        const modal = new bootstrap.Modal(document.getElementById('addItemModal')!);
        modal.show();
      },
      error: (err) => {
        console.error('فشل تحميل بيانات الصنف:', err);
      }
    });
  }
  deleteItem(id:number){
    Swal.fire({
      title: 'هل أنت متأكد؟',
      text: 'هل أنت متأكد من حذف هذا الصنف؟',
      icon: 'warning',
      showCancelButton: true,
      confirmButtonColor: '#3085d6',
      cancelButtonColor: '#d33',
      confirmButtonText: 'نعم',
      cancelButtonText: 'إلغاء'
    }).then((result) => {
      if (result.isConfirmed) {
        this.service.deleteItem(id).subscribe({
          next: (res) => {
            console.log('تم حذف الصنف:', res);
            this.loadItems();
          },
          error: (err) => {
            console.error('فشل حذف الصنف:', err);
          }
        });
      }
    });
  }
  // pagination handlers
  get math(){
    return Math;
  }
  goToPage(page: number) {
    this.currentPage = page;
    this.loadItems();
  }

  search() {
    this.currentPage = 1;
    this.loadItems();
  }
  resetSearch(){
    this.searchTerm = '';
    this.loadItems();
  }
  // 
  resetForm(){
    this.itemForm.reset();
  }
}
