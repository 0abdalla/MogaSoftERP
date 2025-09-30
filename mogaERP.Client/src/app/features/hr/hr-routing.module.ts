import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { EmployeesComponent } from './employees/employees.component';
import { DepartmentsComponent } from './departments/departments.component';
import { JobLevelsComponent } from './job-levels/job-levels.component';
import { JobCategoriesComponent } from './job-categories/job-categories.component';
import { JobsComponent } from './jobs/jobs.component';
import { AddEmployeeComponent } from './add-employee/add-employee.component';

const routes: Routes = [
  {
    path: 'employees',
    component: EmployeesComponent
  },
  {
    path: 'departments',
    component: DepartmentsComponent
  },
  {
    path: 'job-levels',
    component: JobLevelsComponent
  },
  {
    path: 'job-categories',
    component: JobCategoriesComponent
  },
  {
    path: 'job-management',
    component: JobsComponent
  },
  {
    path: 'add-employee',
    component: AddEmployeeComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class HrRoutingModule { }
