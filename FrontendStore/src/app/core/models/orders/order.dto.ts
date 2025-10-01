import { OrderDetailDto } from './order-detail.dto';

export interface OrderDto {
  custId: number;
  empId: number;
  shipperId: number;
  shipName: string;
  shipAddress: string;
  shipCity: string;
  shipRegion?: string;
  shipPostalCode?: string;
  shipCountry: string;
  orderDate: string;
  requiredDate: string;
  shippedDate?: string;
  freight: number;
  detail: OrderDetailDto;
}
