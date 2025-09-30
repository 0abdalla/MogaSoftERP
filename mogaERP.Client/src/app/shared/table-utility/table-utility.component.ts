import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Output } from '@angular/core';
import { FormsModule } from '@angular/forms';
import * as XLSX from 'xlsx-js-style';
import html2pdf from 'html2pdf.js';
declare var bootstrap : any;
@Component({
  selector: 'app-table-utility',
  templateUrl: './table-utility.component.html',
  styleUrl: './table-utility.component.css',
  standalone: true,
  imports: [CommonModule , FormsModule]
})
export class TableUtilityComponent {
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
  isCompact = false;
  isFullscreen = false;
  exportType: string = 'excel';
  fileName: string = 'جدول';
  @Output() toggleCompactMode = new EventEmitter<boolean>();
  constructor(){
    this.userName = localStorage.getItem('fullName') || '';
  }
  toggleCompact() {
    this.isCompact = !this.isCompact;
    const table = document.querySelector('table');
    if (this.isCompact) {
      table?.classList.add('compact');
    } else {
      table?.classList.remove('compact');
    }
  }
  reLoad(){
    location.reload();
  }
  toggleFullscreen() {
    if (!document.fullscreenElement) {
      document.documentElement.requestFullscreen();
      this.isFullscreen = true;
    } else {
      if (document.exitFullscreen) {
        document.exitFullscreen();
        this.isFullscreen = false;
      }
    }
  }
  openExportModal() {
    const modal = new bootstrap.Modal(document.getElementById('exportModal')!);
    modal.show();
  }

  exportTable() {
    const table = document.querySelector('table') as HTMLElement;
    if (!table) return;
  
    if (this.exportType === 'excel') {
      const ws: XLSX.WorkSheet = XLSX.utils.table_to_sheet(table, { raw: true });
      const range = XLSX.utils.decode_range(ws['!ref']!);
      for (let R = range.s.r; R <= range.e.r; ++R) {
        const cellRef = XLSX.utils.encode_cell({ r: R, c: range.e.c });
        delete ws[cellRef];
      }
      range.e.c--;
      ws['!ref'] = XLSX.utils.encode_range(range);
      // Header style
      for (let C = range.s.c; C <= range.e.c; ++C) {
        const cellRef = XLSX.utils.encode_cell({ r: 0, c: C });
        if (ws[cellRef]) {
          ws[cellRef].s = {
            font: { bold: true, sz: 14, color: { rgb: "FFFFFF" } },
            fill: { fgColor: { rgb: "1F4E78" } },
            alignment: { horizontal: "center", vertical: "center" },
            border: {
              top: { style: "thin", color: { rgb: "000000" } },
              bottom: { style: "thin", color: { rgb: "000000" } },
              left: { style: "thin", color: { rgb: "000000" } },
              right: { style: "thin", color: { rgb: "000000" } }
            }
          };
        }
      }
  
      // Data style 
      for (let R = 1; R <= range.e.r; ++R) {
        for (let C = range.s.c; C <= range.e.c; ++C) {
          const cellRef = XLSX.utils.encode_cell({ r: R, c: C });
          if (ws[cellRef]) {
            ws[cellRef].s = {
              alignment: { horizontal: "right", vertical: "center" },
              border: {
                top: { style: "thin", color: { rgb: "000000" } },
                bottom: { style: "thin", color: { rgb: "000000" } },
                left: { style: "thin", color: { rgb: "000000" } },
                right: { style: "thin", color: { rgb: "000000" } }
              }
            };
          }
        }
      }
  
      // Column width
      const colWidths: any[] = [];
      for (let C = range.s.c; C <= range.e.c; ++C) {
        let maxLength = 20;
        for (let R = range.s.r; R <= range.e.r; ++R) {
          const cellRef = XLSX.utils.encode_cell({ r: R, c: C });
          const cellValue = ws[cellRef]?.v ? ws[cellRef].v.toString() : "";
          if (cellValue.length > maxLength) maxLength = cellValue.length;
        }
        colWidths.push({ wch: maxLength + 2 });
      }
      ws['!cols'] = colWidths;
      (ws as any)['!rtl'] = true;
      const wb: XLSX.WorkBook = XLSX.utils.book_new();
      XLSX.utils.book_append_sheet(wb, ws, 'Sheet1');
      XLSX.writeFile(wb, `${this.fileName || 'جدول'}.xlsx`);
    }
    else if (this.exportType === 'pdf') {
      const table = document.querySelector('table') as HTMLElement;
      if (!table) return;
      const clonedTable = table.cloneNode(true) as HTMLElement;
      const headerRow = clonedTable.querySelector("thead tr");
      if (headerRow && headerRow.lastElementChild) {
        headerRow.lastElementChild.remove();
      }
      clonedTable.querySelectorAll("tbody tr").forEach((row) => {
        if (row.lastElementChild) {
          row.lastElementChild.remove();
        }
      });
    
      const container = document.createElement('div');
      container.style.direction = "rtl";
      container.style.textAlign = "center";
      container.style.position = "relative";
      container.style.minHeight = "100%";
    
      const header = document.createElement('div');
      header.style.display = "flex";
      header.style.justifyContent = "space-between";
      header.style.alignItems = "center";
      header.style.marginBottom = "20px";
    
      const companyLogo = document.createElement('img');
      companyLogo.src = 'assets/vendors/imgs/companylogo.png';
      companyLogo.style.width = "140px";
    
      const titleDiv = document.createElement('div');
      titleDiv.innerText = this.fileName || "جدول";
      titleDiv.style.flex = "1";
      titleDiv.style.textAlign = "center";
      titleDiv.style.fontSize = "18px";
      titleDiv.style.fontWeight = "bold";
    
      const systemLogo = document.createElement('img');
      systemLogo.src = 'assets/vendors/imgs/SystemLogo.png';
      systemLogo.style.width = "140px";
    
      header.appendChild(companyLogo);
      // header.appendChild(titleDiv);
      header.appendChild(systemLogo);
    
      clonedTable.style.width = "100%";
      clonedTable.style.borderCollapse = "collapse";
    
      container.appendChild(header);
      container.appendChild(clonedTable);
    
      const footer = document.createElement('div');
      footer.style.marginTop = "30px";
      footer.style.fontSize = "14px";
      footer.style.display = "flex";
      footer.style.justifyContent = "space-between";

      const userDiv = document.createElement('div');
      userDiv.innerText = `المستخدم: ${this.userName || '---'}`;

      const dateDiv = document.createElement('div');
      dateDiv.innerText = this.today;

      footer.appendChild(userDiv);
      footer.appendChild(dateDiv);
      container.appendChild(footer);
    
      const opt: any = {
        margin: 10,
        filename: this.fileName + ".pdf",
        image: { type: "jpeg", quality: 0.98 },
        html2canvas: { scale: 2 },
        jsPDF: { unit: "mm", format: "a4", orientation: "landscape" }
      };
    
      html2pdf().from(container).set(opt).save();
    }
    
  }
}
