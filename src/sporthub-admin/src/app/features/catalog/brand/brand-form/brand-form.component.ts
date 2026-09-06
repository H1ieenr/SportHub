import { ChangeDetectionStrategy, Component, OnInit, computed, inject, signal } from '@angular/core';
import { ReactiveFormsModule, FormBuilder, Validators } from '@angular/forms';
import { NgbActiveOffcanvas } from '@ng-bootstrap/ng-bootstrap';
import { LoadingComponent } from '../../../../shared/components/loading/loading.component';
import { AlertService } from '../../../../shared/service/alert.service';
import { BrandService } from '../../../../core/services/catalog/brand/brand.service';
import { IconDirective, IconService } from '@ant-design/icons-angular';
import { PlusCircleFill, EditFill, EyeFill } from '@ant-design/icons-angular/icons';

export type BrandFormMode = 'create' | 'edit' | 'view';

@Component({
  selector: 'app-brand-form',
  standalone: true,
  imports: [ReactiveFormsModule, LoadingComponent, IconDirective],
  templateUrl: './brand-form.component.html',
  styleUrl: './brand-form.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class BrandFormComponent implements OnInit {
  private iconService = inject(IconService);
  private fb = inject(FormBuilder);
  private brandService = inject(BrandService);
  private alertService = inject(AlertService);
  readonly activeOffcanvas = inject(NgbActiveOffcanvas);

  mode: BrandFormMode = 'create';
  brandId: number | null = null;

  readonly isViewMode = computed(() => this.mode === 'view');
  readonly loading = signal(false);
  readonly submitting = signal(false);

  // ảnh hiển thị ở đầu form, chưa cho sửa (upload sẽ làm sau)
  readonly logoPreviewUrl = signal<string>('assets/images/no-image.png');

  readonly form = this.fb.nonNullable.group({
    name: ['', Validators.required],
    slug: ['', Validators.required],
    logo_url: [''], // vẫn giữ trong form để gửi lên API, chỉ ẩn input trên UI
    description: ['']
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
    if (this.brandId) {
      this.loadDetail(this.brandId);
    }
    if (this.mode === 'view') {
      this.form.disable();
    }
  }

  loadDetail(id: number) {
    this.loading.set(true);
    this.brandService.getView(id).subscribe({
      next: (brand) => {
        this.form.patchValue({
          name: brand.name,
          slug: brand.slug,
          logo_url: brand.logo_url,
          description: brand.description
        });
        this.logoPreviewUrl.set(brand.logo_url || 'assets/images/no-image.png');
        this.loading.set(false);
      },
      error: (err) => {
        this.loading.set(false);
        this.alertService.error(err?.error?.message || 'Không tải được dữ liệu');
      }
    });
  }

  onSubmit() {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    this.submitting.set(true);
    const payload = this.form.getRawValue();

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
    this.activeOffcanvas.dismiss();
  }
}