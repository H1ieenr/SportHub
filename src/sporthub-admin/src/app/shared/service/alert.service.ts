import { Injectable, signal } from '@angular/core';
import { AppAlert, AlertType } from '../../core/models/common/alert.model';

@Injectable({
  providedIn: 'root'
})
export class AlertService {
  readonly alert = signal<AppAlert | null>(null);

  private timer?: ReturnType<typeof setTimeout>;

  show(type: AlertType, message: string, duration = 4000): void {
    if (this.timer) {
      clearTimeout(this.timer);
    }

    this.alert.set({ type, message });

    this.timer = setTimeout(() => {
      this.clear();
    }, duration);
  }

  success(message: string): void {
    this.show('success', message);
  }

  error(message: string): void {
    this.show('danger', message);
  }

  warning(message: string): void {
    this.show('warning', message);
  }

  clear(): void {
    if (this.timer) {
      clearTimeout(this.timer);
    }

    this.timer = undefined;
    this.alert.set(null);
  }
}