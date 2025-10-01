import { Component, Inject, OnInit, OnDestroy, ViewChild } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatTableDataSource, MatTableModule } from '@angular/material/table';
import {
  MAT_DIALOG_DATA,
  MatDialogModule,
  MatDialogRef,
} from '@angular/material/dialog';
import { MatPaginator, MatPaginatorModule } from '@angular/material/paginator';
import { MatButtonModule } from '@angular/material/button';
import { OrdersService } from '@core/services/api/orders.service';
import { Subject, takeUntil } from 'rxjs';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { NotificationService } from '@app/shared/services/notification.service';
import { DataLoaderComponent } from "@app/shared/components/data-loader/data-loader.component";
interface Order {
  orderId: number;
  requiredDate: string;
  shippedDate: string;
  shipName: string;
  shipAddress: string;
  shipCity: string;
}

@Component({
  selector: 'app-order-dialog',
  standalone: true,
  imports: [
    CommonModule,
    MatDialogModule,
    MatTableModule,
    MatPaginatorModule,
    MatButtonModule,
    MatProgressSpinnerModule,
    DataLoaderComponent
],
  templateUrl: './order-dialog.component.html',
  styleUrls: ['./order-dialog.component.css'],
})
export class OrderDialogComponent implements OnInit, OnDestroy {
  displayedColumns: string[] = [
    'orderId',
    'requiredDate',
    'shippedDate',
    'shipName',
    'shipAddress',
    'shipCity',
  ];

  dataSource = new MatTableDataSource<Order>();
  loading = true;
  hasError = false;
  private destroy$ = new Subject<void>();

  @ViewChild(MatPaginator) paginator!: MatPaginator;

  constructor(
    private ordersService: OrdersService,
    private notification: NotificationService,
    public dialogRef: MatDialogRef<OrderDialogComponent>,
    @Inject(MAT_DIALOG_DATA)
    public data: { customerId: number; customerName: string }
  ) {}

  ngOnInit(): void {
    this.ordersService
      .getByCustomer(this.data.customerId)
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: (res) => {
          this.dataSource.data = res;
          this.dataSource.paginator = this.paginator;
          this.loading = false;
          this.notification.info(
            'Orders loaded successfully.'
          );
        },
        error: () => {
          this.loading = false;
          this.hasError = true;
          this.notification.error(
            'Failed to load orders. Please try again.'
          );
        },
      });
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  close(): void {
    this.dialogRef.close();
  }
}
