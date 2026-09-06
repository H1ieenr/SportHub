import { ChangeDetectionStrategy, Component, computed, input, output } from '@angular/core';

@Component({
  selector: 'app-pagination',
  standalone: true,
  templateUrl: './pagination.component.html',
  styleUrl: './pagination.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class PaginationComponent {
  // nhận trực tiếp field từ PagedResult<T>
  readonly pageNumber = input.required<number>();
  readonly pageSize = input.required<number>();
  readonly totalCount = input.required<number>();
  readonly totalPages = input.required<number>();
  readonly hasPreviousPage = input<boolean>(false);
  readonly hasNextPage = input<boolean>(false);

  readonly pageChange = output<number>();

  // ví dụ: 21–50 / 120
  readonly rangeText = computed(() => {
    const total = this.totalCount();
    if (total === 0) return '0 / 0';
    const start = (this.pageNumber() - 1) * this.pageSize() + 1;
    const end = Math.min(this.pageNumber() * this.pageSize(), total);
    return `${start}–${end} / ${total}`;
  });

  // danh sách số trang hiển thị, tối đa 5 nút quanh trang hiện tại
  readonly pages = computed(() => {
    const total = this.totalPages();
    const current = this.pageNumber();
    const delta = 2;
    const start = Math.max(1, current - delta);
    const end = Math.min(total, current + delta);
    const result: number[] = [];
    for (let i = start; i <= end; i++) {
      result.push(i);
    }
    return result;
  });

  onPrevious() {
    if (this.hasPreviousPage()) {
      this.pageChange.emit(this.pageNumber() - 1);
    }
  }

  onNext() {
    if (this.hasNextPage()) {
      this.pageChange.emit(this.pageNumber() + 1);
    }
  }

  onGoTo(page: number) {
    if (page !== this.pageNumber()) {
      this.pageChange.emit(page);
    }
  }
}