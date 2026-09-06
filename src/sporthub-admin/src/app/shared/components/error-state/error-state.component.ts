import { ChangeDetectionStrategy, Component, input, output } from '@angular/core';

@Component({
  selector: 'app-error-state',
  standalone: true,
  templateUrl: './error-state.component.html',
  styleUrl: './error-state.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class ErrorStateComponent {
  readonly message = input<string>('Đã có lỗi xảy ra khi tải dữ liệu');
  readonly retry = output<void>();

  onRetryClick() {
    this.retry.emit();
  }
}