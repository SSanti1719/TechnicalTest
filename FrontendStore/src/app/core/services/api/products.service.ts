import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Product } from '@app/core/models';
import { API_ENDPOINTS } from '@app/core/config/api.config';

@Injectable({ providedIn: 'root' })
export class ProductsService {
  private readonly baseUrl = API_ENDPOINTS.products;

  constructor(private http: HttpClient) {}

  getAll(): Observable<Product[]> {
    return this.http.get<Product[]>(this.baseUrl);
  }
}
