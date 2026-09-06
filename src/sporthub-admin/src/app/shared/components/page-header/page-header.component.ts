import { Component } from '@angular/core';
import { BreadcrumbComponent } from '../breadcrumb/breadcrumb.component';

@Component({
  selector: 'app-page-header',
  standalone: true,
  imports: [BreadcrumbComponent],
  templateUrl: './page-header.component.html',
  styleUrl: './page-header.component.scss'
})
export class PageHeaderComponent {}