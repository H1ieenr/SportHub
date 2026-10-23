import {
  ChangeDetectionStrategy,
  Component,
  OnDestroy,
  computed,
  inject,
  output,
  signal,
} from '@angular/core';
import { Subject } from 'rxjs';
import { debounceTime, distinctUntilChanged } from 'rxjs/operators';

import { IconDirective, IconService } from '@ant-design/icons-angular';
import { PlusCircleFill, EditFill, DeleteFill, EyeFill } from '@ant-design/icons-angular/icons';

import { OptionDefinitionService } from '../../../../core/services/catalog/category/option-definition/option-definition.service';
import { OptionDefinition } from '../../../../core/models/catalog/category/option-definition/option-definition.model';
import { PageHeaderComponent } from '../../../../shared/components/page-header/page-header.component';
import { LoadingComponent } from '../../../../shared/components/loading/loading.component';
import { EmptyStateComponent } from '../../../../shared/components/empty-state/empty-state.component';
import { ErrorStateComponent } from '../../../../shared/components/error-state/error-state.component';
import { ConfirmDialogService } from '../../../../shared/service/confirm-dialog.service';
import { AlertService } from '../../../../shared/service/alert.service';

@Component({
  selector: 'app-option-definition-list',
  standalone: true,
  imports: [
    PageHeaderComponent,
    LoadingComponent,
    EmptyStateComponent,
    ErrorStateComponent,
    IconDirective,
  ],
  templateUrl: './option-definition-list.component.html',
  styleUrl: './option-definition-list.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class OptionDefinitionListComponent implements OnDestroy {
  private readonly service = inject(OptionDefinitionService);
  private readonly confirmDialogService = inject(ConfirmDialogService);
  private readonly alertService = inject(AlertService);
  private readonly iconService = inject(IconService);
  private readonly searchSubject = new Subject<string>();

  readonly items = signal<OptionDefinition[]>([]);
  readonly isLoading = signal<boolean>(false);
  readonly searchText = signal<string>('');
  readonly errorMessage = signal<string | null>(null);

  readonly totalCount = computed(() => this.items().length);
  readonly activeCount = computed(() => this.items().filter(item => item.is_active).length);
  readonly totalValueCount = computed(() => this.items().reduce((total, item) => total + item.option_values.length, 0));

  readonly createRequested = output<void>();
  readonly editRequested = output<OptionDefinition>();
  readonly viewRequested = output<OptionDefinition>();

  constructor() {
    this.iconService.addIcon(PlusCircleFill, EditFill, DeleteFill, EyeFill);
    this.searchSubject.pipe(debounceTime(300), distinctUntilChanged()).subscribe((searchText) => {
      this.searchText.set(searchText);
      this.loadData();
    });
    this.loadData();
  }

  onSearchInput(value: string){
    this.searchSubject.next(value.trim());
  }

  onCreateClick(): void{
    this.createRequested.emit();
  }

  onEditClick(item: OptionDefinition): void {
    this.editRequested.emit(item);
  }

  onViewClick(item: OptionDefinition): void {
    this.viewRequested.emit(item);
  }

  async onDeleteClick(item: OptionDefinition): Promise<void> {
    const confirmed = await this.confirmDialogService.confirm({
      title: 'Xác nhận xoá',
      message: `Bạn có chắc chắn muốn xoá option definition "${item.name}" không?`,
      confirmText: 'Xoá',
      cancelText: 'Huỷ',
    });
    if(!confirmed) return;

    this.service.delete(item.id).subscribe({
      next: result => {
        if (!result.is_success) {
          this.alertService.error(result.message || 'Không thể xoá Definition.');
          return;
        }

        this.alertService.success('Đã xoá Option Definition.');
        this.loadData();
      },
      error: err => {
        this.alertService.error(
          err?.error?.message || 'Đã có lỗi xảy ra khi xoá Definition.'
        );
      }
    });
  }

  onRetry(): void {
    this.loadData();
  }
  
  private loadData(): void {
    this.isLoading.set(true);
    this.errorMessage.set(null);
    this.service.getList({ search_text: this.searchText() }).subscribe({
      next: (items) => {
        this.items.set(items);
        this.isLoading.set(false);
      },
      error: (err) => {
        this.errorMessage.set(err?.error?.message || 'Đã có lỗi xảy ra khi tải dữ liệu.');
        this.isLoading.set(false);
      },
    });
  }

  ngOnDestroy(): void {
    this.searchSubject.complete();
  }
}
