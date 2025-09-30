import { Component } from '@angular/core';
import { DailyEntry } from '../../../core/models/fin-tree/entries';
import { FormGroup, FormBuilder, Validators, FormArray, AbstractControl } from '@angular/forms';
import { MessageService } from 'primeng/api';
import { FinanceService } from '../../../core/services/finance.service';
import { forkJoin } from 'rxjs';
import Swal from 'sweetalert2';
declare var bootstrap : any;

@Component({
  selector: 'app-entries',
  templateUrl: './entries.component.html',
  styleUrl: './entries.component.css',
  providers:[MessageService]
})
export class EntriesComponent {
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
  currentEntryId: number | null = null;
  isFilter: boolean = false;
  // 
  entries!: DailyEntry[];
  restrictionTypes!: any;
  accountingGuidance!: any;
  accounts!: any;
  costCenters!: any;

  // pagination state
  pageSize: number = 2;
  totalCount: number = 100;
  currentPage: number = 1;
  searchTerm: string = '';
  // 
  addEntryForm!: FormGroup;
  // Handlers
  totalDebit: number = 0;
  totalCredit: number = 0;
  isBalanced: boolean = false;
  constructor(private fb : FormBuilder , private finTreeService : FinanceService , private messageService : MessageService){
    this.addEntryForm = this.fb.group({
            restrictionDate: [new Date().toISOString().substring(0, 10)],
      restrictionTypeId: ['', Validators.required],
      accountingGuidanceId: ['', Validators.required],
      description: ['', Validators.required],
      details: this.fb.array([
        this.createdetilsGroup()
      ]),
    });
  }
  createdetilsGroup(): FormGroup {
    const group = this.fb.group({
      accountId: [null, Validators.required],
      debit: [0, Validators.required],
      credit: [0, Validators.required],
      costCenterId: [null, Validators.required],
      note: ['']
    });
  
    group.get('debit')?.valueChanges.subscribe(value => {
      if (value && value > 0) {
        group.get('credit')?.setValue(0);
        group.get('credit')?.disable({ emitEvent: false });
      } else {
        group.get('credit')?.enable({ emitEvent: false });
      }
      this.calculateTotals();
    });
  
    group.get('credit')?.valueChanges.subscribe(value => {
      if (value && value > 0) {
        group.get('debit')?.setValue(0);
        group.get('debit')?.disable({ emitEvent: false });
      } else {
        group.get('debit')?.enable({ emitEvent: false });
      }
      this.calculateTotals();
    });
  
    return group;
  }
  
  get details(): FormArray {
    return this.addEntryForm.get('details') as FormArray;
  }
  
  addItemRow() {
    this.details.push(this.createdetilsGroup());
    this.calculateTotals();
  }
  
  removeItemRow(index: number) {
    if (this.details.length > 1) {
      this.details.removeAt(index);
      this.calculateTotals();
    }
  }
  calculateTotals() {
    this.totalDebit = 0;
    this.totalCredit = 0;
  
    this.details.controls.forEach((group: AbstractControl) => {
      const debit = +group.get('debit')?.value || 0;
      const credit = +group.get('credit')?.value || 0;
      this.totalDebit += debit;
      this.totalCredit += credit;
    });
    this.isBalanced = this.totalDebit === this.totalCredit;
  }

  ngOnInit(){
    this.loadEntries();
    this.loadFormData();
  }
  // CRUD
  loadEntries(){
    this.finTreeService.getDailyEntries(this.currentPage, this.pageSize, this.searchTerm).subscribe({
      next: (res:any) => {
        this.entries = res.result.data;
        this.totalCount = res.result.totalCount;
        console.log('entries',this.entries);
      },
      error: (err) => {
        console.error('Error fetching entries', err);
      }
    });
  }
  loadFormData(){
    forkJoin({
      // restrictionTypes : this.finTreeService.getAllRestrictionTypes(),
      // accountingGuidance : this.finTreeService.getAllAccountingGuidance(),
      accounts : this.finTreeService.getAllAccounts(),
    }).subscribe({
      next : (response : any) => {
        // this.restrictionTypes = response.restrictionTypes.result.data;
        // this.accountingGuidance = response.accountingGuidance.result.data;
        this.accounts = response.accounts.result.data;
        console.log('restrictionTypes',this.restrictionTypes);
        console.log('accountingGuidance',this.accountingGuidance);
        console.log('accounts',this.accounts);
      },
      error : (error) => {
        console.log(error);
      }
    })
  }
  trackByEntry(index : number , item : DailyEntry){
    return item.id;
  }
  
  addEntry() {
    if (this.addEntryForm.invalid) {
      this.addEntryForm.markAllAsTouched();
      return;
    }
  
    const formData = this.addEntryForm.getRawValue();
    formData.restrictionNumber = String(formData.restrictionNumber);
    formData.ledgerNumber = String(formData.ledgerNumber);
    formData.restrictionTypeId = Number(formData.restrictionTypeId);
    formData.details = formData.details.map((item: any) => ({
      accountId: Number(item.accountId),
      debit: Number(item.debit),
      credit: Number(item.credit),
      costCenterId: Number(item.costCenterId),
      note: item.note
    }));
    if (this.isEditMode && this.currentEntryId !== null) {
      this.finTreeService.updateDailyEntry(this.currentEntryId, formData).subscribe({
        next: () => {
          this.loadEntries();
          this.addEntryForm.reset();
          this.isEditMode = false;
          this.currentEntryId = null;
        },
        error: (err) => {
          console.error('فشل التعديل:', err);
        }
      });
    } else {
      this.finTreeService.addDailyEntry(formData).subscribe({
        next: (res:any) => {
          console.log(res);
          console.log(formData);
          this.loadEntries();
          // this.closeModal();
          this.addEntryForm.reset();
        },
        error: (err) => {
          console.error('فشل الإضافة:', err);
        }
      });
    }
  }
  restriction!:any;
  editEntry(id: number) {
    this.isEditMode = true;
    this.currentEntryId = id;
  
    this.finTreeService.getDailyEntryById(id).subscribe({
      next: (data:any) => {
        this.restriction=data.result;
        console.log(this.restriction);
        this.addEntryForm.patchValue({
          restrictionNumber: this.restriction.restrictionNumber,
          restrictionDate: this.restriction.restrictionDate,
          restrictionTypeId: this.restriction.restrictionTypeId,
          ledgerNumber: this.restriction.ledgerNumber,
          details: this.restriction.details,
          
        });
        this.restriction.details.forEach((item:any) => {
          this.details.push(this.fb.group({
            accountId: [item.accountId, Validators.required],
            debit: [item.debit, Validators.required],
            credit: [item.credit, Validators.required],
            costCenterId: [item.costCenterId, Validators.required],
            note: [item.note || '']
          }));
        });
  
        const modal = new bootstrap.Modal(document.getElementById('entriesModal')!);
        modal.show();
      },
      error: (err) => {
        console.error('فشل تحميل بيانات الخزنة:', err);
      }
    });
  }
  deleteEntry(id: number) {
    Swal.fire({
      title: 'هل أنت متأكد؟',
      text: 'هل تريد حذف هذا القيد؟',
      icon: 'warning',
      showCancelButton: true,
      confirmButtonColor: '#3085d6',
      cancelButtonColor: '#d33',
      confirmButtonText: 'نعم، حذف',
      cancelButtonText: 'إلغاء'
    }).then((result) => {
      if (result.isConfirmed) {
        this.finTreeService.deleteDailyEntry(id).subscribe({
          next: () => {
            this.loadEntries();
          },
          error: (err) => {
            console.error('فشل الحذف:', err);
          }
        });
      }
    });
  }
  // 
  generateEntryPDFById(id:number){}
  get math(){
    return Math;
  }
  // pagination handlers
  search(){
    this.currentPage = 1;
    this.loadEntries();
  }
  goToPage(page: number) {
    this.currentPage = page;
    this.loadEntries();
  }
  // 
  resetForm(){
    this.addEntryForm.reset();
    this.isEditMode = false;
    this.currentEntryId = null;
  }
}
