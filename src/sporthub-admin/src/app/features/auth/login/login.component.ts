import { Component, inject, signal } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from '../../../core/auth/auth.service';
import { AlertService } from '../../../core/common/alert/alert.service';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [ReactiveFormsModule],
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss',
})
export class LoginComponent {
  private fb = inject(FormBuilder);
  private authService = inject(AuthService);
  private router = inject(Router);
  private alertService = inject(AlertService);

  loading = signal(false);
  errorMessage = signal<string | null>(null);

  form = this.fb.group({
    email: ['', [Validators.required, Validators.email]],
    password: ['', [Validators.required, Validators.minLength(3)]],
  });

  onSubmit(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    this.loading.set(true);
    this.errorMessage.set(null);

    this.authService.login(this.form.getRawValue() as { email: string; password: string }).subscribe({
      next: (res) => {
        this.loading.set(false);
        if (res.is_success) {
          this.alertService.success(
            res.message || 'Đăng nhập thành công.'
          );

          this.router.navigate(['/dashboard']);
        } else {
          this.alertService.error(
            res.message || 'Đăng nhập thất bại.'
          );
        }
      },
      error: (err) => {
        this.loading.set(false);
        this.alertService.error(
          err.error?.message || 'Có lỗi xảy ra, vui lòng thử lại.'
        );
      },
    });
  }
}