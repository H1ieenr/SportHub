import { Component, inject } from '@angular/core';
import { IconDirective, IconService } from '@ant-design/icons-angular';
import { CloseOutline } from '@ant-design/icons-angular/icons';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-image-preview',
  standalone: true,
  templateUrl: './image-preview.component.html',
  styleUrl: './image-preview.component.scss',
  imports: [IconDirective],
})
export class ImagePreviewComponent {
  readonly activeModal = inject(NgbActiveModal);
  private iconService = inject(IconService);

  imageUrl = '';
  title = '';

  constructor() {
    this.iconService.addIcon(...[CloseOutline]);
  }
  onClose() {
    this.activeModal.dismiss();
  }
}