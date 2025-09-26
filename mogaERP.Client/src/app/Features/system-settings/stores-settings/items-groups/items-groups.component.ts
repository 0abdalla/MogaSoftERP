import { Component, inject } from '@angular/core';
import { MessageService } from 'primeng/api';
import { SystemSettingsService } from '../../../../Core/services/system-settings.service';
import { ToastModule } from "primeng/toast";
import { NgFor, NgIf } from '@angular/common';

@Component({
  selector: 'app-items-groups',
  standalone: true,
  templateUrl: './items-groups.component.html',
  styleUrl: './items-groups.component.css',
  providers:[MessageService],
  imports: [ToastModule,NgIf,NgFor]
})
export class ItemsGroupsComponent {
  private service = inject(SystemSettingsService);
  private messageService = inject(MessageService);
  itemsGroups = this.service.itemsGroups;
  loading = this.service.loading;
  error = this.service.error;
  
  ngOnInit(): void {
    this.loadItemsGroups()
  }
  
  loadItemsGroups() {
    this.service.getAllItemsGroups().subscribe({
      next: () => {
          console.log(this.itemsGroups());
          
        },
        error: () => {
          this.messageService.add({ severity: 'error', summary: 'خطأ', detail: 'فشل في تحميل المجموعات' });
        }
      });
  }
  
  addItemGroup() {
    
  }
  
  editItemGroup(id: number) {
    
  }
  
  deleteItemGroup(id: number) {
    this.service.deleteItemGroup(id).subscribe({
      next: () => {
        this.messageService.add({ severity: 'warn', summary: 'تم', detail: 'تم حذف المجموعة' });
      },
      error: () => {
        this.messageService.add({ severity: 'error', summary: 'خطأ', detail: 'فشل في حذف المجموعة' });
      }
    });
  }
}
