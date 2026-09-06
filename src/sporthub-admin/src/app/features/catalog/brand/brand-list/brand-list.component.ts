import { ChangeDetectionStrategy, Component, inject, signal } from '@angular/core';
import { PageHeaderComponent } from '../../../../shared/components/page-header/page-header.component';
import { LoadingComponent } from '../../../../shared/components/loading/loading.component';
import { EmptyStateComponent } from '../../../../shared/components/empty-state/empty-state.component';
import { ErrorStateComponent } from '../../../../shared/components/error-state/error-state.component';
import { PaginationComponent } from '../../../../shared/components/pagination/pagination.component';
import { ConfirmDialogService } from '../../../../shared/service/confirm-dialog.service';
import { AlertService } from '../../../../shared/service/alert.service';
import { BrandService } from '../../../../core/services/catalog/brand/brand.service';
import {
  initialListPageState,
  applyPagedResult,
  ListPageState,
} from '../../../../core/models/common/list-page-state.model';
import { Brand } from '../../../../core/models/catalog/brand/brand.model';
import { Router } from '@angular/router';
// icon
import { IconService } from '@ant-design/icons-angular';
import { PlusCircleFill, EditFill, DeleteFill } from '@ant-design/icons-angular/icons';

@Component({
  selector: 'app-brand-list',
  standalone: true,
  imports: [
    PageHeaderComponent,
    LoadingComponent,
    EmptyStateComponent,
    ErrorStateComponent,
    PaginationComponent,
  ],
  templateUrl: './brand-list.component.html',
  styleUrl: './brand-list.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class BrandListComponent {
  private iconService = inject(IconService);
  private brandService = inject(BrandService);
  private confirmDialogService = inject(ConfirmDialogService);
  private alertService = inject(AlertService);
  private router = inject(Router);

  readonly state = signal<ListPageState<Brand>>(initialListPageState<Brand>());

  constructor() {
    this.loadData();
    this.iconService.addIcon(...[PlusCircleFill, EditFill, DeleteFill]);
  }

  loadData() {
    const current = this.state();
    this.state.set({ ...current, loading: true, errorMessage: null });

    this.brandService
      .getList({
        page_number: current.pageNumber,
        page_size: current.pageSize,
      })
      .subscribe({
        next: (result) => {
          this.state.update((s) => ({
            ...applyPagedResult(s, result),
            loading: false,
          }));
        },
        error: (err) => {
          this.state.update((s) => ({
            ...s,
            loading: false,
            errorMessage: err?.error?.message || 'Đã có lỗi xảy ra khi tải dữ liệu',
          }));
        },
      });
  }

  onPageChange(page: number) {
    this.state.update((s) => ({ ...s, pageNumber: page }));
    this.loadData();
  }

  onRetry() {
    this.loadData();
  }

  onEditClick(brand: Brand) {
    this.router.navigate(['/brand/edit', brand.id]);
  }

  onAddClick() {
    this.router.navigate(['/brand/create']);
  }

  async onDeleteClick(brand: Brand) {
    const confirmed = await this.confirmDialogService.confirm({
      message: `Bạn có chắc muốn xoá "${brand.name}"?`,
    });
    if (!confirmed) return;

    this.brandService.delete(brand.id).subscribe({
      next: () => {
        this.alertService.success('Xoá thành công');
        this.loadData();
      },
      error: (err) => {
        this.alertService.error(err?.error?.message || 'Xoá thất bại');
      },
    });
  }

  onToggleActive(brand: Brand) {
    const previousValue = brand.is_active;

    this.state.update((s) => ({
      ...s,
      items: s.items.map((b) => (b.id === brand.id ? { ...b, is_active: !previousValue } : b)),
    }));

    this.brandService.toggleActive(brand.id).subscribe({
      next: (res) => {
        if (res.is_success) {
          this.alertService.success(res.message || 'Cập nhật trạng thái thành công');
        }
      },
      error: (err) => {
        this.state.update((s) => ({
          ...s,
          items: s.items.map((b) => (b.id === brand.id ? { ...b, is_active: previousValue } : b)),
        }));
        this.alertService.error(err?.error?.message || 'Cập nhật trạng thái thất bại');
      },
    });
  }
}
