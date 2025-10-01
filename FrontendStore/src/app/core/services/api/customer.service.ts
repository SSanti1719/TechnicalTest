import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { CustomerOrderSummary } from '@app/core/models';
import { API_ENDPOINTS } from '@app/core/config/api.config';

@Injectable({ providedIn: 'root' })
export class CustomersService {
  private readonly baseUrl = API_ENDPOINTS.orders;

  constructor(private http: HttpClient) {}

  getOrderSummaries(): Observable<CustomerOrderSummary[]> {
    return this.http.get<CustomerOrderSummary[]>(`${this.baseUrl}/predicted`);
  }
}
