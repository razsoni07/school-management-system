import { Injectable } from '@angular/core';
import { MatSnackBar, MatSnackBarConfig } from '@angular/material/snack-bar';

@Injectable({
  providedIn: 'root',
})
export class SnackbarService {
  
  constructor(private snackBar: MatSnackBar) {}

  success(message: string) {
    this.open(message, ['snackbar-success']);
  }

  error(message: string) {
    this.open(message, ['snackbar-error']);
  }

  warning(message: string) {
    this.open(message, ['snackbar-warning']);
  }

  info(message: string) {
    this.open(message, ['snackbar-info']);
  }

  private open(message: string, panelClass: string[]) {
    const config: MatSnackBarConfig = {
      duration: 3000,
      horizontalPosition: 'right',
      verticalPosition: 'top',
      panelClass: panelClass
    };

    this.snackBar.open(message, 'Close', config);
  }
}
