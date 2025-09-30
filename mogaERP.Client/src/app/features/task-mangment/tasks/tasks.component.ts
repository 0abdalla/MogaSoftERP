import { Component } from '@angular/core';

@Component({
  selector: 'app-tasks',
  templateUrl: './tasks.component.html',
  styleUrls: ['./tasks.component.css']
})
export class TasksComponent {
  tasks = [
    { id: 'TM-2025-00001', appointment: '06/16/2025 - 06/16/2025', status: 'تم التنفيذ', priority: 'عالية جداً', assignee: '', attachments: 0 },
    { id: 'TM-2025-00002', appointment: '06/16/2025 - 06/16/2025', status: 'لم تنفذ', priority: 'عالية', assignee: '', attachments: 1 },
    { id: 'TM-2025-00003', appointment: '06/16/2025 - 06/16/2025', status: 'قيد التنفيذ', priority: 'متوسطة', assignee: '', attachments: 2 },
    { id: 'TM-2025-00004', appointment: '06/16/2025 - 07/07/2025', status: 'تم التنفيذ', priority: 'منخفضة', assignee: '', attachments: 0 },
  ];


  currentPage = 1;
  itemsPerPage = 5;
  totalPages = 0;
  pageSizes = [5, 10, 20, 50];
  gotoPageNumber = 1;


  getStatusDotClass(status: string): string {
    switch (status.toLowerCase()) {
      case 'تم التنفيذ': return 'bg-success';
      case 'قيد التنفيذ': return 'bg-warning';
      case 'لم تنفذ': return 'bg-danger';
      case 'ملغي': return 'bg-danger';
      default: return 'bg-secondary';
    }
  }
  
  getPriorityDotClass(priority: string): string {
    switch (priority.toLowerCase()) {
      case 'عالية جداً': return 'bg-danger';
      case 'عالية': return 'bg-danger';
      case 'متوسطة': return 'bg-primary';
      case 'منخفضة': return 'bg-success';
      case 'منخفضة جداً': return 'bg-warning';
      default: return 'bg-secondary';
    }
  }
  // 
  updateTotalPages() {
    this.totalPages = Math.ceil(this.tasks.length / this.itemsPerPage);
    if (this.currentPage > this.totalPages) {
      this.currentPage = this.totalPages;
    }
  }

  get displayedTasks() {
    const start = (this.currentPage - 1) * this.itemsPerPage;
    return this.tasks.slice(start, start + this.itemsPerPage);
  }

  prevPage() {
    if (this.currentPage > 1) this.currentPage--;
  }

  nextPage() {
    if (this.currentPage < this.totalPages) this.currentPage++;
  }

  goToPage(page: number) {
    const validPage = Math.max(1, Math.min(page, this.totalPages));
    this.currentPage = validPage;
    this.gotoPageNumber = validPage;
  }

  getVisiblePages(): number[] {
    const pages = [];
    const total = this.totalPages;
    const current = this.currentPage;

    let start = Math.max(1, current - 2);
    let end = Math.min(total, current + 2);

    if (current <= 3) {
      end = Math.min(5, total);
    } else if (current >= total - 2) {
      start = Math.max(total - 4, 1);
    }

    for (let i = start; i <= end; i++) {
      pages.push(i);
    }

    return pages;
  }
}
