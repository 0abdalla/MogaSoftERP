import { Component } from '@angular/core';
import { FormArray, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MessageService } from 'primeng/api';
import { Customers } from '../../../core/models/system-settings/customers';
import { SysSettingsService } from '../../../core/services/sys-settings.service';
import { SalesService } from '../../../core/services/sales.service';
import { forkJoin } from 'rxjs';
import Swal from 'sweetalert2';
import { Items } from '../../../core/models/system-settings/item';
declare var bootstrap : any;
import html2pdf from 'html2pdf.js';

@Component({
  selector: 'app-quotations',
  templateUrl: './quotations.component.html',
  styleUrl: './quotations.component.css',
  providers:[MessageService]
})
export class QuotationsComponent {
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
  currentQuotationId: number | null = null;
  isFilter: boolean = false;
  // 
  quotations: any[] = [];

  // pagination state
  pageSize: number = 16;
  totalCount: number = 100;
  currentPage: number = 1;
  
  searchTerm: string = '';
  // 
  customers: Customers[] = [];
  allItems: Items[] = [];
  // 
  quotationForm: FormGroup;
  // 
  quotataionData: any;
  constructor(private fb: FormBuilder , private systemSettingsService : SysSettingsService , private salesService : SalesService , private messageService : MessageService){
    this.quotationForm = this.fb.group({
      quotationDate: [new Date().toISOString().substring(0, 10)], // Today Date Validator Required
      customerId: [null, Validators.required],
      validityPeriod: ['', Validators.required],
      isTaxIncluded: [false],
      description: [''],
      items: this.fb.array([
        this.createItemGroup()
      ]),
      paymentTerms: this.fb.array([
        this.createPaymentTermGroup()
      ])
    })


    this.items.controls.forEach(control => {
      const group = control as FormGroup;
      this.setupTotalCalculation(group);
    });    

  }
  // 
  setupTotalCalculation(group: FormGroup) {
    group.get('quantity')?.valueChanges.subscribe(() => {
      this.updateTotal(group);
    });
    group.get('unitPrice')?.valueChanges.subscribe(() => {
      this.updateTotal(group);
    });
  }
  
  updateTotal(group: FormGroup) {
    const qty = group.get('quantity')?.value || 0;
    const price = group.get('unitPrice')?.value || 0;
    group.get('totalPrice')?.setValue(qty * price, { emitEvent: false });
  }
  // 
  createItemGroup(): FormGroup {
    return this.fb.group({
      itemId: [null, Validators.required],
      quantity: [0, [Validators.required, Validators.min(1)]],
      unitPrice: [0, [Validators.required, Validators.min(1)]],
      totalPrice: [{ value: 0}]
    });
  }
  createPaymentTermGroup(): FormGroup {
    return this.fb.group({
      condition: ['', Validators.required],
      percentage: [0, [Validators.required, Validators.min(1), Validators.max(100)]],
    });
  }
  get items(): FormArray {
    return this.quotationForm.get('items') as FormArray;
  }
  get paymentTerms(): FormArray {
    return this.quotationForm.get('paymentTerms') as FormArray;
  }
  addItemRow() {
    const group = this.createItemGroup();
    this.items.push(group);
    this.setupTotalCalculation(group);
    
  }
  addPaymentTermRow() {
    this.paymentTerms.push(this.createPaymentTermGroup());
  }
  removeItemRow(index: number) {
    if (this.items.length > 1) {
      this.items.removeAt(index);
    }
  }
  removePaymentTermRow(index: number) {
    if (this.paymentTerms.length > 1) {
      this.paymentTerms.removeAt(index);
    }
  }
  // 
  ngOnInit(): void {
    this.loadQuotations();
    this.loadInitialData();
    
  }
  // CRUD
  loadQuotations() {
    this.salesService.getAllQuotations(this.currentPage, this.pageSize, this.searchTerm).subscribe((res) => {
      this.quotations = res.result.data;
      this.totalCount = res.result.totalCount;
      console.log('Quotations:', this.quotations);
    });
  }
  loadInitialData() {
    forkJoin({
      customers: this.systemSettingsService.getCustomers(1, 100, '', true),
      items: this.systemSettingsService.getAllItems(1, 100, '', true),
    }).subscribe(({ customers , items }) => {
      this.customers = customers.result.data;
      this.allItems = items.result.data;
      console.log('Customers:', this.customers);
    });
  }
  trackByQuotation(index: number, item: any): number {
    return item.id;
  }
  quotation: any | null = null;
  addQuotation() {
      if (this.quotationForm.invalid) {
        this.quotationForm.markAllAsTouched();
        return;
      }
  
      const formData = this.quotationForm.value;
  
      if (this.isEditMode && this.currentQuotationId) {
        this.salesService.updateQuotation(this.currentQuotationId, formData).subscribe({
          next: (res: any) => {
            if (res.isSuccess === true) {
              this.messageService.add({
                severity: 'success',
                summary: 'تم التعديل',
                detail: `${res.message}`
              });
              // this.CloseModal();
              this.loadQuotations();
              console.log(res);
            } else {
              this.messageService.add({
                severity: 'error',
                summary: 'فشل التعديل',
                detail: `${res.message}`
              });
            }
          },
          error: (err) => {
            console.error('فشل التعديل:', err);
          }
        });
      } else {
        this.salesService.addQuotation(formData).subscribe({
          next: (res: any) => {
            this.quotataionData = res.result;
            if (res.isSuccess === true) {
              this.messageService.add({
                severity: 'success',
                summary: 'تم الإضافة',
                detail: `${res.message}`
              });
              // this.CloseModal();
              this.quotationForm.reset();
              console.log(res);
              this.loadQuotations();
              setTimeout(() => {
                const element = document.getElementById('printableQuotation');
                if (element) {
                  html2pdf()
                    .from(element)
                    .set({
                      margin: [0, 0, 0, 0],
                      filename: `${this.quotataionData.quotationNumber}.pdf`,
                      html2canvas: { scale: 2 },
                      jsPDF: { unit: 'px', format: 'a4', orientation: 'portrait' }
                    })
                    .save();
                }
              }, 500);        
              // this.generatePurchaseRequestPDFById(res.results);
            } else {
              this.messageService.add({
                severity: 'error',
                summary: 'فشل الإضافة',
                detail: `${res.message}`
              });
              console.log(res);
              console.log(formData);
              
            }
          },
          error: (err) => {
            console.error('فشل الإضافة:', err);
          }
        });
      }
  }
  editQuotation(id: number) {
    this.isEditMode = true;
    this.currentQuotationId = id;
  
      this.salesService.getQuotationById(id).subscribe({
        next: (res:any) => {
          this.quotation = res.result;
          console.log(this.quotation);
          this.quotationForm.patchValue({
            quotationDate: this.quotation.quotationDate,
            customerId: this.quotation.customerId,
            description: this.quotation.description,
            validityPeriod: this.quotation.validityPeriod,
            isTaxIncluded: this.quotation.isTaxIncluded,
            items: this.quotation.items,
            paymentTerms: this.quotation.paymentTerms
          });
  
          const modal = new bootstrap.Modal(document.getElementById('addQuotationModal')!);
          modal.show();
        },
        error: (err) => {
          console.error('فشل تحميل بيانات عرض السعر:', err);
        }
      });
  }
  deleteQuotation(id: number) {
      Swal.fire({
        title: 'هل أنت متأكد؟',
        text: 'هل أنت متأكد من حذف العرض؟',
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'نعم، حذف',
        cancelButtonText: 'إلغاء'
      }).then((result) => {
        if (result.isConfirmed) {
          this.salesService.deleteQuotation(id).subscribe({
            next: () => {
              this.loadQuotations();
            },
            error: (err) => {
              console.error('فشل حذف العرض:', err);
            }
          });
        }
      });
  }
  // Handlers
  generateQuotationPDFById(id: number) {
    this.salesService.getQuotationById(id).subscribe({
      next: (res:any) => {
        this.quotation = res.result;
        console.log(this.quotation);
        const modal = new bootstrap.Modal(document.getElementById('addQuotationModal')!);
        modal.show();
      },
      error: (err) => {
        console.error('فشل تحميل بيانات عرض السعر:', err);
      }
    });
  }
  getSelectedItemIds(): any[] {
    return this.items.controls
      .map(g => g.get('itemId')?.value)
      .filter(v => v !== null && v !== undefined && v !== '');
  }
  getAvailableItems(rowIndex: number) {
    const selected = this.getSelectedItemIds().map(v => String(v));
    const currentValue = String(this.items.at(rowIndex).get('itemId')?.value ?? '');
    return this.allItems.filter(it => {
      const idStr = String(it.id);
      return idStr === currentValue || !selected.includes(idStr);
    });
  }  
  resetForm() {
    this.quotationForm.reset();
    this.isEditMode = false;
    this.currentQuotationId = null;
  }
  // Pagination
  goToPage(page: number) {
    this.currentPage = page;
    this.loadQuotations();
  }
  get math() {
    return Math;
  }
  search() {
    this.currentPage = 1;
    this.loadQuotations();
  }
}
