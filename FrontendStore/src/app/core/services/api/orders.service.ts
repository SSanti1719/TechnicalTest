import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { OrderDto } from '@app/core/models';
import { API_ENDPOINTS } from '@app/core/config/api.config';

@Injectable({ providedIn: 'root' })
export class OrdersService {
  private readonly baseUrl = API_ENDPOINTS.orders;

  constructor(private http: HttpClient) {}

  getPredicted(): Observable<any[]> {
    return this.http.get<any[]>(`${this.baseUrl}/predicted`);
  }

  getByCustomer(customerId: number): Observable<any[]> {
    return this.http.get<any[]>(`${this.baseUrl}/by-customer/${customerId}`);
  }

  create(order: OrderDto): Observable<any> {
    return this.http.post<any>(`${this.baseUrl}`, order);
  }
}
