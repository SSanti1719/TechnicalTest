import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Employee } from '@app/core/models';
import { API_ENDPOINTS } from '@app/core/config/api.config';

@Injectable({ providedIn: 'root' })
export class EmployeesService {
  private readonly baseUrl = API_ENDPOINTS.employees;

  constructor(private http: HttpClient) {}

  getAll(): Observable<Employee[]> {
    return this.http.get<Employee[]>(this.baseUrl);
  }
}
