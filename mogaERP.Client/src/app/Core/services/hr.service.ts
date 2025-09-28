import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { catchError, Observable, of } from 'rxjs';
import { signal } from '@angular/core';
import { environment } from '../../../env/environment';
import { Department } from '../models/system-settings/departments';
import { JobLevel } from '../models/hr/jobLevel';
import { JobType } from '../models/hr/JobType';
import { JobTitle } from '../models/hr/jobTitle';
import { Employee } from '../models/hr/employee';
@Injectable({
  providedIn: 'root'
})
export class HrService {
  private baseUrl = `${environment.baseUrl}`;
  loading = signal<boolean>(false);
  error = signal<string | null>(null);
  constructor(private http: HttpClient) { }
  // ===================================================== Departments ===============================================================
  getDepartments(
      searchTerm: string = '',
      sortDescending: boolean = true
    ): Observable<{ data: Department[]; totalCount: number }> {
      this.loading.set(true);
      const url = `${this.baseUrl}JobDepartments?SearchTerm=${searchTerm}&SortDescending=${sortDescending}`;
      return this.http.get<{ data: Department[]; totalCount: number }>(url).pipe(
        catchError(err => {
          console.error('Error fetching departments', err);
          return of({ data: [], totalCount: 0 });
        })
      );
    }  
  getDepartmentById(id: number) {
    return this.http.get<Department>(`${this.baseUrl}JobDepartments/${id}`);
  }
  addDepartment(department: Department) {
    return this.http.post<Department>(`${this.baseUrl}JobDepartments`, department);
  }
  updateDepartment(id: number, department: Department) {
    return this.http.put<Department>(`${this.baseUrl}JobDepartments/${id}`, department);
  }
  deleteDepartment(id: number) {
    return this.http.delete<Department>(`${this.baseUrl}JobDepartments/${id}`);
  }
  // ===================================================== Departments ===============================================================
  // ===================================================== Levels ===============================================================
  getLevels(
    searchTerm: string = '',
    sortDescending: boolean = true
  ): Observable<{ data: JobLevel[]; totalCount: number }> {
    this.loading.set(true);
    const url = `${this.baseUrl}JobLevels?SearchTerm=${searchTerm}&SortDescending=${sortDescending}`;
    return this.http.get<{ data: JobLevel[]; totalCount: number }>(url).pipe(
      catchError(err => {
        console.error('Error fetching levels', err);
        return of({ data: [], totalCount: 0 });
      })
    );
  }
  getLevelById(id: number) {
    return this.http.get<JobLevel>(`${this.baseUrl}JobLevels/${id}`);
  }
  addLevel(level: JobLevel) {
    return this.http.post<JobLevel>(`${this.baseUrl}JobLevels`, level);
  }
  updateLevel(id: number, level: JobLevel) {
    return this.http.put<JobLevel>(`${this.baseUrl}JobLevels/${id}`, level);
  }
  deleteLevel(id: number) {
    return this.http.delete<JobLevel>(`${this.baseUrl}JobLevels/${id}`);
  }
  // ===================================================== Levels ===============================================================
  // ===================================================== Types ===============================================================
  getTypes(
    searchTerm: string = '',
    sortDescending: boolean = true
  ): Observable<{ data: JobType[]; totalCount: number }> {
    this.loading.set(true);
    const url = `${this.baseUrl}JobTypes?SearchTerm=${searchTerm}&SortDescending=${sortDescending}`;
    return this.http.get<{ data: JobType[]; totalCount: number }>(url).pipe(
      catchError(err => {
        console.error('Error fetching types', err);
        return of({ data: [], totalCount: 0 });
      })
    );
  }
  getTypeById(id: number) {
    return this.http.get<JobType>(`${this.baseUrl}JobTypes/${id}`);
  }
  addType(type: JobType) {
    return this.http.post<JobType>(`${this.baseUrl}JobTypes`, type);
  }
  updateType(id: number, type: JobType) {
    return this.http.put<JobType>(`${this.baseUrl}JobTypes/${id}`, type);
  }
  deleteType(id: number) {
    return this.http.delete<JobType>(`${this.baseUrl}JobTypes/${id}`);
  }
  // ===================================================== Types ===============================================================
  // ===================================================== Titles ===============================================================
  getTitles(
    searchTerm: string = '',
    sortDescending: boolean = true
  ): Observable<{ data: JobTitle[]; totalCount: number }> {
    this.loading.set(true);
    const url = `${this.baseUrl}JobTitles?SearchTerm=${searchTerm}&SortDescending=${sortDescending}`;
    return this.http.get<{ data: JobTitle[]; totalCount: number }>(url).pipe(
      catchError(err => {
        console.error('Error fetching titles', err);
        return of({ data: [], totalCount: 0 });
      })
    );
  }
  getTitleById(id: number) {
    return this.http.get<JobTitle>(`${this.baseUrl}JobTitles/${id}`);
  }
  addTitle(title: JobTitle) {
    return this.http.post<JobTitle>(`${this.baseUrl}JobTitles`, title);
  }
  updateTitle(id: number, title: JobTitle) {
    return this.http.put<JobTitle>(`${this.baseUrl}JobTitles/${id}`, title);
  }
  deleteTitle(id: number) {
    return this.http.delete<JobTitle>(`${this.baseUrl}JobTitles/${id}`);
  }
  // ===================================================== Jobs ===============================================================
  getEmployees(
    page: number,
    pageSize: number,
    searchTerm: string = '',
    sortDescending: boolean = true
  ): Observable<{ data: Employee[]; totalCount: number }> {
    this.loading.set(true);
    const url = `${this.baseUrl}Staff?SearchTerm=${searchTerm}&SortDescending=${sortDescending}&Page=${page}&PageSize=${pageSize}`;
    return this.http.get<{ data: Employee[]; totalCount: number }>(url).pipe(
      catchError(err => {
        console.error('Error fetching employees', err);
        return of({ data: [], totalCount: 0 });
      })
    );
  }
  getEmployeeById(id: number) {
    return this.http.get<Employee>(`${this.baseUrl}Staff/${id}`);
  }
  addEmployee(employee: FormData) {
    return this.http.post<Employee>(`${this.baseUrl}Staff`, employee);
  }
  updateEmployee(id: number, employee: FormData) {
    return this.http.put<Employee>(`${this.baseUrl}Staff/${id}`, employee);
  }
  deleteEmployee(id: number) {
    return this.http.delete<Employee>(`${this.baseUrl}Staff/${id}`);
  }
}
