import { Component, OnInit, ViewChild, OnDestroy } from '@angular/core';
import { MatPaginator, MatPaginatorModule } from '@angular/material/paginator';
import { MatSort, MatSortModule } from '@angular/material/sort';
import { MatTableDataSource, MatTableModule } from '@angular/material/table';
import { CustomerOrderSummary } from '@core/models/customers/customer-order-summary';
import { OrdersService } from '@core/services/api/orders.service';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { DatePipe } from '@angular/common';
import { MatDialog } from '@angular/material/dialog';
import { OrderDialogComponent } from '@features/customers/order-dialog/order-dialog.component';
import { NewOrderDialogComponent } from '@features/customers/new-order-dialog/new-order-dialog.component';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { Subject, takeUntil } from 'rxjs';
import { NotificationService } from '@app/shared/services/notification.service';
import { DataLoaderComponent } from "@app/shared/components/data-loader/data-loader.component";

@Component({
  selector: 'app-customer-orders',
  standalone: true,
  imports: [
    MatTableModule,
    MatPaginatorModule,
    MatSortModule,
    MatFormFieldModule,
    MatInputModule,
    DatePipe,
    MatCardModule,
    MatButtonModule,
    MatIconModule,
    DataLoaderComponent
],
  templateUrl: './customer-orders.component.html',
  styleUrls: ['./customer-orders.component.css'],
})
export class CustomerOrdersComponent implements OnInit, OnDestroy {
  displayedColumns: string[] = [
    'customerName',
    'lastOrderDate',
    'nextPredictedOrder',
    'actions',
    'newOrder',
  ];

  dataSource = new MatTableDataSource<CustomerOrderSummary>();
  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;

  private destroy$ = new Subject<void>();

  constructor(
    private ordersService: OrdersService,
    private dialog: MatDialog,
    private notification: NotificationService
  ) {}

  loading = false;
  hasError = false;

  ngOnInit(): void {
    this.loading = true;
    this.hasError = false;
    this.ordersService
      .getPredicted()
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: (data) => {
          this.dataSource.data = data;
          this.dataSource.paginator = this.paginator;
          this.dataSource.sort = this.sort;
          this.loading = false;
          this.notification.info(
            'Orders loaded successfully.'
          );
        },
        error: () => {
          this.loading = false;
          this.hasError = true;
          this.notification.error('Failed to load orders. Please try again.');
        },
      });
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  applyFilter(event: Event): void {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();
  }

  openOrdersDialog(customer: CustomerOrderSummary): void {
    this.dialog.open(OrderDialogComponent, {
      width: '90vw',
      height: '90vh',
      maxWidth: '90vw',
      panelClass: 'large-dialog',
      data: {
        customerId: customer.custId,
        customerName: customer.customerName,
      },
    });
  }

  openNewOrderDialog(customer: CustomerOrderSummary): void {
    this.dialog.open(NewOrderDialogComponent, {
      width: '80vw',
      height: '90vh',
      panelClass: 'large-dialog',
      data: {
        customerId: customer.custId,
        customerName: customer.customerName,
      },
    });
  }
}
