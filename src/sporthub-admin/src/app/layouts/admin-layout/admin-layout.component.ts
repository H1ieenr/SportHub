import { Component, inject } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { RouterModule } from '@angular/router';
import { CommonModule } from '@angular/common';

//import { HeaderComponent } from './components/header/header.component';
//import { SidebarComponent } from './components/sidebar/sidebar.component';
//import { FooterComponent } from './components/footer/footer.component';

import { SharedModule } from '../../shared/shared.module';
import { LayoutStateService } from '../../shared/service/layout-state.service';
import { NavigationComponent } from './components/navigation/navigation.component';
import { NavBarComponent } from './components/nav-bar/nav-bar.component';
import { BreadcrumbComponent } from '../../shared/components/breadcrumb/breadcrumb.component';
@Component({
  selector: 'app-admin-layout',
  standalone: true,
  imports: [CommonModule, RouterOutlet, RouterModule, NavigationComponent, 
    NavBarComponent, BreadcrumbComponent, SharedModule,],
  templateUrl: './admin-layout.component.html',
  styleUrl: './admin-layout.component.scss',
})
export class AdminLayoutComponent {
  private layoutState = inject(LayoutStateService);

  // public props
  navCollapsed = false;
  windowWidth: number;

  // Constructor
  constructor() {
    this.windowWidth = window.innerWidth;
  }

  get navCollapsedMob(): boolean {
    return this.layoutState.navCollapsedMob();
  }

  // public method
  navMobClick() {
    this.layoutState.toggleNavCollapsedMob();
    if (document.querySelector('app-navigation.pc-sidebar')?.classList.contains('navbar-collapsed')) {
      document.querySelector('app-navigation.pc-sidebar')?.classList.remove('navbar-collapsed');
    }
  }

  handleKeyDown(event: KeyboardEvent): void {
    if (event.key === 'Escape') {
      this.closeMenu();
    }
  }

  closeMenu() {
    this.layoutState.closeNavCollapsedMob();
  }

  handleNavCollapse() {
    this.navCollapsed = !this.navCollapsed;
  }
}