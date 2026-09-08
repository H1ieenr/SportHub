import { ChangeDetectionStrategy, Component, OnInit, computed, inject, signal } from '@angular/core';
import { ReactiveFormsModule, FormBuilder, Validators } from '@angular/forms';
import { NgbActiveOffcanvas } from '@ng-bootstrap/ng-bootstrap';
import { LoadingComponent } from '../../../../shared/components/loading/loading.component';
import { AlertService } from '../../../../shared/service/alert.service';
import { BrandService } from '../../../../core/services/catalog/brand/brand.service';
import { IconDirective, IconService } from '@ant-design/icons-angular';
import { PlusCircleFill, EditFill, EyeFill } from '@ant-design/icons-angular/icons';
import { ImagePreviewService } from '../../../../shared/service/image-preview.service';
import { RichTextEditorComponent } from '../../../../shared/components/rich-text-editor/rich-text-editor.component';
import { Brand } from '../../../../core/models/catalog/brand/brand.model';
export type BrandFormMode = 'create' | 'edit' | 'view';

@Component({
  selector: 'app-brand-form',
  standalone: true,
  imports: [ReactiveFormsModule, LoadingComponent, IconDirective, RichTextEditorComponent],
  templateUrl: './brand-form.component.html',
  styleUrl: './brand-form.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class BrandFormComponent implements OnInit {
  private iconService = inject(IconService);
  private fb = inject(FormBuilder);
  private brandService = inject(BrandService);
  private alertService = inject(AlertService);
  private imagePreviewService = inject(ImagePreviewService);

  readonly activeOffcanvas = inject(NgbActiveOffcanvas);

  mode: BrandFormMode = 'create';
  brandId: number | null = null;
  brand: Brand | null = null;

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
    active: [true]
  });

  constructor() {
    this.iconService.addIcon(...[PlusCircleFill, EditFill, EyeFill]);
  }

  get title(): string {
    if (this.mode === 'create') return 'Thêm thương hiệu';
    if (this.mode === 'view') return 'Chi tiết thương hiệu';
    return 'Cập nhật thương hiệu';
  }

  get titleIcon(): string {
    if (this.mode === 'create') return 'plus-circle';
    if (this.mode === 'view') return 'eye';
    return 'edit';
  }

  ngOnInit() {
    if (this.brand) {
      this.patchFromBrand(this.brand);
    } else if (this.brandId) {
      this.loadDetail(this.brandId);
    }
    if (this.mode === 'view') {
      this.form.disable();
    }
  }

  ngOnDestroy() {
    this.revokeObjectUrl();
  }

  private patchFromBrand(brand: Brand) {
    this.form.patchValue({
      name: brand.name,
      slug: brand.slug,
      description: brand.description,
      active: brand.is_active
    });
    this.logoPreviewUrl.set(brand.logo_url || 'assets/images/no-image.png');
  }

  loadDetail(id: number) {
    this.loading.set(true);
    this.brandService.getView(id).subscribe({
      next: (brand) => {
        this.patchFromBrand(brand);
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

    this.submitting.set(true);
    const raw = this.form.getRawValue();
    const payload = {
      name: raw.name,
      slug: raw.slug,
      description: raw.description,
      is_active: raw.active,
      file_logo: this.selectedFile()
    };

    const request$ =
      this.mode === 'edit' ? this.brandService.update(this.brandId!, payload) : this.brandService.create(payload);

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
}