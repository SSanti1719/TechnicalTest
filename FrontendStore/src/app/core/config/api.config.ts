import { environment } from '@environments/environment';

export const API_ENDPOINTS = {
  orders: `${environment.apiUrl}/Orders`,
  employees: `${environment.apiUrl}/Employees`,
  products: `${environment.apiUrl}/Products`,
  shippers: `${environment.apiUrl}/Shippers`,
  customers: `${environment.apiUrl}/Orders/predicted`
};
