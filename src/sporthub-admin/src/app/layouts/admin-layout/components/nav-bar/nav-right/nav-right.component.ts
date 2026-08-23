// angular import
import { Component, output, inject, input, signal } from '@angular/core';
import { RouterModule } from '@angular/router';
import { AuthService } from '../../../../../core/auth/auth.service';
import { Router } from '@angular/router';
import { AlertService } from '../../../../../core/common/alert/alert.service';

// project import
import { SharedModule } from '../../../../../shared/shared.module';

// third party

// icon
import { IconService } from '@ant-design/icons-angular';
import {
  BellOutline,
  SettingOutline,
  GiftOutline,
  MessageOutline,
  PhoneOutline,
  LogoutOutline,
  EditOutline,
  UserOutline,
  ProfileOutline,
  QuestionCircleOutline,
} from '@ant-design/icons-angular/icons';
import { NgScrollbarModule } from 'ngx-scrollbar';
@Component({
  selector: 'app-nav-right',
  imports: [SharedModule, RouterModule, NgScrollbarModule],
  templateUrl: './nav-right.component.html',
  styleUrls: ['./nav-right.component.scss']
})
export class NavRightComponent {
  private iconService = inject(IconService);
  private authService = inject(AuthService);
  private router = inject(Router);
  private alertService = inject(AlertService);
  // public props
  styleSelectorToggle = input<boolean>();
  readonly currentUser = this.authService.currentUser;
  readonly Customize = output();
  windowWidth: number;
  screenFull: boolean = true;
  direction: string = 'ltr';
  loading = signal(false);
  
  // constructor
  constructor() {
    this.windowWidth = window.innerWidth;
    this.iconService.addIcon(
      ...[
        MessageOutline,
        SettingOutline,
        PhoneOutline,
        LogoutOutline,
        EditOutline,
        UserOutline,
        EditOutline,
        ProfileOutline,
        QuestionCircleOutline,
        BellOutline,
        GiftOutline
      ]
    );
  }

  profile = [
    {
      icon: 'edit',
      title: 'Edit Profile',
      action: ''
    },
    {
      icon: 'user',
      title: 'View Profile',
      action: ''
    },
    {
      icon: 'logout',
      title: 'Logout',
      action: 'logout'
    }
  ];

  setting = [
    {
      icon: 'question-circle',
      title: 'Support',
      action: ''
    },
  ];

  onProfileAction(action: string): void {
    if (action === 'logout') {
      this.onSubmitLogOut();
      return;
    }

    if (action === 'edit-profile') {
      return;
    }

    if (action === 'view-profile') {
    }
  }

  onSubmitLogOut(): void {
    this.loading.set(true);

    this.authService.logout().subscribe({
      next: (res) => {
        this.loading.set(false);
        if (res.is_success) {
          this.alertService.success(
            res.message || 'Đăng xuất thành công.'
          );
          this.router.navigate(['/login']);
        } else {
          this.alertService.error(
            res.message || 'Đăng xuất thất bại.'
          );
        }
      },
      error: (err) => {
        this.loading.set(false);
        this.alertService.error(
          err.error?.message || 'Có lỗi xảy ra, vui lòng thử lại.'
        );
        this.router.navigate(['/login']);
      }
    });
  }
}
