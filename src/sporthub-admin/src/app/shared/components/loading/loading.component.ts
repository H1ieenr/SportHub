import { ChangeDetectionStrategy, Component, input } from '@angular/core';

@Component({
  selector: 'app-loading',
  standalone: true,
  templateUrl: './loading.component.html',
  styleUrl: './loading.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class LoadingComponent {
  // text hiển thị dưới spinner, cho phép tuỳ biến theo từng trang
  readonly message = input<string>('Đang tải dữ liệu...');
}
