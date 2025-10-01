import { Component, Inject, OnInit, OnDestroy } from '@angular/core';
import {
  MAT_DIALOG_DATA,
  MatDialogRef,
  MatDialogModule,
} from '@angular/material/dialog';
import { CommonModule } from '@angular/common';
import {
  ReactiveFormsModule,
  FormBuilder,
  Validators,
  FormGroup,
} from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatDatepickerModule } from '@angular/material/datepicker';
import {
  MatNativeDateModule,
  provideNativeDateAdapter,
} from '@angular/material/core';
import { MatSelectModule } from '@angular/material/select';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { OrderDto, Employee, Product, Shipper } from '@core/models';
import { Subject, takeUntil } from 'rxjs';
import { OrdersService } from '@app/core/services/api/orders.service';
import { EmployeesService } from '@app/core/services/api/employees.service';
import { ProductsService } from '@app/core/services/api/products.service';
import { ShippersService } from '@app/core/services/api/shippers.service';
import { NotificationService } from '@app/shared/services/notification.service';

@Component({
  selector: 'app-new-order-dialog',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatDialogModule,
    MatFormFieldModule,
    MatInputModule,
    MatDatepickerModule,
    MatNativeDateModule,
    MatSelectModule,
    MatButtonModule,
    MatIconModule,
  ],
  providers: [provideNativeDateAdapter()],
  templateUrl: './new-order-dialog.component.html',
  styleUrls: ['./new-order-dialog.component.css'],
})
export class NewOrderDialogComponent implements OnInit, OnDestroy {
  orderForm: FormGroup;

  employees: Employee[] = [];
  shippers: Shipper[] = [];
  products: Product[] = [];

  private destroy$ = new Subject<void>();

  constructor(
    private fb: FormBuilder,
    private ordersService: OrdersService,
    private employeesService: EmployeesService,
    private productsService: ProductsService,
    private shippersService: ShippersService,
    private notification: NotificationService,
    public dialogRef: MatDialogRef<NewOrderDialogComponent>,
    @Inject(MAT_DIALOG_DATA)
    public data: { customerId: number; customerName: string }
  ) {
    this.orderForm = this.fb.group({
      employee: ['', Validators.required],
      shipper: ['', Validators.required],
      shipName: ['', Validators.required],
      shipAddress: ['', Validators.required],
      shipCity: ['', Validators.required],
      shipRegion: [''],
      shipPostalCode: [''],
      shipCountry: ['', Validators.required],
      orderDate: ['', Validators.required],
      requiredDate: ['', Validators.required],
      shippedDate: [''],
      freight: [0, Validators.required],
      product: ['', Validators.required],
      unitPrice: [0, Validators.required],
      quantity: [1, Validators.required],
      discount: [0],
    });
  }

  ngOnInit(): void {
    this.employeesService
      .getAll()
      .pipe(takeUntil(this.destroy$))
      .subscribe((res) => (this.employees = res));

    this.shippersService
      .getAll()
      .pipe(takeUntil(this.destroy$))
      .subscribe((res) => (this.shippers = res));

    this.productsService
      .getAll()
      .pipe(takeUntil(this.destroy$))
      .subscribe((res) => (this.products = res));
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  save(): void {
    if (this.orderForm.valid) {
      const order = this.mapFormToOrderDto();
      this.ordersService.create(order).subscribe({
        next: (res) => {
          console.log('Order created successfully', res);
          this.dialogRef.close(res);
          this.notification.success('Order created successfully.');
        },
        error: (err) => {
          this.notification.error('Error creating order.');
          console.error('Error creating order', err);
        },
      });
    }
  }

  close(): void {
    this.dialogRef.close();
  }

  private mapFormToOrderDto(): OrderDto {
    const formValue = this.orderForm.value;
    return {
      custId: this.data.customerId,
      empId: formValue.employee,
      shipperId: formValue.shipper,
      shipName: formValue.shipName,
      shipAddress: formValue.shipAddress,
      shipCity: formValue.shipCity,
      shipRegion: formValue.shipRegion,
      shipPostalCode: formValue.shipPostalCode,
      shipCountry: formValue.shipCountry,
      orderDate: formValue.orderDate,
      requiredDate: formValue.requiredDate,
      shippedDate: formValue.shippedDate,
      freight: formValue.freight,
      detail: {
        productId: formValue.product,
        unitPrice: formValue.unitPrice,
        qty: formValue.quantity,
        discount: formValue.discount / 100,
      },
    };
  }
}
