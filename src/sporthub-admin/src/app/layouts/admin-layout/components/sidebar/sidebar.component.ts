import { Component } from '@angular/core';
import { RouterLink, RouterLinkActive } from '@angular/router';

interface NavItem {
  label: string;
  icon: string;
  route: string;
}

@Component({
  selector: 'app-sidebar',
  standalone: true,
  imports: [RouterLink, RouterLinkActive],
  templateUrl: './sidebar.component.html',
  styleUrl: './sidebar.component.scss',
})
export class SidebarComponent {
  navItems: NavItem[] = [
    { label: 'Dashboard', icon: '📊', route: '/dashboard' },
    { label: 'Thương hiệu', icon: '🏷️', route: '/brands' },
    { label: 'Danh mục', icon: '📁', route: '/categories' },
    { label: 'Sản phẩm', icon: '📦', route: '/products' },
    { label: 'Quản lý sân', icon: '🏸', route: '/courts' },
    { label: 'Đơn hàng', icon: '🧾', route: '/orders' },
  ];
}