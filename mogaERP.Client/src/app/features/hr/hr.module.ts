import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { HrRoutingModule } from './hr-routing.module';
import { EmployeesComponent } from './employees/employees.component';
import { DepartmentsComponent } from './departments/departments.component';
import { JobLevelsComponent } from './job-levels/job-levels.component';
import { JobCategoriesComponent } from './job-categories/job-categories.component';
import { JobsComponent } from './jobs/jobs.component';
import { SharedModule } from '../../shared/shared.module';
import { AddEmployeeComponent } from './add-employee/add-employee.component';


@NgModule({
  declarations: [
    EmployeesComponent,
    DepartmentsComponent,
    JobLevelsComponent,
    JobCategoriesComponent,
    JobsComponent,
    AddEmployeeComponent
  ],
  imports: [
    CommonModule,
    HrRoutingModule,
    SharedModule
  ]
})
export class HrModule { }
