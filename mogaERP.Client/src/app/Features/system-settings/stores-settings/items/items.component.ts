import { Component, inject } from '@angular/core';
import { MessageService } from 'primeng/api';
import { SystemSettingsService } from '../../../../Core/services/system-settings.service';
import { ToastModule } from "primeng/toast";
import { NgFor, NgIf } from '@angular/common';

@Component({
  selector: 'app-items',
  standalone: true,
  templateUrl: './items.component.html',
  providers:[MessageService],
  imports: [ToastModule,NgIf,NgFor]
})
export class ItemsComponent {
  private service = inject(SystemSettingsService);
  private messageService = inject(MessageService);
  items = this.service.items;
  loading = this.service.loading;
  error = this.service.error;

  ngOnInit(): void {
    this.loadItems()
  }

  loadItems() {
    this.service.getAllItems().subscribe({
      next: () => {
        console.log(this.items());
      },
      error: () => {
        this.messageService.add({ severity: 'error', summary: 'خطأ', detail: 'فشل في تحميل الأصناف' });
      }
    });
  }

  addItem() {
    
  }
  
  editItem(id: number) {
    
  }
  
  deleteItem(id: number) {
    this.service.deleteItem(id).subscribe({
      next: () => {
        this.messageService.add({ severity: 'warn', summary: 'تم', detail: 'تم حذف الأصناف' });
      },
      error: () => {
        this.messageService.add({ severity: 'error', summary: 'خطأ', detail: 'فشل في حذف المجموعة' });
      }
    });
  }
}
