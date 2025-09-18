import { Component, inject, OnInit } from '@angular/core';
import { MessageService } from 'primeng/api';
import { SystemSettingsService } from '../../../../Core/services/system-settings.service';
import { ToastModule } from "primeng/toast";
import { NgFor, NgIf } from '@angular/common';

@Component({
  selector: 'app-main-groups',
  standalone: true,
  templateUrl: './main-groups.component.html',
  styleUrl: './main-groups.component.css',
  providers: [MessageService],
  imports: [ToastModule,NgIf,NgFor]
})
export class MainGroupsComponent implements OnInit {
  private service = inject(SystemSettingsService);
  private messageService = inject(MessageService);
  mainGroups = this.service.mainGroups;
  loading = this.service.loading;
  error = this.service.error;

  ngOnInit(): void {
    this.loadMainGroups()
  }

  loadMainGroups() {
    this.service.getAllMainGroups().subscribe({
      next: () => {
        console.log(this.mainGroups());
        
      },
      error: () => {
        this.messageService.add({ severity: 'error', summary: 'خطأ', detail: 'فشل في تحميل المجموعات الرئيسية' });
      }
    });
  }

  addMainGroup() {
    
  }

  editMainGroup(id: number) {
    
  }

  deleteMainGroup(id: number) {
    this.service.deleteMainGroup(id).subscribe({
      next: () => {
        this.messageService.add({ severity: 'warn', summary: 'تم', detail: 'تم حذف المجموعة' });
      },
      error: () => {
        this.messageService.add({ severity: 'error', summary: 'خطأ', detail: 'فشل في حذف المجموعة' });
      }
    });
  }
}
