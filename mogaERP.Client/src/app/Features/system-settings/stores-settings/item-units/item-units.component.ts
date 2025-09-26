import { Component, inject } from '@angular/core';
import { MessageService } from 'primeng/api';
import { ToastModule } from 'primeng/toast';
import { SystemSettingsService } from '../../../../Core/services/system-settings.service';
import { NgFor, NgIf } from '@angular/common';

@Component({
  selector: 'app-item-units',
  standalone: true,
  imports: [ToastModule,NgIf,NgFor],
  templateUrl: './item-units.component.html',
  providers:[MessageService]
})
export class ItemUnitsComponent {
  private service = inject(SystemSettingsService);
  private messageService = inject(MessageService);
  itemsUnits = this.service.itemsUnits;
  loading = this.service.loading;
  error = this.service.error;

  ngOnInit(): void {
    this.loadItems()
  }

  loadItems() {
    this.service.getAllItemsUnits().subscribe({
      next: () => {
        console.log(this.itemsUnits());
      },
      error: () => {
        this.messageService.add({ severity: 'error', summary: 'خطأ', detail: 'فشل في تحميل الوحدات' });
      }
    });
  }

  addItem() {
    
  }
  
  editItem(id: number) {
    
  }
  
  deleteItem(id: number) {
    this.service.deleteItemUnit(id).subscribe({
      next: () => {
        this.messageService.add({ severity: 'warn', summary: 'تم', detail: 'تم حذف الوحدة' });
      },
      error: () => {
        this.messageService.add({ severity: 'error', summary: 'خطأ', detail: 'فشل في حذف الوحدة' });
      }
    });
  }
}
