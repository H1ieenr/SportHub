import { ChangeDetectionStrategy, Component, OnInit, computed, inject, signal } from '@angular/core';
import { ReactiveFormsModule, FormBuilder, Validators } from '@angular/forms';
import { NgbActiveOffcanvas } from '@ng-bootstrap/ng-bootstrap';
import { LoadingComponent } from '../../../../shared/components/loading/loading.component';
import { AlertService } from '../../../../shared/service/alert.service';
import { CategoryService } from '../../../../core/services/catalog/category/category.service';
import { IconDirective, IconService } from '@ant-design/icons-angular';
import { PlusCircleFill, EditFill, EyeFill } from '@ant-design/icons-angular/icons';
import { ImagePreviewService } from '../../../../shared/service/image-preview.service';
import { RichTextEditorComponent } from '../../../../shared/components/rich-text-editor/rich-text-editor.component';
import { Category } from '../../../../core/models/catalog/category/category.model';

export type CategoryFormMode = 'create' | 'edit' | 'view';
export interface CategoryFormResult {
  changed: boolean;
  createChildParentId?: number;
}
@Component({
  selector: 'app-category-form',
  standalone: true,
  imports: [ReactiveFormsModule, LoadingComponent, IconDirective, RichTextEditorComponent],
  templateUrl: './category-form.component.html',
  styleUrls: ['./category-form.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class CategoryFormComponent implements OnInit {
  private iconService = inject(IconService);
  private fb = inject(FormBuilder);
  private categoryService = inject(CategoryService);
  private alertService = inject(AlertService);
  private imagePreviewService = inject(ImagePreviewService);

  readonly activeOffcanvas = inject(NgbActiveOffcanvas);

  mode: CategoryFormMode = 'create';
  categoryId: number | null = null;
  category: Category | null = null;

  parentOptions: Category[] = [];
  presetParentId: number | null = null;

  readonly isViewMode = computed(() => this.mode === 'view');
  readonly loading = signal(false);
  readonly submitting = signal(false);

  readonly selectedFile = signal<File | null>(null);
  readonly logoPreviewUrl = signal<string>('assets/images/no-image.png');
  private objectUrl: string | null = null;

  readonly form = this.fb.nonNullable.group({
    name: ['', Validators.required],
    slug: ['', Validators.required],
    description: [''],
    is_active: [true],
    display_order: [0],
    parent_id: this.fb.control<number | null>(null) 
  });

  constructor() {
    this.iconService.addIcon(...[PlusCircleFill, EditFill, EyeFill]);
  }

  get title(): string {
    if (this.mode === 'create') return 'Thêm danh mục';      
    if (this.mode === 'view') return 'Chi tiết danh mục';     
    return 'Cập nhật danh mục';                                
  }

  get titleIcon(): string {
    if (this.mode === 'create') return 'plus-circle';
    if (this.mode === 'view') return 'eye';
    return 'edit';
  }

  ngOnInit() {
    if (this.category) {
      this.patchFromCategory(this.category);
    } else if (this.categoryId) {
      this.loadDetail(this.categoryId);
    } else if (this.mode === 'create' && this.presetParentId !== null) {
      this.form.patchValue({ parent_id: this.presetParentId });
    }
    if (this.mode === 'view') {
      this.form.disable();
    }
  }

  ngOnDestroy() {
    this.revokeObjectUrl();
  }

  private patchFromCategory(category: Category) {
    this.form.patchValue({
      name: category.name,
      slug: category.slug,
      description: category.description,
      is_active: category.is_active,    
      display_order: category.display_order, 
      parent_id: category.parent_id
    });
    this.logoPreviewUrl.set(category.image_url || 'assets/images/no-image.png');
  }

  loadDetail(id: number) {
    this.loading.set(true);
    this.categoryService.getView(id).subscribe({
      next: (category) => {
        this.patchFromCategory(category);
        this.loading.set(false);
      },
      error: (err) => {
        this.loading.set(false);
        this.alertService.error(err?.error?.message || 'Không tải được dữ liệu');
      }
    });
  }

  onFileSelected(event: Event) {
    const input = event.target as HTMLInputElement;
    const file = input.files?.[0];
    if (!file) return;

    if (!file.type.startsWith('image/')) {
      this.alertService.error('Vui lòng chọn file ảnh');
      return;
    }
    if (file.size > 5 * 1024 * 1024) {
      this.alertService.error('Ảnh không được vượt quá 5MB');
      return;
    }

    this.revokeObjectUrl();
    this.objectUrl = URL.createObjectURL(file);
    this.logoPreviewUrl.set(this.objectUrl);
    this.selectedFile.set(file);
  }

  private revokeObjectUrl() {
    if (this.objectUrl) {
      URL.revokeObjectURL(this.objectUrl);
      this.objectUrl = null;
    }
  }

  onSubmit() {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    if (this.mode === 'edit' && this.form.controls.parent_id.value === this.categoryId) {
      this.alertService.error('Danh mục cha không được là chính nó');
      return;
    }

    this.submitting.set(true);
    const raw = this.form.getRawValue();
    const payload = {
      name: raw.name,
      slug: raw.slug,
      description: raw.description,
      is_active: raw.is_active,        
      display_order: raw.display_order, 
      parent_id: raw.parent_id,         
      file_image: this.selectedFile()
    };

    const request$ =
      this.mode === 'edit' ? this.categoryService.update(this.categoryId!, payload) : this.categoryService.create(payload);

    request$.subscribe({
      next: () => {
        this.submitting.set(false);
        this.alertService.success(this.mode === 'edit' ? 'Cập nhật thành công' : 'Tạo mới thành công');
        this.activeOffcanvas.close(true);
      },
      error: (err) => {
        this.submitting.set(false);
        this.alertService.error(err?.error?.message || 'Có lỗi xảy ra');
      }
    });
  }

  onCancel() {
    if (this.submitting()) return;
    this.activeOffcanvas.dismiss();
  }

  onPreviewImageClick() {
    if (this.mode !== 'view') return;
    this.imagePreviewService.open(this.logoPreviewUrl(), this.form.controls.name.value);
  }

  onAddChildClick() {
    const parentId = this.categoryId ?? this.category?.id;
    if (!parentId) return;
    this.activeOffcanvas.close({ changed: false, createChildParentId: parentId } as CategoryFormResult);
  }
}