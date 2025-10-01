import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Shipper } from '@app/core/models';
import { API_ENDPOINTS } from '@app/core/config/api.config';

@Injectable({ providedIn: 'root' })
export class ShippersService {
  private readonly baseUrl = API_ENDPOINTS.shippers;

  constructor(private http: HttpClient) {}

  getAll(): Observable<Shipper[]> {
    return this.http.get<Shipper[]>(this.baseUrl);
  }
}
