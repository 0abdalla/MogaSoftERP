import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { TaskMangmentRoutingModule } from './task-mangment-routing.module';
import { TasksComponent } from './tasks/tasks.component';
import { FormsModule } from '@angular/forms';


@NgModule({
  declarations: [
    TasksComponent
  ],
  imports: [
    CommonModule,
    TaskMangmentRoutingModule,
    FormsModule
  ]
})
export class TaskMangmentModule { }
