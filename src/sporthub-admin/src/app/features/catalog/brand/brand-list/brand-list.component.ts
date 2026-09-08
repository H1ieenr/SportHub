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
import { BrandFormComponent, BrandFormMode } from '../brand-form/brand-form.component';
import { NgbOffcanvas } from '@ng-bootstrap/ng-bootstrap/offcanvas';

import { Subject } from 'rxjs';
import { debounceTime, distinctUntilChanged } from 'rxjs/operators';

import { ViewChild } from '@angular/core';
import { NgbDropdown, NgbDropdownModule } from '@ng-bootstrap/ng-bootstrap';
// icon
import { IconDirective, IconService } from '@ant-design/icons-angular';
import { PlusCircleFill, EditFill, DeleteFill, EyeFill, FilterOutline } from '@ant-design/icons-angular/icons';
import { ImagePreviewService } from '../../../../shared/service/image-preview.service';

@Component({
  selector: 'app-brand-list',
  standalone: true,
  imports: [
    PageHeaderComponent,
    LoadingComponent,
    EmptyStateComponent,
    ErrorStateComponent,
    PaginationComponent,
    IconDirective,
    NgbDropdownModule
  ],
  templateUrl: './brand-list.component.html',
  styleUrl: './brand-list.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class BrandListComponent {
  @ViewChild('filterDropdown') filterDropdown?: NgbDropdown;

  private iconService = inject(IconService);
  private brandService = inject(BrandService);
  private confirmDialogService = inject(ConfirmDialogService);
  private alertService = inject(AlertService);
  private offcanvasService = inject(NgbOffcanvas);
  private imagePreviewService = inject(ImagePreviewService);
  private searchSubject = new Subject<string>();
  private activeFilter: boolean | undefined = undefined;
  draftActiveFilter: boolean | undefined = undefined;

  readonly state = signal<ListPageState<Brand>>(initialListPageState<Brand>());

  constructor() {
    this.loadData();
    this.iconService.addIcon(...[PlusCircleFill, EditFill, DeleteFill, EyeFill, FilterOutline]);

    this.searchSubject.pipe(debounceTime(400), distinctUntilChanged()).subscribe((text) => {
      this.state.update((s) => ({ ...s, searchText: text, pageNumber: 1 }));
      this.loadData();
    });
  }

  onFilterDropdownOpen(isOpen: boolean) {
    if (isOpen) {
      this.draftActiveFilter = this.activeFilter;
      console.log(this.draftActiveFilter)
      console.log(this.activeFilter)
    }
  }

  onFilterCancel() {
    this.draftActiveFilter = this.activeFilter;
    this.filterDropdown?.close();
  }

  onFilterApply() {
    this.activeFilter = this.draftActiveFilter;
    this.state.update((s) => ({ ...s, pageNumber: 1 }));
    this.loadData();
    this.filterDropdown?.close();
  }

  onSearchInput(value: string) {
    this.searchSubject.next(value);
  }

  ngOnDestroy() {
    this.searchSubject.complete();
  }

  loadData() {
    const current = this.state();
    this.state.set({ ...current, loading: true, errorMessage: null });

    this.brandService
      .getList({
        page_number: current.pageNumber,
        page_size: current.pageSize,
        search_text: current.searchText || undefined,
        active: this.activeFilter
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

  private openForm(mode: BrandFormMode, brandId: number | null = null) {
    const ref = this.offcanvasService.open(BrandFormComponent, {
      position: 'end',
      panelClass: 'app-offcanvas-half',
      //backdrop: false
      beforeDismiss: () => {
        const instance = ref.componentInstance as BrandFormComponent;
        return !instance.submitting();
      }
    });

    const instance = ref.componentInstance as BrandFormComponent;
    instance.mode = mode;
    instance.brandId = brandId;

    ref.result.then(
      (changed) => {
        if (changed) this.loadData();
      },
      () => { }
    );
  }

  onAddClick() {
    this.openForm('create');
  }

  onEditClick(brand: Brand) {
    this.openForm('edit', brand.id);
  }

  onViewClick(brand: Brand) {
    this.openForm('view', brand.id);
  }

  onImageClick(brand: Brand) {
    if (!brand.logo_url) return;
    this.imagePreviewService.open(brand.logo_url, brand.name);
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
