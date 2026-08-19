import { Component, inject } from '@angular/core';
import { AuthService } from '../../../../core/auth/auth.service';

@Component({
  selector: 'app-header',
  standalone: true,
  templateUrl: './header.component.html',
  styleUrl: './header.component.scss'
})
export class HeaderComponent {
  private authService = inject(AuthService);

  currentUser = this.authService.currentUser;

  onLogout(): void {
    this.authService.logout();
  }
}