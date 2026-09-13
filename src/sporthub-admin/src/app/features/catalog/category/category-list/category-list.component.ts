import { ChangeDetectionStrategy, Component, ViewChild, inject, signal } from '@angular/core';
import { PageHeaderComponent } from '../../../../shared/components/page-header/page-header.component';
import { LoadingComponent } from '../../../../shared/components/loading/loading.component';
import { EmptyStateComponent } from '../../../../shared/components/empty-state/empty-state.component';
import { ErrorStateComponent } from '../../../../shared/components/error-state/error-state.component';
import { PaginationComponent } from '../../../../shared/components/pagination/pagination.component';
import { ConfirmDialogService } from '../../../../shared/service/confirm-dialog.service';
import { AlertService } from '../../../../shared/service/alert.service';
import { CategoryService } from '../../../../core/services/catalog/category/category.service';
import {
  initialListPageState,
  applyPagedResult,
  ListPageState,
} from '../../../../core/models/common/list-page-state.model';
import { PagedResult } from '../../../../core/models/common/api-response.model';
import { Category, CategoryNode } from '../../../../core/models/catalog/category/category.model';
//import { CategoryFormComponent, CategoryFormMode } from '../category-form/category-form.component';
import { NgbOffcanvas } from '@ng-bootstrap/ng-bootstrap/offcanvas';

import { Subject } from 'rxjs';
import { debounceTime, distinctUntilChanged } from 'rxjs/operators';

import { NgbDropdown, NgbDropdownModule } from '@ng-bootstrap/ng-bootstrap';
import { IconDirective, IconService } from '@ant-design/icons-angular';
import {
  PlusCircleFill,
  EditFill,
  DeleteFill,
  EyeFill,
  FilterOutline,
  MinusSquareOutline,
  PlusSquareOutline,
  LoadingOutline,
} from '@ant-design/icons-angular/icons';
import { ImagePreviewService } from '../../../../shared/service/image-preview.service';
import {
  CategoryFormComponent,
  CategoryFormMode,
  CategoryFormResult,
} from '../category-form/category-form.component';

@Component({
  selector: 'app-category-list',
  standalone: true,
  imports: [
    PageHeaderComponent,
    LoadingComponent,
    EmptyStateComponent,
    ErrorStateComponent,
    PaginationComponent,
    IconDirective,
    NgbDropdownModule,
  ],
  templateUrl: './category-list.component.html',
  styleUrl: './category-list.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class CategoryListComponent {
  @ViewChild('filterDropdown') filterDropdown?: NgbDropdown;

  private iconService = inject(IconService);
  private categoryService = inject(CategoryService);
  private confirmDialogService = inject(ConfirmDialogService);
  private alertService = inject(AlertService);
  private offcanvasService = inject(NgbOffcanvas);
  private imagePreviewService = inject(ImagePreviewService);
  private searchSubject = new Subject<string>();
  private activeFilter: boolean | undefined = undefined;
  draftActiveFilter: boolean | undefined = undefined;
  parentOptions = signal<Category[]>([]);

  readonly state = signal<ListPageState<CategoryNode>>(initialListPageState<CategoryNode>());

  private expandedChildrenIds = new Map<number, number[]>();

  readonly loadingExpandIds = signal<Set<number>>(new Set());

  constructor() {
    this.loadData();
    this.iconService.addIcon(
      ...[
        PlusCircleFill,
        EditFill,
        DeleteFill,
        EyeFill,
        FilterOutline,
        MinusSquareOutline,
        PlusSquareOutline,
        LoadingOutline,
      ],
    );

    this.searchSubject.pipe(debounceTime(400), distinctUntilChanged()).subscribe((text) => {
      this.state.update((s) => ({ ...s, searchText: text, pageNumber: 1 }));
      this.loadData();
    });
  }

  onParentFilterChange(event: Event) {
    const value = (event.target as HTMLSelectElement).value;
  }

  onFilterDropdownOpen(isOpen: boolean) {
    if (isOpen) {
      this.draftActiveFilter = this.activeFilter;
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
    this.loadDataInternal();
  }
  private loadDataInternal(onDone?: () => void) {
    const current = this.state();
    this.state.set({ ...current, loading: true, errorMessage: null });
    this.expandedChildrenIds.clear();

    this.categoryService
      .getList({
        page_number: current.pageNumber,
        page_size: current.pageSize,
        search_text: current.searchText || undefined,
        active: this.activeFilter,
        parent_id: null,
      })
      .subscribe({
        next: (result) => {
          const mapped: PagedResult<CategoryNode> = {
            ...result,
            results: result.results.map((c, i) => this.toNode(c, 0, null, i)),
          };

          this.state.update((s) => ({
            ...applyPagedResult(s, mapped),
            loading: false,
          }));
          onDone?.();
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

  // NEW: reload nhưng cố mở lại đúng các node đang mở trước đó + node cha vừa bị tác động
  private reloadPreservingExpand(affectedParentId?: number | null) {
    const expandedSnapshot = this.state()
      .items.filter((i) => i.expanded)
      .map((i) => ({ id: i.id, level: i.level }))
      .sort((a, b) => a.level - b.level); // mở từ cấp nông -> sâu, để cha có trước khi mở con

    this.loadDataInternal(() => {
      this.reExpandChain(expandedSnapshot, 0, affectedParentId);
    });
  }

  private reExpandChain(
    snapshot: { id: number; level: number }[],
    index: number,
    affectedParentId?: number | null,
  ) {
    if (index >= snapshot.length) {
      // Trường hợp vừa tạo con đầu tiên trong 1 cha CHƯA từng mở trước đó -> mở luôn cha đó cho thấy con mới
      if (affectedParentId) {
        const node = this.state().items.find((i) => i.id === affectedParentId);
        if (node && !node.expanded) this.expandNode(node);
      }
      return;
    }

    const target = snapshot[index];
    const node = this.state().items.find((i) => i.id === target.id);
    if (!node) {
      this.reExpandChain(snapshot, index + 1, affectedParentId);
      return;
    }
    this.expandNode(node, () => this.reExpandChain(snapshot, index + 1, affectedParentId));
  }

  private toNode(
    c: Category,
    level: number,
    parentName: string | null,
    groupIndex: number,
  ): CategoryNode {
    return {
      ...c,
      level,
      has_children: true, // chưa biết chắc trước khi expand lần đầu — xem ghi chú bên dưới
      expanded: false,
      parent_name: parentName,
      groupIndex,
    };
  }

  isExpandLoading(id: number): boolean {
    return this.loadingExpandIds().has(id);
  }

  onToggleExpand(node: CategoryNode) {
    if (this.isExpandLoading(node.id)) return;
    if (node.expanded) {
      this.collapseNode(node);
    } else {
      this.expandNode(node);
    }
  }

  private expandNode(node: CategoryNode, onDone?: () => void) {
    this.loadingExpandIds.update((set) => new Set(set).add(node.id));

    this.categoryService.getAll({ parent_id: node.id }).subscribe({
      next: (children) => {
        const childNodes = children.map((c, i) => this.toNode(c, node.level + 1, node.name, i));
        this.expandedChildrenIds.set(
          node.id,
          childNodes.map((c) => c.id),
        );

        this.state.update((s) => {
          const idx = s.items.findIndex((i) => i.id === node.id);
          if (idx === -1) return s;
          const items = [...s.items];
          items[idx] = { ...items[idx], expanded: true, has_children: childNodes.length > 0 };
          items.splice(idx + 1, 0, ...childNodes);
          return { ...s, items };
        });

        this.loadingExpandIds.update((set) => {
          const next = new Set(set);
          next.delete(node.id);
          return next;
        });
        onDone?.();
      },
      error: (err) => {
        this.loadingExpandIds.update((set) => {
          const next = new Set(set);
          next.delete(node.id);
          return next;
        });
        this.alertService.error(err?.error?.message || 'Không tải được danh mục con');
        onDone?.();
      },
    });
  }

  private collapseNode(node: CategoryNode) {
    const idsToRemove = this.collectDescendantIds(node.id);

    this.state.update((s) => {
      const idx = s.items.findIndex((i) => i.id === node.id);
      if (idx === -1) return s;
      const items = [...s.items];
      items[idx] = { ...items[idx], expanded: false };
      return { ...s, items: items.filter((i) => !idsToRemove.has(i.id)) };
    });

    idsToRemove.forEach((id) => this.expandedChildrenIds.delete(id));
    this.expandedChildrenIds.delete(node.id);
  }

  private collectDescendantIds(nodeId: number): Set<number> {
    const result = new Set<number>();
    const stack = [...(this.expandedChildrenIds.get(nodeId) ?? [])];
    while (stack.length) {
      const id = stack.pop()!;
      result.add(id);
      const children = this.expandedChildrenIds.get(id);
      if (children) stack.push(...children);
    }
    return result;
  }

  onPageChange(page: number) {
    this.state.update((s) => ({ ...s, pageNumber: page }));
    this.loadData();
  }

  onRetry() {
    this.loadData();
  }

  private openForm(
    mode: CategoryFormMode,
    category: Category | null = null,
    presetParentId: number | null = null,
  ) {
    this.categoryService.getAll({}).subscribe({
      next: (all) => {
        const excludedIds = category
          ? this.collectSelfAndDescendantIds(all, category.id)
          : new Set<number>();
        const options = all.filter((c) => !excludedIds.has(c.id));
        this.launchFormOffcanvas(mode, category, presetParentId, options);
      },
      error: () => {
        this.launchFormOffcanvas(mode, category, presetParentId, []);
      },
    });
  }

  private launchFormOffcanvas(
    mode: CategoryFormMode,
    category: Category | null,
    presetParentId: number | null,
    parentOptions: Category[],
  ) {
    const ref = this.offcanvasService.open(CategoryFormComponent, {
      position: 'end',
      panelClass: 'app-offcanvas-half',
      beforeDismiss: () => {
        const instance = ref.componentInstance as CategoryFormComponent;
        return !instance.submitting();
      },
    });

    const instance = ref.componentInstance as CategoryFormComponent;
    instance.mode = mode;
    instance.categoryId = category?.id ?? null;
    instance.category = category;
    instance.presetParentId = presetParentId;
    instance.parentOptions = parentOptions; // FIX: giờ đã có dữ liệu thật

    ref.result.then(
      (result: CategoryFormResult | undefined) => {
        if (result?.changed) {
          this.reloadPreservingExpand(result.affectedParentId ?? undefined); // FIX: dùng bản giữ trạng thái mở
        }
        if (result?.createChildParentId) {
          setTimeout(() => this.openForm('create', null, result.createChildParentId!));
        }
      },
      () => {},
    );
  }

  private collectSelfAndDescendantIds(all: Category[], rootId: number): Set<number> {
    const result = new Set<number>([rootId]);
    const stack = [rootId];
    while (stack.length) {
      const id = stack.pop()!;
      all
        .filter((c) => c.parent_id === id)
        .forEach((child) => {
          if (!result.has(child.id)) {
            result.add(child.id);
            stack.push(child.id);
          }
        });
    }
    return result;
  }

  onAddClick() {
    this.openForm('create');
  }

  onEditClick(category: CategoryNode) {
    this.openForm('edit', category);
  }

  onViewClick(category: CategoryNode) {
    this.openForm('view', category);
  }

  onImageClick(category: CategoryNode) {
    if (!category.image_url) return;
    this.imagePreviewService.open(category.image_url, category.name);
  }

  async onDeleteClick(category: CategoryNode) {
    const confirmed = await this.confirmDialogService.confirm({
      message: `Bạn có chắc muốn xoá "${category.name}"?`,
    });
    if (!confirmed) return;

    this.categoryService.delete(category.id).subscribe({
      next: () => {
        this.alertService.success('Xoá thành công');
        this.loadData();
      },
      error: (err) => {
        this.alertService.error(err?.error?.message || 'Xoá thất bại');
      },
    });
  }

  onToggleActive(category: CategoryNode) {
    const previousValue = category.is_active;

    this.state.update((s) => ({
      ...s,
      items: s.items.map((c) => (c.id === category.id ? { ...c, is_active: !previousValue } : c)),
    }));

    this.categoryService.toggleActive(category.id).subscribe({
      next: (res) => {
        if (res.is_success) {
          this.alertService.success(res.message || 'Cập nhật trạng thái thành công');
        }
      },
      error: (err) => {
        this.state.update((s) => ({
          ...s,
          items: s.items.map((c) =>
            c.id === category.id ? { ...c, is_active: previousValue } : c,
          ),
        }));
        this.alertService.error(err?.error?.message || 'Cập nhật trạng thái thất bại');
      },
    });
  }
}
