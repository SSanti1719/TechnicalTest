import { Injectable } from '@angular/core';
import { MatSnackBar, MatSnackBarConfig } from '@angular/material/snack-bar';

@Injectable({
  providedIn: 'root'
})
export class NotificationService {
  constructor(private snackBar: MatSnackBar) {}

  success(message: string, action: string = 'Cerrar', duration = 3000): void {
    this.open(message, action, ['success-snackbar'], duration);
  }

  error(message: string, action: string = 'Cerrar', duration = 5000): void {
    this.open(message, action, ['error-snackbar'], duration);
  }

  info(message: string, action: string = 'OK', duration = 10000): void {
    this.open(message, action, ['info-snackbar'], duration);
  }

  private open(
    message: string,
    action: string,
    panelClass: string[],
    duration: number
  ): void {
    const config: MatSnackBarConfig = {
      duration,
      panelClass,
      horizontalPosition: 'center',
      verticalPosition: 'bottom'
    };
    this.snackBar.open(message, action, config);
  }
}
